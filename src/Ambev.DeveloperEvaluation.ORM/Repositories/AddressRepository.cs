using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    internal class AddressRepository : IAddressRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of UserRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public AddressRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Address> CreateAsync(Address model, CancellationToken cancellationToken = default)
        {
            await _context.Address.AddAsync(model, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return model;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var model = await GetByIdAsync(id, cancellationToken);
            if (model == null)
                return false;

            _context.Address.Remove(model);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Address?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Address.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<List<Address>> GetListAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Address.ToListAsync(cancellationToken);
        }

        public async Task<Address> UpdateAsync(Address model, CancellationToken cancellationToken = default)
        {
            _context.Update(model);
            await _context.SaveChangesAsync(cancellationToken);
            return model;
        }
    }
}
