namespace WorkerServiceDimo.Models
{
    public class SmartPayReport
    {
        public string MerchantName { get; set; }
        public string MerchantLocationMID { get; set; }
        public string TerminalID { get; set; }
        public string SettlementDate { get; set; }
        public string TrxnDate { get; set; }
        public string BatchNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string AuthId { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string TransType { get; set; }
        public string GrossAmount { get; set; }
        public string DiscountAmount { get; set; }
        public string NetAmount { get; set; }
        public string City { get; set; }
        public string MerchantAccount { get; set; }
        public string MerchantCustomer { get; set; }
        public string Terminal { get; set; }


    }
}
