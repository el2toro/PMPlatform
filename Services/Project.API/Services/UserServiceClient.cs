using Project.API.Dtos;
using System.Text.Json;

namespace Project.API.Services;

public class UserServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    public UserServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
    public async Task<List<UserDto>> GetUsersByTenantIdAsync(Guid tenantId)
    {
        //TODO: move api route to appsettings
        try
        {
            var response = await _httpClient.GetStringAsync($"/api/users/tenant/{tenantId}");

            var users = JsonSerializer
                .Deserialize<List<UserDto>>(response, _jsonSerializerOptions) ?? [];

            return users;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<UserDto>> GetUsersByIdAsync(Guid tenantId, Guid[] userIds)
    {
        //TODO: move api route to appsettings
        try
        {
            var response = await _httpClient.GetStringAsync($"/api/users/tenant/{tenantId}?userIds={string.Join("&userIds=", userIds.Select(id => id.ToString()))}");

            var users = JsonSerializer
                .Deserialize<List<UserDto>>(response, _jsonSerializerOptions) ?? [];

            return users;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
