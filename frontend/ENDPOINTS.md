# Frontend Routes & Pages

> App URL: `http://localhost:5173`

All frontend routes, which page component they render, what guard protects them, and which backend API calls each page makes.

---

## Quick-Access Summary (All 19 Routes)

| # | Route | Page | Guard | Description |
|---|-------|------|-------|-------------|
| | **PUBLIC** | | | |
| 1 | `/` | LandingPage | PublicOnly | Homepage with hero & feature cards |
| 2 | `/login` | LoginPage | PublicOnly | Sign in form |
| 3 | `/register` | RegisterPage | PublicOnly | Sign up form |
| 4 | `/confirm-email` | ConfirmEmailPage | — | OTP verification |
| 5 | `/forgot-password` | ForgotPasswordPage | — | Request password reset |
| 6 | `/reset-password` | ResetPasswordPage | — | Enter OTP + new password |
| 7 | `/plans` | PlansPage | — | Subscription plan cards |
| | **AUTHENTICATED** | | | |
| 8 | `/subscription/success` | SubscriptionSuccessPage | Protected | Post-Stripe redirect |
| 9 | `/profiles` | ProfileSelectorPage | Protected | "Who's watching?" + Manage mode |
| 10 | `/profiles/create` | CreateProfilePage | Protected | Add new profile form |
| 11 | `/profiles/edit` | EditProfilePage | Protected | Update/delete current profile |
| | **PROFILE REQUIRED** | | | |
| 12 | `/browse` | BrowsePage | Profile | Home feed with hero & rows |
| 13 | `/title/:id` | ContentDetailPage | Profile | Detail, rating & user reviews |
| 14 | `/search` | SearchPage | Profile | Search & filter content |
| 15 | `/my-list` | WatchHistoryPage | Profile | Watch history & continue |
| 16 | `/my-ratings` | MyRatingsPage | Profile | All rated titles with edit/delete |
| 17 | `/account` | AccountPage | Profile | Subscription, settings & security |
| 18 | `/watch/:id` | WatchPage | Profile | Fullscreen HLS video player |
| 19 | `*` | — | — | Catch-all → redirects to `/` |

> **Guard legend:**
> - `PublicOnly` — redirects to `/browse` if already logged in
> - `Protected` — requires account login, redirects to `/login` if not
> - `Profile` — requires profile-level JWT, redirects to `/profiles` if no profile selected
> - `—` — open to everyone

---

## Detailed Route Breakdown

### 1. `/` — Landing Page
**File:** `pages/landing/LandingPage.jsx`
**Layout:** None (standalone)
**Guard:** PublicOnly
**API Calls:** None
**Description:** Public marketing page with Netflix branding, "Unlimited movies, TV shows, and more" hero, feature cards, and Sign In / Get Started CTAs.

---

### 2. `/login` — Sign In
**File:** `pages/auth/LoginPage.jsx`
**Layout:** AuthLayout (centered card + NETFLIX logo)
**Guard:** PublicOnly
**API Calls:**
- `POST /api/auth/login` → get JWT tokens
**Flow:** On success → checks roles → `/plans` (if no subscription) or `/profiles` (if subscriber)

---

### 3. `/register` — Sign Up
**File:** `pages/auth/RegisterPage.jsx`
**Layout:** AuthLayout
**Guard:** PublicOnly
**API Calls:**
- `POST /api/auth/register` → create account
**Flow:** On success → redirects to `/confirm-email` with email in state

---

### 4. `/confirm-email` — Email OTP Verification
**File:** `pages/auth/ConfirmEmailPage.jsx`
**Layout:** AuthLayout
**Guard:** None
**API Calls:**
- `POST /api/auth/confirm-email` → verify OTP
- `POST /api/auth/resend-confirmation-otp` → resend code
**Flow:** On success → redirects to `/login`

---

### 5. `/forgot-password` — Request Password Reset
**File:** `pages/auth/ForgotPasswordPage.jsx`
**Layout:** AuthLayout
**Guard:** None
**API Calls:**
- `POST /api/auth/forgot-password` → send reset OTP
**Flow:** On success → shows "Check Your Email" with link to `/reset-password`

---

### 6. `/reset-password` — Reset Password
**File:** `pages/auth/ResetPasswordPage.jsx`
**Layout:** AuthLayout
**Guard:** None
**API Calls:**
- `POST /api/auth/reset-password` → verify OTP + set new password
**Flow:** On success → auto-redirects to `/login` after 2.5s

---

### 7. `/plans` — Choose Subscription Plan
**File:** `pages/subscription/PlansPage.jsx`
**Layout:** None (standalone)
**Guard:** None (public, but subscribe requires login)
**API Calls:**
- `GET /api/subscription/plans` → fetch available plans
- `POST /api/subscription/Subscripe` → start Stripe checkout
**Flow:** On subscribe → redirects to Stripe Checkout URL → Stripe redirects back to `/subscription/success`

---

### 8. `/subscription/success` — Post-Checkout
**File:** `pages/subscription/SubscriptionSuccessPage.jsx`
**Layout:** None (standalone)
**Guard:** Protected
**API Calls:** None
**Flow:** Shows success animation → auto-redirects to `/profiles` after 3s

---

### 9. `/profiles` — Who's Watching?
**File:** `pages/profiles/ProfileSelectorPage.jsx`
**Layout:** None (standalone, fullscreen)
**Guard:** Protected
**API Calls:**
- `GET /api/profile` → fetch user's profiles
- `POST /api/profile/login` → login to selected profile (returns profile JWT)
- `DELETE /api/profile` → delete a profile (in Manage mode)
**Flow:** Click profile → PIN modal (if locked) → on success → `/browse`
**Manage Mode:** Toggle "Manage Profiles" button → shows edit/delete overlay icons on each profile avatar

---

### 10. `/profiles/create` — Add Profile
**File:** `pages/profiles/CreateProfilePage.jsx`
**Layout:** None (standalone)
**Guard:** Protected
**API Calls:**
- `POST /api/profile/Create` → create profile + get profile JWT
**Flow:** On success → auto-redirects to `/browse`

---

### 11. `/profiles/edit` — Edit Profile
**File:** `pages/profiles/EditProfilePage.jsx`
**Layout:** None (standalone)
**Guard:** Protected
**API Calls:**
- `PATCH /api/profile/update` → partial update (name, age, PIN, language)
- `DELETE /api/profile` → permanently delete profile (Danger Zone)
**Components:** Partial update form with floating labels, language selector, age-based kids mode warning, danger zone with delete confirmation

---

### 12. `/browse` — Home Feed
**File:** `pages/browse/BrowsePage.jsx`
**Layout:** MainLayout (Navbar + content)
**Guard:** Profile
**API Calls:**
- `GET /api/catalog/trending` → hero banner + trending row
- `GET /api/profile/watch-history?ContinueWatchingOnly=true` → continue watching row
- `GET /api/catalog/content?ContentTypes=1` → movies row
- `GET /api/catalog/content?ContentTypes=2` → series row
- `GET /api/catalog/content?OrderedByRatingDesending=true` → top rated row
**Components:** Hero banner, content row carousels with horizontal scroll, card hover zoom

---

### 13. `/title/:id` — Content Detail
**File:** `pages/content/ContentDetailPage.jsx`
**Layout:** MainLayout
**Guard:** Profile
**API Calls:**
- `GET /api/catalog/content/{id}` → full content details, cast, metadata
- `GET /api/engagement/content/{id}/rating` → my existing rating
- `POST /api/engagement/content/{id}/rating` → add new rating
- `PATCH /api/engagement/rating/{ratingId}` → update existing rating
- `GET /api/engagement/content/{id}/ratings` → all user reviews (paginated, via MovieReviewsSection)
**Components:** Hero backdrop, metadata row, star rating widget, cast list, sidebar info, **MovieReviewsSection** (paginated user reviews with avatars and load more)

---

### 14. `/search` — Search & Filter
**File:** `pages/search/SearchPage.jsx`
**Layout:** MainLayout
**Guard:** Profile
**Query Params:** `?q=searchterm&type=1`
**API Calls:**
- `GET /api/catalog/content?SearchQuery=...&ContentTypes=...` → filtered results
**Components:** Search input, content type filter pills (Movies / TV Shows / Documentaries), results grid

---

### 15. `/my-list` — Watch History
**File:** `pages/history/WatchHistoryPage.jsx`
**Layout:** MainLayout
**Guard:** Profile
**API Calls:**
- `GET /api/profile/watch-history?ContinueWatchingOnly=false` → all watch history
**Components:** Content cards with progress bars, completion badges, resume playback on click

---

### 16. `/my-ratings` — My Ratings
**File:** `pages/ratings/MyRatingsPage.jsx`
**Layout:** MainLayout
**Guard:** Profile
**API Calls:**
- `GET /api/profile/my-ratings` → all ratings by this profile
- `DELETE /api/engagement/rating/{ratingId}` → remove a rating
**Components:** List of rated content with thumbnails, star display, review text, date, edit button (navigates to `/title/:id`), delete button with confirmation

---

### 17. `/account` — Account Settings
**File:** `pages/account/AccountPage.jsx`
**Layout:** MainLayout
**Guard:** Profile
**API Calls:**
- `GET /api/subscription/my-subscription` → current plan, status, period dates
- `POST /api/auth/logout` → sign out (sends refresh token to backend)
- `POST /api/auth/revoke-all` → revoke all refresh tokens on all devices
**Components:** User info with role badges, subscription status badge, quick links (Switch Profile, Edit Profile, Watch History, My Ratings), Security section (Sign Out of All Devices), Sign Out button

---

### 18. `/watch/:id` — Video Player
**File:** `pages/player/WatchPage.jsx`
**Layout:** None (fullscreen, no navbar)
**Guard:** Profile
**API Calls:**
- `GET /api/catalog/content/{id}/play` → HLS manifest URL + quality
**Components:** HLS.js video player, custom controls (play/pause, seek bar, volume, fullscreen), quality badge, auto-hide controls

---

## Route Guards Reference

| Guard | Component | Condition | Redirect |
|-------|-----------|-----------|----------|
| **PublicOnly** | `PublicOnlyRoute` | Has token + profile → | `/browse` |
| | | Has token, no profile → | `/profiles` |
| **Protected** | `ProtectedRoute` | No token → | `/login` |
| **Profile** | `ProfileRoute` | No token → | `/login` |
| | | Token but no profileId claim → | `/profiles` |

---

## Layouts

| Layout | Includes | Used By |
|--------|----------|---------|
| **AuthLayout** | NETFLIX logo header + centered card + footer | Login, Register, Confirm Email, Forgot/Reset Password |
| **MainLayout** | Navbar (scroll effect, search, dropdown) + content | Browse, Title, Search, My List, My Ratings, Account |
| **(none)** | Standalone fullscreen | Landing, Plans, Success, Profiles, Create/Edit Profile, Watch |
