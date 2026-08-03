using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Receptionist")]

    public class BillsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BillsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // LIST
        // =========================
        public async Task<IActionResult> Index()
        {
            var bills = await _unitOfWork.Bills.GetAllAsync();
            return View(bills);
        }

        // =========================
        // DETAILS
        // =========================
        public async Task<IActionResult> Details(int id)
        {
            var bill = await _unitOfWork.Bills.GetByIdAsync(id);

            if (bill == null)
                return NotFound();

            return View(bill);
        }

        // =========================
        // CREATE (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Patients = await _unitOfWork.Patients.GetAllAsync();
            ViewBag.Appointments = await _unitOfWork.Appointments.GetAllAsync();

            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBillViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _unitOfWork.Patients.GetAllAsync();
                ViewBag.Appointments = await _unitOfWork.Appointments.GetAllAsync();
                return View(model);
            }

            var bill = new Bill
            {
                PatientId = model.PatientId,
                Details = model.Details,
                CreatedAt = DateTime.Now,
                Total = model.Total
            };

            await _unitOfWork.Bills.AddAsync(bill);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var bill = await _unitOfWork.Bills.GetByIdAsync(id);

            if (bill == null)
                return NotFound();

            var model = new CreateBillViewModel
            {
                PatientId = bill.PatientId,
                Total = bill.Total,
                Details = bill.Details
            };

            ViewBag.Patients = await _unitOfWork.Patients.GetAllAsync();

            return View(model);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateBillViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Patients = await _unitOfWork.Patients.GetAllAsync();
                return View(model);
            }

            var bill = await _unitOfWork.Bills.GetByIdAsync(id);

            if (bill == null)
                return NotFound();

            bill.PatientId = model.PatientId;
            bill.Total = model.Total;
            bill.Details = model.Details;

            _unitOfWork.Bills.Update(bill);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var bill = await _unitOfWork.Bills.GetByIdAsync(id);

            if (bill == null)
                return NotFound();

            _unitOfWork.Bills.Delete(bill);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // GENERATE BILL FROM APPOINTMENT 🔥 (IMPORTANT)
        // =========================
        public async Task<IActionResult> GenerateFromAppointment(int appointmentId)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);

            if (appointment == null)
                return NotFound();

            var bill = new Bill
            {
                PatientId = appointment.PatientId,
                Details = $"Consultation for Doctor ID: {appointment.DoctorId}",
                Total = 200, // static fee (can be dynamic later)
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Bills.AddAsync(bill);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));

        }

    }
}