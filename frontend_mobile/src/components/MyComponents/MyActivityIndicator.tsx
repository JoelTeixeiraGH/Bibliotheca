import { useTheme } from '@react-navigation/native';
import { ActivityIndicator } from 'react-native';

export default function MyActivityIndicator() {
  const { colors } = useTheme();

  return <ActivityIndicator size="large" color={colors.primary} />;
}
