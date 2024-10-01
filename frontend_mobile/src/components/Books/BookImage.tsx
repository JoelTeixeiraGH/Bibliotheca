import { Image } from 'react-native';
import { StyleSheet } from 'react-native';

interface BookImageProps {
  thumbnail: string;
}
export function BookImage({ thumbnail }: BookImageProps) {
  return <Image style={styles.bookCoverImage} source={{ uri: thumbnail }} />;
}

const styles = StyleSheet.create({
  bookCoverImage: {
    width: 100,
    height: 150,
    borderRadius: 5,
  },
});
