namespace UserTopUpService.Domain
{
    public class TopUpHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BeneficiaryId { get; set; }
        public Beneficiary Beneficiary { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}

