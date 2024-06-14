namespace UserTopUpService.Domain
{
    public class TopUpResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int NewBalance { get; set; }
    }
}