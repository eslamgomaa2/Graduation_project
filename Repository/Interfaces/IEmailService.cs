namespace OA.Service.Contract
{
    public interface IEmailService
    {
        Task SendEmailAsync(string mailTo, string subject, string body);

    }
}
