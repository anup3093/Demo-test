namespace UserTopUpService.Domain
{
    public class TopUpRequest
    {
        public string BeneficiaryNickname { get; set; }
        public int TopUpAmount { get; set; }
        public int UserId { get; set; }
    }
}
