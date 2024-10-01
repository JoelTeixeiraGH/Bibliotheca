import { Image } from 'react-native';
import { StyleSheet } from 'react-native';

interface ProfileImageProps {
  imageUrl: string;
}
export default function ProfilePhotoHome({ imageUrl }: ProfileImageProps) {
  return <Image style={styles.image} source={{ uri: imageUrl }} />;
}

const styles = StyleSheet.create({
  image: {
    width: 50,
    height: 50,
    borderRadius: 25,
    backgroundColor: '#eeeef3',
  },
});
