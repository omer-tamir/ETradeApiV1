namespace ETradeApiV1.Client.Dtos
{
    public class OptionGreeks
    {
        public double rho { get; set; }
        public double vega { get; set; }
        public double theta { get; set; }
        public double delta { get; set; }
        public double gamma { get; set; }
        public double iv { get; set; }
        public bool currentValue { get; set; }
    }
}