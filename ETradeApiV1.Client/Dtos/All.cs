using System;

namespace ETradeApiV1.Client.Dtos
{
    public class All
    {
        public bool adjustedFlag { get; set; }
        public double annualDividend { get; set; }
        public double ask { get; set; }
        public string askExchange { get; set; }
        public long askSize { get; set; }
        public string askTime { get; set; }
        public double bid { get; set; }
        public string bidExchange { get; set; }
        public long bidSize { get; set; }
        public string bidTime { get; set; }
        public double changeClose { get; set; }
        public double changeClosePercentage { get; set; }
        public string companyName { get; set; }
        public long daysToExpiration { get; set; }
        public string dirLast { get; set; }
        public double dividend { get; set; }
        public double eps { get; set; }
        public double estEarnings { get; set; }
        public long exDividendDate { get; set; }
        public string exchgLastTrade { get; set; }
        public string fsi { get; set; }
        public double high { get; set; }
        public double high52 { get; set; }
        public double highAsk { get; set; }
        public double highBid { get; set; }
        public double lastTrade { get; set; }
        public double low { get; set; }
        public double low52 { get; set; }
        public double lowAsk { get; set; }
        public double lowBid { get; set; }
        public long numberOfTrades { get; set; }
        public double open { get; set; }
        public long openInterest { get; set; }
        public string optionStyle { get; set; }
        public double previousClose { get; set; }
        public long previousDayVolume { get; set; }
        public string primaryExchange { get; set; }
        public string symbolDescription { get; set; }
        public double todayClose { get; set; }
        public long totalVolume { get; set; }
        public long upc { get; set; }
        public long volume10Day { get; set; }
        public long cashDeliverable { get; set; }
        public double marketCap { get; set; }
        public long sharesOutstanding { get; set; }
        public DateTime nextEarningDate { get; set; }
        public double beta { get; set; }
        public double yield { get; set; }
        public double declaredDividend { get; set; }
        public long dividendPayableDate { get; set; }
        public double pe { get; set; }
        public long marketCloseBidSize { get; set; }
        public long marketCloseAskSize { get; set; }
        public long marketCloseVolume { get; set; }
        public long week52LowDate { get; set; }
        public long week52HiDate { get; set; }
        public double longrinsicValue { get; set; }
        public double timePremium { get; set; }
        public double optionMultiplier { get; set; }
        public double contractSize { get; set; }
        public long expirationDate { get; set; }
        public long timeOfLastTrade { get; set; }
        public long averageVolume { get; set; }
    }
}