import React from "react"
import { View, Text, TextInput, TouchableOpacity } from "react-native"
import IconFontAwesome from "react-native-vector-icons/FontAwesome5"
import styles from "../styles"

interface ItemNotReturnValueProps {
  item: any
  index: any
  hasSupportFile: boolean
  inventorySupportFile: any
  isError: boolean
  isDecimal: boolean
  showImgSupportFile: any
  checkKeyBoardType: any
  checkKeyType: any
  checkCompareOnBlur: any
  onChangeTextArr: any
}

const ItemNotReturnValue = (props: ItemNotReturnValueProps) => {
  const {
    item,
    hasSupportFile,
    showImgSupportFile,
    inventorySupportFile,
    isError,
    checkKeyBoardType,
    checkKeyType,
    checkCompareOnBlur,
    index,
    isDecimal,
    onChangeTextArr,
  } = props
  return (
    <View>
      <View style={{ flex: 1 }}>
        <View style={styles.containerItemValue}>
          <View style={styles.rowGeneral}>
            <Text style={styles.redRequire}>
              {item.itemTypeOptionName}
              {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
            </Text>
            {hasSupportFile && (
              <TouchableOpacity style={styles.circlePressable} onPress={() => showImgSupportFile(inventorySupportFile)}>
                <IconFontAwesome name="info-circle" size={16} />
              </TouchableOpacity>
            )}
          </View>
          <View style={[styles.btnDefault, { flexDirection: "row", backgroundColor: isError ? "red" : "white" }]}>
            <View style={{ flex: 9 }}>
              <TextInput
                style={styles.txtSize16}
                keyboardType={checkKeyBoardType(item)}
                returnKeyType={checkKeyType(item)}
                onBlur={() => checkCompareOnBlur()}
                onChangeText={(txt) => onChangeTextArr(txt, 0, index, item)}
                placeholder={isDecimal ? "0.00" : ""}
              />
            </View>
          </View>
        </View>
      </View>
    </View>
  )
}
export default ItemNotReturnValue
