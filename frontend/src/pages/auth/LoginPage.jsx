import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { authService } from '../../api/authService';
import { subscriptionService } from '../../api/subscriptionService';
import { useAuthStore } from '../../stores/authStore';
import styles from '../../layouts/AuthLayout.module.css';
import '../../styles/components.css';

const LoginPage = () => {
  const navigate = useNavigate();
  const { login } = useAuthStore();
  const [form, setForm] = useState({ email: '', password: '' });
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
      const { data } = await authService.login(form);
      login(data.token, data.refreshToken, {
        email: data.email,
        fullName: data.fullName,
        roles: data.roles,
      });

      // Check if user has an active subscription
      const isSubscriber = data.roles.some(r =>
        ['Subscriber', 'SuperAdmin', 'ContentManager'].includes(r)
      );

      if (!isSubscriber) {
        navigate('/plans');
      } else {
        navigate('/profiles');
      }
    } catch (err) {
      const msg = err.response?.data?.message || err.response?.data || '';
      if (typeof msg === 'string' && msg.toLowerCase().includes('email not confirmed')) {
        setError('Email not confirmed. Please check your inbox for the verification code.');
      } else {
        setError(msg || 'Login failed. Please check your credentials.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles['auth-card']}>
      <h1 className={styles['auth-card__title']}>Sign In</h1>

      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <input
            type="email"
            name="email"
            id="login-email"
            className={`form-input ${error ? 'form-input--error' : ''}`}
            placeholder=" "
            value={form.email}
            onChange={handleChange}
            required
          />
          <label htmlFor="login-email" className="form-label">Email</label>
        </div>

        <div className="form-group">
          <input
            type="password"
            name="password"
            id="login-password"
            className="form-input"
            placeholder=" "
            value={form.password}
            onChange={handleChange}
            required
          />
          <label htmlFor="login-password" className="form-label">Password</label>
        </div>

        {error && <span className="form-error">{error}</span>}

        <button type="submit" className="btn btn--primary" disabled={loading}>
          {loading ? <span className="spinner spinner--sm" /> : 'Sign In'}
        </button>
      </form>

      <div style={{ marginTop: 'var(--space-lg)', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Link to="/forgot-password" className="link-text">Forgot password?</Link>
      </div>

      <p style={{ marginTop: 'var(--space-xl)', color: 'var(--text-secondary)', fontSize: 'var(--fs-md)' }}>
        New to Netflix?{' '}
        <Link to="/register" className="link-text link-text--highlight">Sign up now</Link>
      </p>
    </div>
  );
};

export default LoginPage;
