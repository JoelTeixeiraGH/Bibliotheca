import { useState, useEffect } from 'react';
import { View } from 'react-native';
import { Stack, useLocalSearchParams } from 'expo-router';
import { FlatList } from 'react-native-gesture-handler';

import renderBooksWithLink from '@/components/Renders/RenderBooksWithLink';
import MyActivityIndicator from '@/components/MyComponents/MyActivityIndicator';

import { getCategory, getCategoryGenericBooks } from '@/services/apiService';

import { FONTS } from '@/constants/constants';

export default function Page() {
  const { id } = useLocalSearchParams<{ id: string }>();
  const [book, setBooks] = useState<BookType[]>([]);
  const [categoryName, setCategoryName] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetching = async () => {
      setLoading(true);

      await getCategoryGenericBooks(parseInt(id))
        .then((response) => {
          setBooks(response.data);
        })
        .catch((error) => console.error('Error fetching books:', error));

      await getCategory(parseInt(id))
        .then((response) => {
          setCategoryName(response.data.categoryName);
        })
        .catch((error) => console.error('Error fetching category:', error));

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
              title: categoryName,
              headerTitleAlign: 'center',
              headerTitleStyle: { fontSize: 20, fontFamily: FONTS.BOLD },
            }}
          />
          <FlatList
            data={book}
            renderItem={renderBooksWithLink}
            keyExtractor={(item) => item.isbn}
            numColumns={3}
            ItemSeparatorComponent={() => <View style={{ marginBottom: 10 }} />}
          />
        </>
      )}
    </View>
  );
}
