import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { profileService } from '../../api/profileService';
import { useAuthStore } from '../../stores/authStore';
import { FiEdit2, FiTrash2 } from 'react-icons/fi';
import styles from './ProfilesPage.module.css';
import '../../styles/components.css';

const ProfileSelectorPage = () => {
  const navigate = useNavigate();
  const { setProfileToken } = useAuthStore();
  const [profiles, setProfiles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [pinModal, setPinModal] = useState(null);
  const [pin, setPin] = useState('');
  const [error, setError] = useState('');
  const [logging, setLogging] = useState(false);
  const [manageMode, setManageMode] = useState(false);
  const [deleting, setDeleting] = useState(null);

  useEffect(() => {
    fetchProfiles();
  }, []);

  const fetchProfiles = async () => {
    try {
      const { data } = await profileService.getProfiles();
      setProfiles(data);
    } catch (err) {
      if (err.response?.status === 401) {
        navigate('/login');
      }
    } finally {
      setLoading(false);
    }
  };

  const handleProfileClick = (profile) => {
    if (manageMode) return;
    if (profile.hasPin) {
      setPinModal(profile);
      setPin('');
      setError('');
    } else {
      loginToProfile(profile.id, null);
    }
  };

  const loginToProfile = async (profileId, pinValue) => {
    setLogging(true);
    setError('');
    try {
      const { data } = await profileService.loginToProfile({
        profileId,
        pin: pinValue,
      });
      setProfileToken(data.accessToken);
      navigate('/browse');
    } catch (err) {
      setError(err.response?.data?.message || err.response?.data || 'Failed to login. Incorrect PIN?');
    } finally {
      setLogging(false);
    }
  };

  const handlePinSubmit = (e) => {
    e.preventDefault();
    if (pinModal) {
      loginToProfile(pinModal.id, pin);
    }
  };

  const handleDeleteProfile = async (e, profileId) => {
    e.stopPropagation();
    if (!window.confirm('Permanently delete this profile? This removes all watch history and ratings.')) return;
    setDeleting(profileId);
    try {
      // Need to login to the profile first to delete it
      await profileService.loginToProfile({ profileId, pin: null });
      await profileService.deleteProfile();
      setProfiles((prev) => prev.filter((p) => p.id !== profileId));
    } catch (err) {
      alert(err.response?.data?.message || 'Failed to delete profile. It may be PIN-protected.');
    } finally {
      setDeleting(null);
    }
  };

  const getAvatarClass = (index) => {
    return styles[`profile-card__avatar--${(index % 5) + 1}`];
  };

  if (loading) {
    return <div className="loading-screen"><div className="spinner" /></div>;
  }

  return (
    <div className={styles['profiles-page']}>
      <h1 className={styles['profiles-page__title']}>
        {manageMode ? 'Manage Profiles' : "Who\u2019s watching?"}
      </h1>

      <div className={styles['profiles-grid']}>
        {profiles.map((profile, idx) => (
          <div
            key={profile.id}
            className={styles['profile-card']}
            onClick={() => handleProfileClick(profile)}
            style={{ opacity: manageMode ? 0.7 : 1, cursor: manageMode ? 'default' : 'pointer' }}
          >
            <div
              className={`${styles['profile-card__avatar']} ${profile.isKidsMode ? styles['profile-card__avatar--kids'] : getAvatarClass(idx)}`}
              style={{ position: 'relative' }}
            >
              {profile.avatarUrl ? (
                <img src={profile.avatarUrl} alt={profile.name} />
              ) : (
                profile.name[0]?.toUpperCase()
              )}
              {!manageMode && profile.hasPin && <span className={styles['profile-card__lock']}>🔒</span>}
              {manageMode && (
                <div style={{
                  position: 'absolute',
                  inset: 0,
                  background: 'rgba(0,0,0,0.6)',
                  borderRadius: 'inherit',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  gap: 'var(--space-md)',
                }}>
                  <button
                    onClick={(e) => { e.stopPropagation(); navigate('/profiles/edit'); }}
                    style={{
                      width: 40, height: 40, borderRadius: '50%', border: '2px solid white',
                      background: 'transparent', color: 'white', cursor: 'pointer',
                      display: 'flex', alignItems: 'center', justifyContent: 'center',
                      transition: 'all var(--transition-fast)',
                    }}
                    title="Edit"
                  >
                    <FiEdit2 size={16} />
                  </button>
                  <button
                    onClick={(e) => handleDeleteProfile(e, profile.id)}
                    disabled={deleting === profile.id}
                    style={{
                      width: 40, height: 40, borderRadius: '50%', border: '2px solid var(--netflix-red)',
                      background: 'transparent', color: 'var(--netflix-red)', cursor: 'pointer',
                      display: 'flex', alignItems: 'center', justifyContent: 'center',
                      transition: 'all var(--transition-fast)',
                    }}
                    title="Delete"
                  >
                    {deleting === profile.id ? '...' : <FiTrash2 size={16} />}
                  </button>
                </div>
              )}
            </div>
            <span className={styles['profile-card__name']}>{profile.name}</span>
          </div>
        ))}

        {!manageMode && (
          <div className={styles['profile-card']} onClick={() => navigate('/profiles/create')}>
            <div className={styles['profile-add']}>+</div>
            <span className={styles['profile-card__name']}>Add Profile</span>
          </div>
        )}
      </div>

      {/* Manage toggle */}
      <div style={{ textAlign: 'center', marginTop: 'var(--space-2xl)' }}>
        <button
          onClick={() => setManageMode(!manageMode)}
          style={{
            padding: 'var(--space-sm) var(--space-2xl)',
            background: manageMode ? 'white' : 'transparent',
            color: manageMode ? 'black' : 'var(--text-secondary)',
            border: `1px solid ${manageMode ? 'white' : 'var(--text-muted)'}`,
            borderRadius: 'var(--radius-sm)',
            fontSize: 'var(--fs-md)',
            fontWeight: 500,
            cursor: 'pointer',
            letterSpacing: 1,
            transition: 'all var(--transition-fast)',
          }}
        >
          {manageMode ? 'Done' : 'Manage Profiles'}
        </button>
      </div>

      {/* PIN Modal */}
      {pinModal && (
        <div className={styles['pin-modal__backdrop']} onClick={() => setPinModal(null)}>
          <div className={styles['pin-modal']} onClick={(e) => e.stopPropagation()}>
            <h2 className={styles['pin-modal__title']}>Enter PIN</h2>
            <p className={styles['pin-modal__subtitle']}>
              This profile is PIN-protected. Enter the 6-digit PIN.
            </p>
            <form onSubmit={handlePinSubmit}>
              <input
                type="password"
                className={styles['pin-modal__input']}
                value={pin}
                onChange={(e) => setPin(e.target.value.replace(/\D/g, '').slice(0, 6))}
                maxLength={6}
                autoFocus
                placeholder="••••••"
              />
              {error && <span className="form-error">{error}</span>}
              <div className={styles['pin-modal__actions']}>
                <button
                  type="button"
                  className="btn btn--ghost"
                  onClick={() => setPinModal(null)}
                  style={{ flex: 1 }}
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  className="btn btn--primary"
                  disabled={pin.length !== 6 || logging}
                  style={{ flex: 1, marginTop: 0 }}
                >
                  {logging ? '...' : 'Continue'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProfileSelectorPage;
