import { COLORS, FONTS } from '@/constants/constants';
import { useAuth } from '@/context/AuthContext';
import { Stack } from 'expo-router';
import { useEffect, useState } from 'react';
import { Alert, ListRenderItem, View, RefreshControl, Text } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { deleteEvaluation, getUserEvaluations } from '@/services/apiService';
import { FlatList } from 'react-native-gesture-handler';
import { Evaluation } from '@/components/Evaluation';
import MyActivityIndicator from '@/components/MyComponents/MyActivityIndicator';
import globalStyles from '@/styles/globalStyles';

export default function EditProfile() {
  const { authState } = useAuth();
  const globalStyle = globalStyles();
  const [loading, setLoading] = useState(true);
  const [evaluations, setEvaluations] = useState<EvaluationType[]>([]);

  const fetching = async () => {
    setLoading(true);
    await getUserEvaluations(authState?.user?.userId!)
      .then((response) => {
        setEvaluations(response.data);
      })
      .catch((error) => console.error('Error fetching books:', error));

    setLoading(false);
  };

  useEffect(() => {
    fetching();
  }, []);

  const removeEvaluation = async (evaluationId: number) => {
    setLoading(true);
    await deleteEvaluation(evaluationId)
      .then((response) => {
        const newEvaluations = evaluations.filter((evaluation) => evaluation.evaluationId !== evaluationId);
        setEvaluations(newEvaluations);
        Alert.alert('Success', 'Evaluation deleted successfully!');
      })
      .catch((error) => console.error('Error deleting evaluation:', error));
    setLoading(false);
  };

  const renderEvaluations: ListRenderItem<EvaluationType> = ({ item }) => {
    return (
      <Evaluation.Root>
        <Evaluation.Content
          evaluationDescription={item.evaluationDescription}
          evaluationScore={item.evaluationScore}
          userName={authState?.user?.userName!}
          isbn={item.isbn}
          emittedDate={item.emittedDate}
        />
        <View style={{ flexDirection: 'row', alignSelf: 'flex-end' }}>
          <View style={{ marginRight: 5 }}>
            <Evaluation.ShowBookBtn isbn={item.isbn} />
          </View>
          <View style={{ marginRight: 5 }}>
            <Evaluation.Edit evaluationId={item.evaluationId} />
          </View>
          <Evaluation.Delete evaluationId={item.evaluationId} deleteEvaluation={removeEvaluation} />
        </View>
      </Evaluation.Root>
    );
  };

  return (
    <SafeAreaView style={{ flex: 1 }}>
      <Stack.Screen
        options={{
          title: 'My Reviews',
          headerTitleAlign: 'center',
          headerTitleStyle: { fontSize: 20, fontFamily: FONTS.BOLD },
        }}
      />
      {loading ? (
        <MyActivityIndicator />
      ) : evaluations.length === 0 ? (
        <Text style={[globalStyle.colorResponsive, globalStyle.fontRegular, { textAlign: 'center', fontSize: 20 }]}>
          No reviews yet!
        </Text>
      ) : (
        <FlatList
          data={evaluations}
          renderItem={renderEvaluations}
          keyExtractor={(item) => item.evaluationId.toString()}
          refreshControl={<RefreshControl colors={[COLORS.PRIMARY_ORANGE]} refreshing={loading} onRefresh={fetching} />}
        />
      )}
    </SafeAreaView>
  );
}
