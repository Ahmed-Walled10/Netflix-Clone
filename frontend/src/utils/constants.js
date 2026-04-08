export const MATURITY_LABELS = {
  0: 'G',
  1: 'TV-PG',
  7: 'TV-Y7',
  13: 'PG-13',
  14: 'TV-14',
  17: 'TV-MA',
  18: 'NC-17',
};

export const MATURITY_COLORS = {
  0: 'var(--maturity-g)',
  1: 'var(--maturity-pg)',
  7: 'var(--maturity-pg)',
  13: 'var(--maturity-pg13)',
  14: 'var(--maturity-pg13)',
  17: 'var(--maturity-r)',
  18: 'var(--maturity-nc17)',
};

export const CONTENT_TYPE_LABELS = {
  1: 'Movie',
  2: 'Series',
  3: 'Documentary',
};

export const VIDEO_QUALITY_LABELS = {
  1: '720p HD',
  2: '1080p Full HD',
  3: '4K Ultra HD',
};

export const formatDuration = (minutes) => {
  if (!minutes) return '';
  const h = Math.floor(minutes / 60);
  const m = minutes % 60;
  if (h === 0) return `${m}m`;
  return m === 0 ? `${h}h` : `${h}h ${m}m`;
};

export const formatDate = (dateString) => {
  if (!dateString) return '';
  return new Date(dateString).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  });
};

export const formatProgress = (stoppedAt, total) => {
  if (!total || total === 0) return 0;
  return Math.min(Math.round((stoppedAt / total) * 100), 100);
};

export const getMaturityLabel = (rating) => MATURITY_LABELS[rating] || 'NR';
export const getMaturityColor = (rating) => MATURITY_COLORS[rating] || 'var(--text-muted)';
export const getContentTypeLabel = (type) => CONTENT_TYPE_LABELS[type] || 'Unknown';
