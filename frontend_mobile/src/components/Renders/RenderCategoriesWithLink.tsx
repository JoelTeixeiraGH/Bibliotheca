import { Pressable, ListRenderItem, View } from 'react-native';
import React from 'react';
import { Link } from 'expo-router';
import { Category } from '@/components/Categories';

const renderCategories: ListRenderItem<CategoryType> = ({ item }) => {
  return (
    <View style={{ marginHorizontal: 10 }}>
      <Link href={`/category/${item.categoryId}`} asChild>
        <Pressable>
          <Category.Root>
            <Category.Image unicode={0x1f4d6} />
            <Category.Name name={item.categoryName} />
          </Category.Root>
        </Pressable>
      </Link>
    </View>
  );
};

export default renderCategories;
