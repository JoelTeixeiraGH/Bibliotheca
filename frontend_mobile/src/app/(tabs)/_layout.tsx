import { FONTS, TEXTS } from '@/constants/constants';
import { Redirect, Tabs } from 'expo-router';
import { Search, Home, UserCircle2, Book, Gavel } from 'lucide-react-native';
import { useAuth } from '@/context/AuthContext';

export default function TabRoutesLayout() {
  const { authState } = useAuth();

  if (!authState?.authenticated) {
    return <Redirect href="/login" />;
  }

  return (
    <Tabs screenOptions={{ headerTitleAlign: 'center', headerTitleStyle: { fontSize: 20, fontFamily: FONTS.BOLD } }}>
      <Tabs.Screen
        name="index"
        options={{
          title: '📙 Bibliotheca',
          tabBarLabel: () => null,
          tabBarIcon: ({ size, color }) => <Home size={size} color={color} />,
        }}
      />

      <Tabs.Screen
        name="search"
        options={{
          title: TEXTS.SEARCH_TITLE,
          tabBarLabel: () => null,
          tabBarIcon: ({ size, color }) => <Search size={size} color={color} />,
        }}
      />
      <Tabs.Screen
        name="requested"
        options={{
          title: TEXTS.REQUESTS_TITLE,
          tabBarLabel: () => null,
          tabBarIcon: ({ size, color }) => <Book size={size} color={color} />,
        }}
      />
      <Tabs.Screen
        name="punishments"
        options={{
          title: TEXTS.PUNISHMENTS_TITLE,
          tabBarLabel: () => null,
          tabBarIcon: ({ size, color }) => <Gavel size={size} color={color} />,
        }}
      />
      <Tabs.Screen
        name="profile"
        options={{
          title: TEXTS.PROFILE_TITLE,
          tabBarLabel: () => null,
          tabBarIcon: ({ size, color }) => <UserCircle2 size={size} color={color} />,
        }}
      />
    </Tabs>
  );
}
