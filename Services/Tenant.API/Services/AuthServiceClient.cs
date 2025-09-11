using System.Text.Json;
using Tenant.API.Dtos;
using Tenant.API.Enum;
using Tenant.API.Models;

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

    public async Task AddUserToTenant(Guid tenantId, Guid userId, TenantRole tenantRole)
    {
        var response = await _httpClient
            .PostAsJsonAsync($"/api/users/{userId}/tenants/{tenantId}", new UserTenant
            {
                TenantId = tenantId,
                UserId = userId,
                Role = tenantRole
            });

        response.EnsureSuccessStatusCode();

        // Optionally, you can read the response content if needed
        var data = await response.Content.ReadAsStringAsync();
    }

    public async Task RemoveUserFromTenant(Guid tenantId, Guid userId)
    {
        var response = await _httpClient
            .DeleteAsync($"/api/users/{userId}/tenants/{tenantId}");

        response.EnsureSuccessStatusCode();

        // Optionally, you can read the response content if needed
        var data = await response.Content.ReadAsStringAsync();
    }
}
