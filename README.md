# 🎬 NetflixClone — Backend API

<div align="center">

![ASP.NET 9](https://img.shields.io/badge/ASP.NET_9-Minimal_APIs-512BD4?style=for-the-badge&logo=dotnet)
![EF Core 9](https://img.shields.io/badge/EF_Core_9-Code--First-512BD4?style=for-the-badge&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-Database-CC2927?style=for-the-badge&logo=microsoftsqlserver)
![Redis](https://img.shields.io/badge/Redis-Caching-DC382D?style=for-the-badge&logo=redis)
![Azure](https://img.shields.io/badge/Azure_Blob-Storage-0078D4?style=for-the-badge&logo=microsoftazure)
![Stripe](https://img.shields.io/badge/Stripe-Payments-635BFF?style=for-the-badge&logo=stripe)
![MediatR](https://img.shields.io/badge/MediatR-CQRS-00C49F?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Phase_2_In_Progress-F59E0B?style=for-the-badge)

A **production-grade Netflix backend clone** built with ASP.NET 9, Clean Architecture, CQRS, and real-world patterns used in professional streaming platforms.

[MVP Scope](#-mvp-scope) • [Architecture](#️-architecture) • [Tech Stack](#️-tech-stack) • [API Docs](#-api-endpoints) • [Progress](#-build-progress) • [Getting Started](#-getting-started)

</div>

---

## 🎯 MVP Scope

The MVP strips the full project bible down to what actually matters for a first working version — enough to demonstrate real-world patterns without over-engineering.

| Feature | MVP Approach | Status |
|---|---|---|
| JWT Authentication | Register → OTP Email Verify → Login → Refresh Token → Logout | 🔄 In Progress |
| Multi-Profile Support | Up to N profiles per plan, age attribute, optional PIN, preferences | 🔜 Next |
| Stripe Subscriptions | Plans by profile count & billing period, Stripe Checkout, invoices by email | 🔜 Next |
| Video Streaming | Simplified: direct MP4 SAS URLs from Azure Blob, concurrency enforcement | 🔜 Next |
| Media Pipeline | Admin uploads → Azure Blob → store URL → mark ready. No FFmpeg | 🔜 Next |
| Full-Text Search | SQL Server FTS on Title, Description, Cast | 🔜 Next |
| Engagement | Watch history, continue watching, ratings | 🔜 Next |
| Trending | ViewCount++ on every stream start + daily Hangfire snapshot | 🔜 Next |
| Role-Based Access | NotSubscriber \| Subscriber \| ContentManager \| SuperAdmin — via JWT claims | 🔄 In Progress |
| Email Flows | OTP-based confirm email, reset password (via Brevo SMTP) | 🔄 In Progress |

> 💡 **MVP Philosophy:** Build it working first. Add complexity later. Each phase is independently shippable.

---

## ✨ Features (Full Vision — Post-MVP)

> These features are planned for **after the MVP is complete**, as defined in the Project Bible.

- 🔐 **JWT Authentication** — Access tokens (15 min) + refresh tokens (30 days) with rotation
- 👤 **Multi-Profile Support** — Up to 5 profiles per account (Kids mode, PIN lock, maturity ratings)
- 💳 **Stripe Subscriptions** — Basic / Standard / Premium plans with invoicing, upgrades, downgrades & dunning
- 🎥 **Video Streaming** — HLS manifest delivery, concurrent stream enforcement per plan, heartbeat sessions
- 📼 **Media Pipeline** — Azure Blob pre-signed uploads → Hangfire encoding jobs → multi-resolution variants
- 🔍 **Full-Text Search** — SQL Server FTS on titles, descriptions & cast with autocomplete
- 📋 **Engagement** — Watch history, continue watching, My List, ratings, reviews & likes
- 🤖 **Recommendations** — Rule-based scoring engine (genre match + cast + rating + recency), pre-computed daily by Hangfire, cached in Redis
- 📈 **Trending** — Daily/Weekly/Monthly snapshots refreshed by background jobs
- 🔔 **Notifications** — In-app notifications for billing events, new content & profile alerts
- 🛡️ **Role-Based Access** — SuperAdmin, ContentManager, Subscriber, NotSubscriber
- 📧 **Email Flows** — OTP verification, password reset, invoice receipts, payment failure alerts

---

## 🏗️ Architecture

This project follows **Clean Architecture** with strict layer separation:

```
src
 ├── API
 │    └── NetflixClone.Api              → Controllers, Middleware, DI wiring
 ├── Core
 │    ├── NetflixClone.Application      → CQRS (Commands/Queries), DTOs, Interfaces, Services
 │    └── NetflixClone.Domain           → Entities, Enums, Base Primitives
 └── Infrastructure
      ├── NetflixClone.Infrastructure   → Email (MailKit/Brevo), External Services
      └── NetflixClone.Persistence      → NetflixCloneDbContext, EF Configs, Migrations, Seeds

tests
├── NetflixClone.Domain.Tests
├── NetflixClone.Application.Tests
└── NetflixClone.Integration.Tests
```

### Layer Rules
- **Domain** has zero external dependencies
- **Application** depends only on Domain; defines interfaces implemented by Infrastructure
- **Infrastructure** implements Application interfaces (MailKit, Azure SDK, Stripe SDK, Redis)
- **API** references Application only — no domain logic in endpoints
- All cross-cutting concerns (logging, validation, caching) handled via **MediatR pipeline behaviors**

### Bounded Contexts

| Context | Core Entities |
|---|---|
| 🔐 Identity | ApplicationUser, Profile, ProfilePreference, RefreshToken |
| 💳 Subscription | Plan, Subscription, Invoice, PaymentMethod |
| 🎬 Catalog | Content, Season, Episode, Genre, Person, ContentGenre, ContentPerson |
| 📼 Media | StreamingSession |
| ❤️ Engagement | WatchHistory, Rating |

---

## 🛠️ Tech Stack

| Concern | Choice | Notes |
|---|---|---|
| Framework | ASP.NET 9 | Controllers + Middleware |
| ORM | EF Core 9 (Code-First) | Migrations, Fluent API configurations |
| CQRS | MediatR 12 | Commands, Queries, Notifications |
| Mapping | AutoMapper | Entity ↔ Request/Response mapping |
| Auth | ASP.NET Identity + JWT | Identity roles, 15-min access tokens |
| Refresh Tokens | Custom rotation | SHA-256 hashed, 30-day, device-aware |
| OTP | Custom secure OTP | `RandomNumberGenerator` (cryptographically safe) |
| Email | MailKit + Brevo SMTP | OTP confirmation, password reset — HTML templates |
| Caching | Redis (StackExchange.Redis) | Trending, recommendations, rate limits *(planned)* |
| Storage | Azure Blob Storage | Videos, images (MVP: direct MP4 URL) |
| Background Jobs | Hangfire | Trending snapshot, session cleanup *(planned)* |
| Search | SQL Server Full-Text Search | FTS on Title, Description, Cast *(planned)* |
| Payments | Stripe Checkout (test mode) | Subscriptions, invoices, webhooks *(planned)* |
| Testing | xUnit + Moq + Testcontainers | Unit + integration tests |
| Docs | Swagger / Scalar | Auto-generated API docs |

---

## 📡 API Endpoints

### MVP Endpoints

<details>
<summary><strong>🔐 Auth — /api/auth</strong></summary>

| Method | Route | Auth | Description |
|---|---|---|---|
| POST | `/register` | ❌ Public | Create account → sends OTP email → returns JWT (`NotSubscriber` role) |
| POST | `/verify-email` | ✅ JWT | Submit 6-digit OTP to confirm email |
| POST | `/login` | ❌ Public | Validate credentials → returns JWT with user roles |
| POST | `/refresh` | ❌ Public | Rotate refresh token, return new access token |
| POST | `/logout` | ✅ JWT | Revoke refresh token |
| POST | `/forgot-password` | ❌ Public | Send OTP password reset email |
| POST | `/reset-password` | ❌ Public | Reset password with OTP |

</details>

<details>
<summary><strong>👤 Profiles — /api/profiles</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/` | List all profiles for the account |
| POST | `/` | Create profile (enforces plan max) |
| PUT | `/{id}` | Update name, avatar, age, language |
| DELETE | `/{id}` | Delete profile (cannot delete last one) |
| POST | `/{id}/switch` | Switch active profile (returns new JWT with profileId claim) |
| PUT | `/{id}/pin` | Set or update PIN |
| DELETE | `/{id}/pin` | Remove PIN protection |
| GET | `/{id}/preferences` | Get actor/genre/director preferences |
| POST | `/{id}/preferences` | Add a preference |
| DELETE | `/{id}/preferences/{prefId}` | Remove a preference |

</details>

<details>
<summary><strong>💳 Subscriptions — /api/subscriptions</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/plans` | List available plans (public) |
| POST | `/checkout` | Create Stripe Checkout session, return URL |
| GET | `/me` | Get current subscription details |
| PUT | `/cancel` | Set CancelAtPeriodEnd=true |
| GET | `/invoices` | Get billing history |
| POST | `/webhook` | Stripe webhook endpoint (no auth, verify signature) |

</details>

<details>
<summary><strong>🎬 Catalog — /api/catalog</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/` | Browse content (paginated, filtered) |
| GET | `/{slug}` | Get content detail by slug |
| GET | `/genres` | List all genres |
| GET | `/genres/{slug}` | Browse content by genre |
| GET | `/new-releases` | Recently added content |
| GET | `/trending?period=Weekly` | Trending content (cached from snapshots) |
| POST | `/admin` | [ContentManager] Create movie/series/documentary |
| PUT | `/admin/{id}` | [ContentManager] Update content |
| DELETE | `/admin/{id}` | [ContentManager] Soft-delete content |
| POST | `/admin/{id}/seasons` | [ContentManager] Add season to series |
| POST | `/admin/seasons/{id}/episodes` | [ContentManager] Add episode |

</details>

<details>
<summary><strong>📼 Media — /api/media</strong></summary>

| Method | Route | Description |
|---|---|---|
| POST | `/upload-url` | [ContentManager] Generate Azure Blob SAS upload URL |
| POST | `/finalize` | [ContentManager] Save final video URL to Content record |
| POST | `/upload-image` | [ContentManager] Upload thumbnail/poster to Azure Blob |

</details>

<details>
<summary><strong>🎬 Streaming — /api/stream</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/{contentId}` | Get signed SAS URL + create session (concurrency check) |
| POST | `/heartbeat` | Keep session alive (body: `{ sessionId }`) |
| POST | `/{sessionId}/end` | End streaming session, update watch history |

</details>

<details>
<summary><strong>🔍 Search — /api/search</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/?q=batman&type=Movie&genre=Action` | Full-text search with filters |
| GET | `/suggest?q=bat` | Autocomplete: top 5 title suggestions |

</details>

<details>
<summary><strong>❤️ Engagement — /api</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/history` | Get watch history for active profile |
| GET | `/continue-watching` | Get incomplete content ordered by last watched |
| PUT | `/history/{contentId}` | Upsert watch progress |
| POST | `/ratings/{contentId}` | Rate content (1–5 stars) |
| DELETE | `/ratings/{contentId}` | Remove rating |

</details>

---

### Post-MVP Endpoints *(planned)*

<details>
<summary><strong>🤖 Discovery — /api/discover</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/recommendations` | Top picks for active profile (rule-based scoring) |
| GET | `/because-you-watched` | Based on recent watch history |

</details>

<details>
<summary><strong>📝 Reviews — /api/reviews (Post-MVP)</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/{contentId}` | Get reviews for content |
| POST | `/{contentId}` | Post a review |
| DELETE | `/{reviewId}` | Delete own review |
| POST | `/{reviewId}/like` | Like a review |

</details>

<details>
<summary><strong>🔔 Notifications — /api/notifications (Post-MVP)</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/` | Get in-app notifications |
| PUT | `/{id}/read` | Mark notification as read |

</details>

---

## 📋 Subscription Plans

| Plan | Profiles | Max Quality | Monthly | Yearly |
|---|---|---|---|---|
| Basic | 1 | HD 720p | $8.99 | $89.99 |
| Standard | 3 | Full HD 1080p | $13.99 | $139.99 |
| Premium | 5 | 4K UHD | $17.99 | $179.99 |

> Plans are seeded at startup with stable GUIDs — idempotent, safe to re-run on every restart.

---

## 🗺️ Build Progress

> **Legend:** ✅ Done &nbsp;|&nbsp; 🔄 In Progress &nbsp;|&nbsp; 🔜 Next &nbsp;|&nbsp; ⏳ Optional &nbsp;|&nbsp; 🔵 Post-MVP

---

### Phase 1 — Project Setup ✅
> Solution structure, EF Core, DbContext, base entities, Persistence layer, seeds, migrations

- [x] Solution scaffolding (Clean Architecture folder structure)
- [x] EF Core 9 setup with SQL Server
- [x] `NetflixCloneDbContext` with full entity registration
- [x] `NetflixCloneDbContextFactory` for design-time EF Core tooling
- [x] EF Core Fluent API configurations for all bounded contexts:
  - [x] Identity — `ApplicationUser`, `Profile`, `ProfilePreference`, `RefreshToken`
  - [x] Subscriptions — `Plan`, `Subscription`, `Invoice`, `PaymentMethod`
  - [x] Catalog — `Content`, `Season`, `Episode`, `Genre`, `Person`, `ContentGenre`, `ContentPerson`
  - [x] Media — `StreamingSession` (with nullable `EpisodeId` FK)
  - [x] Engagement — `WatchHistory`, `Rating`
- [x] Base primitives — `BaseEntity` (Guid PK), `AuditableEntity` (CreatedAt, UpdatedAt)
- [x] Domain enums — `VideoQuality`, `MaturityRating`, `SubscriptionStatus`, `BillingPeriod`, `ContentType`, `PersonRole`, `PaymentAttemptStatus`
- [x] `JwtSettings` strongly-typed config class
- [x] Data seeding (idempotent, stable GUIDs):
  - [x] `GenreSeeder` — 25 genres seeded
  - [x] `PersonSeeder` — 16 directors & actors seeded
  - [x] `DatabaseSeeder` — Plans (6 tiers), Roles (SuperAdmin, ContentManager, Subscriber, NotSubscriber), Admin user
- [x] Initial EF Core migration generated
- [x] Repository contract (`IBaseRepository<T>`) in Application layer
- [x] `appsettings.json` configured (SQL Server, Redis, JwtSettings)
- [ ] Global exception handling middleware
- [ ] MediatR pipeline (logging, validation, caching behaviors)

---

### Phase 2 — Identity & Authentication 🔄 In Progress
> Register, Login, JWT, Refresh Tokens, OTP Email Verification, Profiles CRUD

#### ✅ Completed
- [x] **`RegisterRequestHandler`** — Create account, generate OTP, send confirmation email, return JWT (`NotSubscriber` role)
- [x] **`LoginRequestHandler`** — Validate credentials, check email confirmed / IsActive / IsSuspended, return JWT with roles
- [x] **`JwtTokenGeneration` service** — Generates HS256 JWT with `sub`, `jti`, `NameIdentifier`, `Name`, `Email`, `Role` claims; reads from `JwtSettings` config section
- [x] **`OtpService`** — Cryptographically secure 6-digit OTP via `RandomNumberGenerator`; expiry validation
- [x] **`EmailService`** (MailKit + Brevo SMTP) — Polished HTML email templates for:
  - [x] Email confirmation OTP
  - [x] Password reset OTP
- [x] **Contracts** — `IJwtTokenGeneration`, `IOtpService`, `IEmailService`
- [x] **Role system** — `NotSubscriber` assigned on register; `Subscriber` assigned after plan purchase
- [x] **Security guards in Login** — Inactive accounts (`IsActive=false`) and suspended accounts (`IsSuspended=true`) are blocked

#### 🔜 Remaining
- [ ] `VerifyEmailCommandHandler` — Validate OTP, mark `EmailConfirmed=true`, clear OTP fields
- [ ] `RefreshTokenCommandHandler` — Rotate refresh token (SHA-256 hashed, stored in DB)
- [ ] `LogoutCommandHandler` — Revoke refresh token
- [ ] `ForgotPasswordCommandHandler` — Generate password reset OTP + send email
- [ ] `ResetPasswordCommandHandler` — Validate OTP, update password, clear OTP fields
- [ ] AutoMapper profile — `RegisterRequest → ApplicationUser`
- [ ] `Program.cs` — Register MediatR, AutoMapper, JWT Bearer auth, OtpService, EmailService, JwtTokenGeneration
- [ ] Profiles CRUD (max enforced by `Plan.MaxProfiles`)
- [ ] Profile switching (new JWT with `profileId` claim)
- [ ] Kids mode auto-set when age < 13
- [ ] Optional PIN (BCrypt-hashed, required on switch if set)
- [ ] Profile preferences (Genre / Actor / Director)

---

### Phase 3 — Subscriptions & Billing 🔜
> Stripe Checkout, subscription lifecycle, invoices, webhooks

- [ ] Stripe Customer creation after email verification
- [ ] Subscribe via Stripe Checkout session (hosted page, no card data on server)
- [ ] Stripe webhook handler:
  - `checkout.session.completed` → create Subscription + send invoice email
  - `invoice.payment_succeeded` → mark Invoice paid, send receipt
  - `invoice.payment_failed` → increment attempt, suspend after 3 failures
  - `customer.subscription.deleted` → set Status=Canceled
  - `customer.subscription.updated` → handle plan change
- [ ] Role upgrade: `NotSubscriber` → `Subscriber` after checkout complete
- [ ] Cancel subscription (`CancelAtPeriodEnd=true`)
- [ ] Invoice history endpoint

---

### Phase 4 — Enforce Plan Limits + RBAC 🔜
> Plan limits on profiles, role enforcement on endpoints

- [ ] Enforce `Plan.MaxProfiles` on profile creation
- [ ] Require active subscription for streaming (`[Authorize(Roles = "Subscriber")]`)
- [ ] Role-based authorization on all endpoints

---

### Phase 5 — Content Catalog 🔜
> Admin CRUD for Movies/Series/Documentaries, Genres, Persons

- [ ] Content CRUD (Movie, Series, Documentary)
- [ ] Season + Episode CRUD
- [ ] Genre management (already seeded)
- [ ] Person (cast/crew) management (already seeded)
- [ ] Maturity ratings enforcement
- [ ] Content browse endpoint (paginated, filterable by genre/year/type)
- [ ] Slug-based content detail endpoint
- [ ] New Releases endpoint (`CreatedAt DESC`)

---

### Phase 6 — Media Upload (Simplified MVP) 🔜
> Azure Blob SAS upload, save VideoUrl, publish toggle — no FFmpeg

- [ ] Generate Azure Blob SAS upload URL for admin
- [ ] Admin uploads MP4 directly to Azure Blob (no traffic through API)
- [ ] `POST /api/media/finalize` — save `VideoUrl` on Content record
- [ ] Thumbnail/poster image upload to Azure Blob
- [ ] Admin flips `IsAvailable=true` to publish content

---

### Phase 7 — Streaming 🔜
> SAS streaming URLs, StreamingSession concurrency, heartbeat

- [ ] `GET /api/stream/{contentId}` — concurrency check + return signed SAS URL
- [ ] Create `StreamingSession` record on stream start
- [ ] Increment `Contents.ViewCount` on each stream start (for trending)
- [ ] Heartbeat endpoint — updates `LastHeartbeatAt`
- [ ] Stream end endpoint — closes session
- [ ] Hangfire recurring job: clean stale sessions every 5 min (heartbeat > 2 min old)

---

### Phase 8 — Full-Text Search 🔜
> SQL Server FTS indexes, search endpoint, autocomplete

- [ ] SQL Server Full-Text catalog + indexes on `Contents` (Title, Description)
- [ ] Full-Text index on `Persons.FullName` (cast search)
- [ ] Search endpoint with filters (genre, year range, maturity, type)
- [ ] Autocomplete endpoint (top 5 title suggestions)

---

### Phase 9 — Trending 🔜
> ViewCount snapshots, Hangfire daily job, Redis cache

- [ ] Hangfire daily job: snapshot top 20 by ViewCount → `TrendingSnapshots` (Daily/Weekly/Monthly)
- [ ] `GET /api/catalog/trending?period=Weekly` — served from Redis (TTL: 1h)

---

### Phase 10 — Engagement 🔜
> Watch history, Continue Watching, Ratings

- [ ] Watch progress upsert on stream end (`StoppedAtSeconds`)
- [ ] Auto-complete at 90% watched (`IsCompleted=true`)
- [ ] Continue Watching query (incomplete, ordered by `WatchedAt DESC`)
- [ ] Ratings — 1–5 star scale (one per profile per content, `AverageRating` updated on change)

---

## 🔵 Post-MVP Roadmap *(Full Project Bible)*

> These features are out of scope for the MVP but are fully designed and planned.

### Advanced Streaming
- [ ] HLS manifest delivery (replace direct MP4 URLs)
- [ ] FFmpeg-based multi-resolution encoding pipeline (360p → 4K)
- [ ] `EncodingJob` status tracking + retry logic (max 3 attempts)
- [ ] Subtitle track upload & storage
- [ ] Quality cap enforcement per plan (720p / 1080p / 4K)
- [ ] Download support (Standard: 2 devices, Premium: 4 devices)

### Plan Upgrades & Dunning
- [ ] Plan upgrade — immediate + prorated invoice
- [ ] Plan downgrade — deferred to next cycle
- [ ] Dunning logic — retry 3×, then suspend account

### Recommendations Engine
- [ ] Rule-based scoring: `(genreMatch × 0.4) + (castMatch × 0.2) + (rating × 0.2) + (recency × 0.2)`
- [ ] Hangfire daily job: compute + upsert `RecommendationScores` per profile
- [ ] "Because You Watched" — same genres + overlapping cast
- [ ] Redis caching of top-N recommendations per profile (TTL: 1h)

### Reviews & Social
- [ ] Reviews CRUD + soft-delete (admin moderation)
- [ ] Review likes (one per profile per review)

### Notifications
- [ ] In-app notifications (billing success/failure, new content, profile alerts)
- [ ] Mark notifications as read

### Admin & Moderation
- [ ] Admin dashboard stats (total users, active subscriptions, revenue)
- [ ] Content moderation (review soft-delete by admin)
- [ ] `PaymentMethod` management endpoints

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (or Docker)
- [Redis](https://redis.io/) (or Docker) *(for Phase 9+)*
- [Azure Storage Account](https://azure.microsoft.com/en-us/products/storage/blobs/) *(for Phase 6+)*
- [Stripe Account](https://stripe.com/) (test mode keys) *(for Phase 3+)*
- A [Brevo](https://www.brevo.com/) account (or any SMTP provider) for email

### Setup

```bash
# Clone the repo
git clone https://github.com/your-username/netflix-clone.git
cd netflix-clone/Backend-Netflix-CLone/Netflix-Clone

# Restore dependencies
dotnet restore
```

Add the following to `appsettings.json` (or use User Secrets):

```json
{
  "ConnectionStrings": {
    "Default": "Server=.;Database=NetflixCloneDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "SecretKey": "your-256-bit-secret-key-here",
    "Issuer": "NetflixCloneApi",
    "Audience": "NetflixCloneClient",
    "AccessTokenExpiryMinutes": 15,
    "RefreshTokenExpiryDays": 30
  },
  "EmailSettings": {
    "SmtpServer": "smtp-relay.brevo.com",
    "SmtpPort": "587",
    "SmtpUsername": "your-brevo-login@email.com",
    "SmtpPassword": "your-brevo-smtp-key",
    "SenderName": "Netflix Clone",
    "SenderEmail": "noreply@yourdomain.com"
  }
}
```

```bash
# Apply database migrations
dotnet ef database update --project NetflixClone.Persistence --startup-project NetflixClone.Api

# Run the API
dotnet run --project NetflixClone.Api
```

> On first startup, `DatabaseSeeder` automatically runs migrations and seeds: **6 plans**, **4 roles**, **1 admin account**, **25 genres**, and **16 persons**.

### Default Admin Account

| Field | Value |
|---|---|
| Email | `admin@netflixclone.dev` |
| Password | `Admin@123456!` |
| Role | `SuperAdmin` |

> ⚠️ **Change this password immediately after first login!** In production, load credentials from environment variables.

### Docker *(coming after MVP)*

```bash
docker-compose up -d
```

API will be available at `https://localhost:5001`  
Swagger API docs at `https://localhost:5001/swagger`

---

## 🧪 Running Tests

```bash
# Unit tests
dotnet test NetflixClone.Domain.Tests
dotnet test NetflixClone.Application.Tests

# Integration tests (requires Docker for Testcontainers)
dotnet test NetflixClone.Integration.Tests
```

---

## 📁 Project Structure (Detailed)

```
NetflixClone.Domain/
├── Common/
│   ├── Enums/              → SubscriptionStatus, MaturityRating, VideoQuality,
│   │                         BillingPeriod, ContentType, PersonRole, PaymentAttemptStatus
│   ├── Identity/           → JwtSettings (strongly-typed config)
│   └── Primitives/         → BaseEntity (Guid PK), AuditableEntity (CreatedAt, UpdatedAt)
└── Entities/
    ├── Identity/           → ApplicationUser, Profile, ProfilePreference, RefreshToken
    ├── Subscriptions/      → Plan, Subscription, Invoice, PaymentMethod
    ├── Catalog/            → Content, Season, Episode, Genre, Person, ContentGenre, ContentPerson
    ├── Media/              → StreamingSession
    └── Engagement/         → WatchHistory, Rating

NetflixClone.Application/
├── Contracts/
│   ├── IJwtTokenGeneration.cs
│   ├── IOtpService.cs
│   └── Infrastructure/
│       └── IEmailService.cs
├── Features/
│   └── Authentication/
│       └── Commands/
│           ├── Register/   → RegisterRequest, RegisterRequestHandler, RegisterResponse
│           ├── Login/      → LoginRequest, LoginRequestHandler, LoginResponse
│           ├── Logout/     → (🔜 in progress)
│           └── RefreshToken/ → (🔜 in progress)
├── Persistence/
│   └── IBaseRepository.cs
└── Services/
    ├── JwtTokenGeneration.cs   → HS256 JWT generation
    └── OtpService.cs           → Cryptographically secure 6-digit OTP

NetflixClone.Infrastructure/
└── Mail/
    └── EmailService.cs     → MailKit + Brevo SMTP; HTML email templates for OTP flows

NetflixClone.Persistence/
├── NetflixCloneDbContext.cs
├── NetflixCloneDbContextFactory.cs
├── PersistenceServiceRegistration.cs
├── Configurations/
│   ├── Identity/           → ApplicationUser, Profile, ProfilePreference, RefreshToken
│   ├── Subscriptions/      → Plan, Subscription, Invoice, PaymentMethod
│   ├── Catalog/            → Content, Season, Episode, Genre/ContentGenre, Person/ContentPerson
│   ├── Media/              → StreamingSession (+ EpisodeId FK)
│   └── Engagement/         → WatchHistory, Rating
├── Migrations/             → EF Core migrations
└── Seeds/
    ├── DatabaseSeeder.cs   → Orchestrator: Plans, Roles, Admin, Genres, Persons
    ├── GenreSeeder.cs      → 25 genres with stable GUIDs
    └── PersonSeeder.cs     → 16 directors & actors with stable GUIDs

NetflixClone.Api/
├── Program.cs              → DI wiring, middleware, endpoint registration
└── appsettings.json        → SQL Server, JwtSettings, Redis

tests/
├── NetflixClone.Domain.Tests/
├── NetflixClone.Application.Tests/
└── NetflixClone.Integration.Tests/
```

---

## 📄 License

This project is built as a portfolio/learning project. MIT License.

---

<div align="center">
Built with ❤️ as a portfolio project demonstrating production-grade .NET patterns
</div>
