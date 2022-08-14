using System;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductService
    {
        public Task<ServiceResponse<Product>> CreateAsync(string name, string description, int rate);
        public Task<ServiceResponse<Product>> GetByIdAsync(int Id);
        public Task<ServiceResponse<IReadOnlyList<Product>>> GetAllASync(); 

    }
}

