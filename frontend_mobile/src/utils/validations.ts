import { Alert } from 'react-native';

/**
 * Validate email and show an alert if it's invalid
 * @param email
 */
export const isValidEmail = (email: string) => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(email)) {
    return false;
  }
  return true;
};

/**
 * Validate name
 * @param name
 * @returns true if valid, false otherwise
 */
export const isValidName = (name: string) => {
  if (!name.trim()) {
    return false;
  }
  return true;
};

/**
 * Validate library id
 * @param libraryId
 * @returns true if valid, false otherwise
 */
export const isValidLibrary = (libraryId: number) => {
  if (libraryId === 0) {
    return false;
  }
  return true;
};

/**
 * Validate password 8 digits minimum
 * @param password
 * @returns true if valid, false otherwise
 */
export const isValidPassword = (password: string) => {
  if (password.length < 7) {
    return false;
  }
  return true;
};

/**
 * Validate if passwords match
 * @param password
 * @param confirmPassword
 * @returns true if valid, false otherwise
 */
export const arePasswordsMatching = (password: string, confirmPassword: string) => {
  if (password !== confirmPassword) {
    return false;
  }
  return true;
};

export const isTextMoreThanCharacters = (text: string, characters: number) => {
  if (text.length > characters) {
    return false;
  }
  return true;
};

/**
 * Show an alert with the error message
 */
export const alertInvalidEmail = () => {
  Alert.alert('Invalid email', 'Please enter a valid email address');
};

export const alertInvalidPassword = () => {
  Alert.alert('Invalid password', 'Please enter a valid password (8 characters minimum)');
};

/**
 * Show an alert with the error message
 */
export const alertInvalidName = () => {
  Alert.alert('Invalid name', 'Please enter a valid name');
};

/**
 * Show an alert with the error message
 */
export const alertInvalidLibrary = () => {
  Alert.alert('Invalid library', 'Please select a library');
};

export const alertPasswordsDoNotMatch = () => {
  Alert.alert('Passwords do not match', 'Please make sure your passwords match');
};

export const alertInvalidEvaluationDescription = () => {
  Alert.alert('Invalid review', 'Review cannot be empty');
};

export const alertInvalidScore = () => {
  Alert.alert('Invalid score', 'Please select a score');
};

export const alertInvalidCharactersLength = (characters: number) => {
  Alert.alert('Invalid characters length', `Please enter at maximum ${characters} characters`);
};
