export enum PhysicalBookStatus {
    AtLibrary = 0,
    Requested = 1,
    Transfer = 2,
}

export interface PhysicalBook {
    physicalBookId: number;
    physicalBookStatus: PhysicalBookStatus;
    genericBook: {
        isbn: string;
        title: string;
    };
    library: {
        libraryId: number;
        libraryAlias: string;
    };
}

export interface PhysicalBookDto {
    isbn: string;
    numOfBooks: number;
    libraryId: number;
}

export const getPhysicalBookStatusLabel = (status: PhysicalBookStatus): string => {
    switch (status) {
        case PhysicalBookStatus.AtLibrary:
            return 'At Library';
        case PhysicalBookStatus.Requested:
            return 'Requested';
        case PhysicalBookStatus.Transfer:
            return 'Transfer';
        default:
            return 'Unknown';
    }
};