import api from './axios';

export const subscriptionService = {
  getPlans: () =>
    api.get('/subscription/plans'),

  subscribe: (planId) =>
    api.post('/subscription/Subscripe', { planId }),

  getMySubscription: () =>
    api.get('/subscription/my-subscription'),
};
