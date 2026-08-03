using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.ViewModels
{
    public class CreateBillViewModel
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total must be greater than 0")]
        public decimal Total { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Details { get; set; }
    }
}