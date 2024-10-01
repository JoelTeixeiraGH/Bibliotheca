import { View, TouchableOpacity, Text, StyleSheet } from 'react-native';

interface TabComponentProps {
  activeTab: number;
  onTabPress: (tabNumber: number) => void;
  tabNames: string[];
}

export default function TabComponent({ activeTab, onTabPress, tabNames }: TabComponentProps) {
  return (
    <View style={styles.container}>
      {tabNames.map((tabName, index) => (
        <TouchableOpacity key={index} style={[styles.tab]} onPress={() => onTabPress(index + 1)}>
          <Text style={[styles.tabText, activeTab === index + 1 && styles.activeTab]}>{tabName}</Text>
        </TouchableOpacity>
      ))}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    backgroundColor: 'black',
    margin: 20,
    borderRadius: 50,
  },
  tab: {
    flex: 1,
    alignItems: 'center',
    paddingVertical: 10,
  },
  activeTab: {
    backgroundColor: '#35383f',
    borderRadius: 50,
    paddingHorizontal: 10,
  },
  tabText: {
    color: 'white',
    fontSize: 13,
    fontFamily: 'urb-b',
    paddingVertical: 15,
  },
  content: {
    marginTop: 10,
  },
});
