
namespace measurement_generator.Models.Erp
{
    public class PressureMeasurement
    {
        public List<string> Timestamps { get; set; }
        public List<double> Values { get; set; }
        public double MaxLimit { get; set; }
        public double MinLimit { get; set; }
    }
}
