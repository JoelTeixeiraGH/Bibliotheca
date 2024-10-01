import { Text, View, StyleSheet } from 'react-native';
import globalStyles from '@/styles/globalStyles';

export default function SingleBookCategory({ id, name }: SingleBookCategory) {
  const globalStyle = globalStyles();
  return (
    <View style={styles.categoryBox}>
      <Text style={[globalStyle.fontSemibold, styles.categoryText]}>{name}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  categoryBox: {
    backgroundColor: '#35383f',
    alignSelf: 'flex-start',
    padding: 5,
    borderRadius: 8,
  },
  categoryText: {
    color: '#fff',
  },
});
