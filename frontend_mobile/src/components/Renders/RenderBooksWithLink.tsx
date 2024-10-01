import { View, Pressable, ListRenderItem } from 'react-native';
import React from 'react';
import { Link } from 'expo-router';
import { Book } from '@/components/Books';

const renderBooksWithLink: ListRenderItem<BookType> = ({ item }) => {
  return (
    <Link href={`/book/${item.isbn}`} asChild>
      <Pressable>
        <View style={{ marginRight: 10 }}>
          <Book.Root>
            <Book.Image thumbnail={item.thumbnail} />
            <Book.Title title={item.title} />
          </Book.Root>
        </View>
      </Pressable>
    </Link>
  );
};

export default renderBooksWithLink;
