export interface GenericBook {
    isbn: string;
    title: string;
    description: string;
    datePublished: Date;
    pageNumber: number;
    language: {
        languageId: number;
        languageAlias: string;
    };
    authors: {
        authorId: number;
        authorName: string;
    }[];
    categories: {
        categoryId: number;
        categoryName: string;
    }[];
    thumbnail: string;
    smallThumbnail: string;
    physicalBooks: {
        libraryId: number;
        libraryAlias: string;
        count: number;
    }[];
    evaluations: {
        evaluationId: number;
        evaluationDescription: string;
        evaluationScore: number;
        emittedDate: Date;
        userId: number;
        userName: string;
    }[];
    numberOfEvaluations: number;
    averageEvaluationScore: number;
}
