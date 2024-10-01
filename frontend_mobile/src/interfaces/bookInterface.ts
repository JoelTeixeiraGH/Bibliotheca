interface BookType {
  isbn: string;
  title: string;
  description: string;
  pageNumber: number;
  datePublished?: string;
  language: LanguageType;
  authors: AuthorType[];
  categories: CategoryType[];
  physicalBooks?: PhysicalBookType[];
  evaluations?: EvaluationBookType[];
  thumbnail: string;
  pageCount?: number;
  numberOfEvaluations?: number;
  averageEvaluationScore?: number;
}
