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
    internal class CartRepository : ICartRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of UserRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public CartRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Cart> CreateAsync(Cart model, CancellationToken cancellationToken = default)
        {
            model.CreatedAtDate();
            model.DateSaveCart(); // DATE CART
            await _context.Cart.AddAsync(model, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var model = await GetByIdAsync(id, cancellationToken);
            if (model == null)
                return false;

            _context.Cart.Remove(model);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Cart
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<List<Cart>> GetListAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Cart
                .Include(c => c.CartItems)
                .ToListAsync(cancellationToken);
        }

        public async Task<Cart> UpdateAsync(Cart model, CancellationToken cancellationToken = default)
        {
            _context.Update(model);
            await _context.SaveChangesAsync(cancellationToken);
            return model;
        }
    }
}
