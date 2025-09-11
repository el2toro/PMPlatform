using System.Text.Json;
using Tenant.API.Dtos;

namespace Tenant.API.Services;

public class AuthServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    public AuthServiceClient(HttpClient httpClient)
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
}
