using HospitalManagement.Interfaces;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Repository.Base
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<IEnumerable<Department>> GetAllWithDoctorsAsync();
    }
}
