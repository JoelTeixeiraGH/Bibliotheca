import { useState, useEffect } from 'react';
import { Text, TextInput, View, Pressable, Alert } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Picker } from '@react-native-picker/picker';
import { Eye, EyeOff } from 'lucide-react-native';
import globalStyles from '@/styles/globalStyles';
import { Stack, router } from 'expo-router';
import { FONTS } from '@/constants/constants';
import { useAuth } from '@/context/AuthContext';
import { getAllLibraries } from '@/services/apiService';
import {
  alertInvalidEmail,
  alertInvalidLibrary,
  alertInvalidPassword,
  alertPasswordsDoNotMatch,
  arePasswordsMatching,
  isValidEmail,
  isValidLibrary,
  isValidPassword,
} from '@/utils/validations';

export default function Register() {
  const { onRegister } = useAuth();
  const globalStyle = globalStyles();

  const [email, setEmail] = useState('');
  const [name, setName] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [selectedOption, setSelectedOption] = useState(0);
  const [libraries, setLibraries] = useState<LibraryType[]>([]);

  /**
   * Get all libraries on component mount
   */
  useEffect(() => {
    getAllLibraries()
      .then((res) => {
        setLibraries(res.data);
      })
      .catch((err) => {
        console.log(err);
      });
  }, []);

  /**
   * Toggle password visibility
   */
  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  /**
   * Toggle confirm password visibility
   */
  const toggleConfirmPasswordVisibility = () => {
    setShowConfirmPassword(!showConfirmPassword);
  };

  /**
   * Handle register
   * @returns
   */
  const handleRegister = async () => {
    if (!isValidEmail(email)) {
      alertInvalidEmail();
      return;
    }

    if (!isValidPassword(password)) {
      alertInvalidPassword();
      return;
    }

    if (!arePasswordsMatching(password, confirmPassword)) {
      alertPasswordsDoNotMatch();
      return;
    }

    if (!isValidLibrary(selectedOption)) {
      alertInvalidLibrary();
      return;
    }

    const result = await onRegister!(name, email, password, selectedOption);
    if (result.status === 201) {
      Alert.alert('Success', 'User created successfully');
      router.back();
    }
  };

  return (
    <SafeAreaView style={globalStyle.containerAuth}>
      <Stack.Screen
        options={{
          title: 'Create an account',
          headerTitleAlign: 'center',
          headerTitleStyle: { fontSize: 20, fontFamily: FONTS.BOLD },
        }}
      />
      <Text
        style={[
          globalStyle.colorResponsive,
          globalStyle.fontBold,
          { fontSize: 24, textAlign: 'center', marginBottom: 30 },
        ]}
      >
        📙 Bibliotheca
      </Text>
      <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold, { marginLeft: 20 }]}>Name</Text>
      <TextInput
        style={[globalStyle.input, globalStyle.colorResponsive, globalStyle.fontRegular]}
        placeholder="Name"
        placeholderTextColor={'white'}
        value={name}
        onChangeText={(text) => setName(text)}
      />
      <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold, { marginLeft: 20 }]}>Email</Text>
      <TextInput
        style={[globalStyle.input, globalStyle.colorResponsive, globalStyle.fontRegular]}
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
          style={[globalStyle.passwordInput, globalStyle.colorResponsive, globalStyle.fontRegular]}
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
      <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold, { marginLeft: 20 }]}>Confirm password</Text>
      <View style={globalStyle.passwordContainer}>
        <TextInput
          style={[globalStyle.passwordInput, globalStyle.colorResponsive, globalStyle.fontRegular]}
          secureTextEntry={!showConfirmPassword}
          placeholder="Confirm password"
          placeholderTextColor={'white'}
          value={confirmPassword}
          onChangeText={(text) => setConfirmPassword(text)}
        />
        <Pressable onPress={toggleConfirmPasswordVisibility}>
          {showConfirmPassword ? <EyeOff size={24} color="white" /> : <Eye size={24} color="white" />}
        </Pressable>
      </View>
      <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold, { marginLeft: 20 }]}>Library</Text>
      <Picker
        mode="dropdown"
        dropdownIconColor={'white'}
        selectedValue={selectedOption}
        onValueChange={(itemValue) => setSelectedOption(itemValue)}
        style={[globalStyle.picker, globalStyle.colorResponsive, { marginLeft: 5 }]}
      >
        <Picker.Item label="Pick your library" value={0} />
        {libraries.map((library) => (
          <Picker.Item key={library.libraryId} label={library.libraryAlias} value={library.libraryId} />
        ))}
      </Picker>
      <Pressable onPress={handleRegister} style={[globalStyle.buttonPrimary]}>
        <Text style={[globalStyle.fontBold, { color: 'white' }]}>Create an account</Text>
      </Pressable>
    </SafeAreaView>
  );
}
