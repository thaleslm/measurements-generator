using CsvHelper;
using CsvHelper.Configuration;
using measurement_generator.Models.csv;
using measurement_generator.Models.Erp;
using measurement_generator.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;

namespace measurement_generator.Services;
public class CSVReaderService
{
    private readonly AppDBContext _db;
    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HeaderValidated = null, // Desliga a validação de headers
        MissingFieldFound = null, // Desliga erro de campo faltando
    };

    public CSVReaderService(AppDBContext db)
    {
        _db = db;

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
                Active = true, // ou false dependendo da lógica
                ScopeId = registro.ScopeId,
                Status = 1,
                AssociatedGroups = new List<int>(), // ou o que precisar
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


    public async Task<List<Erp>> getAllErp()
    {
        var erps = await _db.Erps.ToListAsync();
        return erps;
    }

    public async Task<object> VerifyWhatErpHaveCSV()
    {

        // 1. Lê todos os arquivos da pasta
        var filePaths = Directory.GetFiles("C:\\Users\\woami\\Desktop\\measurements-comgas", "*.csv");

        // 2. Extrai o nome do arquivo sem a extensão e normaliza (ex: "Erp1.csv" -> "erp1")
        var fileDnsList = filePaths
            .Select(path => Path.GetFileNameWithoutExtension(path).Trim().ToLower())
            .ToHashSet();

        // 3. Pega os DnsIot dos ERPs no banco e normaliza também
        var erps = await _db.Erps
            .Where(e => !string.IsNullOrEmpty(e.DnsIot)) // Pega o DnsIot, CodId e o atributo haveFile
            .ToListAsync();

        // 4. Atualiza o atributo 'haveFile' para true nos ERPs que têm arquivo correspondente
        foreach (var erp in erps)
        {
            if (fileDnsList.Contains(erp.DnsIot.Trim().ToLower()))
            {
                // Se o ERP tiver um arquivo correspondente, marca como true
                erp.haveFile = true;
            }
        }

        // 5. Salva as alterações no banco de dados
        await _db.SaveChangesAsync();

        // 6. Retorna a quantidade de ERPs com arquivo correspondente
        var erpsComArquivo = erps
            .Where(erp => erp.haveFile)
            .ToList();

        return erpsComArquivo;



    }
}


