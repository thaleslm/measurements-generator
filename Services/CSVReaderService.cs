using CsvHelper;
using CsvHelper.Configuration;
using measurement_generator.Models.csv;
using measurement_generator.Models.Erp;
using measurement_generator.Models.Request;
using measurement_generator.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace measurement_generator.Services;
public class CSVReaderService
{
    private readonly AppDBContext _db;
    private readonly ErpsService _erpsService;

    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HeaderValidated = null, // turns off header validation
        MissingFieldFound = null, // turn off error missing field
    };

    public CSVReaderService(AppDBContext db,ErpsService erpsService, IConfiguration configuration)
    {
        _db = db;
        _erpsService = erpsService;
        _configuration = configuration;
    }
    public async Task RegisterCsv()
    {

        using var reader = new StreamReader("/app/data/assets/ErpCadastradas.csv");
        using var csv = new CsvReader(reader, config);

        var registros = csv.GetRecords<ErpCsvModel>();

        foreach (var registro in registros)
        {
            var erp = new Erp
            {
                DnsIot = registro.ERP_Cod,
                Type = registro.Type,
                Active = true, 
                ScopeId = registro.ScopeId,
                Status = 1,
                AssociatedGroups = new List<int>(), 
                Authenticated = false,
                PackageQuantity = 0,
                Observations = string.Empty,
                IndexH = 0,
                Location = new Location
                {
                    lat = registro.Lat,
                    lng = registro.Lon
                }
            };

            _db.Erps.Add(erp);
        }

        await _db.SaveChangesAsync();
    }


 

    public async Task<object> VerifyWhatErpHaveCSV()
    {
        // To reading all folder files
        var filePaths = Directory.GetFiles("/app/data", "*.csv");
        // To extract the name without the extension and normalizes (example: "ERP1.csv" to "erp1")
        var fileDnsList = filePaths
            .Select(path => Path.GetFileNameWithoutExtension(path).Trim().ToLower())
            .ToHashSet();
        // Get the DnsIot from erps and normalizes it too
        var erps = await _db.Erps
            .Where(e => !string.IsNullOrEmpty(e.DnsIot)) 
            .ToListAsync();
        // Update the atrribute "HaveFile" to true in Erps that have a corresponding file
        foreach (var erp in erps)
        {
            if (fileDnsList.Contains(erp.DnsIot.Trim().ToLower()))
            {
                erp.HaveFile = true;
            }
        }
        // Save Changes in the dataBase
        await _db.SaveChangesAsync();
        // return all erps with the attribute "HaveFile" equal true and amount exist
        var erpsComArquivo = erps
            .Where(erp => erp.HaveFile)
            .ToList();

        return erpsComArquivo;
    }

    public async Task<List<MeasurementsDTO>> ReadCsvfromErps(DateTime targetDateTime)
    {
        List<Erp> erps = await _erpsService.GetErpsWithHaveFile();
        string[] filePaths = Directory.GetFiles("/app/data", "*.csv");
        List<LastAuxiliary12_000_0> listMeasurements = new List<LastAuxiliary12_000_0>();

        Random rand = new Random();

        foreach (var erp in erps)
        {
            string targetName = $"{erp.DnsIot}.csv";
            string? matchingFile = filePaths.FirstOrDefault(filePath => Path.GetFileName(filePath).Equals(targetName, StringComparison.OrdinalIgnoreCase));
            string? line = matchingFile != null ? ReadLineByDateTime(matchingFile, targetDateTime) : null;
            string[] columns = line?.Split(',');

            bool hasValidData = columns != null &&
                                ParseFloat(columns[1]) > 0 &&
                                ParseFloat(columns[2]) > 0 &&
                                ParseFloat(columns[3]) > 0 &&
                                ParseFloat(columns[4]) > 0 &&
                                ParseFloat(columns[5]) > 0 &&
                                ParseFloat(columns[6]) > 0;

            var existing = await _db.LastMeasurements.FirstOrDefaultAsync(x => x.codId == erp.CodId);

            if (hasValidData)
            {
                var timestamp = DateTime.ParseExact(columns[0], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture,DateTimeStyles.AssumeLocal);

                var measurement = new LastAuxiliary12_000_0
                {
                    codId = erp.CodId,
                    timestamp = timestamp,
                    pressureInputLowLimit = (float)ParseFloat(columns[1]),
                    pressureInputHighLimit = (float)ParseFloat(columns[2]),
                    pressureInput = (float)ParseFloat(columns[3]),
                    pressureOutputLowLimit = (float)ParseFloat(columns[4]),
                    pressureOutputHighLimit = (float)ParseFloat(columns[5]),
                    pressureOutput = (float)ParseFloat(columns[6]),
                    shutoffZASL = new List<float?> { ParseFloat(columns[7]), ParseFloat(columns[8]) },
                    flow = ParseFloat(columns[9]),
                    PDT = new List<float?> { ParseFloat(columns[10]), ParseFloat(columns[11]) },
                    regulator = new List<float?> { ParseFloat(columns[12]) , ParseFloat(columns[13]) },
                    status = 0,
                    BatteryReg = new List<float?>([0, 0]),
                };
                if (existing != null)
                {
                    updateErpsValue(existing, measurement);
                }
                else
                {
                    _db.LastMeasurements.Add(measurement);
                }

                listMeasurements.Add(measurement);
            }
            else if (existing != null)
            {
                // Função para simular com variação de ±10%
                float Simulate(float value) =>
                    value * (float)(1 + (rand.NextDouble() * 0.2 - 0.1));

                var simulated = new LastAuxiliary12_000_0
                {
                    codId = existing.codId,
                    timestamp = targetDateTime,
                    pressureInputLowLimit = Simulate(existing.pressureInputLowLimit),
                    pressureInputHighLimit = Simulate(existing.pressureInputHighLimit),
                    pressureInput = Simulate(existing.pressureInput),
                    pressureOutputLowLimit = Simulate(existing.pressureOutputLowLimit),
                    pressureOutputHighLimit = Simulate(existing.pressureOutputHighLimit),
                    pressureOutput = Simulate(existing.pressureOutput),
                    shutoffZASL = existing.shutoffZASL,
                    flow = existing.flow,
                    PDT = existing.PDT,
                    regulator = existing.regulator,
                    status = existing.status,
                    BatteryReg = existing.BatteryReg,
                };

                updateErpsValue(existing, simulated);

                listMeasurements.Add(simulated);
            }
            await _db.SaveChangesAsync();
        }
        return listMeasurements.Select(m => new MeasurementsDTO
        {
            codId = m.codId,
            timestamp = m.timestamp.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            pressureInputHighLimit = m.pressureInputHighLimit,
            pressureInputLowLimit = m.pressureInputLowLimit,
            pressureInput = m.pressureInput,
            pressureOutput = m.pressureOutput,
            pressureOutputHighLimit = m.pressureOutputHighLimit,
            pressureOutputLowLimit = m.pressureOutputLowLimit,
            shutoffZASL = m.shutoffZASL ?? new List<float?>([0, 0]),
            flow = m.flow,
            pdt = m.PDT ?? new List<float?>([0, 0]),
            regulator = m.regulator ?? new List<float?>([0, 0]),
            batteryReg = m.BatteryReg ?? new List<float?>([0,0]),
            status = (int)m.status
        }).ToList();
    }

    public void updateErpsValue(LastAuxiliary12_000_0 existing, LastAuxiliary12_000_0 measurement)
    {
        existing.timestamp = measurement.timestamp;
        existing.pressureInputLowLimit = measurement.pressureInputLowLimit;
        existing.pressureInputHighLimit = measurement.pressureInputHighLimit;
        existing.pressureInput = measurement.pressureInput;
        existing.pressureOutputLowLimit = measurement.pressureOutputLowLimit;
        existing.pressureOutputHighLimit = measurement.pressureOutputHighLimit;
        existing.pressureOutput = measurement.pressureOutput;
        existing.shutoffZASL = measurement.shutoffZASL;
        existing.flow = measurement.flow;
        existing.PDT = measurement.PDT;
        existing.regulator = measurement.regulator;
        existing.status = measurement.status;
        existing.BatteryReg = measurement.BatteryReg;
    }
    private float? ParseFloat(string? input)
    {
        if (input == null) return 0;
        return float.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out float value)
            ? value
            : 0;
    }
    public string? ReadLineByDateTime(string filePath, DateTime targetDateTime)
    {
        using var reader = new StreamReader(filePath);
        string line;
        //format csv
        string targetText = targetDateTime.ToString("dd/MM/yyyy HH:mm:ss");

        while ((line = reader.ReadLine()) != null)
        {
            if (line.StartsWith(targetText))
            {
                return line;
            }
        }
        return null;
    }

}


