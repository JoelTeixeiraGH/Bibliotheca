export interface Evaluation {
    evaluationId: number;
    evaluationDescription: string;
    evaluationScore: number;
    genericBook: {
        isbn: string;
        title: string;
    }
    user: {
        userId: number;
        userName: string;
    }
}