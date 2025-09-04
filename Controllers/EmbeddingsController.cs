using AI_Engineering.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI;
using OpenAI.Embeddings;
using SharpToken;

namespace AI_Engineering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmbeddingsController : ControllerBase
    {
        private readonly EmbeddingClient _embeddingClient;
        private readonly GptEncoding _enc;

        public EmbeddingsController()
        {
            string apiKey = "";
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("OPENAI_API_KEY environment variable not set.");

            _embeddingClient = new EmbeddingClient(
            model: "text-embedding-3-small",
            apiKey: apiKey
        );
            _enc = GptEncoding.GetEncodingForModel("gpt-4o");
        }

        [HttpPost]
        public async Task<ActionResult<EmbeddingResponse>> Post([FromBody] EmbeddingRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
                return BadRequest("Input text is required.");

            // Tokenize
            List<int> tokens = _enc.Encode(request.Text);
            List<string> tokenPieces = tokens.Select(t => _enc.Decode(new[] { t })).ToList();

            // Get embeddings per token
            var tokenEmbeddings = new List<List<float>>();

            foreach (var token in tokens)
            {
                string tokenStr = _enc.Decode(new int[] { token });

                var response = await _embeddingClient.GenerateEmbeddingAsync(
                    input: tokenStr
                );

                tokenEmbeddings.Add(response.Value.ToFloats().ToArray().ToList());
            }

            var result = new EmbeddingResponse
            {
                Text = request.Text,
                Tokens = tokenPieces,
                TokenEmbeddings = tokenEmbeddings,
                EmbeddingSize = tokenEmbeddings.Count > 0 ? tokenEmbeddings[0].Count : 0
            };

            return Ok(result);
        }
    }
}
