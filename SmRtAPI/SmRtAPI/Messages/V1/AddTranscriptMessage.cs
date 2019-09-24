namespace Speechmatics.Realtime.Client.Messages.V1
{
    /// <summary>
    /// Detailed timings for a transcript
    /// </summary>
    public class AddTranscriptMessage : BaseMessage
    {
        /// <summary>
        /// Message type
        /// </summary>
        public override string message => "AddTranscript";
        /// <summary>
        /// Start time
        /// </summary>
        public double start_time;
        /// <summary>
        /// Length of audio segment
        /// </summary>
        public double length;
        /// <summary>
        /// Transcript ext
        /// </summary>
        public string transcript;
        /// <summary>
        /// Individual word data
        /// </summary>
        public WordSubMessage[] words;
    }
}