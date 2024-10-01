import { useTheme } from '@react-navigation/native';
import { Bell } from 'lucide-react-native';

export default function NotificationBell() {
  const { colors } = useTheme();
  return <Bell size={24} color={colors.text} />;
}
