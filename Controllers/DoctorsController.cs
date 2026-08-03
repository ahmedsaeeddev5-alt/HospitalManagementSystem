using HospitalManagementSystem.Interfaces;
using HospitalManagementSystem.Models;
using HospitalManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Receptionist,Doctor")]
    public class DoctorsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // INDEX
        // =========================
        public async Task<IActionResult> Index()
        {
            var doctors = await _unitOfWork.Doctors.GetAllAsync();
            return View(doctors);
        }

        // =========================
        // DETAILS
        // =========================
        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }

        // =========================
        // CREATE (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();

            ViewBag.Departments = new SelectList(departments, "Id", "Name");

            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDoctorViewModel model, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await _unitOfWork.Departments.GetAllAsync();
                return View(model);
            }

            string imagePath = null;

            // Upload Image
            if (file != null && file.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                string folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/doctors"
                );

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                imagePath = "/images/doctors/" + fileName;
            }

            var doctor = new Doctor
            {
                Name = model.Name,
                Specialty = model.Specialty,
                DepartmentId = model.DepartmentId,
                ImagePath = imagePath
            };

            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);

            if (doctor == null)
                return NotFound();

            ViewBag.Departments = await _unitOfWork.Departments.GetAllAsync();

            var model = new CreateDoctorViewModel
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialty = doctor.Specialty,
                DepartmentId = doctor.DepartmentId,
                ImagePath = doctor.ImagePath
            };

            return View(model);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateDoctorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = await _unitOfWork.Departments.GetAllAsync();
                return View(model);
            }

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(model.Id);

            if (doctor == null)
                return NotFound();

            doctor.Name = model.Name;
            doctor.Specialty = model.Specialty;
            doctor.DepartmentId = model.DepartmentId;

            // Upload new image
            if (model.File != null && model.File.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(model.File.FileName);

                string folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/doctors"
                );

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                // delete old image
                if (!string.IsNullOrEmpty(doctor.ImagePath))
                {
                    var oldPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        doctor.ImagePath.TrimStart('/')
                    );

                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                doctor.ImagePath = "/images/doctors/" + fileName;
            }

            _unitOfWork.Doctors.Update(doctor);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE (GET)
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);

            if (doctor == null)
                return NotFound();

            return View(doctor);
        }

        // =========================
        // DELETE (POST)
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);

            if (doctor == null)
                return NotFound();

            // delete image
            if (!string.IsNullOrEmpty(doctor.ImagePath))
            {
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    doctor.ImagePath.TrimStart('/')
                );

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _unitOfWork.Doctors.Delete(doctor);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}