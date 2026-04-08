import api from './axios';

export const engagementService = {
  addRating: (contentId, value, review = null) =>
    api.post(`/engagement/content/${contentId}/rating`, { value, review }),

  updateRating: (ratingId, data) =>
    api.patch(`/engagement/rating/${ratingId}`, data),

  deleteRating: (ratingId) =>
    api.delete(`/engagement/rating/${ratingId}`),

  getMovieRatings: (contentId, params = {}) => {
    const queryParams = new URLSearchParams();
    if (params.pageNumber) queryParams.append('RatingsResourceParameters.PageNumber', params.pageNumber);
    if (params.pageSize) queryParams.append('RatingsResourceParameters.PageSize', params.pageSize);
    return api.get(`/engagement/content/${contentId}/ratings?${queryParams.toString()}`);
  },

  getMyMovieRating: (contentId) =>
    api.get(`/engagement/content/${contentId}/rating`),
};
