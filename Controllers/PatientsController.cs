using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Receptionist,Doctor,Patient")]
    public class PatientsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // INDEX
        // =========================
        public async Task<IActionResult> Index()
        {
            var patients = await _unitOfWork.Patients.GetAllAsync();
            return View(patients);
        }

        // =========================
        // DETAILS
        // =========================
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // =========================
        // CREATE (GET)
        // =========================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Upload Image
            if (model.ClientFile != null && model.ClientFile.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(model.ClientFile.FileName);

                string folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/patients"
                );

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.ClientFile.CopyToAsync(stream);
                }

                model.ImagePath = "/images/patients/" + fileName;
            }

            var patient = new Patient
            {
                Name = model.Name,
                Age = model.Age,
                Phone = model.Phone,
                MedicalHistory = model.MedicalHistory,
                ImagePath = model.ImagePath
            };

            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var patient = await _unitOfWork.Patients.GetByIdAsync(id);

            if (patient == null)
                return NotFound();

            patient.Name = model.Name;
            patient.Age = model.Age;
            patient.Phone = model.Phone;
            patient.MedicalHistory = model.MedicalHistory;

            // Upload new image
            if (model.ClientFile != null && model.ClientFile.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(model.ClientFile.FileName);

                string folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "patients"
                );

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.ClientFile.CopyToAsync(stream);
                }

                // Delete old image
                if (!string.IsNullOrEmpty(patient.ImagePath))
                {
                    string oldPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        patient.ImagePath.TrimStart('/')
                    );

                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                patient.ImagePath = "/images/patients/" + fileName;
            }

            _unitOfWork.Patients.Update(patient);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE (GET)
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);

            if (patient == null)
                return NotFound();

            return View(patient);
        }

        // =========================
        // DELETE (POST)
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);

            if (patient == null)
                return NotFound();

            // delete image
            if (!string.IsNullOrEmpty(patient.ImagePath))
            {
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    patient.ImagePath.TrimStart('/')
                );

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _unitOfWork.Patients.Delete(patient);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}