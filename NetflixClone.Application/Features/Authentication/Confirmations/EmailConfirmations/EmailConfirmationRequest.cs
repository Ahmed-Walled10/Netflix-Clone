using System.ComponentModel.DataAnnotations;
using MediatR;

namespace NetflixClone.Application.Features.Authentication.Confirmations.EmailConfirmations;

public class EmailConfirmationRequest : IRequest<bool>
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string Otp { get; set; } = string.Empty;
}