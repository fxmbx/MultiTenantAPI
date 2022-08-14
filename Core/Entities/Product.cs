using System;
using Core.Contracts;

namespace Core.Entities
{
    public class Product : BaseEntity, IMustHaveTenant
    {
        public string TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rate { get; set; }

        protected Product()
        {

        }
    }


    public class ProductBuilder : Product
    {
        public ProductBuilder(string name, string description, int rate)
        {
            Rate = rate;
            Name = name;
            Description = description;
        }
    }
}

