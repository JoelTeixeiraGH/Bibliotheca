import { ReactNode } from 'react';
import { View, StyleSheet } from 'react-native';

interface PunishmentRootProps {
  children: ReactNode;
}

export function PunishmentRoot({ children }: PunishmentRootProps) {
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
