﻿using System;
using System.Threading;
using Speechmatics.Realtime.Client.Enumerations;

namespace Speechmatics.Realtime.Client.Interfaces
{
    internal interface ISmRtApi
    {
        Action<string> AddTranscriptCallback { get; }
        string Model { get; }
        int SampleRate { get; }
        AudioFormatType AudioFormat { get; }
        AudioFormatEncoding AudioFormatEncoding { get; }

        /// <summary>
        /// Cancellation token for async operations
        /// </summary>
        CancellationToken CancelToken { get; }

        /// <summary>
        /// The websocket URL this API instance is connected to
        /// </summary>
        Uri WsUrl { get; }
    }
}