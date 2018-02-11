﻿using System;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Speechmatics.Realtime.Client.Enumerations;
using Speechmatics.Realtime.Client.Interfaces;
using Speechmatics.Realtime.Client.Messages;

namespace Speechmatics.Realtime.Client
{
    internal class MessageWriter
    {
        private readonly ClientWebSocket _wsClient;
        private readonly AutoResetEvent _resetEvent;
        private int _sequenceNumber;
        private readonly Stream _stream;
        private readonly ISmRtApi _api;

        internal MessageWriter(ISmRtApi smRtApi,
            ClientWebSocket client,
            AutoResetEvent resetEvent,
            Stream stream)
        {
            _api = smRtApi;
            _stream = stream;
            _wsClient = client;
            _resetEvent = resetEvent;
        }

        public async Task Start()
        {
            await StartRecognition();

            var streamBuffer = new byte[2048];
            int bytesRead;

            while ((bytesRead = _stream.Read(streamBuffer, 0, streamBuffer.Length)) > 0 && !_resetEvent.WaitOne(0))
            {
                await SendData(new ArraySegment<byte>(streamBuffer, 0, bytesRead));
            }

            var endOfStream = new EndOfStreamMessage(_sequenceNumber);
            await endOfStream.Send(_wsClient, _api.CancelToken);
        }

        /// <summary>
        /// Send a block of data as a sequence of AddData and binary messages.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task SendData(ArraySegment<byte> data)
        {
            // This is the size of the binary websocket message
            const int messageBlockSize = 1024;

            var arrayCopy = data.ToArray();
            var finalSectionOffset = 0;

            var msg = new AddDataMessage
            {
                size = data.Count,
                offset = 0,
                seq_no = Interlocked.Add(ref _sequenceNumber, 1)
            };

            await msg.Send(_wsClient, _api.CancelToken);

            if (data.Count > messageBlockSize)
            {
                for (var offset = 0; offset < data.Count / messageBlockSize; offset += messageBlockSize)
                {
                    finalSectionOffset += messageBlockSize;
                    await _wsClient.SendAsync(new ArraySegment<byte>(arrayCopy, offset, messageBlockSize),
                        WebSocketMessageType.Binary, false, _api.CancelToken);
                }
            }

            await _wsClient.SendAsync(
                new ArraySegment<byte>(arrayCopy, finalSectionOffset, data.Count - finalSectionOffset),
                WebSocketMessageType.Binary, true, _api.CancelToken);
        }

        private async Task StartRecognition()
        {
            var audioFormat = new AudioFormatSubMessage(_api.Configuration.AudioFormat,
                _api.Configuration.AudioFormatEncoding,
                _api.Configuration.SampleRate);
            var msg = new StartRecognitionMessage(audioFormat, _api.Configuration.Model, OutputFormat.Json, "rt_test");
            await msg.Send(_wsClient, _api.CancelToken);
        }
    }
}