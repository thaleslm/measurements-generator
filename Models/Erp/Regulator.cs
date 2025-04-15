
namespace measurement_generator.Models.Erp;
public class Regulator
{
    public List<string> Timestamps { get; set; }
    public List<double> OpeningPercentageValues { get; set; }
    public int Pressure { get; set; }
    public int MaxOpenLimit { get; set; }
    public int MinOpenLimit { get; set; }
    public int Order { get; set; }
    public int Number { get; set; }
    public double Battery { get; set; }
}
