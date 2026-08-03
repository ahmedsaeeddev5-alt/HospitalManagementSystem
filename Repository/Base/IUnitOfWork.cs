using HospitalManagement.Interfaces;
using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.Repository.Base;
using System;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Patient> Patients { get; }
        IRepository<Doctor> Doctors { get; }
        IDepartmentRepository Departments { get; }
        IRepository<Appointment> Appointments { get; }
        IRepository<Bill> Bills { get; }

        Task<int> CompleteAsync();
    }
}