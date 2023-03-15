import React from "react"
import { View, Text, TouchableOpacity, TextInput } from "react-native"
import IconFontAwesome from "react-native-vector-icons/FontAwesome5"
import styles from "../styles"

interface ItemReturnValueProps {
  item: any
  hasSupportFile: boolean
  showImgSupportFile: any
  inventorySupportFile: any
  isError: boolean
  checkKeyBoardType: any
  checkKeyType: any
  checkCompareOnBlur: any
  onChangeTextArr: any
  index: any
  isDecimal: any
}

const ItemReturnValue = (props: ItemReturnValueProps) => {
  const {
    item,
    hasSupportFile,
    showImgSupportFile,
    inventorySupportFile,
    isError,
    checkKeyBoardType,
    checkKeyType,
    checkCompareOnBlur,
    onChangeTextArr,
    index,
    isDecimal,
  } = props
  return (
    <View>
      {item?.itemTypeOptionReturnValue != "" &&
        item?.itemTypeOptionReturnValue?.map((i: any, idx: number) => {
          return (
            <View style={{ flex: 1 }}>
              <View style={styles.containerItemValue}>
                <View style={styles.rowGeneral}>
                  <Text style={styles.redRequire}>
                    {item.itemTypeOptionName}
                    {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
                  </Text>
                  {hasSupportFile && (
                    <TouchableOpacity
                      style={styles.circlePressable}
                      onPress={() => showImgSupportFile(inventorySupportFile)}
                    >
                      <IconFontAwesome name="info-circle" size={16} />
                    </TouchableOpacity>
                  )}
                </View>
                <View style={[styles.btnDefault, { flexDirection: "row", backgroundColor: isError ? "red" : "white" }]}>
                  <View style={{ flex: 9 }}>
                    <TextInput
                      style={styles.txtSize16}
                      value={i.returnValue}
                      keyboardType={checkKeyBoardType(item)}
                      returnKeyType={checkKeyType(item)}
                      onBlur={() => checkCompareOnBlur()}
                      onChangeText={(txt) => onChangeTextArr(txt, idx, index, item)}
                      placeholder={isDecimal ? "0.00" : ""}
                    />
                  </View>
                </View>
              </View>
            </View>
          )
        })}
    </View>
  )
}

export default ItemReturnValue
