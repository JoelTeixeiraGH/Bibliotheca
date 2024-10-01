import { Text, View, TextInput, Pressable } from 'react-native';
import { useState } from 'react';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Link, Redirect, Stack } from 'expo-router';
import { useAuth } from '@/context/AuthContext';
import { Eye, EyeOff } from 'lucide-react-native';
import globalStyles from '@/styles/globalStyles';

export default function Login() {
  const { onLogin, authState } = useAuth();
  const globalStyle = globalStyles();

  const login = async () => {
    await onLogin!(email, password);
  };

  if (authState?.authenticated) {
    return <Redirect href="/(tabs)" />;
  }

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  return (
    <SafeAreaView style={globalStyle.containerAuth}>
      <Stack.Screen options={{ headerShown: false }} />
      <Text
        style={[
          globalStyle.colorResponsive,
          globalStyle.fontBold,
          { fontSize: 24, textAlign: 'center', marginBottom: 30 },
        ]}
      >
        📙 Bibliotheca
      </Text>
      <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold, { marginLeft: 20 }]}>Email</Text>
      <TextInput
        style={[globalStyle.colorResponsive, globalStyle.input, globalStyle.fontRegular, { marginBottom: 20 }]}
        placeholder="Email"
        placeholderTextColor={'white'}
        keyboardType="email-address"
        autoCapitalize="none"
        value={email}
        onChangeText={(text) => setEmail(text)}
      />
      <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold, { marginLeft: 20 }]}>Password</Text>
      <View style={globalStyle.passwordContainer}>
        <TextInput
          style={[globalStyle.colorResponsive, globalStyle.passwordInput, globalStyle.fontRegular]}
          secureTextEntry={!showPassword}
          placeholder="Password"
          placeholderTextColor={'white'}
          value={password}
          onChangeText={(text) => setPassword(text)}
        />
        <Pressable onPress={togglePasswordVisibility}>
          {showPassword ? <EyeOff size={24} color="white" /> : <Eye size={24} color="white" />}
        </Pressable>
      </View>

      <Pressable onPress={login} style={[globalStyle.buttonPrimary]}>
        <Text style={[globalStyle.fontBold, { color: 'white' }]}>Login</Text>
      </Pressable>

      <View style={{ alignItems: 'center', marginTop: 20 }}>
        <Link href="/register">
          <Text style={[globalStyle.colorResponsive, globalStyle.fontRegular, { textDecorationLine: 'underline' }]}>
            Create an account
          </Text>
        </Link>
      </View>
    </SafeAreaView>
  );
}
