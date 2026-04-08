import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const SubscriptionSuccessPage = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const timer = setTimeout(() => {
      navigate('/profiles');
    }, 3000);
    return () => clearTimeout(timer);
  }, [navigate]);

  return (
    <div style={{
      minHeight: '100vh',
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'center',
      background: 'var(--bg-primary)',
      padding: 'var(--space-xl)',
      textAlign: 'center',
    }}>
      <div style={{
        width: 80,
        height: 80,
        borderRadius: '50%',
        background: 'var(--success)',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        marginBottom: 'var(--space-xl)',
        animation: 'scaleIn 0.5s ease-out',
      }}>
        <span style={{ fontSize: '2.5rem', color: 'white' }}>✓</span>
      </div>
      <h1 style={{ fontSize: 'var(--fs-3xl)', marginBottom: 'var(--space-md)' }}>
        Welcome to Netflix!
      </h1>
      <p style={{ color: 'var(--text-secondary)', fontSize: 'var(--fs-lg)', maxWidth: 400 }}>
        Your subscription is active. Setting up your profile...
      </p>
      <div className="spinner" style={{ marginTop: 'var(--space-xl)' }} />
    </div>
  );
};

export default SubscriptionSuccessPage;
