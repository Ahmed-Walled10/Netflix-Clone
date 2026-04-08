import { Outlet, Link } from 'react-router-dom';
import styles from './AuthLayout.module.css';

const AuthLayout = () => {
  return (
    <div className={styles['auth-layout']}>
      <header className={styles['auth-layout__header']}>
        <Link to="/" className={styles['auth-layout__logo']}>
          NETFLIX
        </Link>
      </header>

      <div className={styles['auth-layout__content']}>
        <Outlet />
      </div>

      <footer className={styles['auth-layout__footer']}>
        <p>Netflix Clone — Portfolio Project &copy; {new Date().getFullYear()}</p>
      </footer>
    </div>
  );
};

export default AuthLayout;
