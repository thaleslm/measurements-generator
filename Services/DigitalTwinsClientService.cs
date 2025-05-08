using measurement_generator.Models.csv;
using measurement_generator.Models.Request;  // Certifique-se de incluir o namespace

namespace measurement_generator.Services;

public class DigitalTwinsClientService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public DigitalTwinsClientService(HttpClient httpClient, IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task PutMeasurements(List<MeasurementsDTO> erps)
    {
        try
        {
            foreach (var item in erps)
            {
            Console.WriteLine(item);
                
            }
            var response = await _httpClient.PostAsJsonAsync($"{_configuration.GetValue<string>("ApiDigitalTwins")}/api/V1/12000/0/Measurement", erps);
            // Exibe o corpo da resposta, se necessário
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro -----------------------------------------------------------------------: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro -----------------------------------------------------------------------: {ex.Message}", ex);
        }
    }


}

