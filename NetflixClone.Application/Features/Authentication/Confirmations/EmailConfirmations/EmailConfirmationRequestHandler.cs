using NetflixClone.Application.Contracts.Infrasructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Features.Authentication.Confirmations.EmailConfirmations;

public class EmailConfirmationRequestHandler : IRequestHandler<EmailConfirmationRequest, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOtpService _otpService;

    public EmailConfirmationRequestHandler(UserManager<ApplicationUser> userManager
        , IOtpService otpService)
    {
        _userManager = userManager;
        _otpService = otpService;
    }

    public async Task<bool> Handle(EmailConfirmationRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return false;
        }

        if (user.OtpAttempts >= 10 &&
            user.LastOtpAttemptAt.HasValue &&
            DateTime.UtcNow < user.LastOtpAttemptAt.Value.AddMinutes(15))
        {
            return false;
        }

        if (!user.LastOtpAttemptAt.HasValue ||
            DateTime.UtcNow >= user.LastOtpAttemptAt.Value.AddMinutes(15))
        {
            user.OtpAttempts = 0;
        }

        user.OtpAttempts++;
        user.LastOtpAttemptAt = DateTime.UtcNow;

        if (!_otpService.ValidateOtp(request.Otp, user.EmailConfirmationOtp, user.EmailConfirmationOtpExpiration))
        {
            await _userManager.UpdateAsync(user);
            return false;
        }

        user.EmailConfirmed = true;
        user.EmailConfirmationOtp = null;
        user.EmailConfirmationOtpExpiration = null;
        user.OtpAttempts = 0;
        user.LastOtpAttemptAt = null;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}