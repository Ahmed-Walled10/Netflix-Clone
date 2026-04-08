import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { subscriptionService } from '../../api/subscriptionService';
import { useAuthStore } from '../../stores/authStore';
import { FiCheck } from 'react-icons/fi';
import styles from './PlansPage.module.css';
import '../../styles/components.css';

const PlansPage = () => {
  const navigate = useNavigate();
  const { accessToken } = useAuthStore();
  const [plans, setPlans] = useState([]);
  const [loading, setLoading] = useState(true);
  const [subscribing, setSubscribing] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchPlans();
  }, []);

  const fetchPlans = async () => {
    try {
      const { data } = await subscriptionService.getPlans();
      setPlans(data.plans || []);
    } catch (err) {
      setError('Failed to load plans.');
    } finally {
      setLoading(false);
    }
  };

  const handleSubscribe = async (planId) => {
    if (!accessToken) {
      navigate('/login');
      return;
    }

    setSubscribing(planId);
    setError('');

    try {
      const { data } = await subscriptionService.subscribe(planId);
      // Redirect to Stripe Checkout
      window.location.href = data.checkoutUrl;
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to start checkout. Please try again.');
      setSubscribing(null);
    }
  };

  const getFeatures = (plan) => [
    `Up to ${plan.maxProfiles} profile${plan.maxProfiles > 1 ? 's' : ''}`,
    `${plan.videoQuality} video quality`,
    'Watch on any device',
    'Cancel anytime',
  ];

  if (loading) {
    return (
      <div className="loading-screen">
        <div className="spinner" />
      </div>
    );
  }

  return (
    <div className={styles['plans-page']}>
      <h1 className={styles['plans-page__title']}>Choose Your Plan</h1>
      <p className={styles['plans-page__subtitle']}>
        Watch unlimited movies and TV shows. Cancel anytime.
      </p>

      {error && <p className="form-error" style={{ marginBottom: 'var(--space-xl)' }}>{error}</p>}

      <div className={styles['plans-grid']}>
        {plans.map((plan, idx) => (
          <div
            key={plan.id}
            className={`${styles['plan-card']} ${idx === 1 ? styles['plan-card--featured'] : ''}`}
            style={{ animationDelay: `${idx * 0.1}s` }}
          >
            <h2 className={styles['plan-card__name']}>{plan.name}</h2>
            <div className={styles['plan-card__price']}>
              ${plan.price}
              <span>/mo</span>
            </div>
            <ul className={styles['plan-card__features']}>
              {getFeatures(plan).map((feature, fIdx) => (
                <li key={fIdx} className={styles['plan-card__feature']}>
                  <FiCheck className={styles['plan-card__feature-icon']} />
                  {feature}
                </li>
              ))}
            </ul>
            <button
              className={styles['plan-card__cta']}
              onClick={() => handleSubscribe(plan.id)}
              disabled={subscribing === plan.id}
            >
              {subscribing === plan.id ? 'Redirecting...' : 'Get Started'}
            </button>
          </div>
        ))}
      </div>
    </div>
  );
};

export default PlansPage;
