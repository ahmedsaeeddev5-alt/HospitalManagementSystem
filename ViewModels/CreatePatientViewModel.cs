using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.ViewModels
{
    public class CreatePatientViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 150)]
        public int Age { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        public string? MedicalHistory { get; set; }
        public string? ImagePath { get; set; }
      
    }
}