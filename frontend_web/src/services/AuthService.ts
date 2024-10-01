import axiosInstance from './apiConfig';
import { LoginDTO } from '@/models/Auth';

const authService = {
    // Login user
    login: async (loginData: LoginDTO) => {
        try {
            return await axiosInstance.post('/user/login', loginData);
        } catch (error) {
            throw new Error(`Error signing in: ${error}`);
        }
    },

    // Logout user
    logout: async () => {
        try {
            return await axiosInstance.post(`/user/logout`, {});
        } catch (error) {
            throw new Error(`Error loging out: ${error}`);
        }
    },

    // RefreshToken
    refreshToken: async () => {
        try {
            return await axiosInstance.post(`/user/refresh-token`, {});
        } catch (error) {
            throw new Error(`Error refreshing token: ${error}`);
        }
    },
};

export default authService;