import React from "react"
import { View, Text, TouchableOpacity, TextInput } from "react-native"
import IconImage from "react-native-vector-icons/AntDesign"
import styles from "../styles"

interface GpsHasValueProps {
  item: any
  checkCompareOnBlur: any
  onChangeTextArr: any
  index: number
  addMoreTxt: any
}

const GpsHasValue = (props: GpsHasValueProps) => {
  const { addMoreTxt, checkCompareOnBlur, index, item, onChangeTextArr } = props
  return (
    <View>
      {item?.itemTypeOptionReturnValue.map((i: any, idx: number) => {
        return (
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
                    value={i.returnValue}
                    onBlur={() => checkCompareOnBlur()}
                    onChangeText={(txt) => onChangeTextArr(txt, idx, index, item)}
                  />
                </View>
                {item?.itemTypeOptionReturnValue.length < parseInt(item.limitMax) && (
                  <TouchableOpacity onPress={() => addMoreTxt(idx, index)} style={{ flex: 1 }}>
                    <IconImage name={"plus"} size={20} />
                  </TouchableOpacity>
                )}
              </View>
            </View>
          </View>
        )
      })}
    </View>
  )
}

export default GpsHasValue
