using System;
using Core;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext dbContext;


        public ProductService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<ServiceResponse<Product>> CreateAsync(string name, string description, int rate)
        {
            var response = new ServiceResponse<Product>();
            try
            {
                var product = new ProductBuilder(name, description, rate);
                dbContext.products.Add(product);
                await dbContext.SaveChangesAsync();
                response.Data = product;

            }catch(Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<ServiceResponse<IReadOnlyList<Product>>> GetAllASync()
        {
            var response = new ServiceResponse<IReadOnlyList<Product>>();
            try
            {
                var products = await dbContext.products.ToListAsync();
                response.Data = products;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<ServiceResponse<Product>> GetByIdAsync(int Id)
        {
            var response = new ServiceResponse<Product>();
            try
            {
                var productById = await dbContext.products.FirstOrDefaultAsync(x => x.Id == Id);
                response.Data = productById;

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

