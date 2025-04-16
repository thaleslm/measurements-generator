using measurement_generator.Repository;
using measurement_generator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace measurement_generator.Controllers
{
    [ApiController]
    [Route("erps")]
    public class CSVController : ControllerBase
    {
        private readonly CSVReaderService _csvReaderService;
        private readonly ErpsService _erpsService;

        public CSVController(CSVReaderService csvReaderService, ErpsService erpsService)
        {
            _csvReaderService = csvReaderService;
            _erpsService = erpsService;
        }

        [HttpGet("register")]
        public async Task<IActionResult> Register()
        {
            await _csvReaderService.RegisterCsv();      
            return Ok("Ocorreu tudo certo!");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllErps()
        {
            var erps = await _erpsService.GetAllErp();
            return Ok(erps);
        }

        [HttpGet("csv")]
        public async Task<IActionResult> VerifyWhatErpsHaveMeasurements()
        {
            var erps = await _csvReaderService.VerifyWhatErpHaveCSV();
            return Ok(erps);
        }

        [HttpGet("measurements")]
        public async  Task<IActionResult> Measurements()
        {
            DateTime dataTarget = new DateTime(2024, 4, 21, 18, 30, 0); // 21/04/2024 18:30:00
            var erps = await _csvReaderService.ReadCsvfromErps(dataTarget);
            return Ok(erps);
        }
    }
}
