namespace ETradeApiV1.Client.Dtos
{
    public class Option
    {
        public double ask { get; set; }
        public long askSize { get; set; }
        public double bid { get; set; }
        public long bidSize { get; set; }
        public string companyName { get; set; }
        public long daysToExpiration { get; set; }
        public double lastTrade { get; set; }
        public int openInterest { get; set; }
        public double optionPreviousBidPrice { get; set; }
        public double optionPreviousAskPrice { get; set; }
        public string osiKey { get; set; }
        public double intrinsicValue { get; set; }
        public double timePremium { get; set; }
        public double optionMultiplier { get; set; }
        public double contractSize { get; set; }
        public string symbolDescription { get; set; }
        public OptionGreeks OptionGreeks { get; set; }
    }
}