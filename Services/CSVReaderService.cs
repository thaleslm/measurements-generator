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

    public CSVReaderService(AppDBContext db,ErpsService erpsService)
    {
        _db = db;
        _erpsService = erpsService;
    }
    public async Task RegisterCsv()
    {
        using var reader = new StreamReader("C:\\Users\\woami\\Desktop\\Backends\\measurement-generator\\assets\\ErpCadastradas.csv");
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
        var filePaths = Directory.GetFiles("C:\\Users\\woami\\Desktop\\measurements-comgas", "*.csv");
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

    public async Task<List<Erp>> ReadCsvfromErps(DateTime targetDateTime)
    {
        List<Erp> erps = await _erpsService.GetErpsWithHaveFile();
        string[] filePaths = Directory.GetFiles("C:\\Users\\woami\\Desktop\\measurements-comgas", "*.csv");
        int specificLine = 5;
        foreach(var erp in erps)

        {
            string targetName = $"{erp.DnsIot}.csv";
            string? matchingFile = filePaths.FirstOrDefault(filePath => Path.GetFileName(filePath).Equals(targetName, StringComparison.OrdinalIgnoreCase));

            if(matchingFile != null)
            { 
                string? line = ReadLineByDateTime(matchingFile, targetDateTime);
                if (line != null)
                {
                    string[] columns = line.Split(',');

                    if (ParseFloat(columns[1]) > 0 &&
                    ParseFloat(columns[2]) > 0 &&
                    ParseFloat(columns[3]) > 0 &&
                    ParseFloat(columns[4]) > 0 &&
                    ParseFloat(columns[5]) > 0 &&
                    ParseFloat(columns[6]) > 0)
                    {
                        var timestamp = DateTime.ParseExact(columns[0], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                        var existing = await _db.LastMeasurements
                            .FirstOrDefaultAsync(x => x.codId == erp.CodId );

                        var measurement = new LastAuxiliary12_000_0
                        {
                            codId = erp.CodId,
                            timestamp = timestamp,
                            pressureInputLowLimit = ParseFloat(columns[1]),
                            pressureInputHighLimit = ParseFloat(columns[2]),
                            pressureInput = ParseFloat(columns[3]),
                            pressureOutputLowLimit = ParseFloat(columns[4]),
                            pressureOutputHighLimit = ParseFloat(columns[5]),
                            pressureOutput = ParseFloat(columns[6]),
                            shutoffZASL = new List<float?> { ParseFloat(columns[7]), ParseFloat(columns[8]) },
                            flow = ParseFloat(columns[9]),
                            PDT = new List<float?> { ParseFloat(columns[10]), ParseFloat(columns[11]) },
                            regulator = new List<float?> { ParseFloat(columns[12]), ParseFloat(columns[13]) },
                            status = 0,
                            BatteryReg = new List<float?>(),
                            type = 1
                        };

                        if (existing != null)
                        {
                            // Atualiza os campos
                            _db.Entry(existing).CurrentValues.SetValues(measurement);
                        }
                        else
                        {
                            _db.LastMeasurements.Add(measurement);
                        }

                    }
                }    

            }
        }
       await _db.SaveChangesAsync();

        return erps;
    }
    private float? ParseFloat(string? input)
    {
        if (input == null) return null;
        return float.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out float value)
            ? value
            : null;
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


