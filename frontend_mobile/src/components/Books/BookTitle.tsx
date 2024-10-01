import { Text } from 'react-native';
import globalStyles from '@/styles/globalStyles';
import { truncateText } from '@/utils/textManipulation';
interface BookTitleProps {
  title: string;
}

export function BookTitle({ title }: BookTitleProps) {
  const globalStyle = globalStyles();
  return (
    <Text style={[{ fontSize: 14, textAlign: 'left' }, globalStyle.colorResponsive, globalStyle.fontBold]}>
      {truncateText(title, 11)}
    </Text>
  );
}
