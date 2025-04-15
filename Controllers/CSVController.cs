using measurement_generator.Repository;
using measurement_generator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace measurement_generator.Controllers
{
    [ApiController]
    [Route("erps")]
    public class CSVController : ControllerBase
    {
        private readonly CSVReaderService _csvReaderService;

        public CSVController(CSVReaderService csvReaderService)
        {
            _csvReaderService = csvReaderService;
        }

        [HttpGet("register")]
        public async Task<IActionResult> Register()
        {

            await _csvReaderService.RegisterCsv();
               
            return Ok("Ocorreu tudo certo!");
     
        }
        [HttpGet("all")]
        public async Task<IActionResult> get()
        {

            var erps = await _csvReaderService.getAllErp();

            return Ok(erps);
        
        }
        [HttpGet("csv")]
        public async Task<IActionResult> verifyWhatErpsHaveMeasurements()
        {

            var erps = await _csvReaderService.VerifyWhatErpHaveCSV();

            return Ok(erps);

        }

    }
}
