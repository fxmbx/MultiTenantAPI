using System;
namespace Core.Contracts
{
    //interface that every entity must implement
    public interface IMustHaveTenant
    {
        public string TenantId { get; set; }
    }
}

