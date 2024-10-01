import { Text } from 'react-native';
import globalStyles from '@/styles/globalStyles';

interface HelloUserProps {
  userName: string;
}

export default function HelloUser({ userName }: HelloUserProps) {
  const globalStyle = globalStyles();
  return (
    <Text style={[{ fontSize: 20 }, globalStyle.colorResponsive, globalStyle.fontRegular]}>Hello {userName}!</Text>
  );
}
