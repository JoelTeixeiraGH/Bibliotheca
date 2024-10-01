import { createContext, useContext, useState, useEffect } from 'react';
import authService from '../services/AuthService';
import { jwtDecode } from "jwt-decode";
import { useNavigate } from 'react-router-dom';
import { AuthUser } from '@/models/Auth';

const AuthContext = createContext<any>(undefined);

export default AuthContext;

interface Props {
    children: React.ReactNode;
}


const generateContextData = (user: any, token: any, handleLogin: any, logoutUser: any) => ({
    user,
    token,
    handleLogin,
    logoutUser,
});

export const AuthProvider: React.FC<Props> = ({ children }) => {

    const [token, setToken] = useState(() => localStorage.getItem('token') ? localStorage.getItem('token') : null);
    const [user, setUser] = useState<AuthUser | null>(() => {
        if (token) {
            return jwtDecode<AuthUser>(token);
        }
        return null;
    });
    const [loading, setLoading] = useState(true);

    const navigate = useNavigate();

    const handleLogin = async (email: string, password: string) => {
        try {

            const loginData = {
                userEmail: email,
                userPassword: password,
            };

            const response = await authService.login(loginData);

            if (response.status === 200) {
                const accessToken = response.data;
                const decodedToken = jwtDecode<AuthUser>(accessToken);

                // Check if the user has the required role
                const { 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': role } = decodedToken;

                // Check if the user has the required role
                const allowedRoles = ['Admin', 'Librarian']
                // Check if the user has the required role (replace 'requiredRole' with the actual role you are checking for)
                if (decodedToken && allowedRoles.includes(role)) {
                    setToken(accessToken);
                    setUser(decodedToken);
                    localStorage.setItem('token', accessToken);
                    navigate('/');
                } else {
                    alert("User does not have the required role.");
                }
            }

        } catch (error) {
            console.error("Error during login:", error);
            alert("Wrong credentials.")
        }

    };

    const logoutUser = async () => {
        try {

            const response = await authService.logout();

            if (response.status === 200) {
                setUser(null)
                setToken(null)
                localStorage.removeItem('token')
                localStorage.removeItem('library')
                navigate('/login')
            }

        } catch {

            setUser(null)
            setToken(null)
            localStorage.removeItem('token')
            localStorage.removeItem('library')
            navigate('/login')

        }
    }

    const updateToken = async () => {

        if (token) {
            // Check if the token is still valid (not expired)
            const decodedToken: any = jwtDecode(token);
            const currentTimestamp = Date.now() / 1000;
            if (decodedToken.exp > currentTimestamp) {
                // Token is still valid, no need to refresh
                setLoading(false);
                return;
            }

            // Token doesn't exist or has expired, try refreshing
            try {
                const response = await authService.refreshToken();

                if (response.status === 200) {
                    setToken(response.data);
                    setUser(jwtDecode<AuthUser>(response.data));
                    localStorage.setItem('token', response.data);
                } else {
                    setUser(null)
                    setToken(null)
                    localStorage.removeItem('token')
                    navigate('/login')
                }

            } catch (error) {
                console.error("Error during token refresh:", error);
            }
        }

        setLoading(false);

    };

    useEffect(() => {

        if (loading) {
            updateToken();
        }
        const time = 1000 * 60 * 59

        const interval = setInterval(updateToken, time)

        return () => {
            clearInterval(interval);
        }
    }, [loading])

    const contextData = generateContextData(user, token, handleLogin, logoutUser);

    return (
        <AuthContext.Provider value={contextData}>
            {loading ? null : children}
        </AuthContext.Provider>
    )
}

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};