using measurement_generator.Models.Erp;
using System.Globalization;
using System.Text.Json.Serialization;

namespace measurement_generator.Models.csv;

    public class MeasurementsDTO
    {

        public int codId { get; set; }
        public string timestamp { get; set; }
        public float pressureInputHighLimit { get; set; }
        public float pressureInputLowLimit { get; set; }
        public float pressureInput { get; set; }
        public float pressureOutput { get; set; }
        public float pressureOutputHighLimit { get; set; }
        public float pressureOutputLowLimit { get; set; }
        public List<float?> shutoffZASL { get; set; }
        public float? flow { get; set; }
        public List<float?> pdt { get; set; }
        public List<float?> regulator { get; set; }
        public List<float?> batteryReg { get; set; }
        public int status { get; set; }

        public override string ToString()
        {
            return $"Timestamp={timestamp}, " +
                   $"InputLow={pressureInputLowLimit}, InputHigh={pressureInputHighLimit}, Input={pressureInput}, " +
                   $"OutputLow={pressureOutputLowLimit}, OutputHigh={pressureOutputHighLimit}, Output={pressureOutput}, " +
                   $"Flow={flow}, " +
                   $"Shutoff={string.Join(",", shutoffZASL)}, " +
                   $"PDT={string.Join(",", pdt)}, " +
                   $"Regulators={string.Join(",", regulator)}";
        }
     // Propriedade auxiliar pra setar string direto
  


    }
