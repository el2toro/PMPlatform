using Tenant.API.Dtos;

namespace Tenant.API.Services;

public class AuthServiceClient
{
    private readonly HttpClient _httpClient;
    public AuthServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserDto>> GetUsersByTenantIdAsync(IEnumerable<Guid> userIds)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/users/tenant", new { userIds });
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<UserDto>>()
               ?? new List<UserDto>();
    }
}
