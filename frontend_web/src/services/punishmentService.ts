import axiosInstance from './apiConfig';

const punishmentService = {
    // Get all punishments
    getAllPunishments: async () => {
        try {
            return await axiosInstance.get('/punishment/all');
        } catch (error) {
            throw new Error(`Error getting punishments: ${error}`);
        }
    },

    // Get all punishments by user id
    getAllPunishmentsByUserId: async (userId: number) => {
        try {
            return await axiosInstance.get(`/punishment/all/user/${userId}`);
        } catch (error) {
            throw new Error(`Error getting punishments: ${error}`);
        }
    },

    // Delete punishment
    deletePunishment: async (punishmentId: number) => {
        try {
            return await axiosInstance.delete(`/punishment/delete/${punishmentId}`);
        } catch (error) {
            throw new Error(`Error deleting punishment: ${error}`);
        }
    },

}

export default punishmentService