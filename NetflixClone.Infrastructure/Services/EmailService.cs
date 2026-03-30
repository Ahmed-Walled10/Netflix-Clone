using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using NetflixClone.Application.Contracts.Infrasructure;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using NetflixClone.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace NetflixClone.Infrastructure.Mail;

public class EmailService : IEmailService
{
    private readonly EmailOptions _options;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailOptions> options, ILogger<EmailService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendEmailConfirmationOtpAsync(string email, string firstName, string otp)
    {
        var subject = "Confirm Your Email - Netflix";
        var body = $@"
            <!DOCTYPE html>
         <html lang=""en"">
         <head>
           <meta charset=""UTF-8"" />
           <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
           <title>Verify Your Email</title>
           <style>
             body {{ margin: 0; padding: 0; background: #000000; font-family: Arial, sans-serif; }}
             .wrap {{ max-width: 520px; margin: 40px auto; background: #111111; border-radius: 6px; overflow: hidden; border: 1px solid #222; }}
             .top-bar {{ background: #E50914; height: 4px; }}
             .body {{ padding: 40px 40px 32px; }}
             .logo {{ font-size: 22px; font-weight: 900; color: #E50914; letter-spacing: 1px; margin-bottom: 28px; }}
             h1 {{ font-size: 22px; color: #ffffff; margin: 0 0 12px; }}
             p {{ font-size: 15px; color: #aaaaaa; line-height: 1.6; margin: 0 0 28px; }}
             p strong {{ color: #ffffff; font-weight: 600; }}
             .otp-box {{ background: #1a1a1a; border: 1px solid #2a2a2a; border-radius: 6px; text-align: center; padding: 24px 20px; margin-bottom: 24px; }}
             .otp-box .otp {{ font-size: 38px; font-weight: 700; letter-spacing: 14px; color: #ffffff; }}
             .otp-box .expiry {{ font-size: 12px; color: #555; margin-top: 10px; }}
             .note {{ font-size: 13px; color: #555; border-top: 1px solid #222; padding-top: 20px; margin: 0; }}
             .footer {{ background: #0a0a0a; padding: 16px 40px; font-size: 11px; color: #444; border-top: 1px solid #222; }}
           </style>
         </head>
         <body>
           <div class=""wrap"">
             <div class=""top-bar""></div>
             <div class=""body"">
               <div class=""logo"">STREAMVAULT</div>
               <h1>Verify your email</h1>
               <p>Hi <strong>{firstName}</strong>, enter the code below to verify your email address and activate your account.</p>
               <div class=""otp-box"">
                 <div class=""otp"">{otp}</div>
                 <div class=""expiry"">Expires in 15 minutes · Do not share this code</div>
               </div>
               <p class=""note"">If you didn't create a Netflix account, you can safely ignore this email.</p>
             </div>
             <div class=""footer"">© 2026 Netflix, Inc. &nbsp;·&nbsp; Privacy Policy &nbsp;·&nbsp; Help Center</div>
           </div>
         </body>
         </html>
        ";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendPasswordResetOtpAsync(string email, string firstName, string otp)
    {
        var subject = "Reset Your Password - Netflix";
        var body = $@"
            <!DOCTYPE html>
         <html lang=""en"">
         <head>
           <meta charset=""UTF-8"" />
           <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
           <title>Reset Your Password</title>
           <style>
             body {{ margin: 0; padding: 0; background: #000000; font-family: Arial, sans-serif; }}
             .wrap {{ max-width: 520px; margin: 40px auto; background: #111111; border-radius: 6px; overflow: hidden; border: 1px solid #222; }}
             .top-bar {{ background: #E50914; height: 4px; }}
             .body {{ padding: 40px 40px 32px; }}
             .logo {{ font-size: 22px; font-weight: 900; color: #E50914; letter-spacing: 1px; margin-bottom: 28px; }}
             h1 {{ font-size: 22px; color: #ffffff; margin: 0 0 12px; }}
             p {{ font-size: 15px; color: #aaaaaa; line-height: 1.6; margin: 0 0 28px; }}
             p strong {{ color: #ffffff; font-weight: 600; }}
             .otp-box {{ background: #1a1a1a; border: 1px solid #2a2a2a; border-radius: 6px; text-align: center; padding: 24px 20px; margin-bottom: 24px; }}
             .otp-box .otp {{ font-size: 38px; font-weight: 700; letter-spacing: 14px; color: #ffffff; }}
             .otp-box .expiry {{ font-size: 12px; color: #555; margin-top: 10px; }}
             .note {{ font-size: 13px; color: #555; border-top: 1px solid #222; padding-top: 20px; margin: 0; }}
             .footer {{ background: #0a0a0a; padding: 16px 40px; font-size: 11px; color: #444; border-top: 1px solid #222; }}
           </style>
         </head>
         <body>
           <div class=""wrap"">
             <div class=""top-bar""></div>
             <div class=""body"">
               <div class=""logo"">STREAMVAULT</div>
               <h1>Reset your password</h1>
               <p>Hi <strong>{firstName}</strong>, we received a request to reset your password. Use the code below to continue. If this wasn't you, ignore this email — your password won't change.</p>
               <div class=""otp-box"">
                 <div class=""otp"">{otp}</div>
                 <div class=""expiry"">Expires in 15 minutes · Do not share this code</div>
               </div>
               <p class=""note"">For security, never share this code with anyone. Netflix staff will never ask for it.</p>
             </div>
             <div class=""footer"">© 2026 Netflix, Inc. &nbsp;·&nbsp; Privacy Policy &nbsp;·&nbsp; Help Center</div>
           </div>
         </body>
         </html>
        ";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendInvoiceEmailAsync(string email, string firstName, string planName, decimal amount, string cardBrand, string cardLast4, DateTime paymentDate, DateTime periodEnd)
    {
        var subject = $"Your Netflix Invoice - {planName}";
        var body = $@"
            <!DOCTYPE html>
         <html lang=""en"">
         <head>
           <meta charset=""UTF-8"" />
           <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
           <title>Your Invoice</title>
           <style>
             body {{ margin: 0; padding: 0; background: #000000; font-family: Arial, sans-serif; }}
             .wrap {{ max-width: 520px; margin: 40px auto; background: #111111; border-radius: 6px; overflow: hidden; border: 1px solid #222; }}
             .top-bar {{ background: #E50914; height: 4px; }}
             .body {{ padding: 40px 40px 32px; }}
             .logo {{ font-size: 22px; font-weight: 900; color: #E50914; letter-spacing: 1px; margin-bottom: 28px; }}
             h1 {{ font-size: 22px; color: #ffffff; margin: 0 0 12px; }}
             p {{ font-size: 15px; color: #aaaaaa; line-height: 1.6; margin: 0 0 28px; }}
             p strong {{ color: #ffffff; font-weight: 600; }}
             .invoice-box {{ background: #1a1a1a; border: 1px solid #2a2a2a; border-radius: 6px; padding: 24px 20px; margin-bottom: 24px; }}
             .invoice-box .heading {{ font-size: 14px; color: #ffffff; margin-bottom: 15px; font-weight: 600; text-transform: uppercase; }}
             .invoice-box .row {{ display: flex; justify-content: space-between; border-bottom: 1px solid #333; padding: 12px 0; font-size: 15px; color: #ccc; }}
             .invoice-box .row:last-child {{ border-bottom: none; font-weight: 700; color: #fff; font-size: 18px; padding-bottom: 0; }}
             .note {{ font-size: 13px; color: #555; border-top: 1px solid #222; padding-top: 20px; margin: 0; }}
             .footer {{ background: #0a0a0a; padding: 16px 40px; font-size: 11px; color: #444; border-top: 1px solid #222; }}
           </style>
         </head>
         <body>
           <div class=""wrap"">
             <div class=""top-bar""></div>
             <div class=""body"">
               <div class=""logo"">STREAMVAULT</div>
               <h1>Payment Successful!</h1>
               <p>Hi <strong>{firstName}</strong>, your payment for the <strong>{planName}</strong> plan has been processed successfully. Your subscription is now active.</p>
               
               <div class=""invoice-box"">
                 <div class=""heading"">Invoice Details</div>
                 <div class=""row"">
                   <span>Plan</span>
                   <span>{planName}</span>
                 </div>
                 <div class=""row"">
                   <span>Payment Date</span>
                   <span>{paymentDate:MMM dd, yyyy}</span>
                 </div>
                 <div class=""row"">
                   <span>Payment Method</span>
                   <span>{cardBrand.ToUpper()} ending in {cardLast4}</span>
                 </div>
                 <div class=""row"">
                   <span>Subscription Ends</span>
                   <span>{periodEnd:MMM dd, yyyy}</span>
                 </div>
                 <div class=""row"">
                   <span>Total Charged</span>
                   <span>${amount}</span>
                 </div>
               </div>
               
               <p class=""note"">Thank you for subscribing to StreamVault. Enjoy unlimited streaming of your favorite movies and TV shows!</p>
             </div>
             <div class=""footer"">© 2026 Netflix, Inc. &nbsp;·&nbsp; Privacy Policy &nbsp;·&nbsp; Help Center</div>
           </div>
         </body>
         </html>
        ";

        await SendEmailAsync(email, subject, body);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var otpMatch = System.Text.RegularExpressions.Regex.Match(
            body, 
            @"letter-spacing: 8px;[^>]*>(\d{6})</h1>");
        
        var otp = otpMatch.Success ? otpMatch.Groups[1].Value : "N/A";
        
        _logger.LogInformation("========================================");
        _logger.LogInformation("📧 Sending email to: {Email}", toEmail);
        _logger.LogInformation("📝 Subject: {Subject}", subject);
        _logger.LogInformation("🔑 OTP: {Otp}", otp);
        _logger.LogInformation("========================================");
        
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.SenderName, _options.SenderEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_options.SmtpServer, _options.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_options.SmtpUsername, _options.SmtpPassword);
            await client.SendAsync(message);
            
            _logger.LogInformation("✅ Email sent successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Failed to send email via {SmtpServer}", _options.SmtpServer);
            _logger.LogWarning("💡 OTP is available in logs above for testing");
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}