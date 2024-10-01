import { useState, useEffect } from 'react';
import { View } from 'react-native';
import { Stack } from 'expo-router';
import { FlatList } from 'react-native-gesture-handler';

import MyActivityIndicator from '@/components/MyComponents/MyActivityIndicator';

import { getCategories } from '@/services/apiService';

import { FONTS } from '@/constants/constants';
import renderCategories from '@/components/Renders/RenderCategoriesWithLink';

export default function Page() {
  const [loading, setLoading] = useState(true);
  const [categories, setCategories] = useState<CategoryType[]>([]);

  useEffect(() => {
    const fetching = async () => {
      setLoading(true);
      await getCategories()
        .then((response) => {
          setCategories(response.data);
        })
        .catch((error) => console.error('Error fetching categories:', error));
      setLoading(false);
    };
    fetching();
  }, []);

  return (
    <View style={{ flex: 1, margin: 10, alignItems: 'center', justifyContent: 'center' }}>
      <Stack.Screen
        options={{
          title: 'All Categories',
          headerTitleAlign: 'center',
          headerTitleStyle: { fontSize: 20, fontFamily: FONTS.BOLD },
        }}
      />
      {loading ? (
        <MyActivityIndicator />
      ) : (
        <FlatList
          data={categories}
          renderItem={renderCategories}
          keyExtractor={(item) => item.categoryId.toString()}
          numColumns={4}
          ItemSeparatorComponent={() => <View style={{ marginBottom: 10 }} />}
        />
      )}
    </View>
  );
}
