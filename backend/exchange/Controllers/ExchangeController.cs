using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using exchange.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace exchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : Controller
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private readonly string dolarServiceUrl = "http://www.bancoprovincia.com.ar/Principal/Dolar";
        private readonly string realServiceUrl = string.Empty;
        private readonly string dolarCanadianServiceUrl = string.Empty;

        // GET: api/exchange
        [HttpGet("{currency}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExchangeRate(string currency)
        {
            currency = currency.ToLower();

            // check valid request
            if (string.IsNullOrEmpty(currency) || (!currency.Contains("dolar") && !currency.Contains("real") && !currency.Contains("canadian")))
            {
                return NotFound(new NotFoundError("The exchange was not found"));
            }

            if (currency.Contains("dolar"))
            {
                return await GetDolarExchangeRateFromExternal();
            }
            if (currency.Contains("canadian"))
            {
                return await GetDolarCanadianExchangeRateFromExternal();
            }

            return await GetRealExchangeRateFromExternal();

        }

        /// <summary>
        /// Gets the actual exchange from external API.
        /// </summary>
        /// <returns>The exchange rate from external.</returns>
        /// <param name="url">URL of the external service.</param>
        private async Task<IActionResult> GetExchangeRateFromExternal(string url)
        {
            using (var response = await _httpClient.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                    return BadRequest(new InternalServerError("The external service don't response"));

                var responseContent = await response.Content.ReadAsStringAsync();
                var deserializedResponse = JsonConvert.DeserializeObject<List<string>>(responseContent);

                if (deserializedResponse.Count > 0)
                {
                    return Ok(new { buy = deserializedResponse.ElementAt(0), sold = deserializedResponse.ElementAt(1) });
                }

                return BadRequest(new InternalServerError("The external service don't response"));
            }
        }

        /// <summary>
        /// Gets the real exchange rate from external.
        /// </summary>
        /// <returns>The real exchange rate from external.</returns>
        private async Task<IActionResult> GetRealExchangeRateFromExternal()
        {
            if (string.IsNullOrEmpty(realServiceUrl))
            {
                var dolarExchange = await GetExchangeRateFromExternal(dolarServiceUrl);

                if (dolarExchange is OkObjectResult exchange && exchange.Value != null)
                {
                    // take the sold result and make the division to 4 and return.
                    var buy = exchange.Value.GetType().GetProperty("buy").GetValue(exchange.Value).ToString();

                    var sold = exchange.Value.GetType().GetProperty("sold").GetValue(exchange.Value).ToString();

                    var response = new
                    {
                        buy = (double.Parse(buy) / 4).ToString(),
                        sold = (double.Parse(sold) / 4).ToString()
                    };

                    return Ok(response);
                }

                return BadRequest(new InternalServerError("The external service don't response"));
            }

            return await GetExchangeRateFromExternal(realServiceUrl);
        }

        /// <summary>
        /// Gets the actual exchange from external API from Province Bank.
        /// </summary>
        /// <returns>The dolar exchange rate from external.</returns>
        private async Task<IActionResult> GetDolarExchangeRateFromExternal()
        {
            return await GetExchangeRateFromExternal(dolarServiceUrl);
        }

        /// <summary>
        /// Gets the dolar canadian exchange rate.
        /// </summary>
        /// <returns>The dolar canadian exchange rate.</returns>
        private async Task<IActionResult> GetDolarCanadianExchangeRateFromExternal()
        {
            return await GetExchangeRateFromExternal(dolarCanadianServiceUrl);
        }
    }
}