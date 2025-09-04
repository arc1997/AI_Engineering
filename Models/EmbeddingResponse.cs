namespace AI_Engineering.Models
{
    public class EmbeddingResponse
    {
        public string Text { get; set; }
        public List<string> Tokens { get; set; }
        public List<List<float>> TokenEmbeddings { get; set; }
        public int EmbeddingSize { get; set; }
    }
}
