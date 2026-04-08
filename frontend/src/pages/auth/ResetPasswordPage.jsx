import { useState } from 'react';
import { useNavigate, useLocation, Link } from 'react-router-dom';
import { authService } from '../../api/authService';
import styles from '../../layouts/AuthLayout.module.css';
import '../../styles/components.css';

const ResetPasswordPage = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const emailFromState = location.state?.email || '';

  const [form, setForm] = useState({ email: emailFromState, otp: '', newPassword: '' });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
    setError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      await authService.resetPassword(form);
      setSuccess(true);
      setTimeout(() => navigate('/login'), 2500);
    } catch (err) {
      setError(err.response?.data?.message || 'Invalid or expired OTP. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  if (success) {
    return (
      <div className={styles['auth-card']}>
        <h1 className={styles['auth-card__title']}>Password Reset!</h1>
        <p className={styles['auth-card__subtitle']} style={{ color: 'var(--success)' }}>
          Your password has been reset successfully. Redirecting to login...
        </p>
      </div>
    );
  }

  return (
    <div className={styles['auth-card']}>
      <h1 className={styles['auth-card__title']}>Reset Password</h1>
      <p className={styles['auth-card__subtitle']}>Enter the OTP from your email and your new password.</p>

      <form onSubmit={handleSubmit}>
        {!emailFromState && (
          <div className="form-group">
            <input
              type="email"
              name="email"
              id="reset-email"
              className="form-input"
              placeholder=" "
              value={form.email}
              onChange={handleChange}
              required
            />
            <label htmlFor="reset-email" className="form-label">Email</label>
          </div>
        )}

        <div className="form-group">
          <input
            type="text"
            name="otp"
            id="reset-otp"
            className="form-input"
            placeholder=" "
            value={form.otp}
            onChange={(e) => setForm({ ...form, otp: e.target.value.replace(/\D/g, '').slice(0, 6) })}
            required
            maxLength={6}
            style={{ letterSpacing: '8px', fontSize: 'var(--fs-2xl)', textAlign: 'center' }}
          />
          <label htmlFor="reset-otp" className="form-label">OTP Code</label>
        </div>

        <div className="form-group">
          <input
            type="password"
            name="newPassword"
            id="reset-password"
            className="form-input"
            placeholder=" "
            value={form.newPassword}
            onChange={handleChange}
            required
            minLength={6}
          />
          <label htmlFor="reset-password" className="form-label">New Password</label>
        </div>

        {error && <span className="form-error">{error}</span>}

        <button type="submit" className="btn btn--primary" disabled={loading}>
          {loading ? <span className="spinner spinner--sm" /> : 'Reset Password'}
        </button>
      </form>

      <p style={{ marginTop: 'var(--space-xl)', textAlign: 'center' }}>
        <Link to="/login" className="link-text">Back to Sign In</Link>
      </p>
    </div>
  );
};

export default ResetPasswordPage;
