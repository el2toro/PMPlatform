using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Tenant.API.Data;
using Tenant.API.Dtos;

namespace Tenant.API.Repository;

public interface ITenantRepository
{
    Task CreateTenant(string name, string description, Guid ownerId, CancellationToken cancellationToken);
    Task<Models.Tenant> GetTenantById(Guid tenantId, CancellationToken cancellationToken);
    Task<IEnumerable<UserDto>> GetTenantUsers(Guid tenantId);
}

public class TenantRepository : ITenantRepository
{
    private readonly TenantDbContext _tenantDbContext;
    private readonly HttpClient _httpClient;
    public TenantRepository(TenantDbContext tenantDbContext, HttpClient httpClient)
    {
        _tenantDbContext = tenantDbContext;
        _httpClient = httpClient;
    }
    public async Task CreateTenant(string name, string description, Guid ownerId, CancellationToken cancellationToken)
    {
        _tenantDbContext.Tenants.Add(new Models.Tenant
        {
            Name = name,
            Description = description,
            OwnerId = ownerId
        });

        await _tenantDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Models.Tenant> GetTenantById(Guid tenantId, CancellationToken cancellationToken)
    {
        var tenant = await _tenantDbContext.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId);

        ArgumentNullException.ThrowIfNull(tenant);

        return tenant;
    }

    public async Task<IEnumerable<UserDto>> GetTenantUsers(Guid tenantId)
    {
        _httpClient.BaseAddress = new Uri("https://localhost:7194/");
        var users = await _httpClient.GetStringAsync($"api/users/tenant/0ADDB2FC-F72E-4571-A71F-B1F1DA6BF0E9");


        return new List<UserDto>();// await JsonSerializer.Deserialize<IEnumerable<UserDto>>() ?? Enumerable.Empty<UserDto>();
    }
}
