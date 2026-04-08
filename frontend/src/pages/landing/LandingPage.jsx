import { useNavigate } from 'react-router-dom';
import '../../styles/components.css';

const LandingPage = () => {
  const navigate = useNavigate();

  return (
    <div style={{ minHeight: '100vh', background: '#000', position: 'relative', overflow: 'hidden' }}>
      {/* Background gradient */}
      <div style={{ position: 'absolute', inset: 0, background: 'radial-gradient(ellipse at top, rgba(229, 9, 20, 0.15) 0%, transparent 70%)' }} />

      {/* Header */}
      <header style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: 'var(--space-lg) var(--content-padding)', position: 'relative', zIndex: 2 }}>
        <span style={{ fontFamily: 'var(--font-display)', fontSize: 'var(--fs-4xl)', color: 'var(--netflix-red)', letterSpacing: 2, cursor: 'pointer' }}>
          NETFLIX
        </span>
        <button className="btn btn--primary" onClick={() => navigate('/login')} style={{ width: 'auto', marginTop: 0, padding: '8px 20px', fontSize: 'var(--fs-sm)' }}>
          Sign In
        </button>
      </header>

      {/* Hero Section */}
      <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', textAlign: 'center', padding: '10vh 4%', position: 'relative', zIndex: 2, maxWidth: 900, margin: '0 auto' }}>
        <h1 style={{ fontSize: 'clamp(2rem, 5vw, var(--fs-hero))', fontWeight: 900, lineHeight: 1.1, marginBottom: 'var(--space-lg)', animation: 'fadeInUp 0.8s ease-out' }}>
          Unlimited movies, TV shows, and more
        </h1>
        <p style={{ fontSize: 'var(--fs-xl)', color: 'var(--text-secondary)', marginBottom: 'var(--space-lg)', animation: 'fadeInUp 0.8s ease-out 0.1s both' }}>
          Watch anywhere. Cancel anytime.
        </p>
        <p style={{ fontSize: 'var(--fs-md)', color: 'var(--text-muted)', marginBottom: 'var(--space-2xl)', animation: 'fadeInUp 0.8s ease-out 0.2s both' }}>
          Ready to watch? Sign up to create your account.
        </p>
        <button className="btn btn--primary" onClick={() => navigate('/register')} style={{ width: 'auto', marginTop: 0, padding: '16px 40px', fontSize: 'var(--fs-xl)', fontWeight: 700, animation: 'fadeInUp 0.8s ease-out 0.3s both' }}>
          Get Started →
        </button>
      </div>

      {/* Feature Cards */}
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(280px, 1fr))', gap: 'var(--space-xl)', padding: '5vh 4%', position: 'relative', zIndex: 2, maxWidth: 1200, margin: '0 auto' }}>
        {[
          { icon: '📺', title: 'Watch Everywhere', desc: 'Stream on your phone, tablet, laptop, and TV.' },
          { icon: '⬇️', title: 'Save Your Favorites', desc: 'Track what you watch and get personalized recommendations.' },
          { icon: '👶', title: 'Kids Profiles', desc: 'Safe viewing experience for children with age-based filtering.' },
          { icon: '🎬', title: 'Premium Quality', desc: 'Stream in HD, Full HD, or 4K depending on your plan.' },
        ].map((feature, i) => (
          <div key={i} style={{
            background: 'linear-gradient(135deg, rgba(30,30,30,0.8), rgba(20,20,20,0.9))',
            border: '1px solid var(--border-color)',
            borderRadius: 'var(--radius-xl)',
            padding: 'var(--space-2xl)',
            transition: 'all var(--transition-base)',
            animation: `fadeInUp 0.6s ease-out ${0.4 + i * 0.1}s both`,
            cursor: 'default',
          }}
            onMouseEnter={(e) => { e.currentTarget.style.borderColor = 'var(--netflix-red)'; e.currentTarget.style.transform = 'translateY(-4px)'; }}
            onMouseLeave={(e) => { e.currentTarget.style.borderColor = 'var(--border-color)'; e.currentTarget.style.transform = 'translateY(0)'; }}
          >
            <div style={{ fontSize: '2.5rem', marginBottom: 'var(--space-md)' }}>{feature.icon}</div>
            <h3 style={{ fontSize: 'var(--fs-xl)', fontWeight: 700, marginBottom: 'var(--space-sm)' }}>{feature.title}</h3>
            <p style={{ color: 'var(--text-secondary)', fontSize: 'var(--fs-sm)', lineHeight: 1.6 }}>{feature.desc}</p>
          </div>
        ))}
      </div>

      {/* Footer */}
      <footer style={{ padding: 'var(--space-3xl) var(--content-padding)', textAlign: 'center', color: 'var(--text-muted)', fontSize: 'var(--fs-sm)', position: 'relative', zIndex: 2, borderTop: '1px solid var(--border-color)', marginTop: 'var(--space-3xl)' }}>
        <p>Netflix Clone — Portfolio Project &copy; {new Date().getFullYear()}</p>
      </footer>
    </div>
  );
};

export default LandingPage;
