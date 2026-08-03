using HospitalManagement.Interfaces;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Repository.Base;
using System;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;

        public IRepository<Patient> Patients { get; private set; }
        public IRepository<Doctor> Doctors { get; private set; }
        public IDepartmentRepository Departments { get; }
        public IRepository<Appointment> Appointments { get; private set; }
        public IRepository<Bill> Bills { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Patients = new Repository<Patient>(_context);
            Doctors = new Repository<Doctor>(_context);
            Departments = new DepartmentRepository(_context);
            Appointments = new Repository<Appointment>(_context);
            Bills = new Repository<Bill>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}