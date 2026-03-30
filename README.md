# StreamVault — Netflix Clone Backend

<div align="center">

<pre>
███    ██ ███████ ████████ ███████ ██      ██ ██   ██
████   ██ ██         ██    ██      ██      ██  ██ ██
██ ██  ██ █████      ██    █████   ██      ██   ███
██  ██ ██ ██         ██    ██      ██      ██  ██ ██
██   ████ ███████    ██    ██      ███████ ██ ██   ██
</pre>

</div>

**A production-grade streaming platform backend built with Clean Architecture, CQRS, and real-world patterns.**

[![.NET 9](https://img.shields.io/badge/.NET_9-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com)
[![EF Core 9](https://img.shields.io/badge/EF_Core_9-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://learn.microsoft.com/ef)
[![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![Stripe](https://img.shields.io/badge/Stripe-635BFF?style=flat-square&logo=stripe&logoColor=white)](https://stripe.com)
[![Cloudinary](https://img.shields.io/badge/Cloudinary-3448C5?style=flat-square&logo=cloudinary&logoColor=white)](https://cloudinary.com)
[![Status](https://img.shields.io/badge/Status-Complete-22c55e?style=flat-square)](#)

</div>

---

## What Is This?

StreamVault is a fully functional backend for a Netflix-style streaming service. It handles everything from user registration and subscription billing to video delivery and engagement tracking — built the way a real production system would be.

---

## Architecture

```
Netflix-Clone/
├── src/
│   ├── API/
│   │   └── NetflixClone.Api              → Controllers, DI wiring, middleware
│   ├── Core/
│   │   ├── NetflixClone.Domain           → Entities, enums, value objects
│   │   └── NetflixClone.Application      → CQRS handlers, DTOs, interfaces
│   └── Infrastructure/
│       ├── NetflixClone.Infrastructure   → JWT, Email, OTP, Stripe, Cloudinary
│       └── NetflixClone.Persistence      → DbContext, EF configs, repos, seeds
└── tests/
    ├── NetflixClone.Domain.Tests
    ├── NetflixClone.Application.Tests
    └── NetflixClone.Integration.Tests
```

**Layer rules enforced throughout:**

- **Domain** — zero external dependencies. Pure C# models and enums.
- **Application** — depends only on Domain. Defines all infrastructure interfaces.
- **Infrastructure** — implements Application interfaces. Knows about Stripe, Cloudinary, MailKit.
- **API** — references Application only. No domain logic in controllers.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET 9 Web API |
| ORM | Entity Framework Core 9 (Code-First) |
| Database | SQL Server |
| CQRS | MediatR 14 |
| Object Mapping | AutoMapper 16 |
| Authentication | JWT Bearer + BCrypt |
| Payments | Stripe Checkout + Webhooks |
| Media Storage | Cloudinary (images + video) |
| Email | MailKit + SMTP (SendGrid-compatible) |
| Password Hashing | BCrypt.Net |

---

## Core Features

### 🔐 Authentication & Identity

Full email-OTP verification flow with rate-limited attempts and brute-force protection.

```
POST /api/auth/register              → Create account + send email OTP
POST /api/auth/confirm-email         → Verify with 6-digit OTP
POST /api/auth/resend-confirmation-otp
POST /api/auth/login                 → Returns JWT
POST /api/auth/logout
POST /api/auth/forgot-password       → Sends reset OTP
POST /api/auth/reset-password        → OTP + new password
```

**Security details:**
- OTPs are cryptographically random (`RandomNumberGenerator.GetInt32`)
- Max 10 attempts per 15-minute window before lockout
- OTPs expire — email confirmation: 20 min, password reset: 15 min
- BCrypt hashing for profile PINs

---

### 👤 Profiles

Netflix-style multi-profile support under one account. Profile switching issues a new scoped JWT.

```
POST   /api/profile                  → Create profile (enforces plan MaxProfiles)
DELETE /api/profile                  → Delete active profile
POST   /api/profile/switch           → Switch profile → returns profile-scoped token
GET    /api/profile                  → List all profiles for account
```

**Profile JWT claims include:** `profileId`, `isKidsMode`, `age`, `preferredLanguage`, `avatarUrl`

**Kids Mode** is automatically enabled when age is set between 1–12. Content filtering respects this flag on every catalog and streaming request.

---

### 💳 Subscriptions & Billing

Stripe Checkout flow with full webhook-driven subscription lifecycle.

```
POST /api/subscription/Subscripe     → Creates Stripe Checkout session → returns URL
GET  /api/subscription/my-subscription
POST /api/subscription/plans        → [Admin] Create plan
DELETE /api/subscription/plans      → [Admin] Delete plan
GET  /api/subscription/plans        → List all plans
```

**What happens on `checkout.session.completed` webhook:**

1. User found by Stripe Customer ID
2. Plan resolved from webhook metadata
3. `Subscription` record created with period dates from Stripe
4. `Invoice` record created (amount, currency, PDF URL, Stripe IDs)
5. Role changed: `NotSubscriber` → `Subscriber`
6. Invoice email dispatched via `SendInvoiceEmailAsync`

**Seeded plans:**

| Plan | Profiles | Quality | Monthly | Yearly |
|---|---|---|---|---|
| Basic | 1 | 720p HD | $8.99 | $89.99 |
| Standard | 3 | 1080p Full HD | $13.99 | $139.99 |
| Premium | 5 | 4K UHD | $17.99 | $179.99 |

---

### 🎬 Content Catalog

Full admin content pipeline for movies, series, and documentaries with rich metadata.

```
POST   /api/catalog/content              → Create content (movie/series/documentary)
DELETE /api/catalog/content/{id}
GET    /api/catalog/content/{id}         → Full detail with cast
GET    /api/catalog/content              → Paginated browse with filters
GET    /api/catalog/trending             → Top by ViewCount (Kids-safe filter applied)
POST   /api/catalog/genres
DELETE /api/catalog/genres/{id}
POST   /api/catalog/person
DELETE /api/catalog/person/{id}
GET    /api/catalog/person/{id}          → Person detail with filmography
```

**Catalog filters supported:**
`SearchQuery`, `GenreIds`, `ContentTypes`, `MinRating`, `MaturityRatings`, `Languages`, `ReleaseYear`, `IsOriginal`, `FromDate`, `ToDate`, `OrderedByRatingDescending`

**Business rules enforced at handler level:**
- Movies/single documentaries require `DurationMinutes`, cannot have seasons
- Series must not set `DurationMinutes` on the parent — it lives on each episode
- Slugs are auto-generated if omitted (`"The Dark Knight" + 2008 → "the-dark-knight-2008"`)

---

### 📹 Media Upload Pipeline

```
POST /api/catalog/content/{id}/images      → Thumbnail + hero image → Cloudinary
POST /api/catalog/content/{id}/video       → Movie video OR episode video → Cloudinary
POST /api/catalog/episodes/{id}/thumbnail  → Episode still image → Cloudinary
POST /api/catalog/person/{id}/photo        → Actor/director photo → Cloudinary
```

Cloudinary `PublicId` is stored after upload. Streaming URLs are built on-demand per quality tier, never stored directly.

---

### 🎥 Streaming

```
GET /api/catalog/content/{id}/play?episodeId=...
```

One endpoint does a lot:

1. Validates the caller has an active subscription
2. Resolves `MaxVideoQuality` from their plan (`HD_720p` / `FullHD_1080p` / `UHD_4K`)
3. Builds a quality-constrained Cloudinary delivery URL
4. Upserts a `WatchHistory` record (creates on first watch, resets progress on re-watch)
5. Increments `ViewCount` on the parent content

Quality map: `HD_720p → h_720`, `FullHD_1080p → h_1080`, `UHD_4K → h_2160`

---

### ❤️ Engagement

```
POST   /api/engagement/content/{id}/rating   → Rate content (1–5)
DELETE /api/engagement/rating/{ratingId}
GET    /api/engagement/content/{id}/ratings  → All ratings for a title (paginated)
GET    /api/engagement/content/{id}/rating   → My rating for a specific title
GET    /api/profile/my-ratings               → All my ratings
GET    /api/profile/watch-history?ContinueWatchingOnly=true
```

`AverageRating` on `Content` is a denormalized column updated inline on every add/delete — no expensive aggregation queries at read time.

---

## Database Design

16 tables. Full EF Core Fluent API configuration with proper indexes, constraints, and cascade rules.

```
Identity          → AspNetUsers, Profiles, ProfilePreferences, RefreshTokens
Subscriptions     → Plans, Subscriptions, Invoices
Catalog           → Contents, Seasons, Episodes, Genres, ContentGenres, Persons, ContentPersons
Engagement        → WatchHistories, Ratings
```

**Notable indexes:**
- `Contents.Slug` — unique, used for SEO-friendly lookups
- `Contents.ViewCount` — powers trending queries
- `WatchHistories(ProfileId, ContentId, EpisodeId)` — unique, filtered (`WHERE EpisodeId IS NOT NULL`), enables the upsert pattern
- `WatchHistories(ProfileId, IsCompleted, WatchedAt)` — named composite for Continue Watching queries
- `Ratings(ProfileId, ContentId)` — unique, one rating per profile per title
- `AspNetUsers.StripeCustomerId` — partial unique index (`WHERE StripeCustomerId IS NOT NULL`) for fast webhook lookups

---

## Seeded Data

The app self-seeds on every startup (idempotent — skips if data exists).

| Seed | Contents |
|---|---|
| Roles | `SuperAdmin`, `ContentManager`, `Subscriber`, `NotSubscriber` |
| Users | admin, content manager, 2 subscriber accounts |
| Plans | 6 plans (Basic/Standard/Premium × Monthly/Yearly) |
| Genres | 25 genres with slugs |
| Persons | 16 actors and directors (stable GUIDs for test references) |
| Content | Inception (Movie) + Peaky Blinders S1 with 3 episodes |

---

## Email Templates

All emails are HTML with a dark Netflix-style theme. Three flows implemented:

- **Email Confirmation** — OTP with 20-minute expiry
- **Password Reset** — OTP with 15-minute expiry
- **Invoice** — Payment confirmation with plan name, amount, period dates

---

## Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server (local or Docker)
- Cloudinary account
- Stripe account (test mode)
- SMTP credentials (SendGrid or similar)

### Setup

```bash
git clone https://github.com/Ahmed-Walled10/Netflix-Clone.git
cd Netflix-Clone

dotnet restore
```

Copy `appsettings.Example.json` → `appsettings.json` and fill in your values:

```json
{
  "ConnectionStrings": {
    "Default": "Server=.;Database=NetflixCloneDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "SecretKey": "your-key-min-32-chars",
    "Issuer": "NetflixCloneApi",
    "Audience": "NetflixCloneClient",
    "AccessTokenExpiryMinutes": 15,
    "RefreshTokenExpiryDays": 30
  },
  "Cloudinary": { "CloudName": "...", "ApiKey": "...", "ApiSecret": "..." },
  "Stripe": { "SecretKey": "sk_test_...", "WebhookSecret": "whsec_..." },
  "EmailSettings": { "SmtpServer": "...", "SmtpPort": 587, "..." }
}
```

```bash
dotnet run --project NetflixClone.Api
```

Migrations run and seed data is applied automatically on first startup.

Swagger UI: `https://localhost:7254/swagger`

---


<div align="center">

Built as a portfolio project · Clean Architecture · .NET 9 · CQRS with MediatR

</div>
