import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { profileService } from '../../api/profileService';
import { formatProgress } from '../../utils/constants';
import '../../styles/components.css';

const WatchHistoryPage = () => {
  const navigate = useNavigate();
  const [history, setHistory] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadHistory();
  }, []);

  const loadHistory = async () => {
    try {
      const { data } = await profileService.getWatchHistory(false);
      setHistory(data.items || data || []);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div className="loading-screen"><div className="spinner" /></div>;

  return (
    <div style={{ minHeight: '100vh', background: 'var(--bg-primary)', padding: 'var(--space-xl) 4%' }}>
      <h1 style={{ fontSize: 'var(--fs-3xl)', fontWeight: 700, marginBottom: 'var(--space-xl)' }}>My List</h1>

      {history.length === 0 ? (
        <div style={{ textAlign: 'center', padding: 'var(--space-4xl)', color: 'var(--text-muted)' }}>
          <p style={{ fontSize: 'var(--fs-xl)', marginBottom: 'var(--space-md)' }}>Your watch history is empty</p>
          <p style={{ fontSize: 'var(--fs-sm)' }}>Start watching something to see it here!</p>
          <button className="btn btn--primary" onClick={() => navigate('/browse')} style={{ width: 'auto', marginTop: 'var(--space-xl)' }}>
            Browse Content
          </button>
        </div>
      ) : (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(250px, 1fr))', gap: 'var(--space-md)' }}>
          {history.map((item) => (
            <div
              key={item.id}
              onClick={() => navigate(`/watch/${item.contentId}`)}
              style={{
                cursor: 'pointer',
                borderRadius: 'var(--radius-sm)',
                overflow: 'hidden',
                background: 'var(--bg-card)',
                transition: 'transform var(--transition-base)',
              }}
              onMouseEnter={(e) => e.currentTarget.style.transform = 'scale(1.03)'}
              onMouseLeave={(e) => e.currentTarget.style.transform = 'scale(1)'}
            >
              <div style={{ height: 140, overflow: 'hidden', position: 'relative' }}>
                {item.contentThumbnailUrl ? (
                  <img src={item.contentThumbnailUrl} alt={item.contentTitle} style={{ width: '100%', height: '100%', objectFit: 'cover' }} />
                ) : (
                  <div style={{ width: '100%', height: '100%', background: 'linear-gradient(135deg, var(--bg-card), var(--bg-elevated))', display: 'flex', alignItems: 'center', justifyContent: 'center', color: 'var(--text-muted)', fontSize: 'var(--fs-sm)' }}>
                    {item.contentTitle}
                  </div>
                )}
                {/* Progress bar */}
                <div style={{ position: 'absolute', bottom: 0, left: 0, right: 0, height: 4, background: 'rgba(255,255,255,0.2)' }}>
                  <div style={{ height: '100%', background: 'var(--netflix-red)', width: `${formatProgress(item.stoppedAtSeconds, item.totalDurationSeconds || 5400)}%` }} />
                </div>
                {/* Play button overlay */}
                <div style={{ position: 'absolute', inset: 0, display: 'flex', alignItems: 'center', justifyContent: 'center', background: 'rgba(0,0,0,0.3)', opacity: 0, transition: 'opacity 0.2s' }}
                  onMouseEnter={(e) => e.currentTarget.style.opacity = 1}
                  onMouseLeave={(e) => e.currentTarget.style.opacity = 0}
                >
                  <div style={{ width: 50, height: 50, borderRadius: '50%', background: 'rgba(0,0,0,0.7)', border: '2px solid white', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '1.5rem' }}>
                    ▶
                  </div>
                </div>
              </div>
              <div style={{ padding: 'var(--space-sm) var(--space-md)' }}>
                <div style={{ fontSize: 'var(--fs-sm)', fontWeight: 600, marginBottom: 4 }}>{item.contentTitle}</div>
                <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: 'var(--fs-xs)', color: 'var(--text-muted)' }}>
                  <span>{item.isCompleted ? '✓ Completed' : `${formatProgress(item.stoppedAtSeconds, item.totalDurationSeconds || 5400)}% watched`}</span>
                  <span>{new Date(item.watchedAt).toLocaleDateString()}</span>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default WatchHistoryPage;
