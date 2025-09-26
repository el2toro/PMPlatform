namespace Auth.API.Services;

public class TenantServiceClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<Tenant> CreateTenant(Tenant tenant)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/tenants")
        {
            Content = JsonContent.Create(new Tenant
            {
                Name = tenant.Name,
                OwnerId = tenant.OwnerId,
                Description = tenant.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            })
        };

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Tenant>() ?? new Tenant();
    }

    public async Task<Tenant> GetTenantById(Guid tenantId)
    {
        var response = await _httpClient.GetAsync($"api/tenants/{tenantId}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Tenant>() ?? new Tenant();
    }

    public async Task<Tenant?> GetTenantByName(string tenantName)
    {
        var response = await _httpClient.GetAsync($"api/tenants/{tenantName}");
        response.EnsureSuccessStatusCode();

        if (response.Content == null || response.Content.Headers.ContentLength == 0)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<Tenant>(); ;
    }

    public async Task<IEnumerable<Tenant>> GetAllTenants()
    {
        var response = await _httpClient.GetAsync("api/tenants");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IEnumerable<Tenant>>() ?? Enumerable.Empty<Tenant>();
    }
}
