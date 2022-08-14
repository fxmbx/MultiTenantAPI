using System;
namespace Core.Settings
{
    public class TenantSettings
    {
        public Configuration Default { get; set; }
        public List<Tenant> Tenants { get; set; }
    }
}

