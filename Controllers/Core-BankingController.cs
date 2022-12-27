using indexPay.Models.DTOs;
using indexPay.Services.IServices;
using indexPay.Utilities.IUtilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace indexPay.Controllers
{
    [EnableCors("CorsApi")]
    [ApiController]
    [Route("api/v{version:apiVersion}/core-banking")]
    [ApiVersion("1.0")]
    public class CoreBankingController : ControllerBase
    {
        private readonly ICoreBankingServices _corebanking;
        private readonly ILogger<CoreBankingController> _logger;
        private readonly IMemCache _memCache;

        public CoreBankingController(ICoreBankingServices coreBankingServices, ILogger<CoreBankingController> logger, IMemCache memCache)
        {
            _corebanking = coreBankingServices;
            _logger = logger;
            _memCache = memCache;
        }

        [HttpGet]
        [Route("banks")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListBanks([FromHeader] string ApiKey, string provider)
        {
            try
            {
                //authenticate apiKey
                if (ApiKey != "predefinedKey")
                    return BadRequest(new ResponseMessage { Error = true, Description = "Invalid ApiKey", ErrorCode = "91" });

                var banks = await _corebanking.getBanksList(provider);
                if (banks.Error)
                    return BadRequest(new ResponseMessage { Error = true, Description = "Error While Fetching Banks List", ErrorCode = "99" });
                return Ok(new ResponseMessage { Error = false, Description = "success", Data = banks });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMessage { Error = true, Description = "An Error Occurred, Please try again", ErrorCode = "99" });
            }
    
        }


        [HttpPost]
        [Route("validateBankAccount")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> validateBankAccount([FromHeader] string ApiKey, validateBankAccountDTO request)
        {
            try
            {
                //authenticate apiKey
                if (ApiKey != "predefinedKey")
                    return BadRequest(new ResponseMessage { Error = true, Description = "Invalid ApiKey", ErrorCode = "91" });

                var validateAccount = await _corebanking.validateBankAccount(request);
                if (validateAccount.Error)
                    return BadRequest(new ResponseMessage { Error = true, Description = "Account Validation  Error", ErrorCode = "99" });
                return Ok(new ResponseMessage { Error = false, Description = "success", Data = validateAccount.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMessage { Error = true, Description = "An Error Occurred, Please try again" , ErrorCode = "99"});
            }

        }


        [HttpGet]
        [Route("transaction")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getTransaction([FromHeader] string ApiKey,string reference, string provider)
        {
            try
            {
                //authenticate apiKey
                if (ApiKey != "predefinedKey")
                    return BadRequest(new ResponseMessage { Error = true, Description = "Invalid ApiKey", ErrorCode = "91" });

                var validateAccount = await _corebanking.getTransctionStatus(provider, reference);
                if (validateAccount.Error)
                    return BadRequest(new ResponseMessage { Error = true, Description = "Fetch Transaction  Error", ErrorCode = "99" });
                return Ok(new ResponseMessage { Error = false, Description = "success", Data = validateAccount.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMessage { Error = true, Description = "An Error Occurred, Please try again", ErrorCode = "99" });
            }
        }


        [HttpPost]
        [Route("bankTransfer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> bankTransfer([FromHeader] string ApiKey, TransferDTO request)
        {
            try
            {
                //authenticate apiKey
                if (ApiKey != "predefinedKey")
                    return BadRequest(new ResponseMessage { Error = true, Description = "Invalid ApiKey", ErrorCode = "91" });
                //idempotency check
                var checkIdem = _memCache.Get(request.transactionReference);
                if (checkIdem != null)
                {
                    //return stored response
                    var response = JsonConvert.DeserializeObject<ResponseMessage>(checkIdem.ToString());
                    return Ok(response);
                }

                var transfer = await _corebanking.Transfer(request);
                if (transfer.Error)
                {
                    _memCache.Set(request.transactionReference, JsonConvert.SerializeObject(new ResponseMessage { Error = true, Description = "Bank Transfer  Error", ErrorCode = "99" }), 86400);
                    return BadRequest(new ResponseMessage { Error = true, Description = "Bank Transfer  Error", ErrorCode = "99" });
                }
                _memCache.Set(request.transactionReference, JsonConvert.SerializeObject(new ResponseMessage { Error = false, Description = transfer.Description, Data = transfer.Data }), 86400);
                return Ok(new ResponseMessage { Error = false, Description = transfer.Description, Data = transfer.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMessage { Error = true, Description = "An Error Occurred, Please try again", ErrorCode = "99" });
            }
        }

    }
}
