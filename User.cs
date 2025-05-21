using System;

public class User
{
    public string Email { get; }
    public string Password { get; }

    public User(string email, string password)
    {
        Email = email;
        Password = password;
    }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}

public interface IEmailValidator
{
    bool ValidateEmail(string email);
}

public interface IEmailSender
{
    bool SendEmail(MailMessage message);
}

public class EmailValidator : IEmailValidator
{
    public bool ValidateEmail(string email)
    {
        return email.Contains("@"); 
    }
}

public class EmailSender : IEmailSender
{
    private readonly SmtpClient _smtpClient;

    public EmailSender(SmtpClient smtpClient)
    {
        _smtpClient = smtpClient;
    }

    public bool SendEmail(MailMessage message)
    {
        _smtpClient.Send(message);
        return true;
    }
}

public class UserService
{
    private readonly IEmailValidator _emailValidator;
    private readonly IEmailSender _emailSender;

    public UserService(IEmailValidator emailValidator, IEmailSender emailSender)
    {
        _emailValidator = emailValidator;
        _emailSender = emailSender;
    }

    public void Register(string email, string password)
    {
        if (!_emailValidator.ValidateEmail(email))
            throw new ValidationException("Email is not valid");

        var user = new User(email, password);

        _emailSender.SendEmail(new MailMessage("mysite@nowhere.com", email) { Subject = "Welcome!" });
    }
}

public class MailMessage
{
    public string From { get; }
    public string To { get; }
    public string Subject { get; set; }

    public MailMessage(string from, string to)
    {
        From = from;
        To = to;
    }
}

public class SmtpClient
{
    public void Send(MailMessage message)
    {
        Console.WriteLine($"Sending email to {message.To} with subject: {message.Subject}");
    }
}