import axiosInstance from './apiConfig';
import { LanguageDTO } from '@/models/Language';

const languageService = {
    // Get all Categories
    getAllLanguages: async () => {
        try {
            return await axiosInstance.get('/language/all');
        } catch (error) {
            throw new Error(`Error getting categories: ${error}`);
        }
    },

    // Create Category
    createLanguage: async (languageData: LanguageDTO) => {
        try {
            return await axiosInstance.post('/language/add', languageData);
        } catch (error) {
            throw new Error(`Error creating category: ${error}`);
        }
    },

    // Update Category
    updateLanguage: async (languageId: number, languageData: LanguageDTO) => {
        try {
            return await axiosInstance.put(`/language/edit/${languageId}`, languageData);
        } catch (error) {
            throw new Error(`Error editing category: ${error}`);
        }
    },

    // Update Category
    deleteLanguage: async (languageId: number) => {
        try {
            return await axiosInstance.delete(`/language/delete/${languageId}`);
        } catch (error) {
            throw new Error(`Error editing category: ${error}`);
        }
    },

};

export default languageService;