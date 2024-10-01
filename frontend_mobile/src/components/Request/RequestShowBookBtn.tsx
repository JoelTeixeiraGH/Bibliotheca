import { getGenericBook } from '@/services/apiService';
import { useState } from 'react';
import { Pressable, StyleSheet, Modal, View } from 'react-native';
import { BlurView } from 'expo-blur';
import MyActivityIndicator from '../MyComponents/MyActivityIndicator';
import { Book } from '../Books';
import globalStyles from '@/styles/globalStyles';
import { X, BookMarked } from 'lucide-react-native';

interface RequestShowBookBtnProps {
  isbn: string;
}

export function RequestShowBookBtn({ isbn }: RequestShowBookBtnProps) {
  const globalStyle = globalStyles();
  const [book, setBook] = useState<BookType>();
  const [loading, setLoading] = useState(false);
  const [modalVisible, setModalVisible] = useState(false);

  const getBookRequested = async () => {
    setLoading(true);
    await getGenericBook(isbn)
      .then((response) => {
        setBook(response.data);
      })
      .catch((error) => console.error('Error fetching book:', error))
      .finally(() => {
        setLoading(false);
        setModalVisible(true);
      });
  };

  return (
    <>
      <Pressable style={[globalStyle.buttonPrimary, globalStyle.buttonPrimaryIcons]} onPress={() => getBookRequested()}>
        <BookMarked size={18} color="white" />
      </Pressable>

      <Modal
        animationType="slide"
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => {
          setModalVisible(!modalVisible);
        }}
      >
        <BlurView intensity={20} style={StyleSheet.absoluteFill}>
          <View style={globalStyle.centeredView}>
            <View style={globalStyle.modalView}>
              {loading ? (
                <MyActivityIndicator />
              ) : (
                <>
                  <Pressable
                    style={[{ alignSelf: 'flex-end', marginBottom: 5 }]}
                    onPress={() => setModalVisible(!modalVisible)}
                  >
                    <X size={18} color="white" />
                  </Pressable>
                  <View style={{ alignItems: 'center', justifyContent: 'center' }}>
                    <Book.Root>
                      <Book.Image thumbnail={book?.thumbnail!} />
                    </Book.Root>
                  </View>
                </>
              )}
            </View>
          </View>
        </BlurView>
      </Modal>
    </>
  );
}
