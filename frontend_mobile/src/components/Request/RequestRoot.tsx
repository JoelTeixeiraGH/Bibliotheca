import { ReactNode } from 'react';
import { View, StyleSheet } from 'react-native';

interface RequestRootProps {
  children: ReactNode;
}

export function RequestRoot({ children }: RequestRootProps) {
  return <View style={styles.root}>{children}</View>;
}

const styles = StyleSheet.create({
  root: {
    flexDirection: 'column',
    backgroundColor: '#35383f',
    padding: 10,
    borderRadius: 10,
    margin: 10,
  },
});
