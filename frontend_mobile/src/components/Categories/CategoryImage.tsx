import { View, Text, StyleSheet } from 'react-native';

interface CategoryImageProps {
  unicode: number;
}

export function CategoryImage({ unicode }: CategoryImageProps) {
  const emoji = String.fromCodePoint(unicode);
  return (
    <View style={styles.emojiContainer}>
      <Text style={styles.emojiText}>{emoji}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  emojiText: {
    fontSize: 24,
    shadowColor: 'black',
    shadowOpacity: 0.1,
    shadowRadius: 20,
  },
  emojiContainer: {
    backgroundColor: '#35383f',
    padding: 20,
    borderRadius: 50,
    justifyContent: 'center',
    alignItems: 'center',
  },
});
