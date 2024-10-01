import { View, Text, StyleSheet } from 'react-native';
import globalStyles from '@/styles/globalStyles';

interface RequestInfoProps {
  punishmentId: number;
  punishmentReason: number;
  punishmentLevel: string;
  request: {
    requestId: number;
  };
}

export function PunishmentInfo({ punishmentId, punishmentReason, punishmentLevel, request }: RequestInfoProps) {
  const globalStyle = globalStyles();
  // const stateText = getStateText(requestStatus);
  return (
    <View style={{ alignContent: 'flex-start', flex: 1 }}>
      <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
        Punishment no: <Text style={globalStyle.fontRegular}>{punishmentId}</Text>
      </Text>
      <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
        Reason: <Text style={globalStyle.fontRegular}>{punishmentReason}</Text>
      </Text>
      <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
        Level: <Text style={globalStyle.fontRegular}>{punishmentLevel}</Text>
      </Text>
      <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
        Request: <Text style={globalStyle.fontRegular}>{request.requestId}</Text>
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
