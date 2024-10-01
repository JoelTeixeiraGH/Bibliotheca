import { useEffect, useState } from 'react';
import { Text, View, Pressable, FlatList, StyleSheet } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Link } from 'expo-router';
import { ArrowRight } from 'lucide-react-native';

import FakeSearchBar from '@/components/Search/FakeSearchBar';
import ProfilePhotoHome from '@/components/User/ProfilePhotoHome';
import HelloUser from '@/components/User/HelloUser';
import NotificationBell from '@/components/Notifications/NotificationBell';
import renderBooksWithLink from '@/components/Renders/RenderBooksWithLink';

import globalStyles from '@/styles/globalStyles';

import { getCategories, getMostPopularGenericBooks } from '@/services/apiService';
import MyActivityIndicator from '@/components/MyComponents/MyActivityIndicator';
import { TEXTS } from '@/constants/constants';
import { useAuth } from '@/context/AuthContext';
import renderCategories from '@/components/Renders/RenderCategoriesWithLink';

export default function Home() {
  const { authState } = useAuth();
  const globalStyle = globalStyles();
  const [loading, setLoading] = useState(true);
  const [categories, setCategories] = useState<CategoryType[]>([]);
  const [books, setBooks] = useState<BookType[]>([]);

  useEffect(() => {
    const fetching = async () => {
      setLoading(true);

      await getCategories()
        .then((response) => setCategories(response.data))
        .catch((error) => console.error('Error fetching categories:', error));

      await getMostPopularGenericBooks()
        .then((response) => {
          setBooks(response.data);
        })
        .catch((error) => console.error('Error fetching books:', error));

      setLoading(false);
    };
    fetching();
  }, []);

  return (
    <SafeAreaView style={styles.container}>
      {loading ? (
        <MyActivityIndicator />
      ) : (
        <>
          <View style={{ flex: 1 }}>
            <View style={styles.userContainer}>
              <View style={styles.profilePhoto}>
                <Link href={'/profile'} asChild>
                  <Pressable>
                    <ProfilePhotoHome imageUrl="https://imgur.com/9i5Lp0w.png" />
                  </Pressable>
                </Link>
              </View>

              <Link href="/notifications/notification" asChild>
                <Pressable>
                  <View style={styles.notificationBell}>
                    <NotificationBell />
                  </View>
                </Pressable>
              </Link>
            </View>
            <View style={styles.helloMessage}>
              <HelloUser userName={authState?.user?.userName!} />
            </View>
          </View>

          <View style={styles.searchView}>
            <Link href={'/search'} asChild>
              <Pressable>
                <FakeSearchBar />
              </Pressable>
            </Link>
          </View>
          <View style={{ flex: 3 / 2 }}>
            <View
              style={{ flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', marginRight: 20 }}
            >
              <Text style={[styles.titles, globalStyle.fontBold, globalStyle.colorResponsive]}>
                {TEXTS.CATEGORY_TITLE}
              </Text>
              <Link href={'/category/allCategories'}>
                <ArrowRight size={20} color="rgb(248, 147, 0)" />
              </Link>
            </View>
            <FlatList
              horizontal
              showsHorizontalScrollIndicator={false}
              contentContainerStyle={styles.listContainer}
              data={categories}
              renderItem={renderCategories}
              keyExtractor={(item) => item.categoryId.toString()}
            />
          </View>
          <View style={{ flex: 2 }}>
            <Text style={[styles.titles, globalStyle.fontBold, globalStyle.colorResponsive]}>
              {TEXTS.MOST_POPULAR_BOOKS_TITLE}
            </Text>
            <FlatList
              horizontal
              showsHorizontalScrollIndicator={false}
              contentContainerStyle={styles.listContainerBooks}
              renderItem={renderBooksWithLink}
              data={books}
              keyExtractor={(item) => item.isbn}
            />
          </View>
        </>
      )}
    </SafeAreaView>
  );
}
const styles = StyleSheet.create({
  container: {
    flex: 1,
    flexDirection: 'column',
  },
  userContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
  },
  profilePhoto: {
    paddingLeft: 20,
    paddingTop: 20,
  },
  notificationBell: {
    paddingTop: 20,
    paddingRight: 20,
  },
  helloMessage: {
    paddingLeft: 20,
    paddingTop: 10,
  },
  searchView: {
    paddingVertical: 20,
    paddingHorizontal: 20,
    flex: 1 / 2,
  },
  titles: {
    fontSize: 20,
    paddingLeft: 20,
  },
  titles2: {
    fontSize: 20,
    paddingLeft: 20,
    fontFamily: 'urb-b',
    color: 'white',
  },
  listContainer: {
    paddingHorizontal: 10,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    //gap: 20,
  },
  listContainerBooks: {
    paddingHorizontal: 20,
    flexDirection: 'row',
    alignItems: 'center',
  },
});
