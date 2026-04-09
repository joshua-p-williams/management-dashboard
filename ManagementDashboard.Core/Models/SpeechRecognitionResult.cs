namespace ManagementDashboard.Core.Models
{
    public class SpeechRecognitionResult
    {
        public string Text { get; set; } = string.Empty;
        public float Confidence { get; set; } = 0.0f;
        public bool IsSuccess { get; set; } = false;
        public string? ErrorMessage { get; set; }

        public static SpeechRecognitionResult Success(string text, float confidence)
        {
            return new SpeechRecognitionResult
            {
                Text = text,
                Confidence = confidence,
                IsSuccess = true
            };
        }

        public static SpeechRecognitionResult Failure(string errorMessage)
        {
            return new SpeechRecognitionResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
}