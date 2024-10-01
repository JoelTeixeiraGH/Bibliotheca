import { COLORS } from '@/constants/constants';
import { useAuth } from '@/context/AuthContext';
import globalStyles from '@/styles/globalStyles';
import { Link } from 'expo-router';
import { Pressable, Text, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';

export default function Profile() {
  const { onLogout } = useAuth();
  const globalStyle = globalStyles();

  return (
    <SafeAreaView style={{ flex: 1, marginHorizontal: 20 }}>
      <Link href="/profile/editProfile" asChild>
        <Pressable style={globalStyle.buttonPrimary}>
          <Text style={[globalStyle.fontRegular, { color: 'white' }]}>Edit profile</Text>
        </Pressable>
      </Link>
      <View style={{ marginTop: 10 }} />
      <Link href="/profile/newPassword" asChild>
        <Pressable style={globalStyle.buttonPrimary}>
          <Text style={[globalStyle.fontRegular, { color: 'white' }]}>Change password</Text>
        </Pressable>
      </Link>
      <View style={{ marginTop: 10 }} />
      <Link href="/profile/myEvaluations" asChild>
        <Pressable style={globalStyle.buttonPrimary}>
          <Text style={[globalStyle.fontRegular, { color: 'white' }]}>My Reviews</Text>
        </Pressable>
      </Link>
      <View style={{ marginTop: 10 }} />
      <Pressable onPress={onLogout} style={[globalStyle.buttonPrimary, { backgroundColor: COLORS.PRIMARY_RED }]}>
        <Text style={[globalStyle.fontRegular, { color: 'white' }]}>Logout</Text>
      </Pressable>
    </SafeAreaView>
  );
}
