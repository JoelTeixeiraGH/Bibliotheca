import { FONTS } from '@/constants/constants';
import { useAuth } from '@/context/AuthContext';
import { editUserInfo, getAllLibraries } from '@/services/apiService';
import globalStyles from '@/styles/globalStyles';
import { Picker } from '@react-native-picker/picker';
import { Stack, router } from 'expo-router';
import { useEffect, useState } from 'react';
import { Text, TextInput, Pressable, Alert } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { alertInvalidEmail, alertInvalidName, isValidEmail, isValidName } from '@/utils/validations';

export default function EditProfile() {
  const { authState, updateUserInfo } = useAuth();
  const globalStyle = globalStyles();
  const [libraries, setLibraries] = useState<LibraryType[]>([]);
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [selectedOption, setSelectedOption] = useState(0);

  /**
   * Get all libraries and set name, email, library of user on component mount
   */
  useEffect(() => {
    getAllLibraries()
      .then((res) => {
        setLibraries(res.data);
      })
      .catch((err) => {
        console.log(err);
      });

    setName(authState?.user?.userName!);
    setEmail(authState?.user?.userEmail!);
    setSelectedOption(authState?.user?.libraryId!);
  }, []);

  /**
   * Handle edit profile
   */
  const handleEditProfile = async () => {
    if (!isValidEmail(email)) {
      alertInvalidEmail();
      return;
    }

    if (!isValidName(name)) {
      alertInvalidName();
      return;
    }

    const newUserInfo: UserEditType = {
      userName: name,
      userEmail: email,
      libraryId: selectedOption,
    };

    await editUserInfo(authState?.user?.userId!, newUserInfo)
      .then(() => {
        updateUserInfo!(newUserInfo);
        Alert.alert('Success', 'Profile updated successfully');
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
          title: 'Edit profile',
          headerTitleAlign: 'center',
          headerTitleStyle: { fontSize: 20, fontFamily: FONTS.BOLD },
        }}
      />
      <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold, { marginLeft: 20 }]}>Name</Text>
      <TextInput
        style={[globalStyle.input, globalStyle.colorResponsive, globalStyle.fontRegular]}
        value={name}
        onChangeText={(text) => setName(text)}
      />

      <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold, { marginLeft: 20 }]}>Email</Text>
      <TextInput
        style={[globalStyle.input, globalStyle.colorResponsive, globalStyle.fontRegular]}
        value={email}
        onChangeText={(text) => setEmail(text)}
      />

      <Text style={[globalStyle.colorResponsive, globalStyle.fontSemibold, { marginLeft: 20 }]}>Library</Text>
      <Picker
        style={[globalStyle.picker, globalStyle.colorResponsive, { marginLeft: 5 }]}
        mode="dropdown"
        dropdownIconColor={'white'}
        selectedValue={selectedOption}
        onValueChange={(itemValue) => setSelectedOption(itemValue)}
      >
        {libraries.map((library) => (
          <Picker.Item key={library.libraryId} label={library.libraryAlias} value={library.libraryId} />
        ))}
      </Picker>
      <Pressable onPress={handleEditProfile} style={[globalStyle.buttonPrimary]}>
        <Text style={[globalStyle.fontBold, { color: 'white' }]}>Edit profile</Text>
      </Pressable>
    </SafeAreaView>
  );
}
