export interface LoginDTO {
    userEmail: string;
    userPassword: string;
}

export interface AuthUser {
    Id: number;
    Email: string;
    Name: string;
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string;
    LibraryId: number;
}