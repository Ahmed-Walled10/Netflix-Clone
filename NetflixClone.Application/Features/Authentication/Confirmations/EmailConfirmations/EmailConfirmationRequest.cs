using System.ComponentModel.DataAnnotations;
using MediatR;

namespace NetflixClone.Application.Features.Authentication.Confirmations.EmailConfirmations;

public class EmailConfirmationRequest : IRequest<bool>
{
    public string Email { get; set; } = string.Empty;

    public string Otp { get; set; } = string.Empty;
}