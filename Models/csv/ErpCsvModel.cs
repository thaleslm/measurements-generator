namespace measurement_generator.Models.csv
{
    public class ErpCsvModel
    {
        public string? ScopeName { get; set; }
        public int ScopeId { get; set; }
        public string? ERP_Cod { get; set; }
        public string? Type { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public int CodId { get; set; }

        public Boolean haveFile { get; set; }
    }

}
