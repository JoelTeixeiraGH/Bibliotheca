export interface User {
    userId: number;
    userName: string;
    userEmail: string;
    library: {
        libraryId: number;
        libraryAlias: string;
    };
    numberOfRequests: number;
    numberOfPunishments: number;
}

export interface UpdatePasswordDTO {
    oldPassword: string;
    newPassword: string;
}