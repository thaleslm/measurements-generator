using System.ComponentModel.DataAnnotations;

namespace measurement_generator.Models.Request
{
    public class LastAuxiliary12_000_0
    {
        [Key]
        public int Id { get; set; }
        public int codId { get; set; }
        public DateTime? timestamp { get; set; }
        public float? pressureInputHighLimit { get; set; }
        public float? pressureInputLowLimit { get; set; }
        public float? pressureInput { get; set; }
        public float? pressureOutput { get; set; }
        public float? pressureOutputHighLimit { get; set; }
        public float? pressureOutputLowLimit { get; set; }
        public List<float?> shutoffZASL { get; set; }
        public float? flow { get; set; }
        public List<float?> PDT { get; set; }
        public List<float?> regulator { get; set; }
        public List<float?> BatteryReg { get; set; }
        public float? status { get; set; }
        public int type = 1;

        public override string ToString()
        {
            return $"Timestamp={timestamp}, " +
                   $"InputLow={pressureInputLowLimit}, InputHigh={pressureInputHighLimit}, Input={pressureInput}, " +
                   $"OutputLow={pressureOutputLowLimit}, OutputHigh={pressureOutputHighLimit}, Output={pressureOutput}, " +
                   $"Flow={flow}, " +
                   $"Shutoff={string.Join(",", shutoffZASL)}, " +
                   $"PDT={string.Join(",", PDT)}, " +
                   $"Regulators={string.Join(",", regulator)}";
        }
    }
}
