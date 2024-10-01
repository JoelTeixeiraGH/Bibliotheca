import { View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import SearchBar from '@/components/Search/SearchBar';
import { FlatList } from 'react-native-gesture-handler';
import { search } from '@/services/apiService';
import { useState } from 'react';
import renderBooksWithLink from '@/components/Renders/RenderBooksWithLink';
import MyActivityIndicator from '@/components/MyComponents/MyActivityIndicator';

export default function Search() {
  const [books, setBooks] = useState<BookType[]>([]);
  const [loading, setIsLoading] = useState(false);

  const handleSearch = async (text: string) => {
    setIsLoading(true);
    await search(text)
      .then((response) => {
        setBooks(response.data);
      })
      .catch((error) => {
        console.error(error);
      })
      .finally(() => {
        setIsLoading(false);
      });
  };

  return (
    <SafeAreaView style={{ flex: 1, margin: 10 }}>
      <SearchBar onSearch={handleSearch} />
      <View style={{ alignItems: 'center', justifyContent: 'center' }}>
        {loading ? (
          <MyActivityIndicator />
        ) : (
          <FlatList
            style={{ marginTop: 10 }}
            data={books}
            renderItem={renderBooksWithLink}
            keyExtractor={(item) => item.isbn}
            numColumns={3}
          />
        )}
      </View>
    </SafeAreaView>
  );
}
