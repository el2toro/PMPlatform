namespace Auth.API.Services;

public class TenantServiceClient
{
    private readonly HttpClient _httpClient;

    public TenantServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

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

        // Optionally, handle the response content if needed
        return await response.Content.ReadFromJsonAsync<Tenant>() ?? new Tenant();
    }
}
