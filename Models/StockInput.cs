namespace StockPredictionAPI.Models
{
    public class StockInput
    {
        public float Open { get; set; }
        public float High { get; set; }
        public float Year { get; set; }
        public float Low { get; set; }
    }

    public class StockOutput
    {
        public float Close { get; set; }
    }

    public class ErrorResponse
    {
        public string Error { get; set; }
    }
}