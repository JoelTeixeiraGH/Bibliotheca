import axiosInstance from './apiConfig';

const evaluationService = {
    // Get all evaluations
    getAllEvaluations: async () => {
        try {
            return await axiosInstance.get('/evaluation/all');
        } catch (error) {
            throw new Error(`Error getting evaluations: ${error}`);
        }
    },

    // Delete evaluation
    deleteEvaluation: async (requestId: number) => {
        try {
            return await axiosInstance.delete(`/evaluation/delete/${requestId}`);
        } catch (error) {
            throw new Error(`Error deleting evaluation: ${error}`);
        }
    },
}

export default evaluationService;