import axios from 'axios';

const API_BASE_URL = 'https://grouplibrarymanagmentapi.azurewebsites.net/api';

export const apiService = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: true,
});

/**
 * update the token in the header
 * @param token the token to be set
 */
export const setAuthToken = (token: string) => {
  apiService.defaults.headers.common.Authorization = `Bearer ${token}`;
};

export const removeAuthToken = () => {
  apiService.defaults.headers.common.Authorization = '';
};

export const setCookie = (cookie: string) => {
  apiService.defaults.headers.common.Cookie = cookie;
};

export const removeCookie = () => {
  apiService.defaults.headers.common.Cookie = '';
};

export const getCookie = () => {
  return apiService.defaults.headers.common.Cookie;
};

// Define your API methods
/**
 * Get all authors
 * @returns
 */
export const getCategories = () => apiService.get('/category/all');
/**
 * Get a category by id
 * @param id
 * @returns
 */
export const getCategory = (id: number) => apiService.get(`/category/id/${id}`);
/**
 * Get all generic books
 * @returns
 */
export const getGenericBooks = () => apiService.get('/generic-book/all');

/**
 * Get the most popular generic books
 * @returns
 */
export const getMostPopularGenericBooks = () => apiService.get('/generic-book/all/most-popular');
/**
 * Get a book by isbn
 * @param isbn
 * @returns
 */
export const getGenericBook = (isbn: string) => apiService.get(`/generic-book/${isbn}`);
/**
 * Get all books of a category
 * @param categoryId
 * @returns
 */
export const getCategoryGenericBooks = (categoryId: number) =>
  apiService.get(`/generic-book/all/category/${categoryId}`);
/**
 * Get all books of an author
 * @param authorId
 * @returns
 */
export const getAuthorGenericBooks = (authorId: number) => apiService.get(`/generic-book/all/author/${authorId}`);
/**
 * Get author by id
 * @param id
 * @returns
 */
export const getAuthor = (id: number) => apiService.get(`/author/id/${id}`);

/**
 * Get request by user id
 * @param userId
 * @returns
 */
export const getUserRequests = (userId: number) => apiService.get(`/request/all/user/${userId}`);

/**
 * Get request by id
 * @param requestId
 * @returns
 */
export const getRequest = (requestId: number) => apiService.get(`/request/${requestId}`);

/**
 * Make a pending request
 */
export const postPendingRequest = (request: RequestType) => apiService.post('/request/add/pending', request);

/**
 * Make a waiting request
 */
export const postWaitingRequest = (request: RequestType) => apiService.post('/request/add/waiting', request);

/**
 * Make a canceled request
 */
export const changeToCanceled = (requestId: number) => apiService.put(`/request/edit/change-to-canceled/${requestId}`);

/**
 * Search through the generic books
 * @param searchTerm Book title, author name, category name
 * @returns Generic books that match the search term
 */
export const search = (searchTerm: string) => apiService.get(`/generic-book/search/${searchTerm}`);

/**
 * Get all libraries
 * @returns All libraries
 */
export const getAllLibraries = () => apiService.get('/library/all');

/**
 * Get user info by id
 * @param userId User id
 * @returns User info
 */
export const getUserInfo = (userId: number) => apiService.get(`/user/${userId}`);

/**
 * Edit user info
 * @param userId
 * @param userInfo
 * @returns
 */
export const editUserInfo = (userId: number, userInfo: UserEditType) =>
  apiService.put(`/user/edit/${userId}`, userInfo);

/**
 * Change user password
 * @param userId
 * @param newPasswordObj
 * @returns
 */
export const changePassword = (userId: number, newPasswordObj: NewPasswordType) =>
  apiService.put(`/user/edit/${userId}/password`, newPasswordObj);

/**
 * Register a new user
 * @param userName
 * @param userEmail
 * @param userPassword
 * @param libraryId
 */
export const register = (userName: string, userEmail: string, userPassword: string, libraryId: number) =>
  apiService.post('/user/register', { userName, userEmail, userPassword, libraryId });

/**
 * Login
 * @param userEmail
 * @param userPassword
 */
export const login = (userEmail: string, userPassword: string) =>
  apiService.post('/user/login', { userEmail, userPassword });

/**
 * Get all punishments
 * @param userId User id
 * @returns All punishments of a user
 */
export const getUserPunishments = (userId: number) => apiService.get(`/punishment/all/user/${userId}`);

/**
 * Get all evaluations from user
 * @param userId User id
 * @returns All evaluations of a user
 */
export const getUserEvaluations = (userId: number) => apiService.get(`/evaluation/all/user/${userId}`);

/**
 * Post an evaluation
 * @param evaluation Evaluation object
 */
export const postEvaluation = (evaluation: EvaluationSubmitType) => apiService.post('/evaluation/add', evaluation);

/**
 * Edit an evaluation
 * @param evaluationId Evaluation id
 */
export const editEvaluation = (evaluationId: number, evaluation: EvaluationSubmitType) =>
  apiService.put(`/evaluation/edit/${evaluationId}`, evaluation);

/**
 * Delete an evaluation
 * @param evaluationId Evaluation id
 */
export const deleteEvaluation = (evaluationId: number) => apiService.delete(`/evaluation/delete/${evaluationId}`);

/**
 * Get evaluation by id
 * @param evaluationId Evaluation id
 * @returns Evaluation object
 */
export const getEvaluationById = (evaluationId: number) => apiService.get(`/evaluation/id/${evaluationId}`);

/**
 * Refresh auth token
 */
export const refreshToken = () => apiService.post('/user/refresh-token');

/**
 * Get notification by id
 * @param notificationId Notification id
 * @returns Notification object
 */
export const getNotificationById = (notificationId: number) => apiService.get(`/notification/${notificationId}`);

/**
 * Get all notifications by user id
 * @param userId User id
 * @returns All notifications of a user
 */
export const getAllNotificationsByUserId = (userId: number) => apiService.get(`/notification/all/user/${userId}`);

/**
 * Get all notifications by library id for user
 * @param libraryId Library id
 * @returns All notifications of a library for a user
 */
export const getAllNotificationsByLibraryIdForUser = (libraryId: number) =>
  apiService.get(`/notification/all/library/${libraryId}/user`);

/**
 * Get all notifications about requests by user id
 * @param userId User id
 * @returns All notifications about requests of a user
 */
export const getAllRequestNotificationsForUser = (userId: number) =>
  apiService.get(`/notification/all/request/${userId}`);
