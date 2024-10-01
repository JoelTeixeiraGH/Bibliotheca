import { View, Text, StyleSheet } from 'react-native';
import { useTheme } from '@react-navigation/native';
import { Search } from 'lucide-react-native';
import globalStyles from '@/styles/globalStyles';
import { TEXTS } from '@/constants/constants';

export default function FakeSearchBar() {
  const globalStyle = globalStyles();
  const colors = useTheme().colors;
  return (
    <View style={[styles.searchContainer, { borderColor: colors.text }]}>
      <Search size={20} color={colors.text} style={styles.searchIcon} />
      <Text style={[globalStyle.fontRegular, globalStyle.colorResponsive]}>{TEXTS.SEARCH_PLACEHOLDER}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  searchContainer: { flexDirection: 'row', alignItems: 'center', borderWidth: 1, borderRadius: 50, padding: 10 },
  searchIcon: {
    marginRight: 10,
  },
});
