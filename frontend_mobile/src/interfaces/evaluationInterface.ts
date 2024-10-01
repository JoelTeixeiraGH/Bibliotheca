interface EvaluationType {
  evaluationId: number;
  evaluationDescription: string;
  evaluationScore: number;
  emittedDate: string;
  isbn: string;
  userId: number;
}

interface EvaluationBookType {
  evaluationId: number;
  evaluationDescription: string;
  evaluationScore: number;
  emittedDate: string;
  userId: number;
  userName: string;
}

interface EvaluationSubmitType {
  evaluationDescription: string;
  evaluationScore: number;
  isbn: string;
  userId: number;
}
