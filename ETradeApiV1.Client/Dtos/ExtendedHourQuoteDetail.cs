namespace ETradeApiV1.Client.Dtos
{
    public class ExtendedHourQuoteDetail
    {
        public double lastPrice { get; set; }
        public double change { get; set; }
        public double percentChange { get; set; }
        public double bid { get; set; }
        public long bidSize { get; set; }
        public double ask { get; set; }
        public long askSize { get; set; }
        public long volume { get; set; }
        public long timeOfLastTrade { get; set; }
        public string timeZone { get; set; }
        public string quoteStatus { get; set; }
    }
}