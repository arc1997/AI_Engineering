namespace AI_Engineering.Models
{
    public class TokenizeResponse
    {
        public string Text { get; set; }
        public List<string> Tokens { get; set; }
        public List<int> TokenIDs { get; set; }
    }
}
