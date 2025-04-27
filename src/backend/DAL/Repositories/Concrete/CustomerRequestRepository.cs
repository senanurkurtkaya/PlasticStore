using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Concrete
{
    public class CustomerRequestRepository : ICustomerRequestRepository
    {
        private readonly AppDbContext _db;

        public CustomerRequestRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<CustomerRequest>> GetAllCustomerRequestsAsync()
        {
            return await _db.CustomerRequests.ToListAsync();
        }
    }
}
