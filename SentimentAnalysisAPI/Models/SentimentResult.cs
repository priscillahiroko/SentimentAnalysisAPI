namespace SentimentAnalysisAPI.Models
{
    public class SentimentResult
    {
        public string Text { get; set; }
        public double Polarity { get; set; }
        public double Subjectivity { get; set; }
        public string DetectedLanguage { get; set; }
        public string TranslatedText { get; set; }
        public int WordCount { get; set; }
        public int SentenceCount { get; set; }
        public double EstimatedReadingTime { get; set; } // em segundos
    }
}
