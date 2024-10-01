import { View, Text, StyleSheet } from 'react-native';
import globalStyles from '@/styles/globalStyles';

interface EvaluationContentProps {
  evaluationDescription: string;
  evaluationScore: number;
  userName: string;
  isbn: string;
  emittedDate: string;
}

/**
 * Returns an array of stars based on the score
 * @param score
 * @returns array of stars based on the score
 */
const stars = (score: number) => {
  const stars = [];
  for (let i = 0; i < score; i++) {
    stars.push('⭐');
  }
  return stars;
};

export function EvaluationContent({
  evaluationDescription,
  evaluationScore,
  userName,
  isbn,
  emittedDate,
}: EvaluationContentProps) {
  const globalStyle = globalStyles();
  return (
    <View style={{ alignContent: 'flex-start', flex: 1 }}>
      <View style={{ flexDirection: 'row', justifyContent: 'space-between' }}>
        <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>{userName}</Text>
        <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
          {stars(evaluationScore)}
        </Text>
      </View>
      {emittedDate ? (
        <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontRegular]}>{emittedDate}</Text>
      ) : null}
      <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontRegular, { marginTop: 5 }]}>
        {evaluationDescription}
      </Text>
    </View>
  );
}

const styles = StyleSheet.create({
  infoText: {
    fontSize: 14,
    textAlign: 'left',
  },
});
