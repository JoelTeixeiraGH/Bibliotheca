import { SafeAreaView } from 'react-native-safe-area-context';
import { FlatList, ListRenderItem, Text, View, StyleSheet, RefreshControl } from 'react-native';
import { useState, useEffect } from 'react';
import {
  getAllNotificationsByLibraryIdForUser,
  getAllRequestNotificationsForUser,
  getAllNotificationsByUserId,
} from '@/services/apiService';
import { useAuth } from '@/context/AuthContext';
import globalStyles from '@/styles/globalStyles';
import { Stack } from 'expo-router';
import { COLORS, FONTS } from '@/constants/constants';
import MyActivityIndicator from '@/components/MyComponents/MyActivityIndicator';

export default function Page() {
  const { authState } = useAuth();
  const [loading, setLoading] = useState(false);
  const [notifications, setNotifications] = useState<NotificationType[]>([]);
  const globalStyle = globalStyles();

  const parseDate = (dateString: string): Date => {
    const [month, day, year] = dateString.split('/');
    return new Date(Number(year), Number(month) - 1, Number(day));
  };

  const fetching = async () => {
    setLoading(true);
    let tempNotifications: NotificationType[] = [];
    await getAllNotificationsByLibraryIdForUser(authState?.user?.libraryId!)
      .then((res) => {
        if (res.data.length > 0) tempNotifications = tempNotifications.concat(res.data);
      })
      .catch((err) => {
        console.log(err);
      });

    await getAllNotificationsByUserId(authState?.user?.userId!)
      .then((res) => {
        if (res.data.length > 0) tempNotifications = tempNotifications.concat(res.data);
      })
      .catch((err) => {
        console.log(err);
      });

    await getAllRequestNotificationsForUser(authState?.user?.userId!)
      .then((res) => {
        if (res.data.length > 0) tempNotifications = tempNotifications.concat(res.data);
      })
      .catch((err) => {
        console.log(err);
      });

    tempNotifications.sort((a: NotificationType, b: NotificationType) => {
      const dateA = parseDate(a.emittedDate);
      const dateB = parseDate(b.emittedDate);
      return dateB.getTime() - dateA.getTime(); // Use dateB - dateA for descending order, or dateA - dateB for ascending order
    });

    setNotifications(tempNotifications);
    setLoading(false);
  };

  useEffect(() => {
    fetching();
  }, []);

  const renderNotifications: ListRenderItem<NotificationType> = ({ item }) => {
    return (
      <View style={styles.root}>
        <Text style={[globalStyle.colorResponsive, globalStyle.fontBold]}>{item.notificationTitle}</Text>
        <Text style={[globalStyle.colorResponsive, globalStyle.fontRegular]}>{item.notificationDescription}</Text>
        <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold]}>{item.emittedDate}</Text>
      </View>
    );
  };

  return (
    <SafeAreaView style={{ flex: 1, marginHorizontal: 20 }}>
      <Stack.Screen
        options={{
          title: 'Notifications',
          headerTitleAlign: 'center',
          headerTitleStyle: { fontSize: 20, fontFamily: FONTS.BOLD },
        }}
      />
      {loading ? (
        <MyActivityIndicator />
      ) : (
        <FlatList
          data={notifications}
          renderItem={renderNotifications}
          keyExtractor={(item) => item.notificationId.toString()}
          refreshControl={<RefreshControl colors={[COLORS.PRIMARY_ORANGE]} refreshing={loading} onRefresh={fetching} />}
        />
      )}
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  root: {
    flexDirection: 'column',
    backgroundColor: '#35383f',
    padding: 10,
    borderRadius: 10,
    margin: 10,
  },
});
