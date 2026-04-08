import { Navigate, useLocation } from 'react-router-dom';
import { useAuthStore } from '../../stores/authStore';

export const ProtectedRoute = ({ children }) => {
  const { accessToken, user } = useAuthStore();
  const location = useLocation();

  if (!accessToken) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return children;
};

export const ProfileRoute = ({ children }) => {
  const { accessToken, user, hasProfile } = useAuthStore();
  const location = useLocation();

  if (!accessToken) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  if (!hasProfile()) {
    return <Navigate to="/profiles" replace />;
  }

  return children;
};

export const PublicOnlyRoute = ({ children }) => {
  const { accessToken, hasProfile } = useAuthStore();

  if (accessToken && hasProfile()) {
    return <Navigate to="/browse" replace />;
  }

  if (accessToken) {
    return <Navigate to="/profiles" replace />;
  }

  return children;
};
