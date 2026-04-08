import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { engagementService } from '../../api/engagementService';
import { formatDate } from '../../utils/constants';
import '../../styles/components.css';

const MovieReviewsSection = ({ contentId }) => {
  const [reviews, setReviews] = useState([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(false);
  const [totalCount, setTotalCount] = useState(0);

  useEffect(() => {
    loadReviews(1);
  }, [contentId]);

  const loadReviews = async (pageNum) => {
    setLoading(true);
    try {
      const { data } = await engagementService.getMovieRatings(contentId, {
        pageNumber: pageNum,
        pageSize: 10,
      });
      const items = data.items || data || [];
      if (pageNum === 1) {
        setReviews(items);
      } else {
        setReviews((prev) => [...prev, ...items]);
      }
      setTotalCount(data.totalCount || items.length);
      setHasMore(items.length === 10);
      setPage(pageNum);
    } catch (err) {
      console.error('Failed to load reviews', err);
    } finally {
      setLoading(false);
    }
  };

  const renderStars = (value) => {
    return Array.from({ length: 5 }, (_, i) => (
      <span key={i} style={{ color: i < value ? '#FFD700' : 'var(--border-color)', fontSize: 'var(--fs-sm)' }}>★</span>
    ));
  };

  if (loading && reviews.length === 0) {
    return (
      <div style={{ padding: 'var(--space-md) 0' }}>
        <div className="spinner spinner--sm" />
      </div>
    );
  }

  if (reviews.length === 0) return null;

  return (
    <div style={{ marginTop: 'var(--space-2xl)' }}>
      <h3 style={{ fontSize: 'var(--fs-lg)', marginBottom: 'var(--space-md)' }}>
        Reviews <span style={{ color: 'var(--text-muted)', fontSize: 'var(--fs-sm)', fontWeight: 400 }}>({totalCount})</span>
      </h3>

      <div style={{ display: 'flex', flexDirection: 'column', gap: 'var(--space-md)' }}>
        {reviews.map((review, idx) => (
          <div
            key={review.id || idx}
            style={{
              background: 'var(--bg-card)',
              borderRadius: 'var(--radius-lg)',
              padding: 'var(--space-md) var(--space-lg)',
              animation: `fadeInUp 0.3s ease-out ${idx * 0.05}s both`,
            }}
          >
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 'var(--space-sm)' }}>
              <div style={{ display: 'flex', alignItems: 'center', gap: 'var(--space-sm)' }}>
                <div style={{
                  width: 32,
                  height: 32,
                  borderRadius: '50%',
                  background: `hsl(${(review.profileName || 'U').charCodeAt(0) * 37 % 360}, 60%, 40%)`,
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  fontSize: 'var(--fs-xs)',
                  fontWeight: 700,
                  color: 'white',
                }}>
                  {(review.profileName || 'U')[0].toUpperCase()}
                </div>
                <span style={{ fontSize: 'var(--fs-sm)', fontWeight: 500 }}>{review.profileName || 'User'}</span>
              </div>
              <div style={{ display: 'flex', alignItems: 'center', gap: 'var(--space-xs)' }}>
                {renderStars(review.value)}
              </div>
            </div>
            {review.review && (
              <p style={{ fontSize: 'var(--fs-sm)', color: 'var(--text-secondary)', lineHeight: 1.6 }}>
                {review.review}
              </p>
            )}
            {review.createdAt && (
              <span style={{ fontSize: 'var(--fs-xs)', color: 'var(--text-muted)', marginTop: 'var(--space-xs)', display: 'block' }}>
                {formatDate(review.createdAt)}
              </span>
            )}
          </div>
        ))}
      </div>

      {hasMore && (
        <button
          onClick={() => loadReviews(page + 1)}
          disabled={loading}
          style={{
            display: 'block',
            margin: 'var(--space-lg) auto 0',
            padding: 'var(--space-sm) var(--space-xl)',
            background: 'transparent',
            border: '1px solid var(--border-color)',
            borderRadius: 'var(--radius-sm)',
            color: 'var(--text-secondary)',
            fontSize: 'var(--fs-sm)',
            cursor: 'pointer',
            transition: 'all var(--transition-fast)',
          }}
        >
          {loading ? <span className="spinner spinner--sm" /> : 'Load More Reviews'}
        </button>
      )}
    </div>
  );
};

export default MovieReviewsSection;
