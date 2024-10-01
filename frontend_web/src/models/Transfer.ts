import { PhysicalBookStatus } from "./PhysicalBook";

export enum TransferStatus {
    Pending = 0,
    Accepted = 1,
    Rejected = 2,
    Canceled = 3
}

export interface Transfer {
    transferId: number;
    startDate: Date;
    endDate: Date;
    transferStatus: TransferStatus;
    sourceLibrary: {
        sourceLibraryId: number;
        sourceLibraryAlias: string;
    };
    destinationLibrary: {
        destinationLibraryId: number;
        destinationLibraryAlias: string;
    };
    physicalBook: {
        physicalBookId: number;
        physicalBookStatus: PhysicalBookStatus;
        physicalBookLibraryAlias: string;
    }
}

export interface TransferDTO {
    endDate?: string;
    sourceLibraryId?: number;
    destinationLibraryId?: number;
    physicalBookId?: number,
}

export const getTransferStatusLabel = (status: TransferStatus): string => {
    switch (status) {
        case TransferStatus.Pending:
            return 'Pending';
        case TransferStatus.Accepted:
            return 'Accepted';
        case TransferStatus.Rejected:
            return 'Rejected';
        case TransferStatus.Canceled:
            return 'Canceled';
        default:
            return 'N/A';
    }
};