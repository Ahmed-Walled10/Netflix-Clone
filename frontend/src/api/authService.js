import api from './axios';

export const authService = {
  register: (data) =>
    api.post('/auth/register', data),

  login: (data) =>
    api.post('/auth/login', data),

  logout: (refreshToken) =>
    api.post('/auth/logout', { refreshToken }),

  confirmEmail: (data) =>
    api.post('/auth/confirm-email', data),

  resendConfirmationOtp: (email) =>
    api.post('/auth/resend-confirmation-otp', { email }),

  forgotPassword: (email) =>
    api.post('/auth/forgot-password', { email }),

  resetPassword: (data) =>
    api.post('/auth/reset-password', data),

  refreshToken: (refreshToken) =>
    api.post('/auth/refresh-token', { refreshToken }),

  revokeAll: () =>
    api.post('/auth/revoke-all'),
};
