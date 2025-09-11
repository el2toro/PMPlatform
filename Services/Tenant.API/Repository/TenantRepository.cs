using Microsoft.EntityFrameworkCore;
using Tenant.API.Data;
using Tenant.API.Dtos;
using Tenant.API.Enum;
using Tenant.API.Services;

namespace Tenant.API.Repository;

public interface ITenantRepository
{
    Task CreateTenant(string name, string description, Guid ownerId, CancellationToken cancellationToken);
    Task<Models.Tenant> GetTenantById(Guid tenantId, CancellationToken cancellationToken);
    Task<IEnumerable<UserDto>> GetTenantUsers(Guid tenantId);
    Task DeleteTenant(Guid tenantId, CancellationToken cancellationToken);
}

public class TenantRepository : ITenantRepository
{
    private readonly TenantDbContext _tenantDbContext;
    private readonly AuthServiceClient _authServiceClient;
    public TenantRepository(TenantDbContext tenantDbContext, AuthServiceClient authServiceClient)
    {
        _tenantDbContext = tenantDbContext;
        _authServiceClient = authServiceClient;
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

    public async Task DeleteTenant(Guid tenantId, CancellationToken cancellationToken)
    {
        var tenant = _tenantDbContext.Tenants.FirstOrDefault(t => t.Id == tenantId);

        ArgumentNullException.ThrowIfNull(tenant);

        _tenantDbContext.Tenants.Remove(tenant);
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
        return await _authServiceClient.GetUsersByTenantIdAsync(tenantId);
    }
}
