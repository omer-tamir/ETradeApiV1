using System.Collections.Generic;

namespace ETradeApiV1.Client.Dtos
{
    public class QuoteResponse
    {
        public List<QuoteData> QuoteData { get; set; }
        public Messages Messages { get; set; }
    }
}