using System.ComponentModel;
using Newtonsoft.Json;
using Speechmatics.Realtime.Client.Enumerations;

namespace Speechmatics.Realtime.Client.Messages.V1
{
    /// <summary>
    /// Audio format
    /// </summary>
    public class AudioFormatSubMessage
    {
        //   `AudioFormatSubMessage: {type: "raw", encoding: "pcm_s16le", sample_rate: 44100}`
        /// <summary>
        /// 
        /// </summary>
        /// <param name="audioFormatType"></param>
        /// <param name="encoding"></param>
        /// <param name="sampleRate">in Hz</param>
        public AudioFormatSubMessage(AudioFormatType audioFormatType, AudioFormatEncoding encoding, int sampleRate)
        {
            type = audioFormatType.ToApiString();
            this.encoding = encoding.ToApiString();
            sample_rate = sampleRate;
        }

        /// <summary>
        /// Sample rate of audio in Hz
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int sample_rate { get; }
        /// <summary>
        /// Type of audio (e.g. 'raw', 'file' - see docs)
        /// </summary>
        public string type { get; }
        /// <summary>
        /// Encoding (e.g. pcm_s16le)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string encoding { get; }
    }
}