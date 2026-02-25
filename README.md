[README.md](https://github.com/user-attachments/files/25549336/README.md)
# 🎬 NetflixClone — Backend API

<div align="center">

![ASP.NET 9](https://img.shields.io/badge/ASP.NET_9-Minimal_APIs-512BD4?style=for-the-badge&logo=dotnet)
![EF Core 9](https://img.shields.io/badge/EF_Core_9-Code--First-512BD4?style=for-the-badge&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-Database-CC2927?style=for-the-badge&logo=microsoftsqlserver)
![Redis](https://img.shields.io/badge/Redis-Caching-DC382D?style=for-the-badge&logo=redis)
![Azure](https://img.shields.io/badge/Azure_Blob-Storage-0078D4?style=for-the-badge&logo=microsoftazure)
![Stripe](https://img.shields.io/badge/Stripe-Payments-635BFF?style=for-the-badge&logo=stripe)

A **production-grade Netflix backend clone** built with ASP.NET 9, Clean Architecture, CQRS, and all the real-world patterns you'd find in a professional streaming platform.

[Features](#-features) • [Architecture](#-architecture) • [Tech Stack](#-tech-stack) • [API Docs](#-api-endpoints) • [Progress](#-build-progress) • [Getting Started](#-getting-started)

</div>

---

## ✨ Features

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

---

## 🏗️ Architecture

This project follows **Clean Architecture** with strict layer separation:

```
src/
├── NetflixClone.Domain/          → Entities, Value Objects, Domain Events, Aggregates
├── NetflixClone.Application/     → Use Cases (CQRS), DTOs, Interfaces, Validators
├── NetflixClone.Infrastructure/  → EF Core, Repos, External Services, Hangfire
└── NetflixClone.API/             → Minimal API endpoints, Middleware, DI wiring

tests/
├── NetflixClone.Domain.Tests/
├── NetflixClone.Application.Tests/
└── NetflixClone.Integration.Tests/
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
| 🎬 Catalog | Content, Season, Episode, Genre, Person, Tag |
| 📼 Media | VideoAsset, VideoVariant, SubtitleTrack, EncodingJob |
| ❤️ Engagement | WatchHistory, WatchlistItem, Rating, Review |
| 🔍 Discovery | RecommendationScore, TrendingSnapshot, SearchLog |
| 🔔 Notification | Notification, NotificationPreference |

---

## 🛠️ Tech Stack

| Concern | Choice | Notes |
|---|---|---|
| Framework | ASP.NET 9 Minimal APIs | Modern, performant |
| ORM | EF Core 9 (Code-First) | Migrations, owned types |
| CQRS | MediatR 12 | Commands, Queries, Notifications |
| Validation | FluentValidation | Application layer pipeline |
| Auth | JWT + Refresh Tokens | 15-min access / 30-day refresh with rotation |
| Caching | Redis (StackExchange.Redis) | Sessions, trending, recommendations, rate limits |
| Storage | Azure Blob Storage | Videos, images, subtitles, HLS manifests |
| Background Jobs | Hangfire | Encoding, billing, recommendation refresh |
| Search | SQL Server Full-Text Search | FTS on Title, Description, Cast |
| Payments | Stripe SDK (test mode) | Subscriptions, invoices, webhooks |
| Testing | xUnit + Moq + Testcontainers | Unit + integration tests |
| Docs | Scalar / Swagger | Auto-generated API docs |

---

## 📡 API Endpoints

<details>
<summary><strong>🔐 Auth — /api/auth</strong></summary>

| Method | Route | Description |
|---|---|---|
| POST | `/register` | Create account + send verification email |
| POST | `/login` | Returns access + refresh tokens |
| POST | `/refresh` | Rotate refresh token, return new access token |
| POST | `/logout` | Revoke refresh token |
| POST | `/verify-email` | Confirm email with token |
| POST | `/forgot-password` | Send password reset email |
| POST | `/reset-password` | Reset password with token |

</details>

<details>
<summary><strong>👤 Profiles — /api/profiles</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/` | List all profiles for the account |
| POST | `/` | Create profile (max enforced by plan) |
| PUT | `/{id}` | Update profile name/avatar/language |
| DELETE | `/{id}` | Delete profile |
| POST | `/{id}/switch` | Switch active profile (profile-scoped token) |
| PUT | `/{id}/pin` | Set/update Kids PIN |

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
| GET | `/trending` | Trending content (cached from snapshots) |
| POST | `/admin` | [Admin] Create movie/series |
| PUT | `/admin/{id}` | [Admin] Update content |
| DELETE | `/admin/{id}` | [Admin] Soft-delete content |
| POST | `/admin/{id}/seasons` | [Admin] Add season to series |
| POST | `/admin/seasons/{id}/episodes` | [Admin] Add episode |

</details>

<details>
<summary><strong>📼 Media — /api/media</strong></summary>

| Method | Route | Description |
|---|---|---|
| POST | `/upload-url` | [Admin] Get pre-signed Azure Blob upload URL |
| POST | `/assets/{assetId}/encode` | [Admin] Trigger encoding job |
| GET | `/assets/{assetId}/status` | [Admin] Check encoding status |
| GET | `/stream/{assetId}` | [Auth] Get HLS manifest URL (signed CDN URL) |
| POST | `/stream/{assetId}/start` | [Auth] Start streaming session |
| POST | `/stream/heartbeat` | [Auth] Keep session alive (every 30s) |
| POST | `/stream/{sessionId}/end` | [Auth] End streaming session |

</details>

<details>
<summary><strong>💳 Subscriptions — /api/subscriptions</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/plans` | List available plans |
| GET | `/me` | Get current subscription |
| POST | `/` | Subscribe to a plan (Stripe) |
| PUT | `/upgrade` | Upgrade plan (immediate + prorated) |
| PUT | `/downgrade` | Downgrade plan (next cycle) |
| DELETE | `/cancel` | Cancel (access until period end) |
| GET | `/invoices` | List billing history |

</details>

<details>
<summary><strong>❤️ Engagement — /api</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/watchlist` | Get My List |
| POST | `/watchlist/{contentId}` | Add to My List |
| DELETE | `/watchlist/{contentId}` | Remove from My List |
| GET | `/history` | Get watch history |
| GET | `/continue-watching` | Continue Watching row |
| PUT | `/history/{contentId}` | Update watch progress (upsert) |
| POST | `/ratings/{contentId}` | Rate content (ThumbUp/ThumbDown/Double) |
| DELETE | `/ratings/{contentId}` | Remove rating |
| GET | `/reviews/{contentId}` | Get reviews for content |
| POST | `/reviews/{contentId}` | Post a review |
| DELETE | `/reviews/{reviewId}` | Delete own review |
| POST | `/reviews/{reviewId}/like` | Like a review |

</details>

<details>
<summary><strong>🔍 Discovery — /api/discover</strong></summary>

| Method | Route | Description |
|---|---|---|
| GET | `/recommendations` | Top picks for active profile |
| GET | `/because-you-watched` | Based on recent watch history |
| GET | `/search?q=&filters=` | Full-text search with filters |
| GET | `/search/suggest?q=` | Autocomplete suggestions (top 5) |

</details>

---

## 📋 Subscription Plans

| Feature | Basic | Standard | Premium |
|---|---|---|---|
| Profiles | 2 | 2 | 4 |
| Concurrent Streams | 1 | 2 | 4 |
| Max Quality | 1080p | 1080p | 4K + HDR |
| Downloads | ❌ | 2 devices | 4 devices |

---

## 🗺️ Build Progress

### Phase 1 — Project Setup
> Solution structure, EF Core, migrations, base entities, global error handling

- [ ] Solution scaffolding (Clean Architecture folder structure)
- [ ] EF Core 9 setup with SQL Server
- [ ] Base entity interfaces (`IAuditableEntity`, `ISoftDeletable`)
- [ ] Global exception handling middleware
- [ ] MediatR pipeline (logging, validation, caching behaviors)
- [ ] FluentValidation integration
- [ ] Scalar / Swagger docs setup

---

### Phase 2 — Identity
> Register, Login, JWT, Refresh tokens, Email verify, Profiles CRUD

- [ ] Account registration + email verification flow
- [ ] JWT access token generation (15 min)
- [ ] Refresh token issuance + rotation (30 days)
- [ ] Logout (token revocation)
- [ ] Forgot password / reset password
- [ ] Profiles CRUD (max 5 per account, plan-enforced)
- [ ] Profile switching (profile-scoped JWT)
- [ ] Kids mode + PIN protection
- [ ] Role-based authorization (`SuperAdmin`, `ContentManager`, `SupportAgent`, `Subscriber`)

---

### Phase 3 — Subscriptions & Billing
> Plans, Stripe integration, Subscribe/Cancel/Upgrade, Invoice generation

- [ ] Seed Plans table (Basic / Standard / Premium)
- [ ] Stripe Customer creation on account registration
- [ ] Subscribe to plan (Stripe Subscription creation)
- [ ] Free trial (30-day, card required)
- [ ] Plan upgrade — immediate + prorated invoice
- [ ] Plan downgrade — deferred to next cycle
- [ ] Cancel subscription (`CancelAtPeriodEnd`)
- [ ] Stripe webhook handler (payment success, failure, invoice events)
- [ ] Dunning logic — retry 3×, then suspend
- [ ] Invoice history endpoint

---

### Phase 4 — Content Catalog
> Admin CRUD for Movies/Series/Seasons/Episodes, Genres, Persons, Images

- [ ] Content CRUD (Movie, Series, Documentary, Short) — TPH mapping
- [ ] Season + Episode CRUD
- [ ] Genre management + content-genre associations
- [ ] Person (cast/crew) management
- [ ] Content images (Portrait, Landscape, Hero, Logo, Thumbnail)
- [ ] Maturity ratings
- [ ] Content browse endpoint (paginated, filterable)
- [ ] Slug-based content detail endpoint

---

### Phase 5 — Media Upload & Encoding
> Azure Blob pre-signed URLs, EncodingJob tracking, VideoVariants

- [ ] Azure Blob pre-signed upload URL generation
- [ ] VideoAsset creation on upload initiation
- [ ] Hangfire job: trigger FFmpeg-based encoding (multi-resolution)
- [ ] VideoVariant records per resolution (360p → 4K)
- [ ] EncodingJob status tracking + retry logic (max 3 attempts)
- [ ] Subtitle track upload + storage
- [ ] Audio track (dubbed) management

---

### Phase 6 — Streaming
> HLS manifest URLs, StreamingSession concurrency enforcement, heartbeat

- [ ] Signed CDN URL generation for HLS manifests
- [ ] Stream start — concurrent session check vs. plan limit
- [ ] Heartbeat endpoint (updates `LastHeartbeatAt`)
- [ ] Stream end endpoint
- [ ] Background job: clean stale sessions every 5 min (Hangfire recurring)
- [ ] Quality cap enforcement per plan

---

### Phase 7 — Engagement
> WatchHistory, Continue Watching, Watchlist, Ratings, Reviews

- [ ] Watch progress upsert (pause/stop events)
- [ ] Auto-complete at 90% watched (`IsCompleted`)
- [ ] Continue Watching query (incomplete, ordered by `WatchedAt`)
- [ ] Re-watch reset logic
- [ ] Watchlist (My List) add/remove/list
- [ ] Ratings — ThumbUp / ThumbDown / DoubleThumbUp
- [ ] Rating change triggers `AverageRating` recalculation (domain event)
- [ ] Reviews CRUD + soft-delete (admin moderation)
- [ ] Review likes (one per profile per review)

---

### Phase 8 — Search
> FTS indexes, search endpoint, autocomplete, filters

- [ ] SQL Server Full-Text indexes on `Title`, `OriginalTitle`, `Description`
- [ ] Full-Text index on `Persons.FullName` (cast search)
- [ ] Search endpoint with filters (genre, year range, maturity, type, language)
- [ ] Autocomplete endpoint (top 5 via `CONTAINSTABLE`)
- [ ] Anonymous search logging (`SearchLogs`)

---

### Phase 9 — Recommendations
> Scoring algorithm, Hangfire daily job, Redis caching of top-N

- [ ] Rule-based scoring formula: `(genreMatch × 0.4) + (castMatch × 0.2) + (rating × 0.2) + (recency × 0.2)`
- [ ] Hangfire daily job: compute + upsert `RecommendationScores` per profile
- [ ] "Because You Watched" — same genres + overlapping cast
- [ ] "Top Picks" — highest rated in most-watched genres
- [ ] "Trending Now" — Hangfire job for daily/weekly/monthly `TrendingSnapshots`
- [ ] "New Releases" — ordered by `AvailableFrom DESC`
- [ ] Redis caching of top-N recommendations per profile (TTL: 1 hour)

---

### Phase 10 — Notifications & Admin
> Billing alerts, new content alerts, Admin dashboard stats, moderation

- [ ] In-app notification creation (billing success/failure, profile alerts, system messages)
- [ ] Mark notifications as read
- [ ] New content notification on `IsAvailable` toggle
- [ ] Admin dashboard stats (total users, active subscriptions, revenue)
- [ ] Content moderation (review soft-delete by admin)

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (or Docker)
- [Redis](https://redis.io/) (or Docker)
- [Azure Storage Account](https://azure.microsoft.com/en-us/products/storage/blobs/) (or Azurite emulator)
- [Stripe Account](https://stripe.com/) (test mode keys)

### Setup

```bash
# Clone the repo
git clone https://github.com/your-username/netflix-clone.git
cd netflix-clone

# Restore dependencies
dotnet restore

# Configure user secrets
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.;Database=NetflixClone;Trusted_Connection=True"
dotnet user-secrets set "Jwt:Secret" "your-super-secret-key"
dotnet user-secrets set "Stripe:SecretKey" "sk_test_..."
dotnet user-secrets set "Azure:BlobConnectionString" "DefaultEndpointsProtocol=https;..."
dotnet user-secrets set "Redis:ConnectionString" "localhost:6379"

# Apply database migrations
dotnet ef database update --project src/NetflixClone.Infrastructure --startup-project src/NetflixClone.API

# Run the API
dotnet run --project src/NetflixClone.API
```

### Docker (coming soon)

```bash
docker-compose up -d
```

API will be available at `https://localhost:5001`
Scalar API docs at `https://localhost:5001/scalar`

---

## 🧪 Running Tests

```bash
# Unit tests
dotnet test tests/NetflixClone.Domain.Tests
dotnet test tests/NetflixClone.Application.Tests

# Integration tests (requires Docker for Testcontainers)
dotnet test tests/NetflixClone.Integration.Tests
```

---

## 📁 Project Structure (Detailed)

```
src/
├── NetflixClone.Domain/
│   ├── Identity/          → Account, Profile, RefreshToken, Role aggregates
│   ├── Subscription/      → Plan, Subscription, Invoice, PaymentMethod
│   ├── Catalog/           → Content, Season, Episode, Genre, Person
│   ├── Media/             → VideoAsset, VideoVariant, EncodingJob
│   ├── Engagement/        → WatchHistory, Rating, Review, Watchlist
│   ├── Discovery/         → RecommendationScore, TrendingSnapshot
│   └── Shared/            → Base entities, domain events, value objects
│
├── NetflixClone.Application/
│   ├── Identity/          → Register, Login, RefreshToken, Profile commands/queries
│   ├── Subscription/      → Subscribe, Upgrade, Cancel, Invoice queries
│   ├── Catalog/           → Browse, GetDetail, Admin CRUD commands
│   ├── Media/             → Upload, Encode, Stream commands
│   ├── Engagement/        → WatchHistory, Rating, Review commands
│   ├── Discovery/         → Search, Recommendations, Trending queries
│   ├── Common/            → Pipeline behaviors (validation, logging, caching)
│   └── Interfaces/        → Repository & service contracts
│
├── NetflixClone.Infrastructure/
│   ├── Persistence/       → ApplicationDbContext, EF configurations, migrations
│   ├── Repositories/      → EF Core repository implementations
│   ├── Identity/          → JWT, token hashing services
│   ├── Storage/           → Azure Blob service
│   ├── Payment/           → Stripe service, webhook handler
│   ├── Caching/           → Redis implementation
│   ├── Jobs/              → Hangfire job definitions and schedulers
│   └── Search/            → Full-text search implementation
│
└── NetflixClone.API/
    ├── Endpoints/         → Minimal API route definitions per context
    ├── Middleware/         → Exception handling, rate limiting
    └── DependencyInjection → Service registration extensions
```

---

## 📄 License

This project is built as a portfolio/learning project. MIT License.

---

<div align="center">
Built with ❤️ as a portfolio project demonstrating production-grade .NET patterns
</div>
