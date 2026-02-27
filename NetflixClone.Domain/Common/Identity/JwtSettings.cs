using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Domain.Common.Identity
{
    public sealed class JwtSettings
    {
        public const string SectionName = "JwtSettings";

        public string SecretKey { get; init; } = string.Empty;

        public string Issuer { get; init; } = string.Empty;

        public string Audience { get; init; } = string.Empty;

        public int AccessTokenExpiryMinutes { get; init; } = 15;

        public int RefreshTokenExpiryDays { get; init; } = 30;
    }
}
