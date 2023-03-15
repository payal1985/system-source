import React from "react"
import { View, Text, TouchableOpacity } from "react-native"
import styles from "../styles"
import Icon from "react-native-vector-icons/Ionicons"

interface ChooseItemTypeProps {
  disableChoseItemType: boolean
  changeItemType: any
  itemTypeTxt: string
}

const ChooseItemType = (props: ChooseItemTypeProps) => {
  const { changeItemType, disableChoseItemType, itemTypeTxt } = props
  return (
    <View style={{ flex: 1 }}>
      <View style={styles.containerItemValue}>
        <Text style={styles.txtOptionName}>
          Item Type <Text style={{ color: "red" }}>*</Text>
        </Text>
        <TouchableOpacity disabled={disableChoseItemType} style={styles.btnDefault} onPress={() => changeItemType()}>
          <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
            {itemTypeTxt}
          </Text>
          <Icon name={"list"} size={20} />
        </TouchableOpacity>
      </View>
    </View>
  )
}
export default ChooseItemType
