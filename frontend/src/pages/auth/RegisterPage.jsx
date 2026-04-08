import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { authService } from '../../api/authService';
import styles from '../../layouts/AuthLayout.module.css';
import '../../styles/components.css';

const RegisterPage = () => {
  const navigate = useNavigate();
  const [form, setForm] = useState({ firstName: '', lastName: '', email: '', password: '' });
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
      await authService.register(form);
      navigate('/confirm-email', { state: { email: form.email } });
    } catch (err) {
      setError(err.response?.data?.message || err.response?.data || 'Registration failed. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles['auth-card']}>
      <h1 className={styles['auth-card__title']}>Sign Up</h1>

      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <input
            type="text"
            name="firstName"
            id="register-firstName"
            className={`form-input ${error ? 'form-input--error' : ''}`}
            placeholder=" "
            value={form.firstName}
            onChange={handleChange}
            required
            minLength={2}
          />
          <label htmlFor="register-firstName" className="form-label">First Name</label>
        </div>

        <div className="form-group">
          <input
            type="text"
            name="lastName"
            id="register-lastName"
            className="form-input"
            placeholder=" "
            value={form.lastName}
            onChange={handleChange}
            required
            minLength={2}
          />
          <label htmlFor="register-lastName" className="form-label">Last Name</label>
        </div>

        <div className="form-group">
          <input
            type="email"
            name="email"
            id="register-email"
            className="form-input"
            placeholder=" "
            value={form.email}
            onChange={handleChange}
            required
          />
          <label htmlFor="register-email" className="form-label">Email</label>
        </div>

        <div className="form-group">
          <input
            type="password"
            name="password"
            id="register-password"
            className="form-input"
            placeholder=" "
            value={form.password}
            onChange={handleChange}
            required
            minLength={6}
          />
          <label htmlFor="register-password" className="form-label">Password</label>
        </div>

        {error && <span className="form-error">{error}</span>}

        <button type="submit" className="btn btn--primary" disabled={loading}>
          {loading ? <span className="spinner spinner--sm" /> : 'Sign Up'}
        </button>
      </form>

      <p style={{ marginTop: 'var(--space-xl)', color: 'var(--text-secondary)', fontSize: 'var(--fs-md)' }}>
        Already have an account?{' '}
        <Link to="/login" className="link-text link-text--highlight">Sign in</Link>
      </p>
    </div>
  );
};

export default RegisterPage;
