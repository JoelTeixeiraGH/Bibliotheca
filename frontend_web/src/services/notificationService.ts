import axiosInstance from './apiConfig';
import { NotificationDTO } from '@/models/Notification';

const notificationService = {
    // Get all Notifications
    getAllNotificationsByLibraryIdForLibrary: async (libraryId: number) => {
        try {
            return await axiosInstance.get(`/notification/all/library/${libraryId}/library`);
        } catch (error) {
            throw new Error(`Error getting notifications: ${error}`);
        }
    },

    // Create Notification
    createNotification: async (notification: NotificationDTO) => {
        try {
            return await axiosInstance.post('/notification/add', notification);
        } catch (error) {
            throw new Error(`Error creating notification: ${error}`);
        }
    },

}

export default notificationService;