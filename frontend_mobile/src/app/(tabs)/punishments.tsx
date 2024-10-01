import { useState, useEffect } from 'react';
import { useAuth } from '@/context/AuthContext';
import { FlatList, ListRenderItem, RefreshControl } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { getUserPunishments } from '@/services/apiService';
import { Punishment } from '@/components/Punishment';
import { COLORS } from '@/constants/constants';

export default function Punishments() {
  const { authState } = useAuth();
  const [loading, setLoading] = useState(false);
  const [punishments, setPunishments] = useState<PunishmentType[]>([]);

  /**
   * Fetches the punishments of the user.
   */
  const fetching = async () => {
    setLoading(true);
    await getUserPunishments(authState?.user?.userId!)
      .then((response) => {
        setPunishments(response.data);
      })
      .catch((error) => console.error('Error fetching punishments:', error));
    setLoading(false);
  };

  /**
   * Fetches the punishments of the user when the component mounts.
   */
  useEffect(() => {
    fetching();
  }, []);

  const renderPunishments: ListRenderItem<PunishmentType> = ({ item }) => {
    return (
      <Punishment.Root>
        <Punishment.Info
          punishmentId={item.punishmentId}
          punishmentReason={item.punishmentReason}
          punishmentLevel={item.punishmentLevel}
          request={item.request}
        />
      </Punishment.Root>
    );
  };

  return (
    <SafeAreaView style={{ flex: 1, marginHorizontal: 20 }}>
      <FlatList
        data={punishments}
        renderItem={renderPunishments}
        keyExtractor={(item) => item.punishmentId.toString()}
        refreshControl={<RefreshControl colors={[COLORS.PRIMARY_ORANGE]} refreshing={loading} onRefresh={fetching} />}
      />
    </SafeAreaView>
  );
}
