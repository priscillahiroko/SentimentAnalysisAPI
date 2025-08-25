using Microsoft.AspNetCore.Mvc;
using SentimentAnalysisAPI.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using SentimentAnalysisAPI.Filters;
using System;

namespace SentimentAnalysisAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ApiKeyFilter))]
    public class SentimentController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        
        public SentimentController(IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _httpClient = httpClientFactory.CreateClient("sentiment");
            _cache = cache;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze([FromBody] InputRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
                return BadRequest("O campo 'text' é obrigatório.");

            // Verifica no cache
            if (_cache.TryGetValue(request.Text, out SentimentResult cachedResult))
            {
                return Ok(cachedResult);
            }

            var response = await _httpClient.PostAsJsonAsync("/analyze", request);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erro no serviço de NLP.");

            var result = await response.Content.ReadFromJsonAsync<SentimentResult>();

            // Armazena no cache por 5 minutos
            _cache.Set(request.Text, result, TimeSpan.FromMinutes(5));

            return Ok(result);
        }

        [HttpPost("analyze/batch")]
        public async Task<IActionResult> AnalyzeBatch([FromBody] List<InputRequest> requests)
        {
            if (requests == null || requests.Count == 0)
                return BadRequest("A lista de textos não pode ser vazia.");

            var response = await _httpClient.PostAsJsonAsync("/analyze/batch", requests);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Erro no serviço de NLP.");

            var results = await response.Content.ReadFromJsonAsync<List<SentimentResult>>();
            return Ok(results);
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok("API .NET está operando normalmente.");
        }
    }
}
