import axiosInstance from './apiConfig';
import { Library } from '@/models/Library';

const libraryService = {
    // Get all Libraries
    getAllLibraries: async () => {
        try {
            return await axiosInstance.get('/library/all');
        } catch (error) {
            throw new Error(`Error getting libraries: ${error}`);
        }
    },

    // Get library by id
    getLibraryById: async (libraryId: number) => {
        try {
            return await axiosInstance.get(`/library/id/${libraryId}`);
        } catch (error) {
            throw new Error(`Error getting library: ${error}`);
        }
    },

    addLibrary: async (libraryData: Library) => {
        try {
            return await axiosInstance.post('/library/add', libraryData);
        } catch (error) {
            throw new Error(`Erro ao adicionar biblioteca: ${error}`);
        }
    },
}

export default libraryService;