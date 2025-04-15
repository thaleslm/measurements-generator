
namespace measurement_generator.Models.Erp
{
    public class CommunicationInfo
    {
        public int Status { get; set; }
        public int TelecomunicationTechnology { get; set; }
        public int TelemetrySystem { get; set; }
        public int Protocol { get; set; }
        public string Ip { get; set; }
        public string Mac { get; set; }
    }
}
