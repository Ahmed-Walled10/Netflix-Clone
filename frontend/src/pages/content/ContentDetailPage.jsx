import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { catalogService } from '../../api/catalogService';
import { engagementService } from '../../api/engagementService';
import { getMaturityLabel, getMaturityColor, getContentTypeLabel, formatDuration } from '../../utils/constants';
import { FiPlay, FiStar, FiClock, FiCalendar, FiGlobe } from 'react-icons/fi';
import MovieReviewsSection from '../../components/content/MovieReviewsSection';
import '../../styles/components.css';

const ContentDetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [content, setContent] = useState(null);
  const [myRating, setMyRating] = useState(null);
  const [hoveredStar, setHoveredStar] = useState(0);
  const [loading, setLoading] = useState(true);
  const [ratingLoading, setRatingLoading] = useState(false);

  useEffect(() => {
    loadContent();
  }, [id]);

  const loadContent = async () => {
    try {
      const [contentRes, ratingRes] = await Promise.allSettled([
        catalogService.getContentById(id),
        engagementService.getMyMovieRating(id),
      ]);
      if (contentRes.status === 'fulfilled') setContent(contentRes.value.data);
      if (ratingRes.status === 'fulfilled' && ratingRes.value.data) setMyRating(ratingRes.value.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleRate = async (value) => {
    setRatingLoading(true);
    try {
      if (myRating) {
        await engagementService.updateRating(myRating.id, { value });
        setMyRating({ ...myRating, value });
      } else {
        const { data } = await engagementService.addRating(id, value);
        setMyRating(data);
      }
    } catch (err) {
      console.error('Failed to rate', err);
    } finally {
      setRatingLoading(false);
    }
  };

  if (loading) return <div className="loading-screen"><div className="spinner" /></div>;
  if (!content) return <div className="loading-screen"><p style={{ color: 'var(--text-muted)' }}>Content not found</p></div>;

  return (
    <div style={{ background: 'var(--bg-primary)', minHeight: '100vh' }}>
      {/* Hero */}
      <div style={{ position: 'relative', height: '70vh', minHeight: 400 }}>
        <div style={{ position: 'absolute', inset: 0 }}>
          {(content.heroImageUrl || content.thumbnailUrl) ? (
            <img src={content.heroImageUrl || content.thumbnailUrl} alt={content.title} style={{ width: '100%', height: '100%', objectFit: 'cover' }} />
          ) : (
            <div style={{ width: '100%', height: '100%', background: 'linear-gradient(135deg, #1a1a2e, #16213e)' }} />
          )}
          <div style={{ position: 'absolute', bottom: 0, left: 0, right: 0, height: '60%', background: 'linear-gradient(transparent, var(--bg-primary))' }} />
          <div style={{ position: 'absolute', inset: 0, background: 'linear-gradient(90deg, rgba(20,20,20,0.8) 0%, transparent 60%)' }} />
        </div>

        <div style={{ position: 'relative', zIndex: 2, height: '100%', display: 'flex', alignItems: 'flex-end', padding: '0 4%', paddingBottom: '5%' }}>
          <div style={{ maxWidth: 600, animation: 'fadeInUp 0.8s ease-out' }}>
            {content.isOriginal && (
              <span style={{ display: 'inline-block', background: 'rgba(229,9,20,0.15)', border: '1px solid var(--netflix-red)', borderRadius: 'var(--radius-full)', padding: '4px 14px', fontSize: 'var(--fs-xs)', fontWeight: 600, color: 'var(--netflix-red)', marginBottom: 'var(--space-md)', letterSpacing: 1, textTransform: 'uppercase' }}>
                N Original
              </span>
            )}
            <h1 style={{ fontSize: 'var(--fs-5xl)', fontWeight: 900, lineHeight: 1.1, marginBottom: 'var(--space-md)', textShadow: '2px 2px 8px rgba(0,0,0,0.8)' }}>
              {content.title}
            </h1>
            <div style={{ display: 'flex', alignItems: 'center', gap: 'var(--space-md)', marginBottom: 'var(--space-lg)', flexWrap: 'wrap' }}>
              <span style={{ color: 'var(--success)', fontWeight: 600 }}>★ {Number(content.averageRating).toFixed(1)}</span>
              <span style={{ color: 'var(--text-secondary)' }}>{content.releaseYear}</span>
              {content.durationMinutes && <span style={{ color: 'var(--text-secondary)' }}>{formatDuration(content.durationMinutes)}</span>}
              <span className="maturity-badge" style={{ borderColor: getMaturityColor(content.maturityRating), color: getMaturityColor(content.maturityRating) }}>
                {getMaturityLabel(content.maturityRating)}
              </span>
              <span style={{ color: 'var(--text-muted)', fontSize: 'var(--fs-sm)' }}>{getContentTypeLabel(content.contentType)}</span>
            </div>
            <div style={{ display: 'flex', gap: 'var(--space-md)' }}>
              <button className="btn btn--play" onClick={() => navigate(`/watch/${content.id}`)}>
                <FiPlay /> Play
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* Details */}
      <div style={{ padding: '0 4%', maxWidth: 1200 }}>
        <div style={{ display: 'grid', gridTemplateColumns: '2fr 1fr', gap: 'var(--space-3xl)' }}>
          <div>
            <p style={{ fontSize: 'var(--fs-md)', color: 'var(--text-secondary)', lineHeight: 1.8, marginBottom: 'var(--space-xl)' }}>
              {content.description}
            </p>
            {content.tagline && (
              <p style={{ fontSize: 'var(--fs-lg)', fontStyle: 'italic', color: 'var(--text-muted)', marginBottom: 'var(--space-xl)' }}>
                &ldquo;{content.tagline}&rdquo;
              </p>
            )}

            {/* Rating Widget */}
            <div style={{ marginBottom: 'var(--space-2xl)' }}>
              <h3 style={{ fontSize: 'var(--fs-lg)', marginBottom: 'var(--space-md)' }}>Your Rating</h3>
              <div style={{ display: 'flex', gap: 4 }}>
                {[1, 2, 3, 4, 5].map((star) => (
                  <button
                    key={star}
                    onClick={() => handleRate(star)}
                    onMouseEnter={() => setHoveredStar(star)}
                    onMouseLeave={() => setHoveredStar(0)}
                    disabled={ratingLoading}
                    style={{
                      background: 'none',
                      border: 'none',
                      fontSize: 'var(--fs-3xl)',
                      cursor: 'pointer',
                      color: star <= (hoveredStar || myRating?.value || 0) ? '#FFD700' : 'var(--text-muted)',
                      transition: 'color 0.15s, transform 0.15s',
                      transform: star <= hoveredStar ? 'scale(1.2)' : 'scale(1)',
                    }}
                  >
                    ★
                  </button>
                ))}
                {myRating && <span style={{ color: 'var(--text-muted)', fontSize: 'var(--fs-sm)', alignSelf: 'center', marginLeft: 'var(--space-sm)' }}>({myRating.value}/5)</span>}
              </div>
            </div>

            {/* Cast */}
            {content.cast && content.cast.length > 0 && (
              <div>
                <h3 style={{ fontSize: 'var(--fs-lg)', marginBottom: 'var(--space-md)' }}>Cast</h3>
                <div style={{ display: 'flex', flexWrap: 'wrap', gap: 'var(--space-md)' }}>
                  {content.cast.map((person) => (
                    <div
                      key={person.personId}
                      onClick={() => navigate(`/person/${person.personId}`)}
                      style={{ display: 'flex', alignItems: 'center', gap: 'var(--space-sm)', background: 'var(--bg-card)', borderRadius: 'var(--radius-lg)', padding: '8px 14px', cursor: 'pointer', transition: 'background var(--transition-fast), transform var(--transition-fast)' }}
                      onMouseEnter={(e) => { e.currentTarget.style.background = 'var(--bg-elevated)'; e.currentTarget.style.transform = 'translateY(-2px)'; }}
                      onMouseLeave={(e) => { e.currentTarget.style.background = 'var(--bg-card)'; e.currentTarget.style.transform = 'translateY(0)'; }}
                    >
                      {person.photoUrl ? (
                        <img src={person.photoUrl} alt={person.fullName} style={{ width: 36, height: 36, borderRadius: '50%', objectFit: 'cover' }} />
                      ) : (
                        <div style={{ width: 36, height: 36, borderRadius: '50%', background: 'var(--bg-elevated)', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: 'var(--fs-sm)', fontWeight: 600 }}>
                          {person.fullName?.[0]}
                        </div>
                      )}
                      <div>
                        <div style={{ fontSize: 'var(--fs-sm)', fontWeight: 500 }}>{person.fullName}</div>
                        {person.characterName && <div style={{ fontSize: 'var(--fs-xs)', color: 'var(--text-muted)' }}>{person.characterName}</div>}
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            )}

            {/* All User Reviews */}
            <MovieReviewsSection contentId={id} />
          </div>

          {/* Sidebar */}
          <div style={{ fontSize: 'var(--fs-sm)', color: 'var(--text-secondary)' }}>
            <div style={{ marginBottom: 'var(--space-md)' }}>
              <span style={{ color: 'var(--text-muted)' }}>Views: </span>{content.viewCount?.toLocaleString()}
            </div>
            <div style={{ marginBottom: 'var(--space-md)' }}>
              <span style={{ color: 'var(--text-muted)' }}>Total Ratings: </span>{content.totalRatings}
            </div>
            <div style={{ marginBottom: 'var(--space-md)' }}>
              <span style={{ color: 'var(--text-muted)' }}>Language: </span>{content.originalLanguage?.toUpperCase()}
            </div>
            {content.trailerUrl && (
              <div style={{ marginTop: 'var(--space-xl)' }}>
                <h4 style={{ marginBottom: 'var(--space-sm)', color: 'var(--text-primary)' }}>Trailer</h4>
                <a href={content.trailerUrl} target="_blank" rel="noopener noreferrer" className="btn btn--ghost" style={{ fontSize: 'var(--fs-sm)' }}>
                  Watch Trailer ↗
                </a>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default ContentDetailPage;
