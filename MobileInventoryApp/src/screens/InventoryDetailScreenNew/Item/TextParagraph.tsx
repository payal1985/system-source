import React from "react"
import { View, Text, TextInput } from "react-native"
import styles from "../styles"

interface TextParagraphProps {
  item: any
  checkCompareOnBlur: any
  handleOnEndEditing: any
  valuesTextInputTextParagraph: any
  onTextChangedTxtFeild: any
}

const TextParagraph = (props: TextParagraphProps) => {
  const { item, checkCompareOnBlur, handleOnEndEditing, valuesTextInputTextParagraph, onTextChangedTxtFeild } = props
  return (
    <View style={styles.containerItemValue}>
      <Text style={styles.redRequire}>
        {item.itemTypeOptionName}
        {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
      </Text>
      <View style={[styles.btnDefault, { justifyContent: "flex-start", minHeight: 100 }]}>
        <TextInput
          style={styles.txtSize16}
          multiline
          numberOfLines={4}
          onBlur={() => checkCompareOnBlur()}
          onEndEditing={() => handleOnEndEditing(valuesTextInputTextParagraph, item)}
          value={valuesTextInputTextParagraph}
          onChangeText={(txt) => onTextChangedTxtFeild(item.itemTypeOptionId, txt)}
        />
      </View>
    </View>
  )
}
export default TextParagraph
