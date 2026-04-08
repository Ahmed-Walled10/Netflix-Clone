import api from './axios';

export const profileService = {
  getProfiles: () =>
    api.get('/profile'),

  createProfile: (data) =>
    api.post('/profile/Create', data),

  deleteProfile: () =>
    api.delete('/profile'),

  loginToProfile: (data) =>
    api.post('/profile/login', data),

  switchProfile: (data) =>
    api.post('/profile/switch', data),

  updateProfile: (data) =>
    api.patch('/profile/update', data),

  getWatchHistory: (continueWatchingOnly = false) =>
    api.get(`/profile/watch-history?ContinueWatchingOnly=${continueWatchingOnly}`),

  getMyRatings: () =>
    api.get('/profile/my-ratings'),
};
