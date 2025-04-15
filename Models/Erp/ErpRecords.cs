
namespace measurement_generator.Models.Erp;
public class ErpRecords
{
    public double NominalInputPressure { get; set; }
    public double NominalOutputPressure { get; set; }
    public List<int> Clients { get; set; }
    public List<RegulatorRecord> RegulatorsRecords { get; set; }

}

