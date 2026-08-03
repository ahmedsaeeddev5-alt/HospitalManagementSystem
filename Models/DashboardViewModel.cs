namespace HospitalManagementSystem.Models
{
    public class DashboardViewModel
    {
        public int PatientsCount { get; set; }
        public int DoctorsCount { get; set; }
        public int AppointmentsCount { get; set; }
        public int BillsCount { get; set; }
        public decimal CountRevenue { get; set; }
        public int PendingAppointments { get; set; }
        public int ApprovedAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int CancelledAppointments { get; set; }

    }
}
