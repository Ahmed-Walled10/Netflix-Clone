import api from './axios';

export const catalogService = {
  getCatalog: (params = {}) => {
    const queryParams = new URLSearchParams();

    if (params.pageNumber) queryParams.append('CatalogResourceParameters.PageNumber', params.pageNumber);
    if (params.pageSize) queryParams.append('CatalogResourceParameters.PageSize', params.pageSize);
    if (params.searchQuery) queryParams.append('CatalogResourceParameters.SearchQuery', params.searchQuery);
    if (params.contentTypes) {
      params.contentTypes.forEach(ct => queryParams.append('CatalogResourceParameters.ContentTypes', ct));
    }
    if (params.genreIds) {
      params.genreIds.forEach(id => queryParams.append('CatalogResourceParameters.GenreIds', id));
    }
    if (params.minRating) queryParams.append('CatalogResourceParameters.MinRating', params.minRating);
    if (params.releaseYear) queryParams.append('CatalogResourceParameters.ReleaseYear', params.releaseYear);
    if (params.isOriginal !== undefined) queryParams.append('CatalogResourceParameters.IsOriginal', params.isOriginal);
    if (params.orderedByRatingDescending !== undefined) queryParams.append('CatalogResourceParameters.OrderedByRatingDesending', params.orderedByRatingDescending);

    return api.get(`/catalog/content?${queryParams.toString()}`);
  },

  getContentById: (id) =>
    api.get(`/catalog/content/${id}`),

  getTrending: () =>
    api.get('/catalog/trending'),

  playContent: (contentId, episodeId = null) => {
    const params = episodeId ? `?EpisodeId=${episodeId}` : '';
    return api.get(`/catalog/content/${contentId}/play${params}`);
  },

  getPersonById: (id) =>
    api.get(`/catalog/person/${id}`),
};
