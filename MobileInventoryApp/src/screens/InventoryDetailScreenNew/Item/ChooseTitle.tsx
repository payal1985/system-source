import React from "react"
import { View, Text, TouchableOpacity, TextInput } from "react-native"
import styles from "../styles"
import Icon from "react-native-vector-icons/Ionicons"

interface ChooseTitleProps {
  handleOpenModal: any
  custBuidingName: string
  custFloor: string
  handleSaveDataLocation: any
  setcustGPS: any
  custGPS: string
  setAreanumber: any
  Areanumber: string
}

const ChooseTitle = (props: ChooseTitleProps) => {
  const {
    Areanumber,
    custBuidingName,
    custFloor,
    custGPS,
    handleOpenModal,
    handleSaveDataLocation,
    setAreanumber,
    setcustGPS,
  } = props
  return (
    <>
      <View style={styles.choseTitleContainer}>
        <View style={styles.subChoseTitleContainer}>
          <Text style={styles.txtTitleChose}>
            Building Name <Text style={{ color: "red" }}>*</Text>
          </Text>
          <TouchableOpacity style={styles.btnDefault} onPress={() => handleOpenModal("Building")}>
            <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
              {custBuidingName}
            </Text>
            <Icon name={"list"} size={20} />
          </TouchableOpacity>
        </View>
      </View>
      <View style={styles.choseTitleContainer}>
        <View style={styles.subChoseTitleContainer}>
          <Text style={styles.txtTitleChose}>
            Floor <Text style={{ color: "red" }}>*</Text>
          </Text>
          <TouchableOpacity style={styles.btnDefault} onPress={() => handleOpenModal("Floors")}>
            <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
              {custFloor}
            </Text>
            <Icon name={"list"} size={20} />
          </TouchableOpacity>
        </View>
      </View>
      <View style={styles.choseTitleContainer}>
        <View style={styles.subChoseTitleContainer}>
          <Text style={styles.txtTitleChose}>
            GPS <Text style={{ color: "red" }}>*</Text>
          </Text>
          <View style={styles.btnDefault}>
            <TextInput
              style={styles.txtSize16}
              numberOfLines={2}
              onBlur={() => handleSaveDataLocation("gps")}
              maxFontSizeMultiplier={1}
              onChangeText={(text) => setcustGPS(text)}
            >
              {custGPS}
            </TextInput>
            <Icon name={"location"} size={20} />
          </View>
        </View>
      </View>

      <View style={styles.choseTitleContainer}>
        <View style={styles.subChoseTitleContainer}>
          <Text style={styles.txtTitleChose}>
            Area or Room number <Text style={{ color: "red" }}>*</Text>
          </Text>
          <View style={styles.btnDefault}>
            <TextInput
              style={styles.txtSize16}
              maxFontSizeMultiplier={1}
              onBlur={() => handleSaveDataLocation("areaNumber")}
              onChangeText={(text) => setAreanumber(text)}
            >
              {Areanumber}
            </TextInput>
          </View>
        </View>
      </View>
    </>
  )
}
export default ChooseTitle
