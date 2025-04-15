
namespace measurement_generator.Models.Erp;
public class Flow
{
    public List<string> Timestamps { get; set; }
    public List<double> Values { get; set; }
    public int Order { get; set; }
    public int Number { get; set; }
    public int Battery { get; set; }

}
