using Core.Settings;
namespace Core.Interfaces
{
    public interface ITenantService
    {
        public ServiceResponse<string> GetDatabaseProvider();
        public ServiceResponse<string> GetConnectionString();
        public ServiceResponse<Tenant> GetTenant();
    }
}

