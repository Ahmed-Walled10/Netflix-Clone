import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { profileService } from '../../api/profileService';
import { engagementService } from '../../api/engagementService';
import { getMaturityLabel, getMaturityColor, formatDate } from '../../utils/constants';
import { FiStar, FiTrash2, FiEdit3 } from 'react-icons/fi';
import '../../styles/components.css';

const MyRatingsPage = () => {
  const navigate = useNavigate();
  const [ratings, setRatings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [deleting, setDeleting] = useState(null);

  useEffect(() => {
    loadRatings();
  }, []);

  const loadRatings = async () => {
    try {
      const { data } = await profileService.getMyRatings();
      setRatings(data.items || data || []);
    } catch (err) {
      console.error('Failed to load ratings', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (ratingId) => {
    if (!window.confirm('Are you sure you want to remove this rating?')) return;
    setDeleting(ratingId);
    try {
      await engagementService.deleteRating(ratingId);
      setRatings((prev) => prev.filter((r) => r.id !== ratingId));
    } catch (err) {
      console.error('Failed to delete rating', err);
    } finally {
      setDeleting(null);
    }
  };

  const renderStars = (value) => {
    return Array.from({ length: 5 }, (_, i) => (
      <span key={i} style={{ color: i < value ? '#FFD700' : 'var(--text-muted)', fontSize: 'var(--fs-lg)' }}>★</span>
    ));
  };

  if (loading) return <div className="loading-screen"><div className="spinner" /></div>;

  return (
    <div style={{ minHeight: '100vh', background: 'var(--bg-primary)', padding: 'var(--space-xl) 4%' }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 'var(--space-xl)' }}>
        <h1 style={{ fontSize: 'var(--fs-3xl)', fontWeight: 700 }}>My Ratings</h1>
        <span style={{ color: 'var(--text-muted)', fontSize: 'var(--fs-sm)' }}>
          {ratings.length} rated title{ratings.length !== 1 ? 's' : ''}
        </span>
      </div>

      {ratings.length === 0 ? (
        <div style={{ textAlign: 'center', padding: 'var(--space-4xl)', color: 'var(--text-muted)' }}>
          <FiStar size={48} style={{ marginBottom: 'var(--space-md)', opacity: 0.3 }} />
          <p style={{ fontSize: 'var(--fs-xl)', marginBottom: 'var(--space-md)' }}>No ratings yet</p>
          <p style={{ fontSize: 'var(--fs-sm)', marginBottom: 'var(--space-xl)' }}>Start rating movies and TV shows to see them here!</p>
          <button className="btn btn--primary" onClick={() => navigate('/browse')} style={{ width: 'auto' }}>
            Browse Content
          </button>
        </div>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: 'var(--space-md)' }}>
          {ratings.map((rating) => (
            <div
              key={rating.id}
              style={{
                display: 'flex',
                gap: 'var(--space-lg)',
                background: 'var(--bg-card)',
                borderRadius: 'var(--radius-lg)',
                overflow: 'hidden',
                transition: 'transform var(--transition-fast), box-shadow var(--transition-fast)',
                cursor: 'pointer',
              }}
              onMouseEnter={(e) => { e.currentTarget.style.transform = 'translateX(4px)'; e.currentTarget.style.boxShadow = 'var(--shadow-md)'; }}
              onMouseLeave={(e) => { e.currentTarget.style.transform = 'translateX(0)'; e.currentTarget.style.boxShadow = 'none'; }}
            >
              {/* Thumbnail */}
              <div
                style={{ width: 160, minHeight: 100, flexShrink: 0, overflow: 'hidden', cursor: 'pointer' }}
                onClick={() => navigate(`/title/${rating.contentId}`)}
              >
                {rating.contentThumbnailUrl ? (
                  <img src={rating.contentThumbnailUrl} alt={rating.contentTitle} style={{ width: '100%', height: '100%', objectFit: 'cover' }} />
                ) : (
                  <div style={{ width: '100%', height: '100%', background: 'linear-gradient(135deg, var(--bg-card), var(--bg-elevated))', display: 'flex', alignItems: 'center', justifyContent: 'center', color: 'var(--text-muted)', fontSize: 'var(--fs-xs)', textAlign: 'center', padding: 'var(--space-sm)' }}>
                    {rating.contentTitle}
                  </div>
                )}
              </div>

              {/* Info */}
              <div style={{ flex: 1, padding: 'var(--space-md)', display: 'flex', flexDirection: 'column', justifyContent: 'center' }}
                onClick={() => navigate(`/title/${rating.contentId}`)}
              >
                <h3 style={{ fontSize: 'var(--fs-md)', fontWeight: 600, marginBottom: 'var(--space-xs)' }}>
                  {rating.contentTitle}
                </h3>
                <div style={{ display: 'flex', alignItems: 'center', gap: 'var(--space-sm)', marginBottom: 'var(--space-xs)' }}>
                  {renderStars(rating.value)}
                  <span style={{ color: 'var(--text-muted)', fontSize: 'var(--fs-sm)', marginLeft: 'var(--space-xs)' }}>
                    {rating.value}/5
                  </span>
                </div>
                {rating.review && (
                  <p style={{ fontSize: 'var(--fs-sm)', color: 'var(--text-secondary)', lineHeight: 1.5, marginTop: 'var(--space-xs)', display: '-webkit-box', WebkitLineClamp: 2, WebkitBoxOrient: 'vertical', overflow: 'hidden' }}>
                    "{rating.review}"
                  </p>
                )}
                {rating.createdAt && (
                  <span style={{ fontSize: 'var(--fs-xs)', color: 'var(--text-muted)', marginTop: 'var(--space-xs)' }}>
                    Rated on {formatDate(rating.createdAt)}
                  </span>
                )}
              </div>

              {/* Actions */}
              <div style={{ display: 'flex', alignItems: 'center', gap: 'var(--space-sm)', padding: 'var(--space-md)', flexShrink: 0 }}>
                <button
                  onClick={(e) => { e.stopPropagation(); navigate(`/title/${rating.contentId}`); }}
                  style={{ width: 36, height: 36, borderRadius: '50%', border: '1px solid var(--border-color)', background: 'transparent', color: 'var(--text-secondary)', display: 'flex', alignItems: 'center', justifyContent: 'center', cursor: 'pointer', transition: 'all var(--transition-fast)' }}
                  title="Edit rating"
                  onMouseEnter={(e) => { e.currentTarget.style.borderColor = 'var(--text-primary)'; e.currentTarget.style.color = 'var(--text-primary)'; }}
                  onMouseLeave={(e) => { e.currentTarget.style.borderColor = 'var(--border-color)'; e.currentTarget.style.color = 'var(--text-secondary)'; }}
                >
                  <FiEdit3 size={14} />
                </button>
                <button
                  onClick={(e) => { e.stopPropagation(); handleDelete(rating.id); }}
                  disabled={deleting === rating.id}
                  style={{ width: 36, height: 36, borderRadius: '50%', border: '1px solid var(--border-color)', background: 'transparent', color: 'var(--text-secondary)', display: 'flex', alignItems: 'center', justifyContent: 'center', cursor: 'pointer', transition: 'all var(--transition-fast)' }}
                  title="Delete rating"
                  onMouseEnter={(e) => { e.currentTarget.style.borderColor = 'var(--netflix-red)'; e.currentTarget.style.color = 'var(--netflix-red)'; }}
                  onMouseLeave={(e) => { e.currentTarget.style.borderColor = 'var(--border-color)'; e.currentTarget.style.color = 'var(--text-secondary)'; }}
                >
                  {deleting === rating.id ? <span className="spinner spinner--sm" /> : <FiTrash2 size={14} />}
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default MyRatingsPage;
