import { View, StyleSheet } from 'react-native';
import { ReactNode } from 'react';

interface CategoryRootProps {
  children: ReactNode;
}

// in the future, on click should go to search page with category filter

export function CategoryRoot({ children }: CategoryRootProps) {
  return <View style={styles.root}>{children}</View>;
}

const styles = StyleSheet.create({
  root: {
    alignItems: 'center',
    justifyContent: 'center',
  },
});
