interface UserType {
  userEmail: string;
  userId: number;
  libraryId: number;
  userName: string;
}

interface UserInfoFullType {
  userId: number;
  userName: string;
  userEmail: string;
  library: LibraryType;
  notifications: [];
  evaluations: [];
  numberOfRequests: number;
  requests: [];
  numberOfPunishments: number;
}

interface UserInfoDecodedType {
  Email: string;
  Id: string;
  LibraryId: string;
  Name: string;
  exp: number;
  // http://schemas.microsoft.com/ws/2008/06/identity/claims/role: string;
}

interface UserEditType {
  userName: string;
  userEmail: string;
  libraryId: number;
}
