using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    internal class ProductRepository(DefaultContext _context, CacheContext _cacheContext, IMapper _mapper) : IProductRepository
    {
        private const string PRODUCT_KEY = "products";
        public async Task<Product> CreateAsync(Product model, CancellationToken cancellationToken = default)
        {
            model.CreatedAtDate();
            await _context.Product.AddAsync(model, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var model = await GetByIdAsync(id, cancellationToken);
            if (model == null)
                return false;

            _context.Product.Remove(model);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Product.Include(o => o.Category).FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<List<Product>> GetListAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var cached = await _cacheContext.GetAsync<List<ProductDto>>(PRODUCT_KEY);
                if (cached is not null)
                {
                    var produclist = _mapper.Map<List<Product>>(cached);
                    return produclist;
                }

                var products = await _context.Product.Include(o => o.Category).ToListAsync(cancellationToken);
                // salvando em cache o DTO e depois usando auto mapper, por que essa abordagem? Estou usando Entity com propriedades privadas e a Deserialização do Json não
                // consegue setar um valor e uma propriedade privada, então se fez necessario adicionar um DTO.

                var productDTO = _mapper.Map<List<ProductDto>>(products);
                await _cacheContext.SetAsync(PRODUCT_KEY, productDTO);

                return products;
            }
            catch (Exception ex)
            {
                var message = ex.InnerException != null ? $"{ex.Message} -> {ex.InnerException.Message}" : ex.Message;
                throw new InvalidDataException(message);
            }
}

        public async Task<Product> UpdateAsync(Product model, CancellationToken cancellationToken = default)
        {
            _context.Update(model);
            await _context.SaveChangesAsync(cancellationToken);
            return model;
        }
    }
}
