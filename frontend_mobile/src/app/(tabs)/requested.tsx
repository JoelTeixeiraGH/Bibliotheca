import { useState, useEffect } from 'react';
import { View, ListRenderItem, RefreshControl } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { FlatList } from 'react-native-gesture-handler';

import { Request } from '@/components/Request';
import TabComponent from '@/components/Tab/Tab';

import { getUserRequests } from '@/services/apiService';
import { useAuth } from '@/context/AuthContext';
import { COLORS, TEXTS } from '@/constants/constants';

export default function Requested() {
  const { authState } = useAuth();
  const [activeTab, setActiveTab] = useState<number>(1);
  const [openRequests, setOpenRequests] = useState<RequestUserType[]>([]);
  const [closeRequests, setCloseRequests] = useState<RequestUserType[]>([]);
  const [loading, setLoading] = useState(false);
  const tabNames = [TEXTS.REQUESTS_OPEN_TITLE, TEXTS.REQUESTS_CLOSED_TITLE];

  /**
   * Handles the press of a tab.
   * @param tabNumber
   */
  const handleTabPress = (tabNumber: number) => {
    setActiveTab(tabNumber);
  };

  /**
   * Fetches the requests of the user.
   */
  const fetching = async () => {
    setLoading(true);
    await getUserRequests(authState?.user?.userId!)
      .then((response) => {
        if (response.data.length > 0) {
          setOpenRequests(
            response.data.filter(
              (request: RequestUserType) => request.requestStatus !== 3 && request.requestStatus !== 5
            )
          );
          setCloseRequests(
            response.data.filter(
              (request: RequestUserType) => request.requestStatus === 3 || request.requestStatus === 5
            )
          );
        }
      })
      .catch((error) => console.error('Error fetching requests:', error));
    setLoading(false);
  };

  /**
   * Fetches the requests of the user when the component mounts.
   */
  useEffect(() => {
    fetching();
  }, []);

  /**
   * Renders the requests.
   * @param item The request.
   * @returns The request component.
   */
  const renderRequests: ListRenderItem<RequestUserType> = ({ item }) => {
    return (
      <Request.Root>
        <Request.Info
          requestId={item.requestId}
          requestStatus={item.requestStatus}
          startDate={item.startDate}
          endDate={item.endDate}
          isbn={item.isbn}
        />
        <View style={{ flexDirection: 'row', alignSelf: 'flex-end' }}>
          {item.requestStatus === 1 && (
            <View style={{ marginRight: 5 }}>
              <Request.Extend />
            </View>
          )}
          {item.requestStatus === 4 && (
            <View style={{ marginRight: 5 }}>
              <Request.Cancel requestId={item.requestId} fun={fetching} />
            </View>
          )}
          <Request.ShowBookBtn isbn={item.isbn} />
        </View>
      </Request.Root>
    );
  };

  return (
    <SafeAreaView style={{ flex: 1 }}>
      <TabComponent activeTab={activeTab} onTabPress={handleTabPress} tabNames={tabNames} />
      {activeTab === 1 && (
        <FlatList
          data={openRequests}
          renderItem={renderRequests}
          keyExtractor={(item) => item.requestId.toString()}
          refreshControl={<RefreshControl colors={[COLORS.PRIMARY_ORANGE]} refreshing={loading} onRefresh={fetching} />}
        />
      )}
      {activeTab === 2 && (
        <FlatList data={closeRequests} renderItem={renderRequests} keyExtractor={(item) => item.requestId.toString()} />
      )}
    </SafeAreaView>
  );
}
