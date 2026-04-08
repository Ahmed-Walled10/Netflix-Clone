import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { profileService } from '../../api/profileService';
import { useAuthStore } from '../../stores/authStore';
import styles from '../profiles/ProfilesPage.module.css';
import '../../styles/components.css';

const EditProfilePage = () => {
  const navigate = useNavigate();
  const { user } = useAuthStore();
  const [form, setForm] = useState({
    name: '',
    age: '',
    pinHash: '',
    preferredLanguage: 'en',
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
    setError('');
    setSuccess('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    setSuccess('');

    try {
      const payload = {};
      if (form.name.trim()) payload.name = form.name.trim();
      if (form.age) payload.age = parseInt(form.age);
      if (form.pinHash && form.pinHash.length === 6) payload.pinHash = form.pinHash;
      if (form.preferredLanguage) payload.preferredLanguage = form.preferredLanguage;

      await profileService.updateProfile(payload);
      setSuccess('Profile updated successfully!');
      setTimeout(() => setSuccess(''), 3000);
    } catch (err) {
      setError(err.response?.data?.message || err.response?.data || 'Failed to update profile.');
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteProfile = async () => {
    if (!window.confirm('Are you sure you want to delete this profile? This action cannot be undone.')) return;

    try {
      await profileService.deleteProfile();
      navigate('/profiles');
    } catch (err) {
      setError(err.response?.data?.message || err.response?.data || 'Failed to delete profile.');
    }
  };

  return (
    <div className={styles['create-profile']}>
      <div className={styles['create-profile__card']}>
        <h1 className={styles['create-profile__title']}>Edit Profile</h1>
        <p className={styles['create-profile__subtitle']}>
          Update your profile settings. Leave fields empty to keep them unchanged.
        </p>

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <input
              type="text"
              name="name"
              id="edit-name"
              className="form-input"
              placeholder=" "
              value={form.name}
              onChange={handleChange}
              maxLength={20}
            />
            <label htmlFor="edit-name" className="form-label">New Name</label>
          </div>

          <div className="form-group">
            <input
              type="number"
              name="age"
              id="edit-age"
              className="form-input"
              placeholder=" "
              value={form.age}
              onChange={handleChange}
              min={1}
              max={120}
            />
            <label htmlFor="edit-age" className="form-label">New Age</label>
          </div>

          <div className="form-group">
            <input
              type="password"
              name="pinHash"
              id="edit-pin"
              className="form-input"
              placeholder=" "
              value={form.pinHash}
              onChange={(e) => setForm({ ...form, pinHash: e.target.value.replace(/\D/g, '').slice(0, 6) })}
              maxLength={6}
            />
            <label htmlFor="edit-pin" className="form-label">New PIN (6 digits)</label>
          </div>

          <div className="form-group">
            <select
              name="preferredLanguage"
              id="edit-language"
              className="form-input"
              value={form.preferredLanguage}
              onChange={handleChange}
              style={{ paddingTop: 'var(--space-md)' }}
            >
              <option value="en">English</option>
              <option value="es">Spanish</option>
              <option value="fr">French</option>
              <option value="de">German</option>
              <option value="ar">Arabic</option>
              <option value="ja">Japanese</option>
              <option value="ko">Korean</option>
            </select>
            <label htmlFor="edit-language" className="form-label" style={{ top: '10px', transform: 'none', fontSize: 'var(--fs-xs)' }}>Language</label>
          </div>

          {form.age && parseInt(form.age) < 13 && (
            <p style={{ color: 'var(--warning)', fontSize: 'var(--fs-sm)', marginBottom: 'var(--space-md)' }}>
              ⚠️ Kids Mode will be automatically enabled for users under 13.
            </p>
          )}

          {error && <span className="form-error">{error}</span>}
          {success && <span style={{ color: 'var(--success)', fontSize: 'var(--fs-sm)', display: 'block', marginTop: 'var(--space-xs)' }}>{success}</span>}

          <div style={{ display: 'flex', gap: 'var(--space-md)', marginTop: 'var(--space-xl)' }}>
            <button type="button" className="btn btn--ghost" onClick={() => navigate(-1)} style={{ flex: 1 }}>
              Cancel
            </button>
            <button type="submit" className="btn btn--primary" disabled={loading} style={{ flex: 1, marginTop: 0 }}>
              {loading ? <span className="spinner spinner--sm" /> : 'Save Changes'}
            </button>
          </div>
        </form>

        {/* Delete Profile */}
        <div style={{ marginTop: 'var(--space-3xl)', paddingTop: 'var(--space-xl)', borderTop: '1px solid var(--border-color)' }}>
          <h3 style={{ fontSize: 'var(--fs-md)', color: 'var(--netflix-red)', marginBottom: 'var(--space-sm)' }}>Danger Zone</h3>
          <p style={{ fontSize: 'var(--fs-sm)', color: 'var(--text-muted)', marginBottom: 'var(--space-md)' }}>
            Permanently delete this profile and all its watch history and ratings.
          </p>
          <button
            onClick={handleDeleteProfile}
            style={{
              padding: 'var(--space-sm) var(--space-xl)',
              background: 'transparent',
              border: '1px solid var(--netflix-red)',
              borderRadius: 'var(--radius-sm)',
              color: 'var(--netflix-red)',
              fontSize: 'var(--fs-sm)',
              fontWeight: 600,
              cursor: 'pointer',
              transition: 'all var(--transition-fast)',
            }}
            onMouseEnter={(e) => { e.currentTarget.style.background = 'var(--netflix-red)'; e.currentTarget.style.color = 'white'; }}
            onMouseLeave={(e) => { e.currentTarget.style.background = 'transparent'; e.currentTarget.style.color = 'var(--netflix-red)'; }}
          >
            Delete This Profile
          </button>
        </div>
      </div>
    </div>
  );
};

export default EditProfilePage;
