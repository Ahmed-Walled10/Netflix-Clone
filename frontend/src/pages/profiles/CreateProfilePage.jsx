import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { profileService } from '../../api/profileService';
import { useAuthStore } from '../../stores/authStore';
import styles from './ProfilesPage.module.css';
import '../../styles/components.css';

const CreateProfilePage = () => {
  const navigate = useNavigate();
  const { setProfileToken } = useAuthStore();
  const [form, setForm] = useState({ name: '', age: '', pinHash: '', preferredLanguage: 'en' });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
    setError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      const payload = {
        name: form.name,
        age: parseInt(form.age),
        preferredLanguage: form.preferredLanguage,
      };
      if (form.pinHash && form.pinHash.length === 6) {
        payload.pinHash = form.pinHash;
      }

      const { data } = await profileService.createProfile(payload);
      setProfileToken(data.accessToken);
      navigate('/browse');
    } catch (err) {
      setError(err.response?.data?.message || err.response?.data || 'Failed to create profile.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles['create-profile']}>
      <div className={styles['create-profile__card']}>
        <h1 className={styles['create-profile__title']}>Add Profile</h1>
        <p className={styles['create-profile__subtitle']}>
          Add a profile for another person watching Netflix.
        </p>

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <input
              type="text"
              name="name"
              id="profile-name"
              className="form-input"
              placeholder=" "
              value={form.name}
              onChange={handleChange}
              required
              maxLength={20}
            />
            <label htmlFor="profile-name" className="form-label">Name</label>
          </div>

          <div className="form-group">
            <input
              type="number"
              name="age"
              id="profile-age"
              className="form-input"
              placeholder=" "
              value={form.age}
              onChange={handleChange}
              required
              min={1}
              max={120}
            />
            <label htmlFor="profile-age" className="form-label">Age</label>
          </div>

          <div className="form-group">
            <input
              type="password"
              name="pinHash"
              id="profile-pin"
              className="form-input"
              placeholder=" "
              value={form.pinHash}
              onChange={(e) => setForm({ ...form, pinHash: e.target.value.replace(/\D/g, '').slice(0, 6) })}
              maxLength={6}
            />
            <label htmlFor="profile-pin" className="form-label">PIN (optional, 6 digits)</label>
          </div>

          <div className="form-group">
            <select
              name="preferredLanguage"
              id="profile-language"
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
            <label htmlFor="profile-language" className="form-label" style={{ top: '10px', transform: 'none', fontSize: 'var(--fs-xs)' }}>Language</label>
          </div>

          {form.age && parseInt(form.age) < 13 && (
            <p style={{ color: 'var(--warning)', fontSize: 'var(--fs-sm)', marginBottom: 'var(--space-md)' }}>
              ⚠️ Kids Mode will be automatically enabled for users under 13.
            </p>
          )}

          {error && <span className="form-error">{error}</span>}

          <div style={{ display: 'flex', gap: 'var(--space-md)', marginTop: 'var(--space-xl)' }}>
            <button type="button" className="btn btn--ghost" onClick={() => navigate('/profiles')} style={{ flex: 1 }}>
              Cancel
            </button>
            <button type="submit" className="btn btn--primary" disabled={loading} style={{ flex: 1, marginTop: 0 }}>
              {loading ? <span className="spinner spinner--sm" /> : 'Create'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateProfilePage;
