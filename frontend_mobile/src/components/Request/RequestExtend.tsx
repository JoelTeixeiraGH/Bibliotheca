import { useState } from 'react';
import { Pressable, StyleSheet, Modal, View, Text } from 'react-native';
import { BlurView } from 'expo-blur';
import globalStyles from '@/styles/globalStyles';
import { X, CalendarPlus } from 'lucide-react-native';
import { TEXTS } from '@/constants/constants';

export function RequestExtend() {
  const globalStyle = globalStyles();
  const [modalVisible, setModalVisible] = useState(false);

  return (
    <>
      <Pressable
        style={[globalStyle.buttonPrimary, globalStyle.buttonPrimaryIcons]}
        onPress={() => setModalVisible(true)}
      >
        <CalendarPlus size={18} color="white" />
      </Pressable>

      <Modal
        animationType="slide"
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => {
          setModalVisible(!modalVisible);
        }}
      >
        <BlurView intensity={20} style={StyleSheet.absoluteFill}>
          <View style={globalStyle.centeredView}>
            <View style={globalStyle.modalView}>
              <Pressable
                style={[{ alignSelf: 'flex-end', marginBottom: 5 }]}
                onPress={() => setModalVisible(!modalVisible)}
              >
                <X size={18} color="white" />
              </Pressable>
              <View style={{ alignItems: 'center', justifyContent: 'center' }}>
                <Text style={[globalStyle.fontRegular, { color: 'white', textAlign: 'center' }]}>
                  {TEXTS.REQUEST_EXTEND_TEXT}
                </Text>
              </View>
            </View>
          </View>
        </BlurView>
      </Modal>
    </>
  );
}
