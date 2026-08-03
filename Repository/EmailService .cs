using HospitalManagementSystem.Repository.Base;
public class EmailService : IEmailService
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        // مؤقتًا (Console)
        Console.WriteLine("To: " + email);
        Console.WriteLine("Subject: " + subject);
        Console.WriteLine("Message: " + message);

        return Task.CompletedTask;
    }
}