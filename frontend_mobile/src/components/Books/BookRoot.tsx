import { View } from 'react-native';
import { ReactNode } from 'react';

interface BookRootProps {
  children: ReactNode;
}

export function BookRoot({ children }: BookRootProps) {
  return <View>{children}</View>;
}
