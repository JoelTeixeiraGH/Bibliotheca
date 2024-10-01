import axiosInstance from './apiConfig';
import { Author } from '@/models/Author';

const authorService = {
  // Get all Authors
  getAllAuthors: async () => {
    try {
      return await axiosInstance.get('/author/all');
    } catch (error) {
      throw new Error(`Error getting authors: ${error}`);
    }
  },

  // Get Author by Id
  getAuthorById: async (authorId: number) => {
    try {
      return await axiosInstance.get(`/author/id/${authorId}`);
    } catch (error) {
      throw new Error(`Error getting author: ${error}`);
    }
  },

  // Create Author
  createAuthor: async (authorData: Author) => {
    try {
      return await axiosInstance.post('/author/add', authorData);
    } catch (error) {
      throw new Error(`Error creating author: ${error}`);
    }
  },

  // Edit Author
  editAuthor: async (authorData: Author) => {
    try {
      return await axiosInstance.put(`/author/edit/${authorData.authorId}`, authorData);
    } catch (error) {
      throw new Error(`Error editing author: ${error}`);
    }
  },

  // Delete Author
  deleteAuthor: async (authorId: number) => {
    try {
      return await axiosInstance.delete(`/author/delete/${authorId}`);
    } catch (error) {
      throw new Error(`Error deleting Author: ${error}`)
    }
  }
};

export default authorService;