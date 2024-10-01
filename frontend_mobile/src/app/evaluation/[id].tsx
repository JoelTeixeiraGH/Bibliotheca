import MyActivityIndicator from '@/components/MyComponents/MyActivityIndicator';
import { FONTS } from '@/constants/constants';
import { useAuth } from '@/context/AuthContext';
import { editEvaluation, getEvaluationById, postEvaluation } from '@/services/apiService';
import globalStyles from '@/styles/globalStyles';
import {
  alertInvalidCharactersLength,
  alertInvalidEvaluationDescription,
  alertInvalidScore,
  isTextMoreThanCharacters,
  isValidLibrary,
  isValidName,
} from '@/utils/validations';
import { Picker } from '@react-native-picker/picker';
import { Stack, router, useLocalSearchParams } from 'expo-router';
import { useEffect, useState } from 'react';
import { Alert, Pressable, Text, View } from 'react-native';
import { TextInput } from 'react-native-gesture-handler';

export default function Page() {
  const { id } = useLocalSearchParams<{ id: string; id2?: string }>();
  const { authState } = useAuth();

  const globalStyle = globalStyles();

  const [loading, setLoading] = useState(false);
  const [evaluationId, setEvaluationId] = useState<number>();
  const [evaluation, setEvaluation] = useState<EvaluationSubmitType>();
  const [code, setCode] = useState<string>();

  /**
   * Fetch evaluation by id
   */
  const fetchEvaluation = async (evaluationId: number) => {
    setLoading(true);
    await getEvaluationById(evaluationId)
      .then((res) => {
        setEvaluation(res.data);
      })
      .catch((err) => {
        console.log(err);
      });
    setLoading(false);
  };

  /**
   * Get evaluation id from url
   * If code is 'edit', fetch evaluation by id
   * If code is 'add', set isbn from url
   */
  useEffect(() => {
    const idSplitted = id.split(';');
    const code = idSplitted[0];
    setCode(code);
    const idValue = idSplitted[1];
    if (code === 'edit') {
      setEvaluationId(parseInt(idValue!));
      fetchEvaluation(parseInt(idValue!));
    } else {
      setEvaluation({ ...evaluation!, userId: authState?.user?.userId!, isbn: idValue! });
    }
  }, []);

  /**
   * Scores
   */
  const scores = [
    {
      label: '★ Did not like it at all',
      value: 1,
    },
    {
      label: '★★ Mostly didn’t like it',
      value: 2,
    },
    {
      label: '★★★ Liked it',
      value: 3,
    },
    {
      label: '★★★★ Really liked it',
      value: 4,
    },
    {
      label: '★★★★★ Loved it',
      value: 5,
    },
  ];

  /**
   * Submit review
   */
  const submitReview = () => {
    if (!isValidName(evaluation?.evaluationDescription!)) {
      alertInvalidEvaluationDescription();
      return;
    }

    if (!isValidLibrary(evaluation?.evaluationScore!)) {
      alertInvalidScore();
      return;
    }

    if (!isTextMoreThanCharacters(evaluation?.evaluationDescription!, 255)) {
      alertInvalidCharactersLength(255);
      return;
    }

    postEvaluation(evaluation!)
      .then((res) => {
        Alert.alert('Success', 'Review submitted successfully');
        router.replace('/profile/myEvaluations');
      })
      .catch((err) => {
        Alert.alert('Error', 'Failed to submit review, check if you have already submitted a review for this book');
      });
  };

  /**
   * Edit review
   */
  const putEvaluation = () => {
    const filteredEvaluation: EvaluationSubmitType = {
      evaluationDescription: evaluation?.evaluationDescription!,
      evaluationScore: evaluation?.evaluationScore!,
      userId: evaluation?.userId!,
      isbn: evaluation?.isbn!,
    };

    if (!isValidName(filteredEvaluation.evaluationDescription!)) {
      alertInvalidEvaluationDescription();
      return;
    }

    if (!isValidLibrary(filteredEvaluation.evaluationScore!)) {
      alertInvalidScore();
      return;
    }

    if (!isTextMoreThanCharacters(filteredEvaluation.evaluationDescription!, 255)) {
      alertInvalidCharactersLength(255);
      return;
    }

    editEvaluation(evaluationId!, filteredEvaluation)
      .then((res) => {
        Alert.alert('Success', 'Review edited successfully, refresh the page to see the changes.');
        router.back();
      })
      .catch((err) => {
        console.log(err.response.data);
        Alert.alert('Error', 'Failed to edit review');
      });
  };

  return (
    <View style={{ flex: 1, margin: 10 }}>
      <Stack.Screen
        options={{
          title: 'Write your review',
          headerTitleAlign: 'center',
          headerTitleStyle: { fontSize: 20, fontFamily: FONTS.BOLD },
        }}
      />

      {loading ? (
        <MyActivityIndicator />
      ) : (
        <>
          <TextInput
            multiline={true}
            style={[globalStyle.input, globalStyle.colorResponsive, globalStyle.fontRegular]}
            placeholder="Write your review..."
            placeholderTextColor={'white'}
            value={evaluation?.evaluationDescription}
            onChangeText={(text) => setEvaluation({ ...evaluation!, evaluationDescription: text })}
          />

          <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold, { marginLeft: 20 }]}>Score</Text>
          <Picker
            mode="dropdown"
            dropdownIconColor={'white'}
            selectedValue={evaluation?.evaluationScore}
            onValueChange={(itemValue) => setEvaluation({ ...evaluation!, evaluationScore: itemValue })}
            style={[globalStyle.picker, globalStyle.colorResponsive, { marginLeft: 5 }]}
          >
            <Picker.Item label="Not Yet Scored" value={0} />
            {scores.map((score) => (
              <Picker.Item key={score.value} label={score.label} value={score.value} />
            ))}
          </Picker>
          <View style={{ alignItems: 'center' }}>
            <Pressable
              style={globalStyle.buttonPrimary}
              onPress={() => {
                code === 'add' ? submitReview() : putEvaluation();
              }}
            >
              <Text style={[globalStyle.colorResponsive, globalStyle.fontRegular]}>
                {code === 'add' ? 'Submit review' : 'Edit review'}
              </Text>
            </Pressable>
          </View>
        </>
      )}
    </View>
  );
}
