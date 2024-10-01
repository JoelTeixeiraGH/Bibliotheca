import { Text } from 'react-native';
import globalStyles from '@/styles/globalStyles';
import { truncateText } from '@/utils/textManipulation';

interface CategoryNameProps {
  name: string;
}

export function CategoryName({ name }: CategoryNameProps) {
  const globalStyle = globalStyles();
  return (
    <Text style={[{ fontSize: 14, textAlign: 'center' }, globalStyle.colorResponsive, globalStyle.fontBold]}>
      {truncateText(name, 7)}
    </Text>
  );
}
