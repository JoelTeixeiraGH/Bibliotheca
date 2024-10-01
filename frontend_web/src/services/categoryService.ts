import axiosInstance from './apiConfig';
import { Category } from '@/models/Category';

const categoryService = {
    // Get all Categories
    getAllCategories: async () => {
        try {
            return await axiosInstance.get('/category/all');
        } catch (error) {
            throw new Error(`Error getting categories: ${error}`);
        }
    },

    // Create Category
    createCategory: async (categoryData: Category) => {
        try {
            return await axiosInstance.post('/category/add', categoryData);
        } catch (error) {
            throw new Error(`Error creating category: ${error}`);
        }
    },

    // Update Category
    updateCategory: async (categoryData: Category) => {
        try {
            return await axiosInstance.put(`/category/edit/${categoryData.categoryId}`, categoryData);
        } catch (error) {
            throw new Error(`Error editing category: ${error}`);
        }
    },

    deleteCategory: async (categoryId: number) => {
        try {
            return await axiosInstance.delete(`/category/delete/${categoryId}`);
        } catch (error) {
            throw new Error(`Error deleting category: ${error}`);
        }
    }

};

export default categoryService;