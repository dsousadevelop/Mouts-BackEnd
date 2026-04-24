using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    internal class CategoryRepository(DefaultContext _context, CacheContext _cacheContext, IMapper _mapper) : ICategoryRepository
    {
        private const string CACHE_KEY = "category_cache";
        public async Task<Category> CreateAsync(Category model, CancellationToken cancellationToken = default)
        {
            model.CreatedAtDate();
            await _context.Category.AddAsync(model, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var model = await GetByIdAsync(id, cancellationToken);
            if (model == null)
                return false;

            _context.Category.Remove(model);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public Task<Category?> GetByDescriptionAsync(string description, CancellationToken cancellationToken = default)
        {
            return _context.Category.FirstOrDefaultAsync(o => o.Description.ToUpper().Trim() == description.ToUpper().Trim(), cancellationToken);
        }

        public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Category.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<List<Category>> GetListAllAsync(CancellationToken cancellationToken = default)
        {
            var cached = await _cacheContext.GetAsync<List<CategoryDto>>(CACHE_KEY);
            if (cached is not null)
            {
                var produclist = _mapper.Map<List<Category>>(cached);
                return produclist;
            }

            var entityList = await _context.Category.ToListAsync(cancellationToken);
            // salvando em cache o DTO e depois usando auto mapper, por que essa abordagem? Estou usando Entity com propriedades privadas e a Deserialização do Json não
            // consegue setar um valor e uma propriedade privada, então se fez necessario adicionar um DTO.

            var entityListtDTO = _mapper.Map<List<CategoryDto>>(entityList);
            await _cacheContext.SetAsync(CACHE_KEY, entityListtDTO);
            return entityList;
            
        }

        public async Task<Category> UpdateAsync(Category model, CancellationToken cancellationToken = default)
        {
            model.UpdateAtDate();
            _context.Update(model);
            await _context.SaveChangesAsync(cancellationToken);
            return model;
        }
    }
}
