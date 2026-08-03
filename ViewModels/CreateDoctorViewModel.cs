using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.ViewModels
{
    public class CreateDoctorViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Specialty { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

        public string? ImagePath { get; set; }

        public IFormFile? File { get; set; }

    }
}