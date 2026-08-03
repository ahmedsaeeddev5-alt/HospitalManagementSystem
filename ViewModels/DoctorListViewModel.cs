namespace HospitalManagementSystem.ViewModels
{
    public class DoctorListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
    }
}