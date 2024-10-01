import { StyleSheet } from 'react-native';
import { useTheme } from '@react-navigation/native';
import { COLORS } from '@/constants/constants';

export default function globalStyles() {
  const { colors } = useTheme();

  return StyleSheet.create({
    fontBold: {
      fontFamily: 'urb-b',
    },
    fontSemibold: {
      fontFamily: 'urb-sb',
    },
    fontRegular: {
      fontFamily: 'urb',
    },
    colorResponsive: {
      color: colors.text,
    },
    centeredView: {
      flex: 1,
      justifyContent: 'center',
      alignItems: 'center',
    },
    title: {
      fontSize: 24,
      fontWeight: 'bold',
      marginBottom: 32,
    },
    modalView: {
      backgroundColor: '#121212',
      borderRadius: 20,
      marginHorizontal: 20,
      paddingHorizontal: 30,
      paddingVertical: 25,
      borderColor: 'white',
      borderWidth: 1,
      shadowColor: '#000',
      shadowOffset: {
        width: 0,
        height: 2,
      },
      shadowOpacity: 0.25,
      shadowRadius: 4,
      elevation: 5,
    },
    buttonPrimary: {
      alignItems: 'center',
      justifyContent: 'center',
      paddingVertical: 12,
      paddingHorizontal: 32,
      borderRadius: 18,
      elevation: 3,
      backgroundColor: COLORS.PRIMARY_ORANGE,
      marginTop: 4,
    },
    buttonPrimaryIcons: {
      paddingVertical: 10,
      paddingHorizontal: 10,
    },
    input: {
      height: 40,
      borderColor: 'gray',
      borderWidth: 1,
      marginBottom: 16,
      paddingHorizontal: 20,
      borderRadius: 20,
    },
    passwordContainer: {
      flexDirection: 'row',
      alignItems: 'center',
      height: 40,
      borderColor: 'gray',
      borderWidth: 1,
      marginBottom: 16,
      paddingHorizontal: 20,
      borderRadius: 20,
    },
    passwordInput: {
      flex: 1,
    },
    toggleButton: {
      padding: 10,
      backgroundColor: '#e0e0e0',
      borderRadius: 5,
    },
    picker: {
      height: 40,
      width: '100%',
      marginBottom: 16,
    },
    containerAuth: {
      flex: 1,
      marginHorizontal: 20,
      justifyContent: 'center',
    },
    containerAuthEdit: {
      flex: 1,
      marginHorizontal: 20,
    },
  });
}
