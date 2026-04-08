using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NetflixClone.Application.Features.Authentication.Commands.Login;
using NetflixClone.Application.Features.Authentication.Commands.Logout;
using NetflixClone.Application.Features.Authentication.Commands.Register;
using NetflixClone.Application.Features.Authentication.Commands.RefreshToken;
using NetflixClone.Application.Features.Authentication.Commands.RevokeToken;
using NetflixClone.Application.Features.Authentication.Commands.RevokeAllTokens;
using NetflixClone.Application.Features.Authentication.Commands.ForgotPassword;
using NetflixClone.Application.Features.Authentication.Commands.ResetPassword;
using NetflixClone.Application.Features.Authentication.Confirmations.EmailConfirmations;
using NetflixClone.Application.Features.Authentication.Confirmations.ResendEmailConfirmationOtp;
using System.Security.Claims;

namespace NetflixClone.Api.Controller
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var result = await _mediator.Send(registerRequest);

            return Ok(new
            {
                message = "Registration successful! Please check your email for OTP to confirm your account.",
                data = result
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            //if (User.Identity?.IsAuthenticated == true)
             //   return BadRequest("Already logged in.");

            var result = await _mediator.Send(loginRequest);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequest logoutRequest)
        {
            var result = await _mediator.Send(logoutRequest);

            if (!result)
            {
                return BadRequest(new { message = "Logout failed. Invalid or already-revoked refresh token." });
            }

            return Ok(new { message = "Logged out successfully." });
        }

        /// <summary>
        /// Exchange an expired access token by providing a valid refresh token.
        /// Returns a new access token + rotated refresh token.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var result = await _mediator.Send(refreshTokenRequest);
            return Ok(result);
        }

        /// <summary>
        /// Revoke a specific refresh token (e.g. from another device).
        /// </summary>
        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(RevokeTokenRequest revokeTokenRequest)
        {
            var result = await _mediator.Send(revokeTokenRequest);

            if (!result)
            {
                return BadRequest(new { message = "Token revocation failed. Token may be invalid or already revoked." });
            }

            return Ok(new { message = "Token revoked successfully." });
        }

        /// <summary>
        /// Revoke ALL active refresh tokens for the current user ("log out everywhere").
        /// </summary>
        [Authorize]
        [HttpPost("revoke-all")]
        public async Task<IActionResult> RevokeAllTokens()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new { message = "Unable to identify user." });
            }

            var request = new RevokeAllTokensRequest { UserId = userId };
            var revokedCount = await _mediator.Send(request);

            return Ok(new
            {
                message = $"All sessions terminated. {revokedCount} token(s) revoked.",
                revokedCount
            });
        }

        [AllowAnonymous]
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationRequest emailConfirmationRequest)
        {
            var result = await _mediator.Send(emailConfirmationRequest);
            if (!result)
            {
                return BadRequest(new { message = "Invalid Or Expired Otp , Please try again" });
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("resend-confirmation-otp")]
        public async Task<IActionResult> ResendConfirmationOtp(ResendEmailConfirmationOtpRequest
            resendEmailConfirmationOtpRequest)
        {
            var result = await _mediator.Send(resendEmailConfirmationOtpRequest);
            if (!result)
            {
                return BadRequest(new
                { message = "Could not resend OTP. Email may already be confirmed or does not exist." });
            }

            return Ok(new { message = "OTP resent successfully! Please check your email." });
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            var result = await _mediator.Send(forgotPasswordRequest);
            if (!result)
            {
                return BadRequest(new { message = "Could not forgot password. Please try again." });
            }

            return Ok(new { message = "If your email exists, you will receive a password reset OTP shortly." });
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            var result = await _mediator.Send(resetPasswordRequest);

            if (!result)
            {
                return BadRequest(new
                { message = "Invalid or expired OTP. Please try again or request a new password reset." });
            }

            return Ok(new { message = "Password reset successfully! You can now login with your new password." });
        }
    }
}
