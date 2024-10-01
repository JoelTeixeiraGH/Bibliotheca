export enum RequestStatus {
    Pending = 0,
    Requested = 1,
    NotReturned = 2,
    Returned = 3,
    Waiting = 4,
    Canceled = 5
}

export interface Request {
    requestId: number;
    requestStatus: RequestStatus;
    startDate: Date;
    endDate: Date;
    user: {
        userId: number;
        userEmail: string;
    };
    isbn: string;
    physicalBook: {
        physicalBookId: number;
        available: boolean;
    };
    library: {
        librayId: number;
        libraryAlias: string;
    }
}

export interface RequestDTO {
    endDate?: string;
    userId?: number;
    physicalBookId?: number;
    isbn?: string,
    libaryId?: number,
}

export const getRequestStatusLabel = (status: RequestStatus): string => {
    switch (status) {
        case RequestStatus.Pending:
            return 'Pending';
        case RequestStatus.Requested:
            return 'Requested';
        case RequestStatus.NotReturned:
            return 'Not Returned';
        case RequestStatus.Returned:
            return 'Returned';
        case RequestStatus.Waiting:
            return 'Waiting';
        case RequestStatus.Canceled:
            return 'Canceled';
        default:
            return 'N/A';
    }
};