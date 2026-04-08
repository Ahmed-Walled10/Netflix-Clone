import { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { authService } from '../../api/authService';
import styles from '../../layouts/AuthLayout.module.css';
import '../../styles/components.css';

const ConfirmEmailPage = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const emailFromState = location.state?.email || '';

  const [email, setEmail] = useState(emailFromState);
  const [otp, setOtp] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);
  const [resending, setResending] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      await authService.confirmEmail({ email, otp });
      setSuccess('Email confirmed! Redirecting to login...');
      setTimeout(() => navigate('/login'), 2000);
    } catch (err) {
      setError(err.response?.data?.message || 'Invalid or expired OTP. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const handleResend = async () => {
    if (!email) {
      setError('Please enter your email first.');
      return;
    }
    setResending(true);
    setError('');
    try {
      await authService.resendConfirmationOtp(email);
      setSuccess('New OTP sent! Check your email.');
      setTimeout(() => setSuccess(''), 3000);
    } catch (err) {
      setError(err.response?.data?.message || 'Could not resend OTP.');
    } finally {
      setResending(false);
    }
  };

  return (
    <div className={styles['auth-card']}>
      <h1 className={styles['auth-card__title']}>Confirm Your Email</h1>
      <p className={styles['auth-card__subtitle']}>
        We sent a 6-digit code to <strong>{email || 'your email'}</strong>. Enter it below to verify your account.
      </p>

      <form onSubmit={handleSubmit}>
        {!emailFromState && (
          <div className="form-group">
            <input
              type="email"
              id="confirm-email"
              className="form-input"
              placeholder=" "
              value={email}
              onChange={(e) => { setEmail(e.target.value); setError(''); }}
              required
            />
            <label htmlFor="confirm-email" className="form-label">Email</label>
          </div>
        )}

        <div className="form-group">
          <input
            type="text"
            id="confirm-otp"
            className="form-input"
            placeholder=" "
            value={otp}
            onChange={(e) => { setOtp(e.target.value.replace(/\D/g, '').slice(0, 6)); setError(''); }}
            required
            maxLength={6}
            style={{ letterSpacing: '8px', fontSize: 'var(--fs-2xl)', textAlign: 'center' }}
          />
          <label htmlFor="confirm-otp" className="form-label">Verification Code</label>
        </div>

        {error && <span className="form-error">{error}</span>}
        {success && <span style={{ color: 'var(--success)', fontSize: 'var(--fs-sm)', display: 'block', marginTop: 'var(--space-xs)' }}>{success}</span>}

        <button type="submit" className="btn btn--primary" disabled={loading || otp.length !== 6}>
          {loading ? <span className="spinner spinner--sm" /> : 'Verify Email'}
        </button>
      </form>

      <p style={{ marginTop: 'var(--space-xl)', color: 'var(--text-secondary)', fontSize: 'var(--fs-sm)' }}>
        Didn&apos;t receive the code?{' '}
        <button
          onClick={handleResend}
          className="link-text link-text--highlight"
          disabled={resending}
          style={{ background: 'none', border: 'none', cursor: 'pointer' }}
        >
          {resending ? 'Sending...' : 'Resend OTP'}
        </button>
      </p>
    </div>
  );
};

export default ConfirmEmailPage;
