import { FONTS } from '@/constants/constants';
import { useAuth } from '@/context/AuthContext';
import { changePassword } from '@/services/apiService';
import globalStyles from '@/styles/globalStyles';
import { Stack, router } from 'expo-router';
import { useState } from 'react';
import { Text, TextInput, Pressable, Alert, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Eye, EyeOff } from 'lucide-react-native';
import {
  alertInvalidPassword,
  alertPasswordsDoNotMatch,
  arePasswordsMatching,
  isValidPassword,
} from '@/utils/validations';

export default function NewPassword() {
  const { authState } = useAuth();
  const globalStyle = globalStyles();

  const [oldPassword, setOldPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmNewPassword, setConfirmNewPassword] = useState('');
  const [showOldPassword, setShowOldPassword] = useState(false);
  const [showNewPassword, setShowNewPassword] = useState(false);
  const [showConfirmNewPassword, setShowConfirmNewPassword] = useState(false);

  /**
   * Handle change password
   * @returns
   */
  const handleNewPassword = async () => {
    if (!isValidPassword(newPassword)) {
      alertInvalidPassword();
      return;
    }

    if (!arePasswordsMatching(newPassword, confirmNewPassword)) {
      alertPasswordsDoNotMatch();
      return;
    }

    const newPasswordObj: NewPasswordType = {
      oldPassword: oldPassword,
      newPassword: newPassword,
    };

    await changePassword(authState?.user?.userId!, newPasswordObj)
      .then(() => {
        Alert.alert('Success', 'Password changed successfully');
        router.back();
      })
      .catch((err) => {
        console.log(err);
      });
  };

  return (
    <SafeAreaView style={globalStyle.containerAuthEdit}>
      <Stack.Screen
        options={{
          title: 'Create new password',
          headerTitleAlign: 'center',
          headerTitleStyle: { fontSize: 20, fontFamily: FONTS.BOLD },
        }}
      />
      <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold, { marginLeft: 20 }]}>Old password</Text>
      <View style={globalStyle.passwordContainer}>
        <TextInput
          style={[globalStyle.passwordInput, globalStyle.colorResponsive, globalStyle.fontRegular]}
          secureTextEntry={!showOldPassword}
          placeholder="Old Password"
          placeholderTextColor={'white'}
          value={oldPassword}
          onChangeText={(text) => setOldPassword(text)}
        />
        <Pressable onPress={() => setShowOldPassword(!showOldPassword)}>
          {showOldPassword ? <EyeOff size={24} color="white" /> : <Eye size={24} color="white" />}
        </Pressable>
      </View>
      <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold, { marginLeft: 20 }]}>New password</Text>
      <View style={globalStyle.passwordContainer}>
        <TextInput
          style={[globalStyle.passwordInput, globalStyle.colorResponsive, globalStyle.fontRegular]}
          secureTextEntry={!showNewPassword}
          placeholder="New password"
          placeholderTextColor={'white'}
          value={newPassword}
          onChangeText={(text) => setNewPassword(text)}
        />
        <Pressable onPress={() => setShowNewPassword(!showNewPassword)}>
          {showNewPassword ? <EyeOff size={24} color="white" /> : <Eye size={24} color="white" />}
        </Pressable>
      </View>
      <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold, { marginLeft: 20 }]}>
        Confirm new password
      </Text>
      <View style={globalStyle.passwordContainer}>
        <TextInput
          style={[globalStyle.passwordInput, globalStyle.colorResponsive, globalStyle.fontRegular]}
          secureTextEntry={!showConfirmNewPassword}
          placeholder="Confirm new password"
          placeholderTextColor={'white'}
          value={confirmNewPassword}
          onChangeText={(text) => setConfirmNewPassword(text)}
        />
        <Pressable onPress={() => setShowConfirmNewPassword(!showConfirmNewPassword)}>
          {showConfirmNewPassword ? <EyeOff size={24} color="white" /> : <Eye size={24} color="white" />}
        </Pressable>
      </View>

      <Pressable onPress={handleNewPassword} style={[globalStyle.buttonPrimary]}>
        <Text style={[globalStyle.fontBold, { color: 'white' }]}>Create new password</Text>
      </Pressable>
    </SafeAreaView>
  );
}
