namespace measurement_generator.Models.Erp;


public class CurrentMeasurements
    {
    public List<double> TotalizerHist { get; set; }
    public int DataDisponibility { get; set; }
    public int EletricityGenerationHigh { get; set; }
    public int EletricityGenerationLow { get; set; }
    public string Eqpcomm { get; set; }
    public PressureMeasurement InputPressure { get; set; }
    public PressureMeasurement OutputPressure { get; set; }
    public List<Pdt> Pdts { get; set; }
    public List<Regulator> Regulators { get; set; }
    public List<Shutoff> Shutoff { get; set; }
    public Battery Battery { get; set; }
    public List<Flow> Flows { get; set; }
}

