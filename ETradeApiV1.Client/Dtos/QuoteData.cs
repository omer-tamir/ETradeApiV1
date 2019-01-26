namespace ETradeApiV1.Client.Dtos
{
    public class QuoteData
    {
        public string dateTime { get; set; }
        public string dateTimeUTC { get; set; }
        public string quoteStatus { get; set; }
        public string ahFlag { get; set; }
        public All All { get; set; }
        public Option Option { get; set; }
        public Product Product { get; set; }
    }
}