using Microsoft.AspNetCore.Mvc;
using ServerApplication.HardwareInterface;
using ServerApplication.Storage;
using System.Threading.Tasks;
using System.Linq;
using ServerApplication.PredictionUnit;

namespace ServerApplication.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class SensorDataController : Controller
    {
        private IHardwareInterface hardwareInterface;
        private IStorageUnit storageUnit;
        private IPredictionUnit predictionUnit;

        public SensorDataController(IHardwareInterface hwIface, IStorageUnit storage, IPredictionUnit prediction)
        {
            hardwareInterface = hwIface;
            storageUnit = storage;
            predictionUnit = prediction;
        }

        [HttpGet("GetTemperature/{nRecords}")]
        public async Task<IActionResult> GetTemperature(int nRecords)
        {
            var retrievedRecords = await storageUnit.RetrieveSensorDataRecords(nRecords);
            var result = retrievedRecords.Select(element => { return new { element.Timestamp, element.Temperature }; }).OrderBy(e => e.Timestamp);
            return Json(result.Skip(0).Take(result.ToList().Count > nRecords ? nRecords : result.ToList().Count));
        }

        [HttpGet("GetHumidity/{nRecords}")]
        public async Task<IActionResult> GetHumidity(int nRecords)
        {
            var retrievedRecords = await storageUnit.RetrieveSensorDataRecords(nRecords);
            var result = retrievedRecords.Select(element => { return new { element.Timestamp, element.Humidity}; }).OrderBy(e => e.Timestamp);
            return Json(result.Skip(0).Take(result.ToList().Count > nRecords ? nRecords : result.ToList().Count));
        }

        [HttpGet("GetHeartRate/{nRecords}")]
        public async Task<IActionResult> GetHeartRate(int nRecords)
        {
            var retrievedRecords = await storageUnit.RetrieveSensorDataRecords(nRecords);
            var result = retrievedRecords.Select(element => { return new { element.Timestamp, element.HeartRate }; }).OrderBy(e => e.Timestamp);
            return Json(result.Skip(0).Take(result.ToList().Count > nRecords ? nRecords : result.ToList().Count));
        }

        [HttpGet("GetOxygenLevel/{nRecords}")]
        public async Task<IActionResult> GetOxygenLevel(int nRecords)
        {
            var retrievedRecords = await storageUnit.RetrieveSensorDataRecords(nRecords);
            var result = retrievedRecords.Select(element => { return new { element.Timestamp, element.Oxygen }; }).OrderBy(e => e.Timestamp);
            return Json(result.Skip(0).Take(result.ToList().Count > nRecords ? nRecords : result.ToList().Count));
        }

        [HttpGet("GetPrediction")]
        public string GetPrediction()
        {
            return predictionUnit.CurrentPrediction;
        }

        [HttpGet("SetSetpoint/{value}")]
        public void SetSetpoint(float value)
        {
            hardwareInterface.SetSetpoint(value);
        }

    }
}
