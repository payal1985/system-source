import React from "react"
import { TouchableOpacity } from "react-native"
import { TextField } from "react-native-material-textfield"
import Icon from "react-native-vector-icons/FontAwesome5"
import { BaseSelectProps } from "./types"
import { fontSizer, responsiveH } from "/utils/dimension"
import styles from "./styles"

const BaseSelect = ({
  openModal,
  value,
  checkDisabled,
  label,
  tintColor,
  baseColor,
  textColor,
  disableColor,
  placeholder,
  validateText,
}: BaseSelectProps) => {
  return (
    <TouchableOpacity
      disabled={checkDisabled}
      onPress={openModal}
      style={{ justifyContent: "center", marginVertical: responsiveH(-4) }}
    >
      <Icon
        style={{ position: "absolute", right: 0, bottom: value ? (validateText ? 38 : 20) : validateText ? 30 : 15 }}
        name={"chevron-down"}
        size={16}
        color={checkDisabled ? "#848780" : "black"}
      />
      <TextField
        fontSize={fontSizer(13)}
        label={label}
        textColor={checkDisabled ? "#848780" : textColor}
        tintColor={tintColor}
        baseColor={baseColor}
        value={value}
        lineWidth={0.5}
        multiline={true}
        editable={false}
        error={validateText}
        style={[
          styles.textField,
          {
            backgroundColor: checkDisabled == true ? disableColor : "transparent",
          },
        ]}
        placeholder={checkDisabled == true ? placeholder : ""}
      />
    </TouchableOpacity>
  )
}

export default BaseSelect
