import axiosInstance from './apiConfig';
import { TransferDTO } from '../models/Transfer';

const transferService = {
    // Get all Transfers
    getAllTransfersByLibraryId: async (libraryId: number) => {
        try {
            return await axiosInstance.get(`/transfer/all/library/${libraryId}`);
        } catch (error) {
            throw new Error(`Error getting transfers: ${error}`);
        }
    },

    // Add new Transfer
    createTransfer: async (transferData: TransferDTO) => {
        try {
            return await axiosInstance.post('/transfer/add', transferData);
        } catch (error) {
            throw new Error(`Error creating transfer: ${error} `);
        }
    },

    // Change Transfer status to canceled
    changeTransferStatusToCanceled: async (transferId: number) => {
        try {
            return await axiosInstance.put(`/transfer/edit/change-to-canceled/${transferId}`);
        } catch (error) {
            throw new Error(`Error editing transfer: ${error} `);
        }
    },

    // Change Transfer status to accepted
    changeTransferStatusToAccepted: async (transferId: number) => {
        try {
            return await axiosInstance.put(`/transfer/edit/change-to-accepted/${transferId}`);
        } catch (error) {
            throw new Error(`Error editing transfer: ${error} `);
        }
    },

    // Change Transfer status to rejected
    changeTransferStatusToRejected: async (transferId: number) => {
        try {
            return await axiosInstance.put(`/transfer/edit/change-to-rejected/${transferId}`);
        } catch (error) {
            throw new Error(`Error editing transfer: ${error} `);
        }
    },

    // Delete Transfer
    deleteTransfer: async (transferId: number) => {
        try {
            return await axiosInstance.put(`/transfer/delete/${transferId}`);
        } catch (error) {
            throw new Error(`Error deleting transfer: ${error} `);
        }
    },
}

export default transferService