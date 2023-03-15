import React from "react"
import { View, Text, TextInput, TouchableOpacity } from "react-native"
import styles from "../styles"
import IconImage from "react-native-vector-icons/AntDesign"

interface TextBoxPlusProps {
  item: any
  checkCompareOnBlur: any
  onChangeTextArr: any
  index: number
  addMoreTxt: any
}

const TextBoxPlus = (props: TextBoxPlusProps) => {
  const { addMoreTxt, checkCompareOnBlur, index, item, onChangeTextArr } = props
  return (
    <View>
      <View style={{ flex: 1 }}>
        <View style={styles.containerItemValue}>
          <Text style={styles.txtOptionName}>
            {item.itemTypeOptionName}
            {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
          </Text>
          <View style={[styles.btnDefault, { flexDirection: "row" }]}>
            <View style={{ flex: 9 }}>
              <TextInput
                style={styles.txtSize16}
                onBlur={() => checkCompareOnBlur()}
                onChangeText={(txt) => onChangeTextArr(txt, 0, index, item)}
              />
            </View>
            <TouchableOpacity onPress={() => addMoreTxt(0, index)} style={{ flex: 1 }}>
              <IconImage name={"plus"} size={20} />
            </TouchableOpacity>
          </View>
        </View>
      </View>
    </View>
  )
}

export default TextBoxPlus
