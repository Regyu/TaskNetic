using Azure.Communication.Email;
using Azure;
using Microsoft.AspNetCore.Identity.UI.Services;
using TaskNetic.Data;

namespace TaskNetic.Components.Account
{
    public interface IEmailSender
    {
        Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink);
        Task SendPasswordResetLinkAsync(string email, string resetLink);
    }

    public class AzureEmailSender : IEmailSender
    {
        private readonly EmailClient _emailClient;
        private readonly string _senderAddress;
        private readonly ILogger<AzureEmailSender> _logger;

        public AzureEmailSender(IConfiguration configuration, ILogger<AzureEmailSender> logger)
        {
            var connectionString = configuration.GetConnectionString("AzureCommunication");
            _senderAddress = "DoNotReply@ebf1f529-6ee2-4c8e-8c11-e32978833c65.azurecomm.net";
            _emailClient = new EmailClient(connectionString);
            _logger = logger;
        }

        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            var emailContent = new EmailContent("Confirm your email")
            {
                Html = GetConfirmationEmailTemplate(confirmationLink)
            };

            var emailMessage = new EmailMessage(
                senderAddress: _senderAddress,
                recipientAddress: email,
                content: emailContent
            );

            try
            {
                var operation = await _emailClient.SendAsync(
                    WaitUntil.Started,
                    emailMessage
                );
                _logger.LogInformation("Confirmation email sent to {Email}. Operation ID: {OperationId}",
                    email, operation.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send confirmation email to {Email}", email);
                throw;
            }
        }

        public async Task SendPasswordResetLinkAsync(string email, string resetLink)
        {
            var emailContent = new EmailContent("Reset your password")
            {
                Html = GetPasswordResetTemplate(resetLink)
            };

            var emailMessage = new EmailMessage(
                senderAddress: _senderAddress,
                recipientAddress: email,
                content: emailContent
            );

            try
            {
                var operation = await _emailClient.SendAsync(
                    WaitUntil.Started,
                    emailMessage
                );
                _logger.LogInformation("Password reset email sent to {Email}. Operation ID: {OperationId}",
                    email, operation.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to {Email}", email);
                throw;
            }
        }

        private string GetConfirmationEmailTemplate(string confirmationLink) =>
            $@"<html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2>Welcome to TaskNetic!</h2>
                    <p>Please confirm your email address by clicking the button below:</p>
                    <p style='text-align: center;'>
                        <a href='{confirmationLink}' 
                           style='background-color: #007bff; color: white; padding: 10px 20px; 
                                  text-decoration: none; border-radius: 5px; display: inline-block;'>
                            Confirm Email Address
                        </a>
                    </p>
                    <p>If you didn't create this account, you can ignore this email.</p>
                </div>
            </body>
        </html>";
        private string GetPasswordResetTemplate(string resetLink) =>
        $@"<html>
            <body style='font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f4f4f4;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #ffffff; 
                          border-radius: 8px; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);'>
                    <h2 style='color: #333333; text-align: center;'>Password Reset Request</h2>
                    <p style='color: #666666; font-size: 16px; line-height: 1.5;'>
                        We received a request to reset your password for your TaskNetic account. 
                        To reset your password, click the button below:
                    </p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' 
                           style='background-color: #dc3545; color: white; padding: 12px 24px; 
                                  text-decoration: none; border-radius: 5px; display: inline-block;
                                  font-weight: bold;'>
                            Reset Password
                        </a>
                    </div>
                    <p style='color: #666666; font-size: 14px; line-height: 1.5;'>
                        This password reset link will expire in 24 hours. If you didn't request a password reset, 
                        please ignore this email or contact support if you have concerns.
                    </p>
                    <div style='background-color: #fff3cd; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <p style='color: #856404; margin: 0; font-size: 14px;'>
                            <strong>Security Tip:</strong> Never share your password or this reset link with anyone.
                        </p>
                    </div>
                    <hr style='border: none; border-top: 1px solid #eeeeee; margin: 20px 0;'>
                    <p style='color: #999999; font-size: 12px; text-align: center;'>
                        This is an automated message from TaskNetic. Please do not reply to this email.
                    </p>
                </div>
            </body>
        </html>";
    }
}

