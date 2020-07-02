using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using exchange.Helper;
using exchange.Models;
using exchange.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace exchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : Controller
    {
        private readonly PurchaseService _purchaseService;
        private readonly ExchangeController _exchangeController;
        private readonly ILoggerManager _logger;

        public PurchasesController(PurchaseService purchaseService, ExchangeController exchangeController, ILoggerManager logger)
        {
            _purchaseService = purchaseService;
            _exchangeController = exchangeController;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Purchase>> PurchaseCurrency(PurchaseDto purchaseDto)
        {
            try
            {
                double exchange = 0;
                double totalAmountPurchase = 0;

                if (purchaseDto == null)
                {
                    _logger.LogError("Purchase object sent from client is null.");
                    return BadRequest("Purchase object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid purchase object sent from client.");

                    return BadRequest("Invalid model object");
                }

                var purchase = new Purchase
                {
                    UserId = purchaseDto.userId,
                    Amount = double.Parse(purchaseDto.amount),
                    Currency = purchaseDto.currency.ToLower(),
                    Month = DateTime.Today.Month,
                    Year = DateTime.Today.Year
                };

                // valid amount
                if (purchase.Amount <= 0)
                {
                    _logger.LogError("Invalid purchase amount sent from client.");

                    return NotFound(new NotFoundError("Invalid amount"));
                }

                // check if is a valid currency.
                if (purchase.Currency != "dolar" && purchase.Currency != "real")
                {
                    _logger.LogError("Invalid purchase currency sent from client.");

                    return NotFound(new NotFoundError("The currency was not found"));
                }

                // get all purchase for this user in this month with this currency.
                var purchaseByMonthList = _purchaseService.GetByUserIdAndMonthAndCurrency(purchase);

                foreach (var purchaseByMonth in purchaseByMonthList)
                {
                    totalAmountPurchase = totalAmountPurchase + purchaseByMonth.Amount;
                }

                // check if exceeds the minimal amount.
                if ((totalAmountPurchase >= 200 && purchase.Currency == "dolar") || (totalAmountPurchase >= 300 && purchase.Currency == "real"))
                {
                    _logger.LogError("Invalid purchase because the user exceeds monthly purchase limit.");

                    return NotFound(new NotFoundError("The user exceeds monthly purchase limit"));
                }

                var exchangeResponse = await _exchangeController.ExchangeRate(purchase.Currency);
                if (exchangeResponse is OkObjectResult exchangeRate && exchangeRate.Value != null)
                {
                    // get the exchange rate sold.
                    var sold = exchangeRate.Value.GetType().GetProperty("sold").GetValue(exchangeRate.Value).ToString();

                    exchange = double.Parse(sold);
                    purchase.Amount = purchase.Amount / exchange;
                    totalAmountPurchase = totalAmountPurchase + purchase.Amount;

                    if ((totalAmountPurchase >= 200 && purchase.Currency == "dolar") || (totalAmountPurchase >= 300 && purchase.Currency == "real"))
                    {
                        _logger.LogError("Invalid purchase because the user exceeds monthly purchase limit.");

                        return NotFound(new NotFoundError("The user exceeds monthly purchase limit"));
                    }

                    _purchaseService.Create(purchase);

                    _logger.LogInformation($"The creation of a purchase was successfully.");

                    return CreatedAtRoute("GetPurchase", new { id = purchase.Id }, purchase);
                }

                return BadRequest(new InternalServerError("The external service don't response"));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild Create purchase. Message: {ex.Message}");

                return BadRequest(new InternalServerError());
            }
        }

        [HttpGet("{id:length(24)}", Name = "GetPurchase")]
        public ActionResult Get(string id)
        {
            try
            {
                var purcharse = _purchaseService.Get(id);

                if (purcharse == null)
                {
                    _logger.LogInformation($"Purchase with id: {id}, hasn't been found in db.");

                    return NotFound();
                }

                _logger.LogInformation($"Returned purchase with id: {id}");

                return Ok(purcharse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Get Purchase by id. Message: {ex.Message}");

                return BadRequest(new InternalServerError());
            }
        }
    }
}
