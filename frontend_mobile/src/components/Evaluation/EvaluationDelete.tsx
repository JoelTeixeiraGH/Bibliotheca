import { useState } from 'react';
import { COLORS } from '@/constants/constants';
import globalStyles from '@/styles/globalStyles';
import { BlurView } from 'expo-blur';
import { Trash2, X } from 'lucide-react-native';
import { View, StyleSheet, Modal, Pressable, Text } from 'react-native';

interface EvaluationDeleteProps {
  evaluationId: number;
  deleteEvaluation: (evaluationId: number) => void;
}

export function EvaluationDelete({ evaluationId, deleteEvaluation }: EvaluationDeleteProps) {
  const globalStyle = globalStyles();
  const [modalVisible, setModalVisible] = useState(false);

  return (
    <>
      <Pressable
        style={[globalStyle.buttonPrimary, globalStyle.buttonPrimaryIcons, { backgroundColor: COLORS.PRIMARY_RED }]}
        onPress={() => setModalVisible(true)}
      >
        <Trash2 size={18} color="white" />
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
                  Are you sure you want to delete this evaluation?
                </Text>
                <Pressable
                  onPress={() => deleteEvaluation(evaluationId)}
                  style={[globalStyle.buttonPrimary, { marginTop: 10, backgroundColor: COLORS.PRIMARY_RED }]}
                >
                  <Text style={[globalStyle.fontRegular, { color: 'white' }]}>Yes</Text>
                </Pressable>
              </View>
            </View>
          </View>
        </BlurView>
      </Modal>
    </>
  );
}
