import { useState } from 'react';
import { Link } from 'react-router-dom';
import { authService } from '../../api/authService';
import styles from '../../layouts/AuthLayout.module.css';
import '../../styles/components.css';

const ForgotPasswordPage = () => {
  const [email, setEmail] = useState('');
  const [submitted, setSubmitted] = useState(false);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      await authService.forgotPassword(email);
      setSubmitted(true);
    } catch (err) {
      setError(err.response?.data?.message || 'Something went wrong. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  if (submitted) {
    return (
      <div className={styles['auth-card']}>
        <h1 className={styles['auth-card__title']}>Check Your Email</h1>
        <p className={styles['auth-card__subtitle']}>
          If an account exists for <strong>{email}</strong>, you will receive a password reset OTP shortly.
        </p>
        <Link to="/reset-password" state={{ email }}>
          <button className="btn btn--primary">Enter Reset Code</button>
        </Link>
        <p style={{ marginTop: 'var(--space-lg)', textAlign: 'center' }}>
          <Link to="/login" className="link-text">Back to Sign In</Link>
        </p>
      </div>
    );
  }

  return (
    <div className={styles['auth-card']}>
      <h1 className={styles['auth-card__title']}>Forgot Password</h1>
      <p className={styles['auth-card__subtitle']}>
        Enter your email and we&apos;ll send you a code to reset your password.
      </p>

      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <input
            type="email"
            id="forgot-email"
            className="form-input"
            placeholder=" "
            value={email}
            onChange={(e) => { setEmail(e.target.value); setError(''); }}
            required
          />
          <label htmlFor="forgot-email" className="form-label">Email</label>
        </div>

        {error && <span className="form-error">{error}</span>}

        <button type="submit" className="btn btn--primary" disabled={loading}>
          {loading ? <span className="spinner spinner--sm" /> : 'Send Reset Code'}
        </button>
      </form>

      <p style={{ marginTop: 'var(--space-xl)', textAlign: 'center' }}>
        <Link to="/login" className="link-text">Back to Sign In</Link>
      </p>
    </div>
  );
};

export default ForgotPasswordPage;
