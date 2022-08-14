using System;
using Core;
using Core.Interfaces;
using Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings tenantSettings;
        private HttpContext httpContext;
        private Tenant currentTenant;
        public TenantService(IOptions<TenantSettings> tenantSettings, IHttpContextAccessor contextAccessor)
        {
            this.tenantSettings = tenantSettings.Value;
            httpContext = contextAccessor.HttpContext;

            if (httpContext != null)
            {
                if(httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetTenant(tenantId);
                }
            }
        }

        public void SetTenant(string id)
        {
            currentTenant = tenantSettings.Tenants.Where(a => a.TID == id).FirstOrDefault();
            if (currentTenant == null) throw new Exception("Invalid Tenant");
            if (string.IsNullOrEmpty(currentTenant.ConnectionString))
            {
                SetDefaultConnectionStringToCurrentTenant();
            }
        }
        public void SetDefaultConnectionStringToCurrentTenant()
        {
            currentTenant.ConnectionString = tenantSettings.Default.ConnectionString;
        }
        public ServiceResponse<string> GetConnectionString()
        {
            var response = new ServiceResponse<string>();
            try {
                response.Data = currentTenant?.ConnectionString;
                response.Message = "Thie connection string is";
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
      
            return response;
        }

        public ServiceResponse<string> GetDatabaseProvider()
        {
            var response = new ServiceResponse<string>();
            try
            {
                response.Data = tenantSettings.Default?.DBProvider;
                response.Message = "This is the Database Provider";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public ServiceResponse<Tenant> GetTenant()
        {
            var response = new ServiceResponse<Tenant>();
            try
            {
                response.Data = currentTenant;
                response.Message = "This is the current tenane";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }
    }
}

