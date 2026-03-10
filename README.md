# 🎬 NetflixClone — Backend API

<div align="center">

![ASP.NET 9](https://img.shields.io/badge/ASP.NET_9-Minimal_APIs-512BD4?style=for-the-badge&logo=dotnet)
![EF Core 9](https://img.shields.io/badge/EF_Core_9-Code--First-512BD4?style=for-the-badge&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-Database-CC2927?style=for-the-badge&logo=microsoftsqlserver)
![Redis](https://img.shields.io/badge/Redis-Caching-DC382D?style=for-the-badge&logo=redis)
![Azure](https://img.shields.io/badge/Azure_Blob-Storage-0078D4?style=for-the-badge&logo=microsoftazure)
![Stripe](https://img.shields.io/badge/Stripe-Payments-635BFF?style=for-the-badge&logo=stripe)
![Status](https://img.shields.io/badge/Status-MVP_In_Progress-F59E0B?style=for-the-badge)

A **production-grade Netflix backend clone** built with ASP.NET 9, Clean Architecture, CQRS, and real-world patterns used in professional streaming platforms.


[MVP Scope](#-mvp-scope) • [Architecture](#️-architecture) • [Tech Stack](#️-tech-stack) • [API Docs](#-api-endpoints) • [Progress](#-build-progress) • [Getting Started](#-getting-started)

</div>

---

## 🎯 MVP Scope
The MVP strips the full project bible down to what actually matters for a first working version — enough to demonstrate real-world patterns without over-engineering.

| Feature | MVP Approach | Status |
|---|---|---|
| JWT Authentication | Register → Verify Email → Login → Refresh Token → Logout | ✅ Done |
| Multi-Profile Support | Up to N profiles per plan, age attribute, optional PIN, preferences | ✅ Done |
| Stripe Subscriptions | Plans by profile count & billing period, Stripe Checkout, invoices by email | 🔄 In Progress |
| Video Streaming | Simplified: direct MP4 SAS URLs from Azure Blob, concurrency enforcement | 🔜 Next |
| Media Pipeline | Admin uploads → Azure Blob → store URL → mark ready. No FFmpeg | 🔜 Next |
| Full-Text Search | SQL Server FTS on Title, Description, Cast | 🔜 Next |
| Engagement | Watch history, continue watching, thumb ratings — add after core is done | 🔄 In Progress |
| Trending | ViewCount++ on every stream start + daily Hangfire snapshot | 🔜 Next |
| Role-Based Access | Subscriber \| ContentManager \| SuperAdmin — via JWT claims | 🔜 Next |
| Email Flows | Confirm email, reset password, reset PIN, send invoice | ✅ Done |

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
- 📋 **Engagement** — Watch history, continue watching, My List, ratings (ThumbUp / Double Thumb), reviews & likes
- 🤖 **Recommendations** — Rule-based scoring engine (genre match + cast + rating + recency), pre-computed daily by Hangfire, cached in Redis
- 📈 **Trending** — Daily/Weekly/Monthly snapshots refreshed by background jobs
- 🔔 **Notifications** — In-app notifications for billing events, new content & profile alerts
- 🛡️ **Role-Based Access** — SuperAdmin, ContentManager, SupportAgent, Subscriber
- 📧 **Email Flows** — Verification, password reset, PIN reset, invoice receipts, payment failure alerts

---

## 🏗️ Architecture
This project follows **Clean Architecture** with strict layer separation:

```
src
 ├── API
 │    └── NetflixClone.Api          → Minimal API endpoints, Middleware, DI wiring
 ├── Core
 │    ├── NetflixClone.Application  → Use Cases (CQRS), DTOs, Interfaces, Validators
 │    └── NetflixClone.Domain       → Entities, Value Objects, Domain Events, Aggregates
 └── Infrastructure
      ├── NetflixClone.Infrastructure → Repos, External Services, Hangfire
      └── NetflixClone.Persistence    → NetflixCloneDbContext, EF configurations, migrations

tests
├── NetflixClone.Domain.Tests
├── NetflixClone.Application.Tests
└── NetflixClone.Integration.Tests
```

### Layer Rules
- **Domain** has zero external dependencies
- **Application** depends only on Domain; defines interfaces implemented by Infrastructure
- **Infrastructure** implements Application interfaces (EF Core, Azure SDK, Stripe SDK, Redis)
- **API** references Application only — no domain logic in endpoints
- All cross-cutting concerns (logging, validation, caching) handled via **MediatR pipeline behaviors**

### Bounded Contexts
| Context | Core Entities |
|---|---|
| 🔐 Identity | Account, Profile, RefreshToken, Role |
| 💳 Subscription | Plan, Subscription, Invoice, PaymentMethod |
| 🎬 Catalog | Content, Season, Episode, Genre, Person |
| 📼 Media | VideoAsset, StreamingSession |
| ❤️ Engagement | WatchHistory, Rating |
| 📈 Discovery | TrendingSnapshot |
| 🔔 Notification | Notification *(post-MVP)* |

---

## 🛠️ Tech Stack
| Concern | Choice | Notes |
|---|---|---|
| Framework | ASP.NET 9 Minimal APIs | Modern, performant |
| ORM | EF Core 9 (Code-First) | Migrations, owned types |
| CQRS | MediatR 12 | Commands, Queries, Notifications |
| Validation | FluentValidation | Application layer pipeline |
| Auth | JWT + Refresh Tokens | 15-min access / 30-day refresh with rotation |
| Caching | Redis (StackExchange.Redis) | Trending, recommendations, rate limits |
| Storage | Azure Blob Storage | Videos, images (MVP: direct MP4 URL) |
| Background Jobs | Hangfire | Trending snapshot, session cleanup |
| Search | SQL Server Full-Text Search | FTS on Title, Description, Cast |
| Payments | Stripe Checkout (test mode) | Subscriptions, invoices, webhooks |
| Email | SendGrid / Resend | Verification, invoices, alerts |
| Testing | xUnit + Moq + Testcontainers | Unit + integration tests |
| Docs | Scalar / Swagger | Auto-generated API docs |

---

## 📡 API Endpoints

### MVP Endpoints

<details>
<summary><strong>🔐 Auth — /api/auth</strong></summary>

| Method | Route | Description |
|---|---|---|
| POST | `/register` | Create account + send verification email |
| POST | `/verify-email` | Confirm email with OTP |
| POST | `/resend-confirmation` | Resend email confirmation OTP |
| POST | `/login` | Returns access JWT + sets refresh token cookie |
| POST | `/refresh` | Rotate refresh token, return new access token |
| POST | `/logout` | Revoke refresh token |
| POST | `/forgot-password` | Send password reset email (1h token) |
| POST | `/reset-password` | Reset password with token |

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
| POST | `/upgrade` | Upgrade current plan |
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
| GET | `/{id}` | Get content detail by ID |
| GET | `/genres` | List all genres |
| GET | `/trending` | Trending content (from ViewCount snapshots) |
| GET | `/new-releases` | Recently added content |
| POST | `/admin` | [ContentManager] Create movie/series/documentary |
| PUT | `/admin/{id}` | [ContentManager] Update content |
| DELETE | `/admin/{id}` | [ContentManager] Soft-delete content |
| PATCH | `/admin/{id}/publish` | [ContentManager] Toggle IsAvailable |
| POST | `/admin/{id}/seasons` | [ContentManager] Add season to series |
| POST | `/admin/seasons/{id}/episodes` | [ContentManager] Add episode |
| POST | `/genres` | [ContentManager] Create genre |
| PUT | `/genres/{id}` | [ContentManager] Update genre |
| DELETE | `/genres/{id}` | [ContentManager] Delete genre |

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
<summary><strong>❤️ Engagement — /api (Optional Phase)</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/history` | Get watch history for active profile |
| GET | `/continue-watching` | Get incomplete content ordered by last watched |
| PUT | `/history/{contentId}` | Upsert watch progress |
| POST | `/ratings/{contentId}` | Rate content (ThumbUp / ThumbDown / DoubleThumbUp) |
| DELETE | `/ratings/{contentId}` | Remove rating |
| GET | `/ratings` | Get all ratings for active profile |

</details>

---

### Post-MVP Endpoints *(planned for after MVP is complete)*

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

### MVP Plans
| Plan | Profiles | Concurrent Streams | Monthly | Yearly |
|---|---|---|---|---|
| Basic | 1 | 1 | $8.99 | $89.99 |
| Standard | 3 | 2 | $13.99 | $139.99 |
| Premium | 5 | 4 | $17.99 | $179.99 |

### Post-MVP Additions
- Download support (Standard: 2 devices, Premium: 4 devices)
- 4K + HDR quality cap per plan
- Plan upgrade (immediate + prorated) / downgrade (next cycle)
- Dunning logic — retry 3×, then suspend

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
- [x] EF Core Fluent API configurations for all bounded contexts
- [x] Base entity interfaces (`IAuditableEntity`, `ISoftDeletable`)
- [x] Domain entities — Identity, Subscriptions, Catalog, Media, Engagement
- [x] `VideoQuality` enum + `MaturityRating`, `SubscriptionStatus` enums
- [x] `StreamingSession` entity
- [x] Data seeding — Genres, Persons, initial seed structure
- [x] Initial EF Core migration generated
- [x] Repository contract (`IBaseRepository`) in Application layer
- [x] `appsettings.json` configured with connection string
- [x] `Program.cs` updated to register Persistence services
- [ ] Global exception handling middleware
- [ ] MediatR pipeline (logging, validation, caching behaviors)

---

### Phase 2 — Identity ✅
> Register, Login, JWT, Refresh tokens, Email verification, Profiles CRUD

- [x] Account registration
- [x] Email confirmation via OTP flow
- [x] Resend email confirmation OTP
- [x] JWT access token generation (15 min)
- [x] Refresh token issuance + rotation (30 days, HttpOnly cookie)
- [x] Logout (token revocation)
- [x] Forgot password (send reset email with 1h token)
- [x] Reset password with token
- [x] Profiles CRUD — Create, Update, Delete, Get all profiles
  - Plan `MaxProfiles` enforcement wired in profile creation
- [ ] Profile switching (new JWT with `profileId` claim)
- [ ] Kids mode auto-set when age < 13
- [ ] Optional PIN (BCrypt-hashed, required on switch if set)
- [ ] Profile preferences (Genre / Actor / Director)
- [ ] Role-based authorization (`SuperAdmin`, `ContentManager`, `Subscriber`)

---

### Phase 3 — Subscriptions & Billing 🔄
> Stripe Checkout, subscription lifecycle, invoices, webhooks

- [x] Subscription plan listing (`GetPlans` query)
- [x] Subscribe to a plan (`SubscribePlan` command)
- [x] Upgrade plan (`UpgradePlan` command)
- [x] Get current subscription details (`GetMySubscription` query)
- [x] Create invoice (`CreateInvoice` command)
- [ ] Seed Plans table (Basic / Standard / Premium — monthly & yearly)
- [ ] Stripe Customer creation after email verification
- [ ] Subscribe via Stripe Checkout session (hosted page, no card data on server)
- [ ] 30-day free trial support
- [ ] Stripe webhook handler:
  - `checkout.session.completed` → create Subscription + send invoice email
  - `invoice.payment_succeeded` → mark Invoice paid, send receipt
  - `invoice.payment_failed` → increment attempt, suspend after 3 failures
  - `customer.subscription.deleted` → set Status=Canceled
  - `customer.subscription.updated` → handle plan change
- [ ] Cancel subscription (`CancelAtPeriodEnd=true`)
- [ ] Invoice history endpoint

---

### Phase 4 — Enforce Plan Limits + RBAC 🔜
> Plan limits on profiles, role enforcement on endpoints

- [ ] Enforce `Plan.MaxProfiles` on profile creation
- [ ] Require active subscription for streaming
- [ ] Role-based authorization on all endpoints
- [ ] Seed SuperAdmin account

---

### Phase 5 — Content Catalog 🔄
> Admin CRUD for Movies/Series/Documentaries, Genres, Persons

- [x] Content CRUD — Create, Update, Delete (soft), MakeAvailable (publish toggle)
- [x] `GetAllContent` — paginated browse endpoint
- [x] `GetContentById` — content detail endpoint
- [x] `GetTrendingContent` — trending query
- [x] Genre management — Create, Update, Delete genres
- [ ] Season + Episode CRUD
- [ ] Person (cast/crew) management (already seeded)
- [ ] Maturity ratings enforcement
- [ ] Content browse by genre (filterable)
- [ ] Slug-based content detail endpoint
- [ ] New Releases endpoint (`AvailableFrom DESC`)

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

### Phase 10 — Engagement 🔄
> Watch history, Continue Watching, Ratings

- [x] Add rating (ThumbUp / ThumbDown / DoubleThumbUp)
- [x] Delete rating
- [x] Get ratings for active profile
- [x] Get watch history
- [ ] Watch progress upsert on stream end (`StoppedAtSeconds`, `TotalDurationSeconds`)
- [ ] Auto-complete at 90% watched (`IsCompleted=true`)
- [ ] Continue Watching query (incomplete, ordered by `WatchedAt DESC`)
- [ ] Re-watch reset logic
- [ ] Rating change triggers `AverageRating` recalculation

---

## 🔵 Post-MVP Roadmap *(Full Project Bible)*
> These features are out of scope for the MVP but are fully designed and planned.

### Advanced Streaming
- [ ] HLS manifest delivery (replace direct MP4 URLs)
- [ ] FFmpeg-based multi-resolution encoding pipeline (360p → 4K)
- [ ] `EncodingJob` status tracking + retry logic (max 3 attempts)
- [ ] `VideoVariant` records per resolution
- [ ] Subtitle track upload & storage
- [ ] Quality cap enforcement per plan (1080p / 4K)
- [ ] Download support (Standard: 2 devices, Premium: 4 devices)

### Plan Upgrades & Dunning
- [ ] Plan upgrade — immediate + prorated invoice
- [ ] Plan downgrade — deferred to next cycle
- [ ] Dunning logic — retry 3×, then suspend account

### Recommendations Engine
- [ ] Rule-based scoring: `(genreMatch × 0.4) + (castMatch × 0.2) + (rating × 0.2) + (recency × 0.2)`
- [ ] Hangfire daily job: compute + upsert `RecommendationScores` per profile
- [ ] "Because You Watched" — same genres + overlapping cast
- [ ] "Top Picks" — highest rated in most-watched genres
- [ ] Redis caching of top-N recommendations per profile (TTL: 1h)

### Reviews & Social
- [ ] Reviews CRUD + soft-delete (admin moderation)
- [ ] Review likes (one per profile per review)

### Notifications
- [ ] In-app notifications (billing success/failure, new content, profile alerts)
- [ ] Mark notifications as read
- [ ] New content notification on `IsAvailable` toggle

### Admin & Moderation
- [ ] Admin dashboard stats (total users, active subscriptions, revenue)
- [ ] Content moderation (review soft-delete by admin)
- [ ] `SupportAgent` role for user management
- [ ] `PaymentMethod` management endpoints

---

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (or Docker)
- [Redis](https://redis.io/) (or Docker)
- [Azure Storage Account](https://azure.microsoft.com/en-us/products/storage/blobs/) (or Azurite emulator)
- [Stripe Account](https://stripe.com/) (test mode keys)
- SendGrid or [Resend](https://resend.com/) account (for email)

### Setup
```bash
# Clone the repo
git clone https://github.com/Ahmed-Walled10/Netflix-Clone.git
cd Netflix-Clone/Backend-Netflix-CLone/Netflix-Clone

# Restore dependencies
dotnet restore

# Configure user secrets (run from NetflixClone.Api project dir)
dotnet user-secrets set "ConnectionStrings:Default" "Server=.;Database=NetflixClone;Trusted_Connection=True"
dotnet user-secrets set "Jwt:Secret" "your-256-bit-secret-key-here"
dotnet user-secrets set "Jwt:Issuer" "NetflixClone"
dotnet user-secrets set "Jwt:Audience" "NetflixClone"
dotnet user-secrets set "Stripe:SecretKey" "sk_test_..."
dotnet user-secrets set "Stripe:WebhookSecret" "whsec_..."
dotnet user-secrets set "Azure:BlobConnectionString" "DefaultEndpointsProtocol=https;..."
dotnet user-secrets set "Azure:ContainerName" "netflix-videos"
dotnet user-secrets set "Redis:ConnectionString" "localhost:6379"
dotnet user-secrets set "Email:ApiKey" "SG.xxxx"

# Apply database migrations
dotnet ef database update --project NetflixClone.Persistence --startup-project NetflixClone.Api

# Run the API
dotnet run --project NetflixClone.Api
```

### Docker *(coming after MVP)*
```bash
docker-compose up -d
```

API will be available at `https://localhost:5001`  
Scalar API docs at `https://localhost:5001/scalar`

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
├── Entities/
│   ├── Identity/           → ApplicationUser, Profile, RefreshToken
│   ├── Subscriptions/      → Plan, Subscription, Invoice, PaymentMethod
│   ├── Catalog/            → Content, Season, Episode, Genre, Person
│   ├── Media/              → StreamingSession
│   └── Engagement/         → WatchHistory, Rating
├── Common/
│   ├── Enums/              → SubscriptionStatus, MaturityRating, VideoQuality, ...
│   ├── Primitives/         → Base entity, IAuditableEntity, ISoftDeletable
│   └── Identity/           → Value objects

NetflixClone.Application/
├── Persistence/            → IBaseRepository (repository contracts)
└── Features/
    ├── Authentication/     → Register, Login, Logout, RefreshToken, ForgotPassword, ResetPassword, EmailConfirmation
    ├── Profiles/           → CreateProfile, UpdateProfile, DeleteProfile, GetProfiles
    ├── Subscriptions/      → SubscribePlan, UpgradePlan, GetMySubscription
    ├── Subscription-Plans/ → GetPlans
    ├── Invoices/           → CreateInvoice
    ├── Content/            → CreateContent, UpdateContent, DeleteContent, MakeContentAvailable, GetAllContent, GetContentById, GetTrendingContent
    ├── Content-genres/     → CreateGenre, UpdateGenre, DeleteGenre
    └── Engagement/         → AddRating, DeleteRating, GetRatings, GetWatchHistory

NetflixClone.Persistence/
├── NetflixCloneDbContext.cs
├── NetflixCloneDbContextFactory.cs
├── Configurations/         → EF Fluent API configs (Identity, Subscriptions, Catalog, Media, Engagement)
├── Migrations/             → EF Core migrations
└── Seeds/                  → DatabaseSeeder, GenreSeeder, PersonSeeder

NetflixClone.Infrastructure/
└── [Repositories, JWT, Azure Blob, Stripe, Redis, Hangfire, Email]

NetflixClone.Api/
├── Program.cs              → DI wiring, middleware, endpoint registration
└── appsettings.json        → Configuration

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
