import { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { catalogService } from '../../api/catalogService';
import { profileService } from '../../api/profileService';
import { getMaturityLabel, getMaturityColor, getContentTypeLabel, formatDuration } from '../../utils/constants';
import { FiPlay, FiInfo, FiChevronLeft, FiChevronRight } from 'react-icons/fi';
import styles from './BrowsePage.module.css';
import '../../styles/components.css';

const BrowsePage = () => {
  const navigate = useNavigate();
  const [hero, setHero] = useState(null);
  const [trending, setTrending] = useState([]);
  const [continueWatching, setContinueWatching] = useState([]);
  const [movies, setMovies] = useState([]);
  const [series, setSeries] = useState([]);
  const [topRated, setTopRated] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadBrowseData();
  }, []);

  const loadBrowseData = async () => {
    try {
      const [trendingRes, continueRes, moviesRes, seriesRes, topRatedRes] = await Promise.allSettled([
        catalogService.getTrending(),
        profileService.getWatchHistory(true),
        catalogService.getCatalog({ contentTypes: [1], pageSize: 20 }),
        catalogService.getCatalog({ contentTypes: [2], pageSize: 20 }),
        catalogService.getCatalog({ orderedByRatingDescending: true, pageSize: 20 }),
      ]);

      const trendingData = trendingRes.status === 'fulfilled' ? trendingRes.value.data : [];
      setTrending(trendingData);

      if (trendingData.length > 0) {
        const randomIndex = Math.floor(Math.random() * Math.min(trendingData.length, 5));
        setHero(trendingData[randomIndex]);
      }

      if (continueRes.status === 'fulfilled') {
        setContinueWatching(continueRes.value.data?.items || []);
      }

      if (moviesRes.status === 'fulfilled') {
        setMovies(moviesRes.value.data?.items || []);
      }

      if (seriesRes.status === 'fulfilled') {
        setSeries(seriesRes.value.data?.items || []);
      }

      if (topRatedRes.status === 'fulfilled') {
        setTopRated(topRatedRes.value.data?.items || []);
      }
    } catch (err) {
      console.error('Failed to load browse data', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className={styles['browse-page']}>
        <div className={styles.hero} style={{ background: 'var(--bg-secondary)' }}>
          <div className="animate-shimmer" style={{ width: '100%', height: '100%', position: 'absolute', inset: 0 }} />
        </div>
        <div className={styles['content-section']}>
          <ContentRowSkeleton />
          <ContentRowSkeleton />
        </div>
      </div>
    );
  }

  return (
    <div className={styles['browse-page']}>
      {/* Hero Banner */}
      {hero && (
        <div className={styles.hero}>
          <div className={styles.hero__backdrop}>
            {hero.heroImageUrl || hero.thumbnailUrl ? (
              <img src={hero.heroImageUrl || hero.thumbnailUrl} alt={hero.title} />
            ) : (
              <div style={{ width: '100%', height: '100%', background: 'linear-gradient(135deg, #1a1a2e, #16213e)' }} />
            )}
            <div className={styles['hero__gradient-bottom']} />
            <div className={styles['hero__gradient-left']} />
          </div>
          <div className={styles.hero__content}>
            {hero.isOriginal && <span className={styles.hero__tag}>N Original</span>}
            <h1 className={styles.hero__title}>{hero.title}</h1>
            <div className={styles.hero__meta}>
              <span className={styles.hero__rating}>★ {Number(hero.averageRating).toFixed(1)}</span>
              <span>{hero.releaseYear}</span>
              <span className="maturity-badge" style={{ borderColor: getMaturityColor(hero.maturityRating), color: getMaturityColor(hero.maturityRating) }}>
                {getMaturityLabel(hero.maturityRating)}
              </span>
              <span>{getContentTypeLabel(hero.contentType)}</span>
            </div>
            <p className={styles.hero__desc}>{hero.description}</p>
            <div className={styles.hero__actions}>
              <button className="btn btn--play" onClick={() => navigate(`/watch/${hero.id}`)}>
                <FiPlay /> Play
              </button>
              <button className="btn btn--secondary" onClick={() => navigate(`/title/${hero.id}`)}>
                <FiInfo /> More Info
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Content Rows */}
      <div className={styles['content-section']}>
        {continueWatching.length > 0 && (
          <ContentRow
            title="Continue Watching"
            items={continueWatching}
            navigate={navigate}
            isContinue
          />
        )}
        <ContentRow title="Trending Now" items={trending} navigate={navigate} />
        <ContentRow title="Movies" items={movies} navigate={navigate} />
        <ContentRow title="TV Shows" items={series} navigate={navigate} />
        <ContentRow title="Top Rated" items={topRated} navigate={navigate} />
      </div>
    </div>
  );
};

const ContentRow = ({ title, items, navigate, isContinue = false }) => {
  const sliderRef = useRef(null);

  const scroll = (direction) => {
    if (sliderRef.current) {
      const scrollAmount = sliderRef.current.offsetWidth * 0.8;
      sliderRef.current.scrollBy({
        left: direction === 'left' ? -scrollAmount : scrollAmount,
        behavior: 'smooth',
      });
    }
  };

  if (!items || items.length === 0) return null;

  return (
    <div className={styles['content-row']}>
      <div className={styles['content-row__header']}>
        <h2 className={styles['content-row__title']}>{title}</h2>
      </div>
      <div style={{ position: 'relative' }}>
        <button className={`${styles['content-row__arrow']} ${styles['content-row__arrow--left']}`} onClick={() => scroll('left')}>
          <FiChevronLeft />
        </button>
        <div className={styles['content-row__slider']} ref={sliderRef}>
          {items.map((item) => (
            <ContentCard
              key={item.id || item.contentId}
              item={item}
              navigate={navigate}
              isContinue={isContinue}
            />
          ))}
        </div>
        <button className={`${styles['content-row__arrow']} ${styles['content-row__arrow--right']}`} onClick={() => scroll('right')}>
          <FiChevronRight />
        </button>
      </div>
    </div>
  );
};

const ContentCard = ({ item, navigate, isContinue }) => {
  const contentId = item.contentId || item.id;
  const title = item.contentTitle || item.title;
  const thumbnail = item.contentThumbnailUrl || item.thumbnailUrl;

  return (
    <div
      className={styles['content-card']}
      onClick={() => navigate(isContinue ? `/watch/${contentId}` : `/title/${contentId}`)}
    >
      <div className={styles['content-card__image']}>
        {thumbnail ? (
          <img src={thumbnail} alt={title} loading="lazy" />
        ) : (
          <div className={styles['content-card__placeholder']}>{title}</div>
        )}
        {isContinue && item.stoppedAtSeconds > 0 && (
          <div className={styles['progress-bar']}>
            <div
              className={styles['progress-bar__fill']}
              style={{ width: `${Math.min(Math.round((item.stoppedAtSeconds / (item.totalDurationSeconds || 1)) * 100), 100)}%` }}
            />
          </div>
        )}
      </div>
      <div className={styles['content-card__info']}>
        <div className={styles['content-card__title-text']}>{title}</div>
        <div className={styles['content-card__meta']}>
          {item.averageRating > 0 && <span className={styles['content-card__match']}>★ {Number(item.averageRating).toFixed(1)}</span>}
          {item.releaseYear && <span>{item.releaseYear}</span>}
          {item.maturityRating !== undefined && (
            <span className="maturity-badge" style={{ borderColor: getMaturityColor(item.maturityRating), color: getMaturityColor(item.maturityRating), fontSize: '9px', padding: '1px 4px' }}>
              {getMaturityLabel(item.maturityRating)}
            </span>
          )}
        </div>
      </div>
    </div>
  );
};

const ContentRowSkeleton = () => (
  <div className={styles['content-row']}>
    <div className={styles['content-row__header']}>
      <div style={{ width: 200, height: 24, background: 'var(--bg-elevated)', borderRadius: 'var(--radius-sm)' }} />
    </div>
    <div className={styles['content-row__slider']}>
      {Array.from({ length: 8 }).map((_, i) => (
        <div key={i} className={styles['skeleton-card']} />
      ))}
    </div>
  </div>
);

export default BrowsePage;
