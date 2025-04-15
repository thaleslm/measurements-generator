namespace measurement_generator.Models.Erp;
public class Pdt
{
    public List<string> Timestamps { get; set; }
    public List<double> Values { get; set; }
    public int MaxLimit { get; set; }
    public int MinLimit { get; set; }
    public int Order { get; set; }
    public int Number { get; set; }
    public int Battery { get; set; }
}

