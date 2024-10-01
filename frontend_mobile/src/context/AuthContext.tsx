import 'core-js/stable/atob';

import { createContext, useContext, useEffect, useState } from 'react';
import { Alert } from 'react-native';
import { jwtDecode } from 'jwt-decode';

import * as SecureStore from 'expo-secure-store';
import {
  login,
  register,
  removeAuthToken,
  setAuthToken,
  refreshToken,
  setCookie,
  removeCookie,
} from '@/services/apiService';

interface AuthProps {
  authState?: { token: string | null; authenticated: boolean | null; user: UserType | null };
  onRegister?: (userName: string, userEmail: string, userPassword: string, libraryId: number) => Promise<any>;
  onLogin?: (userEmail: string, userPassword: string) => Promise<any>;
  onLogout?: () => Promise<any>;
  updateUserInfo?: (userEdit: UserEditType) => Promise<any>;
}

const TOKEN_KEY = 'token';
const REFRESH_TOKEN_KEY = 'refreshToken';

const AuthContext = createContext<AuthProps>({});

export const useAuth = () => {
  return useContext(AuthContext);
};

/**
 * Decoded token type
 * @param decoded Decoded token UserInfoDecodedType
 * @returns User Type Object
 */
const convertDecodedTokenToUserInfo = (decoded: UserInfoDecodedType): UserType => {
  const userInfo: UserType = {
    userId: parseInt(decoded.Id),
    userEmail: decoded.Email,
    userName: decoded.Name,
    libraryId: parseInt(decoded.LibraryId),
  };
  return userInfo;
};

export const AuthProvider = ({ children }: any) => {
  const [authState, setAuthState] = useState<{
    token: string | null;
    authenticated: boolean | null;
    user: UserType | null;
  }>({
    token: null,
    authenticated: null,
    user: null,
  });

  const loadToken = async () => {
    const token = await SecureStore.getItemAsync(TOKEN_KEY);
    if (token) {
      const decoded: UserInfoDecodedType = jwtDecode(token);
      setAuthState({ token, authenticated: true, user: convertDecodedTokenToUserInfo(decoded) });
      setAuthToken(token);
    }
  };

  const loadRefreshToken = async () => {
    const refreshToken = await SecureStore.getItemAsync(REFRESH_TOKEN_KEY);
    if (refreshToken) {
      setCookie(refreshToken);
    }
  };

  const callLoadTokensInOrder = async () => {
    await loadToken();
    await loadRefreshToken();
    await refreshTokenFun();
  };

  useEffect(() => {
    callLoadTokensInOrder();
  }, []);

  const handleRegister = async (userName: string, userEmail: string, userPassword: string, libraryId: number) => {
    try {
      return await register(userName, userEmail, userPassword, libraryId);
    } catch (error: any) {
      Alert.alert('Error', 'Error creating account');
      console.error(error);
      return error;
    }
  };

  const handleLogin = async (userEmail: string, userPassword: string) => {
    try {
      const result = await login(userEmail, userPassword);

      let refreshToken = result.headers['set-cookie']?.[0].toString().split(';')[0]!;
      refreshToken = adjustToken(refreshToken);
      setCookie(refreshToken);

      const decoded: UserInfoDecodedType = jwtDecode(result.data);

      setAuthState({ token: result.data, authenticated: true, user: convertDecodedTokenToUserInfo(decoded) });
      setAuthToken(result.data);

      await SecureStore.setItemAsync(TOKEN_KEY, result.data);
      await SecureStore.setItemAsync(REFRESH_TOKEN_KEY, refreshToken);
    } catch (error) {
      Alert.alert('Error', 'Invalid credentials');
      return error;
    }
  };

  const logout = async () => {
    await SecureStore.deleteItemAsync(TOKEN_KEY);
    await SecureStore.deleteItemAsync(REFRESH_TOKEN_KEY);
    removeAuthToken();
    removeCookie();
    setAuthState({ token: null, authenticated: false, user: null });
  };

  const updateUserInfo = async (userEdit: UserEditType) => {
    const newUserInfo: UserType = {
      userId: authState?.user?.userId!,
      userName: userEdit.userName,
      userEmail: userEdit.userEmail,
      libraryId: userEdit.libraryId,
    };
    setAuthState({ token: authState?.token!, authenticated: true, user: newUserInfo });
  };

  const adjustToken = (token: string) => {
    let newToken = token.slice(0, -6);
    newToken = newToken + '==';
    return newToken;
  };

  const refreshTokenFun = async () => {
    refreshToken()
      .then(async (response) => {
        const token = response.data;
        const decoded: UserInfoDecodedType = jwtDecode(token);

        setAuthToken(response.data);

        let refreshToken = response.headers['set-cookie']?.[0].toString().split(';')[0]!;
        refreshToken = adjustToken(refreshToken);
        setCookie(refreshToken);

        setAuthState({ token, authenticated: true, user: convertDecodedTokenToUserInfo(decoded) });

        await SecureStore.setItemAsync(TOKEN_KEY, response.data);
        await SecureStore.setItemAsync(REFRESH_TOKEN_KEY, refreshToken);
      })
      .catch((error) => {
        console.log(error);
      });
  };

  // schedule to update token every 1h59m seconds
  useEffect(() => {
    const interval = setInterval(() => {
      refreshTokenFun();
    }, 7150000);
    return () => clearInterval(interval);
  }, []);

  const value = {
    onRegister: handleRegister,
    onLogin: handleLogin,
    onLogout: logout,
    updateUserInfo: updateUserInfo,
    authState,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
