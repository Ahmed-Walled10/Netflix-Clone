import { create } from 'zustand';
import { getUserFromToken, isTokenExpired, hasProfileToken } from '../utils/jwt';

const ACCESS_TOKEN_KEY = 'nf_access_token';
const REFRESH_TOKEN_KEY = 'nf_refresh_token';

export const useAuthStore = create((set, get) => ({
  // ── State ──────────────────────────────────────────────────
  accessToken: localStorage.getItem(ACCESS_TOKEN_KEY) || null,
  refreshToken: localStorage.getItem(REFRESH_TOKEN_KEY) || null,
  user: null,
  isInitialized: false,

  // ── Computed getters ───────────────────────────────────────
  get isAuthenticated() {
    const token = get().accessToken;
    return !!token && !isTokenExpired(token);
  },

  // ── Actions ────────────────────────────────────────────────
  initialize: () => {
    const token = localStorage.getItem(ACCESS_TOKEN_KEY);
    if (token && !isTokenExpired(token)) {
      const user = getUserFromToken(token);
      set({ user, isInitialized: true });
    } else {
      set({ isInitialized: true });
    }
  },

  login: (accessToken, refreshToken, loginData) => {
    localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
    localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
    const user = getUserFromToken(accessToken);
    set({
      accessToken,
      refreshToken,
      user: {
        ...user,
        email: loginData?.email || user?.email,
        fullName: loginData?.fullName || user?.name,
        roles: loginData?.roles || user?.roles,
      },
    });
  },

  setTokens: (accessToken, refreshToken) => {
    localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
    if (refreshToken) {
      localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
    }
    const user = getUserFromToken(accessToken);
    set({ accessToken, refreshToken: refreshToken || get().refreshToken, user });
  },

  setProfileToken: (accessToken) => {
    localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
    const user = getUserFromToken(accessToken);
    set({ accessToken, user });
  },

  logout: () => {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    set({ accessToken: null, refreshToken: null, user: null });
  },

  hasProfile: () => {
    const token = get().accessToken;
    return token ? hasProfileToken(token) : false;
  },

  hasRole: (role) => {
    const user = get().user;
    return user?.roles?.includes(role) || false;
  },

  isSubscriber: () => {
    const user = get().user;
    return user?.roles?.some(r => ['Subscriber', 'SuperAdmin', 'ContentManager'].includes(r)) || false;
  },
}));
