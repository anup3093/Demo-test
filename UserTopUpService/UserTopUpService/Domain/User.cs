namespace UserTopUpService.Domain
{
    public class User
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public int IsVerified { get; set; }
        public string PhoneNumber { get; set; }
        public List<Beneficiary> Beneficiaries { get; set; }
    }
}