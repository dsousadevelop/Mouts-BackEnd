using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    internal class CartItemRepository(DefaultContext _context) : ICartItemRepository
    {
        public async Task<CartItem> CreateAsync(CartItem model, CancellationToken cancellationToken = default)
        {
            await _context.CartItem.AddAsync(model, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var model = await GetByIdAsync(id, cancellationToken);
            if (model == null)
                return false;

            _context.CartItem.Remove(model);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<CartItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.CartItem.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<List<CartItem>> GetListAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.CartItem.ToListAsync(cancellationToken);
        }

        public async Task<List<CartItem>> GetListAllAsync(int idCart, CancellationToken cancellationToken = default)
        {
            return await _context.CartItem.Where(c => c.CartId.Equals(idCart)).ToListAsync(cancellationToken);
        }

        public async Task<CartItem> UpdateAsync(CartItem model, CancellationToken cancellationToken = default)
        {
            _context.Update(model);
            await _context.SaveChangesAsync(cancellationToken);
            return model;
        }
    }
}
