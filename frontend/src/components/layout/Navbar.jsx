import { useState, useEffect, useRef } from 'react';
import { useNavigate, useLocation, Link } from 'react-router-dom';
import { FiSearch, FiX } from 'react-icons/fi';
import { useAuthStore } from '../../stores/authStore';
import { authService } from '../../api/authService';
import styles from './Navbar.module.css';

const Navbar = () => {
  const [isScrolled, setIsScrolled] = useState(false);
  const [showSearch, setShowSearch] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');
  const [showDropdown, setShowDropdown] = useState(false);
  const searchRef = useRef(null);
  const dropdownRef = useRef(null);
  const navigate = useNavigate();
  const location = useLocation();
  const { user, logout, refreshToken } = useAuthStore();

  useEffect(() => {
    const handleScroll = () => setIsScrolled(window.scrollY > 0);
    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  useEffect(() => {
    const handleClickOutside = (e) => {
      if (dropdownRef.current && !dropdownRef.current.contains(e.target)) {
        setShowDropdown(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const handleSearch = (e) => {
    e.preventDefault();
    if (searchQuery.trim()) {
      navigate(`/search?q=${encodeURIComponent(searchQuery.trim())}`);
      setShowSearch(false);
    }
  };

  const handleLogout = async () => {
    try {
      if (refreshToken) {
        await authService.logout(refreshToken);
      }
    } catch (err) {
      console.error('Logout API failed', err);
    } finally {
      logout();
      navigate('/login');
    }
  };

  const isActive = (path) => location.pathname === path;

  return (
    <nav className={`${styles.navbar} ${isScrolled ? styles['navbar--solid'] : styles['navbar--transparent']}`}>
      <div className={styles.navbar__content}>
        <div className={styles.navbar__left}>
          <Link to="/browse" className={styles.navbar__logo}>
            NETFLIX
          </Link>
          <ul className={styles.navbar__links}>
            <li>
              <button
                className={`${styles.navbar__link} ${isActive('/browse') ? styles['navbar__link--active'] : ''}`}
                onClick={() => navigate('/browse')}
              >
                Home
              </button>
            </li>
            <li>
              <button
                className={`${styles.navbar__link} ${isActive('/search') ? styles['navbar__link--active'] : ''}`}
                onClick={() => navigate('/search?type=2')}
              >
                TV Shows
              </button>
            </li>
            <li>
              <button
                className={`${styles.navbar__link} ${isActive('/search') && location.search.includes('type=1') ? styles['navbar__link--active'] : ''}`}
                onClick={() => navigate('/search?type=1')}
              >
                Movies
              </button>
            </li>
            <li>
              <button
                className={styles.navbar__link}
                onClick={() => navigate('/my-list')}
              >
                My List
              </button>
            </li>
            <li>
              <button
                className={`${styles.navbar__link} ${isActive('/my-ratings') ? styles['navbar__link--active'] : ''}`}
                onClick={() => navigate('/my-ratings')}
              >
                My Ratings
              </button>
            </li>
          </ul>
        </div>

        <div className={styles.navbar__right}>
          {showSearch ? (
            <form onSubmit={handleSearch} className={styles['navbar__search-input-wrapper']}>
              <FiSearch size={16} color="white" />
              <input
                ref={searchRef}
                type="text"
                placeholder="Titles, people, genres"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className={styles['navbar__search-input']}
                autoFocus
              />
              <button type="button" onClick={() => { setShowSearch(false); setSearchQuery(''); }}>
                <FiX size={16} color="white" />
              </button>
            </form>
          ) : (
            <button
              className={styles['navbar__search-btn']}
              onClick={() => setShowSearch(true)}
              aria-label="Search"
            >
              <FiSearch size={20} />
            </button>
          )}

          <div className={styles.navbar__profile} ref={dropdownRef}>
            <button
              className={styles.navbar__avatar}
              onClick={() => setShowDropdown(!showDropdown)}
              aria-label="Profile menu"
            >
              {user?.name?.[0] || user?.email?.[0] || 'U'}
            </button>

            {showDropdown && (
              <div className={styles.navbar__dropdown}>
                <button
                  className={styles['navbar__dropdown-item']}
                  onClick={() => { navigate('/profiles'); setShowDropdown(false); }}
                >
                  Switch Profile
                </button>
                <button
                  className={styles['navbar__dropdown-item']}
                  onClick={() => { navigate('/account'); setShowDropdown(false); }}
                >
                  Account
                </button>
                <div className={styles['navbar__dropdown-divider']} />
                <button
                  className={styles['navbar__dropdown-item']}
                  onClick={handleLogout}
                >
                  Sign out
                </button>
              </div>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
