using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Doctor,Receptionist")]

    public class AppointmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // LIST
        // =========================
        public async Task<IActionResult> Index()
        {
            var appointments = await _unitOfWork.Appointments.GetAllAsync();
            return View(appointments);
        }

        // =========================
        // DETAILS
        // =========================
        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        // =========================
        // CREATE (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Patients = await _unitOfWork.Patients.GetAllAsync();
            ViewBag.Doctors = await _unitOfWork.Doctors.GetAllAsync();

            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _unitOfWork.Patients.GetAllAsync();
                ViewBag.Doctors = await _unitOfWork.Doctors.GetAllAsync();
                return View(model);
            }

            var appointment = new Appointment
            {
                PatientId = model.PatientId,
                DoctorId = model.DoctorId,
                Date = model.Date,
                Notes = model.Notes,
                Status = "Pending"
            };

            await _unitOfWork.Appointments.AddAsync(appointment);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);

            if (appointment == null)
                return NotFound();

            ViewBag.Patients = await _unitOfWork.Patients.GetAllAsync();
            ViewBag.Doctors = await _unitOfWork.Doctors.GetAllAsync();

            var model = new CreateAppointmentViewModel
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                Date = appointment.Date,
                Notes = appointment.Notes
            };

            return View(model);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _unitOfWork.Patients.GetAllAsync();
                ViewBag.Doctors = await _unitOfWork.Doctors.GetAllAsync();
                return View(model);
            }

            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);

            if (appointment == null)
                return NotFound();

            appointment.PatientId = model.PatientId;
            appointment.DoctorId = model.DoctorId;
            appointment.Date = model.Date;
            appointment.Notes = model.Notes;

            _unitOfWork.Appointments.Update(appointment);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);

            if (appointment == null)
                return NotFound();

            _unitOfWork.Appointments.Delete(appointment);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // CHANGE STATUS (BONUS 🔥)
        // =========================
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);

            if (appointment == null)
                return NotFound();

            appointment.Status = status;

            _unitOfWork.Appointments.Update(appointment);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}