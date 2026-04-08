import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { catalogService } from '../../api/catalogService';
import { FiCalendar, FiFilm, FiArrowLeft } from 'react-icons/fi';
import '../../styles/components.css';

const roleLabels = {
  0: 'Actor',
  1: 'Director',
  2: 'Writer',
  3: 'Producer',
  4: 'Creator',
  Actor: 'Actor',
  Director: 'Director',
  Writer: 'Writer',
  Producer: 'Producer',
  Creator: 'Creator',
};

const PersonDetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [person, setPerson] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadPerson();
  }, [id]);

  const loadPerson = async () => {
    setLoading(true);
    try {
      const { data } = await catalogService.getPersonById(id);
      setPerson(data);
    } catch (err) {
      console.error('Failed to load person', err);
    } finally {
      setLoading(false);
    }
  };

  const calculateAge = (birthDate) => {
    if (!birthDate) return null;
    const birth = new Date(birthDate);
    const today = new Date();
    let age = today.getFullYear() - birth.getFullYear();
    const m = today.getMonth() - birth.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birth.getDate())) age--;
    return age;
  };

  const formatBirthDate = (birthDate) => {
    if (!birthDate) return null;
    return new Date(birthDate).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  };

  if (loading) return <div className="loading-screen"><div className="spinner" /></div>;
  if (!person) return (
    <div className="loading-screen">
      <p style={{ color: 'var(--text-muted)', fontSize: 'var(--fs-lg)' }}>Person not found</p>
      <button className="btn btn--ghost" onClick={() => navigate(-1)} style={{ marginTop: 'var(--space-md)' }}>Go Back</button>
    </div>
  );

  const age = calculateAge(person.birthDate);

  return (
    <div style={{ minHeight: '100vh', background: 'var(--bg-primary)', padding: 'var(--space-xl) 4%' }}>
      {/* Back Button */}
      <button
        onClick={() => navigate(-1)}
        style={{
          display: 'inline-flex', alignItems: 'center', gap: 'var(--space-sm)',
          background: 'none', border: 'none', color: 'var(--text-secondary)',
          fontSize: 'var(--fs-sm)', cursor: 'pointer', marginBottom: 'var(--space-xl)',
          transition: 'color var(--transition-fast)',
        }}
        onMouseEnter={(e) => e.currentTarget.style.color = 'var(--text-primary)'}
        onMouseLeave={(e) => e.currentTarget.style.color = 'var(--text-secondary)'}
      >
        <FiArrowLeft size={16} /> Back
      </button>

      {/* Profile Header */}
      <div style={{
        display: 'flex', gap: 'var(--space-2xl)', marginBottom: 'var(--space-3xl)',
        animation: 'fadeInUp 0.6s ease-out',
      }}>
        {/* Photo */}
        <div style={{ flexShrink: 0 }}>
          {person.photoUrl ? (
            <img
              src={person.photoUrl}
              alt={person.fullName}
              style={{
                width: 220, height: 280, objectFit: 'cover',
                borderRadius: 'var(--radius-xl)',
                boxShadow: 'var(--shadow-lg)',
              }}
            />
          ) : (
            <div style={{
              width: 220, height: 280, borderRadius: 'var(--radius-xl)',
              background: 'linear-gradient(135deg, var(--bg-card), var(--bg-elevated))',
              display: 'flex', alignItems: 'center', justifyContent: 'center',
              fontSize: '4rem', fontWeight: 700, color: 'var(--text-muted)',
              boxShadow: 'var(--shadow-lg)',
            }}>
              {person.fullName?.[0]?.toUpperCase()}
            </div>
          )}
        </div>

        {/* Info */}
        <div style={{ flex: 1, display: 'flex', flexDirection: 'column', justifyContent: 'center' }}>
          <h1 style={{
            fontSize: 'var(--fs-4xl)', fontWeight: 800, marginBottom: 'var(--space-md)',
            lineHeight: 1.1,
          }}>
            {person.fullName}
          </h1>

          <div style={{ display: 'flex', flexWrap: 'wrap', gap: 'var(--space-lg)', marginBottom: 'var(--space-lg)' }}>
            {person.birthDate && (
              <div style={{ display: 'flex', alignItems: 'center', gap: 'var(--space-xs)', color: 'var(--text-secondary)', fontSize: 'var(--fs-sm)' }}>
                <FiCalendar size={14} />
                <span>{formatBirthDate(person.birthDate)}</span>
                {age !== null && <span style={{ color: 'var(--text-muted)' }}>({age} years old)</span>}
              </div>
            )}
            {person.work && person.work.length > 0 && (
              <div style={{ display: 'flex', alignItems: 'center', gap: 'var(--space-xs)', color: 'var(--text-secondary)', fontSize: 'var(--fs-sm)' }}>
                <FiFilm size={14} />
                <span>{person.work.length} title{person.work.length !== 1 ? 's' : ''}</span>
              </div>
            )}
          </div>

          {person.bio && (
            <p style={{
              fontSize: 'var(--fs-md)', color: 'var(--text-secondary)',
              lineHeight: 1.7, maxWidth: 600,
            }}>
              {person.bio}
            </p>
          )}

          {!person.bio && (
            <p style={{ fontSize: 'var(--fs-sm)', color: 'var(--text-muted)', fontStyle: 'italic' }}>
              No biography available.
            </p>
          )}
        </div>
      </div>

      {/* Filmography */}
      {person.work && person.work.length > 0 && (
        <div style={{ animation: 'fadeInUp 0.6s ease-out 0.2s both' }}>
          <h2 style={{ fontSize: 'var(--fs-xl)', fontWeight: 700, marginBottom: 'var(--space-lg)' }}>
            Filmography
          </h2>

          <div style={{
            display: 'grid',
            gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))',
            gap: 'var(--space-lg)',
          }}>
            {person.work.map((item, idx) => (
              <div
                key={`${item.contentId}-${idx}`}
                onClick={() => navigate(`/title/${item.contentId}`)}
                style={{
                  background: 'var(--bg-card)',
                  borderRadius: 'var(--radius-lg)',
                  overflow: 'hidden',
                  cursor: 'pointer',
                  transition: 'transform var(--transition-base), box-shadow var(--transition-base)',
                  animation: `fadeInUp 0.4s ease-out ${0.1 + idx * 0.05}s both`,
                }}
                onMouseEnter={(e) => {
                  e.currentTarget.style.transform = 'translateY(-6px) scale(1.02)';
                  e.currentTarget.style.boxShadow = 'var(--shadow-lg)';
                }}
                onMouseLeave={(e) => {
                  e.currentTarget.style.transform = 'translateY(0) scale(1)';
                  e.currentTarget.style.boxShadow = 'none';
                }}
              >
                {/* Thumbnail */}
                <div style={{ width: '100%', aspectRatio: '16/9', overflow: 'hidden' }}>
                  {item.thumbnailUrl ? (
                    <img
                      src={item.thumbnailUrl}
                      alt={item.title}
                      style={{ width: '100%', height: '100%', objectFit: 'cover', transition: 'transform 0.3s' }}
                    />
                  ) : (
                    <div style={{
                      width: '100%', height: '100%',
                      background: 'linear-gradient(135deg, #1a1a2e, #16213e)',
                      display: 'flex', alignItems: 'center', justifyContent: 'center',
                      color: 'var(--text-muted)', fontSize: 'var(--fs-xs)',
                      textAlign: 'center', padding: 'var(--space-sm)',
                    }}>
                      {item.title}
                    </div>
                  )}
                </div>

                {/* Card Info */}
                <div style={{ padding: 'var(--space-md)' }}>
                  <h3 style={{
                    fontSize: 'var(--fs-sm)', fontWeight: 600,
                    marginBottom: 'var(--space-xs)',
                    overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap',
                  }}>
                    {item.title}
                  </h3>
                  <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <span style={{
                      fontSize: 'var(--fs-xs)',
                      padding: '2px 8px',
                      borderRadius: 'var(--radius-full)',
                      background: 'rgba(229, 9, 20, 0.12)',
                      color: 'var(--netflix-red)',
                      fontWeight: 500,
                    }}>
                      {roleLabels[item.role] || 'Cast'}
                    </span>
                    {Number(item.averageRating) > 0 && (
                      <span style={{ color: '#FFD700', fontSize: 'var(--fs-xs)', fontWeight: 600 }}>
                        ★ {Number(item.averageRating).toFixed(1)}
                      </span>
                    )}
                  </div>
                  {item.characterName && (
                    <p style={{
                      fontSize: 'var(--fs-xs)', color: 'var(--text-muted)',
                      marginTop: 'var(--space-xs)', fontStyle: 'italic',
                    }}>
                      as {item.characterName}
                    </p>
                  )}
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* No work */}
      {(!person.work || person.work.length === 0) && (
        <div style={{ textAlign: 'center', padding: 'var(--space-3xl)', color: 'var(--text-muted)' }}>
          <FiFilm size={40} style={{ marginBottom: 'var(--space-md)', opacity: 0.3 }} />
          <p>No filmography available yet.</p>
        </div>
      )}
    </div>
  );
};

export default PersonDetailPage;
