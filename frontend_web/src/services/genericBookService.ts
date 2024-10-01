import axiosInstance from './apiConfig';
import { GenericBook } from '../models/GenericBook'

const genericBookService = {
    // Get all GenericBooks
    getAllGenericBooks: async () => {
        try {
            return await axiosInstance.get('/generic-book/all');
        } catch (error) {
            throw new Error(`Error getting generic books: ${error}`);
        }
    },

    // Get all GenericBooks
    getAllGenericBooksByAuthor: async (authorId: number) => {
        try {
            return await axiosInstance.get(`/generic-book/all/author/${authorId}`);
        } catch (error) {
            throw new Error(`Error getting generic books: ${error}`);
        }
    },

    // Get all GenericBooks
    getAllGenericBooksByCategory: async (categoryId: number) => {
        try {
            return await axiosInstance.get(`/generic-book/all/category/${categoryId}`);
        } catch (error) {
            throw new Error(`Error getting generic books: ${error}`);
        }
    },

    // Get GenericBook by ISBN
    getGenericBookByISBN: async (isbn: string) => {
        try {
            return await axiosInstance.get(`/generic-book/${isbn}`);
        } catch (error) {
            throw new Error(`Error getting generic book: ${error}`)
        }
    },

    // create GenericBook
    createGenericBook: async (bookData: GenericBook) => {
        try {
            return await axiosInstance.post('/generic-book/add', bookData);
        } catch (error) {
            throw new Error(`Error creating generic book: ${error}`);
        }
    },

    // Update GenericBook
    updateGenericBook: async (bookData: GenericBook) => {
        try {
            return await axiosInstance.put('/generic-book/edit', bookData);
        } catch (error) {
            throw new Error(`Error editing generic book: ${error}`);
        }
    },

    // Delete GenericBook
    deleteGenericBook: async (isbn: string) => {
        try {
            return await axiosInstance.delete(`/generic-book/delete/${isbn}`);
        } catch (error) {
            throw new Error(`Error deleting generic book: ${error}`);
        }
    },
}

export default genericBookService