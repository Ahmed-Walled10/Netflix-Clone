import { jwtDecode } from 'jwt-decode';

export const decodeToken = (token) => {
  try {
    return jwtDecode(token);
  } catch {
    return null;
  }
};

export const isTokenExpired = (token) => {
  const decoded = decodeToken(token);
  if (!decoded || !decoded.exp) return true;
  return decoded.exp * 1000 < Date.now();
};

export const getUserFromToken = (token) => {
  const decoded = decodeToken(token);
  if (!decoded) return null;

  return {
    userId: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || decoded.sub,
    email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || decoded.email,
    name: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || decoded.unique_name,
    roles: extractRoles(decoded),
    profileId: decoded.profileId || null,
    isKidsMode: decoded.isKidsMode === 'True' || decoded.isKidsMode === 'true',
  };
};

const extractRoles = (decoded) => {
  const roleClaim = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || decoded.role;
  if (!roleClaim) return [];
  return Array.isArray(roleClaim) ? roleClaim : [roleClaim];
};

export const hasRole = (token, role) => {
  const user = getUserFromToken(token);
  return user?.roles?.includes(role) || false;
};

export const hasProfileToken = (token) => {
  const user = getUserFromToken(token);
  return !!user?.profileId;
};
