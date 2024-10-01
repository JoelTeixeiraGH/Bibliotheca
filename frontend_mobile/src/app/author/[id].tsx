import { useState, useEffect } from 'react';
import { View } from 'react-native';
import React from 'react';
import { Stack, useLocalSearchParams } from 'expo-router';
import { getAuthor, getAuthorGenericBooks } from '@/services/apiService';
import { FlatList } from 'react-native-gesture-handler';
import { FONTS } from '@/constants/constants';
import renderBooksWithLink from '@/components/Renders/RenderBooksWithLink';
import MyActivityIndicator from '@/components/MyComponents/MyActivityIndicator';

export default function Page() {
  const { id } = useLocalSearchParams<{ id: string }>();
  const [book, setBooks] = useState<BookType[]>([]);
  const [authorName, setAuthorName] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetching = async () => {
      setLoading(true);

      await getAuthorGenericBooks(parseInt(id))
        .then((response) => {
          setBooks(response.data);
        })
        .catch((error) => console.error('Error fetching books:', error));

      await getAuthor(parseInt(id))
        .then((response) => {
          setAuthorName(response.data.authorName);
        })
        .catch((error) => console.error('Error fetching author:', error));

      setLoading(false);
    };
    fetching();
  }, []);

  // TODO: make numColumns responsive for tablets using useWindowDimensions

  return (
    <View style={{ flex: 1, margin: 10, alignItems: 'center', justifyContent: 'center' }}>
      {loading ? (
        <>
          <Stack.Screen
            options={{
              title: '',
              headerTitleAlign: 'center',
              headerTitleStyle: { fontSize: 20, fontFamily: FONTS.BOLD },
            }}
          />
          <MyActivityIndicator />
        </>
      ) : (
        <>
          <Stack.Screen
            options={{
              title: authorName,
              headerTitleAlign: 'center',
              headerTitleStyle: { fontSize: 20, fontFamily: FONTS.BOLD },
            }}
          />
          <FlatList data={book} renderItem={renderBooksWithLink} keyExtractor={(item) => item.isbn} numColumns={3} />
        </>
      )}
    </View>
  );
}
