using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Application.Features.Profiles.Commands.LoginToProfile
{
    public class LoginToProfileRequest : IRequest<LoginToProfileResponce>
    {
        public string UserId { get; set; } = string.Empty;

        public Guid ProfileId { get; set; }


        [StringLength(10)]
        public string? Pin { get; set; }

    }
}
