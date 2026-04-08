import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { catalogService } from '../../api/catalogService';
import { getMaturityLabel, getMaturityColor, getContentTypeLabel } from '../../utils/constants';
import { FiSearch } from 'react-icons/fi';
import '../../styles/components.css';

const SearchPage = () => {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();
  const [query, setQuery] = useState(searchParams.get('q') || '');
  const [results, setResults] = useState([]);
  const [loading, setLoading] = useState(false);
  const [contentType, setContentType] = useState(searchParams.get('type') || '');

  useEffect(() => {
    const q = searchParams.get('q') || '';
    const type = searchParams.get('type') || '';
    setQuery(q);
    setContentType(type);
    if (q || type) {
      performSearch(q, type);
    }
  }, [searchParams]);

  const performSearch = async (q, type) => {
    setLoading(true);
    try {
      const params = { pageSize: 50 };
      if (q) params.searchQuery = q;
      if (type) params.contentTypes = [parseInt(type)];
      const { data } = await catalogService.getCatalog(params);
      setResults(data.items || []);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = (e) => {
    e.preventDefault();
    const params = {};
    if (query) params.q = query;
    if (contentType) params.type = contentType;
    setSearchParams(params);
  };

  const handleTypeFilter = (type) => {
    const newType = contentType === type ? '' : type;
    setContentType(newType);
    const params = {};
    if (query) params.q = query;
    if (newType) params.type = newType;
    setSearchParams(params);
  };

  return (
    <div style={{ minHeight: '100vh', background: 'var(--bg-primary)', padding: 'var(--space-xl) 4%' }}>
      {/* Search Header */}
      <form onSubmit={handleSearch} style={{ display: 'flex', gap: 'var(--space-md)', marginBottom: 'var(--space-xl)', maxWidth: 600 }}>
        <div style={{ flex: 1, position: 'relative' }}>
          <FiSearch size={18} style={{ position: 'absolute', left: 14, top: '50%', transform: 'translateY(-50%)', color: 'var(--text-muted)' }} />
          <input
            type="text"
            placeholder="Search titles, people, genres..."
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            style={{ width: '100%', padding: '12px 12px 12px 42px', background: 'var(--bg-input)', border: '1px solid var(--border-color)', borderRadius: 'var(--radius-sm)', color: 'var(--text-primary)', fontSize: 'var(--fs-md)' }}
          />
        </div>
        <button type="submit" className="btn btn--primary" style={{ width: 'auto', marginTop: 0 }}>Search</button>
      </form>

      {/* Type Filters */}
      <div style={{ display: 'flex', gap: 'var(--space-sm)', marginBottom: 'var(--space-xl)', flexWrap: 'wrap' }}>
        {[{ value: '1', label: 'Movies' }, { value: '2', label: 'TV Shows' }, { value: '3', label: 'Documentaries' }].map((t) => (
          <button
            key={t.value}
            onClick={() => handleTypeFilter(t.value)}
            style={{
              padding: '8px 20px',
              borderRadius: 'var(--radius-full)',
              border: contentType === t.value ? '1px solid var(--text-primary)' : '1px solid var(--border-color)',
              background: contentType === t.value ? 'var(--text-primary)' : 'transparent',
              color: contentType === t.value ? 'var(--bg-primary)' : 'var(--text-secondary)',
              fontSize: 'var(--fs-sm)',
              cursor: 'pointer',
              transition: 'all var(--transition-fast)',
            }}
          >
            {t.label}
          </button>
        ))}
      </div>

      {/* Results */}
      {loading ? (
        <div style={{ display: 'flex', justifyContent: 'center', padding: 'var(--space-4xl)' }}>
          <div className="spinner" />
        </div>
      ) : results.length === 0 ? (
        <div style={{ textAlign: 'center', padding: 'var(--space-4xl)', color: 'var(--text-muted)' }}>
          <p style={{ fontSize: 'var(--fs-xl)', marginBottom: 'var(--space-md)' }}>
            {query || contentType ? 'No results found' : 'Start searching for your next binge'}
          </p>
          <p style={{ fontSize: 'var(--fs-sm)' }}>Try searching for a title, actor, or genre</p>
        </div>
      ) : (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))', gap: 'var(--space-md)' }}>
          {results.map((item) => (
            <div
              key={item.id}
              onClick={() => navigate(`/title/${item.id}`)}
              style={{
                cursor: 'pointer',
                borderRadius: 'var(--radius-sm)',
                overflow: 'hidden',
                background: 'var(--bg-card)',
                transition: 'transform var(--transition-base), box-shadow var(--transition-base)',
              }}
              onMouseEnter={(e) => { e.currentTarget.style.transform = 'scale(1.05)'; e.currentTarget.style.boxShadow = 'var(--shadow-card)'; }}
              onMouseLeave={(e) => { e.currentTarget.style.transform = 'scale(1)'; e.currentTarget.style.boxShadow = 'none'; }}
            >
              <div style={{ height: 120, overflow: 'hidden' }}>
                {item.thumbnailUrl ? (
                  <img src={item.thumbnailUrl} alt={item.title} style={{ width: '100%', height: '100%', objectFit: 'cover' }} />
                ) : (
                  <div style={{ width: '100%', height: '100%', background: 'linear-gradient(135deg, var(--bg-card), var(--bg-elevated))', display: 'flex', alignItems: 'center', justifyContent: 'center', color: 'var(--text-muted)', fontSize: 'var(--fs-xs)', padding: 'var(--space-sm)', textAlign: 'center' }}>
                    {item.title}
                  </div>
                )}
              </div>
              <div style={{ padding: 'var(--space-sm)' }}>
                <div style={{ fontSize: 'var(--fs-sm)', fontWeight: 600, marginBottom: 4, whiteSpace: 'nowrap', overflow: 'hidden', textOverflow: 'ellipsis' }}>{item.title}</div>
                <div style={{ display: 'flex', alignItems: 'center', gap: 'var(--space-xs)', fontSize: '11px', color: 'var(--text-muted)' }}>
                  <span style={{ color: 'var(--success)' }}>★ {Number(item.averageRating).toFixed(1)}</span>
                  <span>{item.releaseYear}</span>
                  <span className="maturity-badge" style={{ borderColor: getMaturityColor(item.maturityRating), color: getMaturityColor(item.maturityRating), fontSize: '9px', padding: '1px 4px' }}>
                    {getMaturityLabel(item.maturityRating)}
                  </span>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default SearchPage;
