using AI_Engineering.Models;
using Microsoft.AspNetCore.Mvc;
using SharpToken;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AI_Engineering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenizeController : ControllerBase
    {
        [HttpPost]
        public ActionResult<TokenizeResponse> Post([FromBody] TokenizeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest("Input text is required.");
            }

            // Get encoding for GPT-4o
            GptEncoding enc = GptEncoding.GetEncodingForModel("gpt-4o");
            // Encode text into token IDs
            List<int> tokens = enc.Encode(request.Text);

            // Decode tokens back into string pieces
            List<string> tokenPieces = new List<string>();
            foreach (int token in tokens)
            {
                tokenPieces.Add(enc.Decode(new int[] { token }));
            }

            // Prepare response
            var response = new TokenizeResponse
            {
                Text = request.Text,
                Tokens = tokenPieces,
                TokenIDs = tokens
            };

            return Ok(response);
        }
    }
}
