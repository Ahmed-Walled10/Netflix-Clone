import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { subscriptionService } from '../../api/subscriptionService';
import { authService } from '../../api/authService';
import { useAuthStore } from '../../stores/authStore';
import { formatDate } from '../../utils/constants';
import '../../styles/components.css';

const AccountPage = () => {
  const navigate = useNavigate();
  const { user, logout, refreshToken } = useAuthStore();
  const [subscription, setSubscription] = useState(null);
  const [loading, setLoading] = useState(true);
  const [revoking, setRevoking] = useState(false);

  useEffect(() => {
    loadSubscription();
  }, []);

  const loadSubscription = async () => {
    try {
      const { data } = await subscriptionService.getMySubscription();
      setSubscription(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = async () => {
    try {
      if (refreshToken) {
        await authService.logout(refreshToken);
      }
    } catch (err) {
      console.error('Logout API call failed', err);
    } finally {
      logout();
      navigate('/login');
    }
  };

  const handleRevokeAll = async () => {
    if (!window.confirm('This will sign you out of all devices. Continue?')) return;
    setRevoking(true);
    try {
      await authService.revokeAll();
      logout();
      navigate('/login');
    } catch (err) {
      console.error('Revoke all failed', err);
      setRevoking(false);
    }
  };

  const linkStyle = {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    background: 'var(--bg-card)',
    borderRadius: 'var(--radius-lg)',
    padding: 'var(--space-md) var(--space-xl)',
    color: 'var(--text-primary)',
    cursor: 'pointer',
    border: 'none',
    width: '100%',
    textAlign: 'left',
    fontSize: 'var(--fs-md)',
    transition: 'background var(--transition-fast)',
  };

  return (
    <div style={{ minHeight: '100vh', background: 'var(--bg-primary)', padding: 'var(--space-xl) 4%', maxWidth: 800, margin: '0 auto' }}>
      <h1 style={{ fontSize: 'var(--fs-3xl)', fontWeight: 700, marginBottom: 'var(--space-2xl)', borderBottom: '1px solid var(--border-color)', paddingBottom: 'var(--space-md)' }}>
        Account
      </h1>

      {/* User Info */}
      <section style={{ marginBottom: 'var(--space-2xl)' }}>
        <h2 style={{ fontSize: 'var(--fs-xs)', color: 'var(--text-muted)', marginBottom: 'var(--space-md)', textTransform: 'uppercase', letterSpacing: 1 }}>
          Membership & Billing
        </h2>
        <div style={{ background: 'var(--bg-card)', borderRadius: 'var(--radius-lg)', padding: 'var(--space-xl)' }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 'var(--space-md)' }}>
            <div>
              <div style={{ fontWeight: 600, marginBottom: 4 }}>{user?.email || 'N/A'}</div>
              <div style={{ fontSize: 'var(--fs-sm)', color: 'var(--text-muted)' }}>{user?.name || user?.fullName || 'User'}</div>
            </div>
            <div style={{ display: 'flex', flexWrap: 'wrap', gap: 'var(--space-xs)' }}>
              {user?.roles?.map((role) => (
                <span key={role} style={{ background: 'var(--netflix-red)', padding: '2px 10px', borderRadius: 'var(--radius-full)', fontSize: 'var(--fs-xs)', fontWeight: 600 }}>
                  {role}
                </span>
              ))}
            </div>
          </div>
        </div>
      </section>

      {/* Subscription */}
      <section style={{ marginBottom: 'var(--space-2xl)' }}>
        <h2 style={{ fontSize: 'var(--fs-xs)', color: 'var(--text-muted)', marginBottom: 'var(--space-md)', textTransform: 'uppercase', letterSpacing: 1 }}>
          Plan Details
        </h2>
        <div style={{ background: 'var(--bg-card)', borderRadius: 'var(--radius-lg)', padding: 'var(--space-xl)' }}>
          {loading ? (
            <div className="spinner spinner--sm" />
          ) : subscription ? (
            <div>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 'var(--space-md)' }}>
                <span style={{ fontSize: 'var(--fs-xl)', fontWeight: 700 }}>{subscription.planName}</span>
                <span style={{
                  padding: '4px 12px',
                  borderRadius: 'var(--radius-full)',
                  fontSize: 'var(--fs-xs)',
                  fontWeight: 600,
                  background: subscription.status === 'Active' ? 'rgba(70, 211, 105, 0.15)' : 'rgba(229, 9, 20, 0.15)',
                  color: subscription.status === 'Active' ? 'var(--success)' : 'var(--netflix-red)',
                  border: `1px solid ${subscription.status === 'Active' ? 'var(--success)' : 'var(--netflix-red)'}`,
                }}>
                  {subscription.status}
                </span>
              </div>
              <div style={{ fontSize: 'var(--fs-sm)', color: 'var(--text-secondary)' }}>
                <div style={{ marginBottom: 'var(--space-xs)' }}>
                  Current period: {formatDate(subscription.currentPeriodStart)} — {formatDate(subscription.currentPeriodEnd)}
                </div>
                {subscription.cancelAtPeriodEnd && (
                  <div style={{ color: 'var(--warning)', marginTop: 'var(--space-sm)' }}>
                    ⚠️ Cancellation scheduled — access until {formatDate(subscription.currentPeriodEnd)}
                  </div>
                )}
              </div>
            </div>
          ) : (
            <div>
              <p style={{ color: 'var(--text-muted)', marginBottom: 'var(--space-md)' }}>No active subscription</p>
              <button className="btn btn--primary" onClick={() => navigate('/plans')} style={{ width: 'auto' }}>
                Choose a Plan
              </button>
            </div>
          )}
        </div>
      </section>

      {/* Quick Links */}
      <section style={{ marginBottom: 'var(--space-2xl)' }}>
        <h2 style={{ fontSize: 'var(--fs-xs)', color: 'var(--text-muted)', marginBottom: 'var(--space-md)', textTransform: 'uppercase', letterSpacing: 1 }}>
          Profile & Settings
        </h2>
        <div style={{ display: 'flex', flexDirection: 'column', gap: 'var(--space-sm)' }}>
          <button onClick={() => navigate('/profiles')} style={linkStyle}>
            <span>Switch Profile</span>
            <span style={{ color: 'var(--text-muted)' }}>→</span>
          </button>
          <button onClick={() => navigate('/profiles/edit')} style={linkStyle}>
            <span>Edit Current Profile</span>
            <span style={{ color: 'var(--text-muted)' }}>→</span>
          </button>
          <button onClick={() => navigate('/my-list')} style={linkStyle}>
            <span>Watch History</span>
            <span style={{ color: 'var(--text-muted)' }}>→</span>
          </button>
          <button onClick={() => navigate('/my-ratings')} style={linkStyle}>
            <span>My Ratings</span>
            <span style={{ color: 'var(--text-muted)' }}>→</span>
          </button>
        </div>
      </section>

      {/* Security */}
      <section style={{ marginBottom: 'var(--space-2xl)' }}>
        <h2 style={{ fontSize: 'var(--fs-xs)', color: 'var(--text-muted)', marginBottom: 'var(--space-md)', textTransform: 'uppercase', letterSpacing: 1 }}>
          Security
        </h2>
        <div style={{ display: 'flex', flexDirection: 'column', gap: 'var(--space-sm)' }}>
          <button
            onClick={handleRevokeAll}
            disabled={revoking}
            style={{
              ...linkStyle,
              color: 'var(--warning)',
            }}
          >
            <span>{revoking ? 'Revoking...' : 'Sign Out of All Devices'}</span>
            <span style={{ color: 'var(--text-muted)' }}>⚠</span>
          </button>
        </div>
      </section>

      {/* Sign Out */}
      <button
        onClick={handleLogout}
        style={{ width: '100%', padding: 'var(--space-md)', background: 'var(--bg-card)', border: '1px solid var(--border-color)', borderRadius: 'var(--radius-lg)', color: 'var(--text-secondary)', fontSize: 'var(--fs-md)', cursor: 'pointer', transition: 'all var(--transition-fast)' }}
      >
        Sign Out
      </button>
    </div>
  );
};

export default AccountPage;
