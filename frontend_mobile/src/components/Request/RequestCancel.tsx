import { useState } from 'react';
import { Pressable, StyleSheet, Modal, View, Text } from 'react-native';
import { BlurView } from 'expo-blur';
import globalStyles from '@/styles/globalStyles';
import { X, BookMinus } from 'lucide-react-native';
import { changeToCanceled } from '@/services/apiService';
import { COLORS, TEXTS } from '@/constants/constants';

interface RequestCancelProps {
  requestId: number;
  fun: () => void;
}

export function RequestCancel({ requestId, fun }: RequestCancelProps) {
  const globalStyle = globalStyles();
  const [modalVisible, setModalVisible] = useState(false);
  const [loading, setLoading] = useState(false);

  const cancelRequest = async () => {
    setLoading(true);
    await changeToCanceled(requestId)
      .then(() => {
        setLoading(false);
        setModalVisible(false);
        fun();
      })
      .catch((error) => {
        console.error('Error canceling request:', error);
        alert(TEXTS.REQUEST_CANCEL_ERROR_TEXT);
        setLoading(false);
        setModalVisible(false);
      });
  };

  return (
    <>
      <Pressable
        style={[globalStyle.buttonPrimary, globalStyle.buttonPrimaryIcons, { backgroundColor: COLORS.PRIMARY_RED }]}
        onPress={() => setModalVisible(true)}
      >
        <BookMinus size={18} color="white" />
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
                <Text style={[globalStyle.fontRegular, { color: 'white', textAlign: 'center', marginBottom: 5 }]}>
                  {TEXTS.REQUEST_CANCEL_TEXT}
                </Text>
                <Pressable
                  style={[
                    globalStyle.buttonPrimary,
                    globalStyle.buttonPrimaryIcons,
                    { backgroundColor: COLORS.PRIMARY_RED },
                  ]}
                  onPress={() => cancelRequest()}
                >
                  <Text style={[globalStyle.fontRegular, { color: 'white', textAlign: 'center' }]}>Cancel</Text>
                </Pressable>
              </View>
            </View>
          </View>
        </BlurView>
      </Modal>
    </>
  );
}
