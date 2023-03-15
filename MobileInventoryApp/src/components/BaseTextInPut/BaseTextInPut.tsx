import React, { useEffect, useRef } from "react"
import { Platform, View } from "react-native"
import Icon from "react-native-vector-icons/FontAwesome5"
import { TextField } from "rn-material-ui-textfield"

import Colors from "/configs/Colors"
import { fontSizer } from "../../utils/dimension"
import { BaseTextInPutProps } from "./types"

const BaseTextInPut = ({
  label,
  value,
  textColor,
  baseColor,
  keyboardType = "default",
  onChangeText,
  secureTextEntry = false,
  width,
  onFocus,
  maxLength,
  placeholder,
  onBlur,
  validateText,
  disable,
  colorText = "black",
  disableText,
  canRenderRight,
  nameIcon,
  colorIcon,
  iconSize,
  fontSize = fontSizer(14),
}: BaseTextInPutProps) => {
  const ref = useRef<any>(null)

  useEffect(() => {
    if (ref && ref.current) {
      ref.current.setNativeProps({
        style: {
          ...Platform.select({
            ios: { fontFamily: "Arial" },
            android: { fontFamily: "Roboto" },
          }),
        },
      })
    }
  }, [])
  const iconRight = () => {
    if (!canRenderRight) return <></>
    return <Icon name={nameIcon} color={colorIcon} size={iconSize} />
  }
  return (
    <View style={{ justifyContent: "center", width: width }}>
      <TextField
        fontSize={fontSize}
        label={label}
        textColor={textColor}
        tintColor={Colors.baseColor}
        baseColor={baseColor}
        value={value}
        labelTextStyle={{ color: "black", paddingVertical: 5 }}
        activeLineWidth={1}
        maxLength={maxLength}
        lineWidth={0.5}
        multiline={secureTextEntry ? false : true}
        error={validateText}
        editable={!disable}
        placeholder={placeholder}
        secureTextEntry={secureTextEntry}
        autoCapitalize={"none"}
        keyboardType={keyboardType}
        onFocus={onFocus}
        containerStyle={{ overflow: "hidden" }}
        renderRightAccessory={iconRight}
        onChangeText={onChangeText}
        style={{
          color: disableText ? "#848780" : colorText,
          flex: 1,
        }}
        onBlur={onBlur}
      />
    </View>
  )
}

export default BaseTextInPut
