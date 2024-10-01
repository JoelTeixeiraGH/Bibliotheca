import { View, Text, StyleSheet } from 'react-native';
import globalStyles from '@/styles/globalStyles';
import { getStateText } from '@/utils/stateTextConverter';
import { REQUEST_STATUSES, TEXTS } from '@/constants/constants';

interface RequestInfoProps {
  requestId: number;
  requestStatus: number;
  startDate: string;
  endDate: string;
  isbn: string;
}

export function RequestInfo({ requestId, requestStatus, startDate, endDate, isbn }: RequestInfoProps) {
  const globalStyle = globalStyles();
  const stateText = getStateText(requestStatus);
  return (
    <View style={{ alignContent: 'flex-start', flex: 1 }}>
      <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
        {TEXTS.REQUEST_NUMBER_TEXT} <Text style={globalStyle.fontRegular}>{requestId}</Text>
      </Text>
      <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
        ISBN: <Text style={globalStyle.fontRegular}>{isbn}</Text>
      </Text>
      <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
        {TEXTS.REQUEST_REQUISITION_DATE_TEXT} <Text style={globalStyle.fontRegular}>{startDate}</Text>
      </Text>
      {stateText !== REQUEST_STATUSES.CANCELED && stateText !== REQUEST_STATUSES.WAITING && (
        <>
          <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
            {TEXTS.REQUEST_REQUISITION_DEADLINE_TEXT} <Text style={globalStyle.fontRegular}>{endDate}</Text>
          </Text>
        </>
      )}
      <Text style={[styles.infoText, globalStyle.colorResponsive, globalStyle.fontBold]}>
        {TEXTS.REQUEST_STATE} <Text style={globalStyle.fontRegular}>{stateText}</Text>
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
