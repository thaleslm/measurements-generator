

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace measurement_generator.Models.Erp;
public class Erp
{
    [Key]  // Defina o CodId como chave primária
    public int CodId { get; set; }
    public string DnsIot { get; set; }
    public string Type { get; set; }
    public bool Active { get; set; }
    public int ScopeId { get; set; }
    [NotMapped]
    public Location Location { get; set; }
    public int Status { get; set; }

    [NotMapped]
    public List<Alarms> Alarms { get; set; }
    [NotMapped]
    public CurrentMeasurements CurrentMeasurements { get; set; }
    [NotMapped]
    public CommunicationInfo CommunicationInfo { get; set; }
    public List<int> AssociatedGroups { get; set; }
    public bool Authenticated { get; set; }
    public int PackageQuantity { get; set; }
    public string Observations { get; set; } = string.Empty;
    [NotMapped]
    public ErpRecords ErpRecords { get; set; }
    public double IndexH { get; set; }

    public Boolean haveFile { get; set; }

}
