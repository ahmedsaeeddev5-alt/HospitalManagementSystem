using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // DASHBOARD HOME
        // =========================
        public async Task<IActionResult> Index()
        {
            var patients = await _unitOfWork.Patients.GetAllAsync();
            var doctors = await _unitOfWork.Doctors.GetAllAsync();
            var appointments = await _unitOfWork.Appointments.GetAllAsync();
            var bills = await _unitOfWork.Bills.GetAllAsync();

            var model = new DashboardViewModel
            {
                PatientsCount = patients.Count(),
                DoctorsCount = doctors.Count(),
                AppointmentsCount = appointments.Count(),
                BillsCount = bills.Count(),
                CountRevenue = bills.Sum(x => x.Total)
            };

            return View(model);
        }
    }
}