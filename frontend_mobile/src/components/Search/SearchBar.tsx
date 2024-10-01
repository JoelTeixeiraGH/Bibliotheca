import { useRef, useEffect, useState } from 'react';
import { View, TextInput, StyleSheet } from 'react-native';
import { useTheme } from '@react-navigation/native';
import { Search } from 'lucide-react-native';
import globalStyles from '@/styles/globalStyles';
import { TEXTS } from '@/constants/constants';
import { useIsFocused } from '@react-navigation/native';
import { search } from '@/services/apiService';

interface SearchBarProps {
  onSearch: (text: string) => void;
}

export default function SearchBar({ onSearch }: SearchBarProps) {
  const globalStyle = globalStyles();
  const colors = useTheme().colors;
  const inputRef = useRef<TextInput>(null);
  const isFocused = useIsFocused();
  const [inputText, setInputText] = useState('');

  useEffect(() => {
    if (isFocused && inputRef.current) {
      inputRef.current.focus();
    }
  }, [isFocused]);

  const passSearch = () => {
    onSearch(inputText);
  };

  return (
    <View style={[styles.searchContainer, { borderColor: colors.text }]}>
      <Search size={20} color={colors.text} style={styles.searchIcon} />
      <TextInput
        placeholder={TEXTS.SEARCH_PLACEHOLDER}
        placeholderTextColor={colors.text}
        clearButtonMode="while-editing"
        style={[globalStyle.fontRegular, globalStyle.colorResponsive, { flex: 1 }]}
        ref={inputRef}
        onChangeText={(text) => setInputText(text)}
        maxLength={255} // check how many characters are allowed in the API
        returnKeyType="search"
        onSubmitEditing={() => passSearch()}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  searchContainer: { flexDirection: 'row', alignItems: 'center', borderWidth: 1, borderRadius: 50, padding: 10 },
  searchIcon: {
    marginRight: 10,
  },
});
