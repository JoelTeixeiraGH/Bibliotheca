import axiosInstance from './apiConfig';
import { RequestDTO } from '../models/Request'

const requestService = {
    // Get all Requests by Library Id
    getAllRequestsByLibraryId: async (libraryId: number) => {
        try {
            return await axiosInstance.get(`/request/all/library/${libraryId}`);
        } catch (error) {
            throw new Error(`Error getting requests: ${error}`);
        }
    },

    // Get all Requests by User Id
    getAllRequestsByUserId: async (userId: number) => {
        try {
            return await axiosInstance.get(`/request/all/user/${userId}`);
        } catch (error) {
            throw new Error(`Error getting requests: ${error}`);
        }
    },

    // Add new Request with status requested
    createRequestWithStatusRequested: async (requestData: RequestDTO) => {
        try {
            return await axiosInstance.post('/request/add/requested', requestData);
        } catch (error) {
            throw new Error(`Error creating request: ${error}`);
        }
    },

    // Add new Request with status waiting
    createRequestWithStatusWaiting: async (requestData: RequestDTO) => {
        try {
            return await axiosInstance.post('/request/add/waiting', requestData);
        } catch (error) {
            throw new Error(`Error creating request: ${error}`);
        }
    },

    // Change Request status to canceled
    changeRequestStatusToCanceled: async (requestId: number) => {
        try {
            return await axiosInstance.put(`/request/edit/change-to-canceled/${requestId}`);
        } catch (error) {
            throw new Error(`Error editing request: ${error}`);
        }
    },

    // Change Request status to requested
    changeRequestStatusToRequested: async (requestId: number, selectedDate: any) => {
        try {
            return await axiosInstance.put(`/request/edit/change-to-requested/${requestId}`, selectedDate);
        } catch (error) {
            throw new Error(`Error editing request: ${error}`);
        }
    },

    // Change Request status to returned
    changeRequestStatusToReturned: async (requestId: number) => {
        try {
            return await axiosInstance.put(`/request/edit/change-to-returned/${requestId}`);
        } catch (error) {
            throw new Error(`Error editing request: ${error}`);
        }
    },

    // Extend Request time
    extendTime: async (requestId: number, selectedDate: any) => {
        try {
            return await axiosInstance.put(`/request/edit/extend-time/${requestId}`, selectedDate);
        } catch (error) {
            throw new Error(`Error editing request: ${error}`);
        }
    },

    // Delete Request
    deleteRequest: async (requestId: number) => {
        try {
            return await axiosInstance.delete(`/request/delete/${requestId}`);
        } catch (error) {
            throw new Error(`Error deleting request: ${error}`);
        }
    },
}

export default requestService