import { useEffect } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useAuthStore } from './stores/authStore';
import { ProtectedRoute, ProfileRoute, PublicOnlyRoute } from './components/common/RouteGuards';
import MainLayout from './layouts/MainLayout';
import AuthLayout from './layouts/AuthLayout';

// Pages
import LandingPage from './pages/landing/LandingPage';
import LoginPage from './pages/auth/LoginPage';
import RegisterPage from './pages/auth/RegisterPage';
import ConfirmEmailPage from './pages/auth/ConfirmEmailPage';
import ForgotPasswordPage from './pages/auth/ForgotPasswordPage';
import ResetPasswordPage from './pages/auth/ResetPasswordPage';
import PlansPage from './pages/subscription/PlansPage';
import SubscriptionSuccessPage from './pages/subscription/SubscriptionSuccessPage';
import ProfileSelectorPage from './pages/profiles/ProfileSelectorPage';
import CreateProfilePage from './pages/profiles/CreateProfilePage';
import EditProfilePage from './pages/profiles/EditProfilePage';
import BrowsePage from './pages/browse/BrowsePage';
import ContentDetailPage from './pages/content/ContentDetailPage';
import WatchPage from './pages/player/WatchPage';
import SearchPage from './pages/search/SearchPage';
import WatchHistoryPage from './pages/history/WatchHistoryPage';
import MyRatingsPage from './pages/ratings/MyRatingsPage';
import PersonDetailPage from './pages/person/PersonDetailPage';
import AccountPage from './pages/account/AccountPage';

import './styles/global.css';
import './styles/components.css';

function App() {
  const { initialize, isInitialized } = useAuthStore();

  useEffect(() => {
    initialize();
  }, [initialize]);

  if (!isInitialized) {
    return (
      <div className="loading-screen">
        <div className="spinner" />
      </div>
    );
  }

  return (
    <BrowserRouter>
      <Routes>
        {/* Public Landing */}
        <Route path="/" element={
          <PublicOnlyRoute><LandingPage /></PublicOnlyRoute>
        } />

        {/* Auth Pages (with AuthLayout) */}
        <Route element={<AuthLayout />}>
          <Route path="/login" element={
            <PublicOnlyRoute><LoginPage /></PublicOnlyRoute>
          } />
          <Route path="/register" element={
            <PublicOnlyRoute><RegisterPage /></PublicOnlyRoute>
          } />
          <Route path="/confirm-email" element={<ConfirmEmailPage />} />
          <Route path="/forgot-password" element={<ForgotPasswordPage />} />
          <Route path="/reset-password" element={<ResetPasswordPage />} />
        </Route>

        {/* Subscription */}
        <Route path="/plans" element={<PlansPage />} />
        <Route path="/subscription/success" element={
          <ProtectedRoute><SubscriptionSuccessPage /></ProtectedRoute>
        } />

        {/* Profiles */}
        <Route path="/profiles" element={
          <ProtectedRoute><ProfileSelectorPage /></ProtectedRoute>
        } />
        <Route path="/profiles/create" element={
          <ProtectedRoute><CreateProfilePage /></ProtectedRoute>
        } />
        <Route path="/profiles/edit" element={
          <ProtectedRoute><EditProfilePage /></ProtectedRoute>
        } />

        {/* Main App (with Navbar) */}
        <Route element={
          <ProfileRoute><MainLayout /></ProfileRoute>
        }>
          <Route path="/browse" element={<BrowsePage />} />
          <Route path="/title/:id" element={<ContentDetailPage />} />
          <Route path="/search" element={<SearchPage />} />
          <Route path="/my-list" element={<WatchHistoryPage />} />
          <Route path="/my-ratings" element={<MyRatingsPage />} />
          <Route path="/person/:id" element={<PersonDetailPage />} />
          <Route path="/account" element={<AccountPage />} />
        </Route>

        {/* Video Player (fullscreen, no layout) */}
        <Route path="/watch/:id" element={
          <ProfileRoute><WatchPage /></ProfileRoute>
        } />

        {/* Catch all */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
