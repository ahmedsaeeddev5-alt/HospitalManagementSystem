using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllWithDoctorsAsync()
        {
            return await _context.Departments
                .Include(d => d.Doctors)
                .ToListAsync();
        }

    }
}
