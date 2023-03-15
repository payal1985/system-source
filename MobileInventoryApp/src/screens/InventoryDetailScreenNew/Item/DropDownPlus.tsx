import React from "react"
import { View, Text, TouchableOpacity } from "react-native"
import Icon from "react-native-vector-icons/Ionicons"
import styles from "../styles"

interface DropDownPlusProps {
  item: any
  openModal: any
  custClient: any
}

const DropDownPlus = (props: DropDownPlusProps) => {
  const { item, custClient, openModal } = props
  return (
    <View style={styles.containerItemValue}>
      <Text style={styles.redRequire}>
        {item.itemTypeOptionName}
        {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
      </Text>
      <TouchableOpacity style={styles.btnDefault} onPress={() => openModal(item)}>
        <Text style={[styles.txtSize16, { marginVertical: 5 }]} maxFontSizeMultiplier={1}>
          {custClient}
        </Text>
        <Icon name={"list"} size={20} />
      </TouchableOpacity>
    </View>
  )
}
export default DropDownPlus
