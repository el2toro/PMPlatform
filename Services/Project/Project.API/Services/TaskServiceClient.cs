using System.Text.Json;

namespace Project.API.Services;

public class TaskServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    public TaskServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<ProjectProgressDto> GetProgresData(Guid tenantId, Guid projectId)
    {
        var data = await _httpClient.GetStringAsync($"tenants/{tenantId}/projects/{projectId}/tasks/progress");

        var deserializedData = JsonSerializer.Deserialize<ProjectProgressDto>(data, _jsonSerializerOptions);

        return deserializedData!;
    }
}
