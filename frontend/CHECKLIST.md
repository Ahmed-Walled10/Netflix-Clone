# Netflix Clone Frontend — Full Checklist

> Comparing all backend endpoints against frontend implementation.
> ✅ = Done | ❌ = Not built | ⚠️ = Partial

---

## USER UI (Subscriber)

### Authentication (`AuthenticationController`)

| # | Backend Endpoint | Frontend Page/Feature | Status |
|---|-----------------|----------------------|--------|
| 1 | `POST /api/auth/register` | RegisterPage | ✅ |
| 2 | `POST /api/auth/login` | LoginPage | ✅ |
| 3 | `POST /api/auth/logout` | Navbar + AccountPage | ✅ |
| 4 | `POST /api/auth/confirm-email` | ConfirmEmailPage | ✅ |
| 5 | `POST /api/auth/resend-confirmation-otp` | ConfirmEmailPage | ✅ |
| 6 | `POST /api/auth/forgot-password` | ForgotPasswordPage | ✅ |
| 7 | `POST /api/auth/reset-password` | ResetPasswordPage | ✅ |
| 8 | `POST /api/auth/refresh-token` | Axios interceptor (auto) | ✅ |
| 9 | `POST /api/auth/revoke-token` | (revoke single token) | ⚠️ Not needed — covered by logout |
| 10 | `POST /api/auth/revoke-all` | AccountPage "Sign Out All Devices" | ✅ |

### Profiles (`ProfileController`)

| # | Backend Endpoint | Frontend Page/Feature | Status |
|---|-----------------|----------------------|--------|
| 1 | `GET /api/profile` | ProfileSelectorPage | ✅ |
| 2 | `POST /api/profile/Create` | CreateProfilePage | ✅ |
| 3 | `DELETE /api/profile` | EditProfilePage + ProfileSelector manage mode | ✅ |
| 4 | `POST /api/profile/login` | ProfileSelectorPage (PIN modal) | ✅ |
| 5 | `POST /api/profile/switch` | API service wired, no dedicated UI | ⚠️ Uses login instead |
| 6 | `PATCH /api/profile/update` | EditProfilePage | ✅ |
| 7 | `GET /api/profile/watch-history` | BrowsePage + WatchHistoryPage | ✅ |
| 8 | `GET /api/profile/my-ratings` | MyRatingsPage | ✅ |

### Subscriptions (`SubscriptionController` + `SubscriptionPlansController`)

| # | Backend Endpoint | Frontend Page/Feature | Status |
|---|-----------------|----------------------|--------|
| 1 | `GET /api/subscription/plans` | PlansPage | ✅ |
| 2 | `POST /api/subscription/Subscripe` | PlansPage → Stripe redirect | ✅ |
| 3 | `GET /api/subscription/my-subscription` | AccountPage | ✅ |
| 4 | `POST /api/subscription/webhook` | Backend-only (Stripe webhook) | ✅ N/A |

### Catalog — User Endpoints (`CatalogController`)

| # | Backend Endpoint | Frontend Page/Feature | Status |
|---|-----------------|----------------------|--------|
| 1 | `GET /api/catalog/content` | BrowsePage + SearchPage | ✅ |
| 2 | `GET /api/catalog/content/{id}` | ContentDetailPage | ✅ |
| 3 | `GET /api/catalog/trending` | BrowsePage hero + trending row | ✅ |
| 4 | `GET /api/catalog/content/{id}/play` | WatchPage (HLS player) | ✅ |
| 5 | `GET /api/catalog/person/{id}` | PersonDetailPage (bio, filmography) | ✅ |

### Engagement (`EngagementController`)

| # | Backend Endpoint | Frontend Page/Feature | Status |
|---|-----------------|----------------------|--------|
| 1 | `POST /api/engagement/content/{id}/rating` | ContentDetailPage (star widget) | ✅ |
| 2 | `PATCH /api/engagement/rating/{ratingId}` | ContentDetailPage (update star) | ✅ |
| 3 | `DELETE /api/engagement/rating/{ratingId}` | MyRatingsPage (delete button) | ✅ |
| 4 | `GET /api/engagement/content/{id}/ratings` | ContentDetailPage (MovieReviewsSection) | ✅ |
| 5 | `GET /api/engagement/content/{id}/rating` | ContentDetailPage (my rating) | ✅ |

---

## ADMIN UI (SuperAdmin / ContentManager)

### Subscription Plans Management (`SubscriptionPlansController`)

| # | Backend Endpoint | Frontend Page/Feature | Status |
|---|-----------------|----------------------|--------|
| 1 | `POST /api/subscription/plans` | Add new plan | ❌ |
| 2 | `DELETE /api/subscription/plans` | Delete plan | ❌ |
| 3 | `PATCH /api/subscription/plans/{id}` | Update plan | ❌ |

### Content Management (`CatalogController`)

| # | Backend Endpoint | Frontend Page/Feature | Status |
|---|-----------------|----------------------|--------|
| 1 | `POST /api/catalog/content` | Create content (movie/series) | ❌ |
| 2 | `PATCH /api/catalog/content/{id}` | Update content | ❌ |
| 3 | `DELETE /api/catalog/content/{id}` | Delete content | ❌ |
| 4 | `POST /api/catalog/content/{id}/images` | Upload thumbnail + hero image | ❌ |
| 5 | `POST /api/catalog/content/{id}/video` | Upload video (Cloudinary → HLS) | ❌ |
| 6 | `POST /api/catalog/episodes/{id}/thumbnail` | Upload episode thumbnail | ❌ |

### Genre Management (`CatalogController`)

| # | Backend Endpoint | Frontend Page/Feature | Status |
|---|-----------------|----------------------|--------|
| 1 | `POST /api/catalog/genres` | Create genre | ❌ |
| 2 | `DELETE /api/catalog/genres/{id}` | Delete genre | ❌ |
| 3 | `PATCH /api/catalog/genres/{id}` | Update genre | ❌ |

### Person/Cast Management (`CatalogController`)

| # | Backend Endpoint | Frontend Page/Feature | Status |
|---|-----------------|----------------------|--------|
| 1 | `POST /api/catalog/person` | Create person | ❌ |
| 2 | `DELETE /api/catalog/person/{id}` | Delete person | ❌ |
| 3 | `PATCH /api/catalog/person/{id}` | Update person | ❌ |
| 4 | `POST /api/catalog/person/{id}/photo` | Upload person photo | ❌ |
| 5 | `GET /api/catalog/person/{id}` | View person detail | ❌ |

---

## OTHER MISSING FEATURES (UX / Polish)

### Pages & Components

| # | Feature | Description | Status |
|---|---------|-------------|--------|
| 1 | Person Detail Page | Click on a cast member → see bio, filmography | ✅ |
| 2 | Genre Browsing | Filter content by genre (genre list UI) | ❌ |
| 3 | Episode Selector | For Series: list seasons/episodes, pick episode to play | ❌ |
| 4 | Toast Notifications | Global success/error toasts instead of inline errors | ❌ |
| 5 | Mobile Responsive Menu | Hamburger menu for navbar on small screens | ❌ |
| 6 | Change Password (logged in) | Let user change password while authenticated | ❌ No endpoint |
| 7 | 404 / Error Pages | Styled "not found" and error boundary pages | ❌ |
| 8 | Loading Skeletons | Skeleton shimmer on all pages (only BrowsePage has it) | ⚠️ Partial |
| 9 | Infinite Scroll / Pagination | Browse + Search currently load one page | ❌ |
| 10 | Profile Avatar Upload | Upload custom avatar image (currently text-based) | ❌ |

### Technical / Infrastructure

| # | Feature | Description | Status |
|---|---------|-------------|--------|
| 1 | Environment Variables | Move `http://localhost:5120` to `.env.local` | ❌ |
| 2 | Code Splitting | Lazy-load routes with `React.lazy()` | ❌ |
| 3 | Error Boundary | Global React error boundary component | ❌ |
| 4 | SEO per Page | Dynamic `<title>` and meta tags per route | ❌ |
| 5 | PWA / Offline | Service worker, manifest.json | ❌ |
| 6 | Unit Tests | Jest / Vitest component tests | ❌ |
| 7 | E2E Tests | Playwright / Cypress integration tests | ❌ |

---

## SUMMARY

| Category | Done | Missing | Total |
|----------|------|---------|-------|
| **User Auth** | 9 | 0 | 9 |
| **User Profiles** | 7 | 0 | 7 |
| **User Subscriptions** | 3 | 0 | 3 |
| **User Catalog** | 5 | 0 | 5 |
| **User Engagement** | 5 | 0 | 5 |
| **Admin Plans** | 0 | 3 | 3 |
| **Admin Content** | 0 | 6 | 6 |
| **Admin Genres** | 0 | 3 | 3 |
| **Admin Person/Cast** | 0 | 5 | 5 |
| **UX Polish** | 2 | 8 | 10 |
| **Technical** | 0 | 7 | 7 |
| **TOTAL** | **31** | **32** | **63** |

> **User UI: 29/29 endpoints done (100%)**
> **Admin UI: 0/17 endpoints done (0%)**
> **UX/Tech: 2/17 items done (12%)**
