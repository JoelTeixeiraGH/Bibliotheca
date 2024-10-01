import axiosInstance from './apiConfig';
import { PhysicalBook, PhysicalBookDto } from '../models/PhysicalBook.ts';

const physicalBookService = {
    // Get all PhysicalBooks
    getAllPhysicalBooks: async () => {
        try {
            return await axiosInstance.get('/physical-book/all');
        } catch (error) {
            throw new Error(`Error getting physical books: ${error}`);
        }
    },

    // Get all PhysicalBooks by ISBN
    getAllPhysicalByISBN: async (isbn: string) => {
        try {
            return await axiosInstance.get(`/physical-book/all/generic-book/${isbn}`);
        } catch (error) {
            throw new Error(`Error getting physical books: ${error}`);
        }
    },

    // Get all PhysicalBooks by ISBN and library id
    getAllPhysicalBooksByISBNByLibraryId: async (isbn: string, libraryId: number) => {
        try {
            return await axiosInstance.get(`/physical-book/all/generic-book/${isbn}/${libraryId}`);
        } catch (error) {
            throw new Error(`Error getting physical books: ${error}`);
        }
    },

    // Get all PhysicalBooks by ISBN currently at library id and status AtLibrary
    getAllPhysicalBooksByISBNCurrentlyAtLibrary: async (isbn: string, libraryId: number) => {
        try {
            return await axiosInstance.get(`/physical-book/all/generic-book/${isbn}/library/${libraryId}`);
        } catch (error) {
            throw new Error(`Error getting physical books: ${error}`);
        }
    },

    // Get all PhysicalBooks at library id and status transfer
    getAllPhysicalBooksWithTransferStatusForLibrary: async (libraryId: number) => {
        try {
            return await axiosInstance.get(`/physical-book/all/transfer/library/${libraryId}`);
        } catch (error) {
            throw new Error(`Error getting physical books: ${error}`);
        }
    },

    // Get physical book by id
    getPhysicalBook: async (physicalBookId: number) => {
        try {
            return await axiosInstance.get(`/physical-book/${physicalBookId}`);
        } catch (error) {
            throw new Error(`Error getting physical book: ${error}`);
        }
    },

    // create physical book
    createPhysicalBook: async (physicalBook: PhysicalBook) => {
        try {
            return await axiosInstance.post('/physical-book/add', physicalBook);
        } catch (error) {
            throw new Error(`Error creating physical book: ${error}`);
        }
    },

    // create physical books by number of books
    createPhysicalBooksByNum: async (physicalBookDto: PhysicalBookDto) => {
        try {
            return await axiosInstance.post(`/physical-book/add/${physicalBookDto.numOfBooks}`, physicalBookDto);
        } catch (error) {
            throw new Error(`Error creating physical books: ${error}`);
        }
    },

    // Update physical book status to arrived
    updatePhysicalBookStatus: async (physicalBookId: number, libraryId: number) => {
        try {
            return await axiosInstance.put(`/physical-book/edit/status/${physicalBookId}/${libraryId}`);
        } catch (error) {
            throw new Error(`Error updating physical book: ${error}`);
        }
    },

    // Delete physical book
    deletePhysicalBook: async (physicalBookId: number) => {
        try {
            return await axiosInstance.delete(`/physical-book/delete/${physicalBookId}`);
        } catch (error) {
            throw new Error(`Error deleting physical book: ${error}`);
        }
    },
}

export default physicalBookService