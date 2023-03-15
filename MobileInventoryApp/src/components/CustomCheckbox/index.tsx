import React, { memo } from "react"
import { StyleSheet, Platform, TouchableOpacity, View, ViewStyle, StyleProp } from "react-native"
import CheckBox from "@react-native-community/checkbox"
import { Colors } from "../../configs"

type CustomCheckboxProps = {
  style?: StyleProp<ViewStyle> | undefined
  disabled?: boolean
  onPress?: () => void
  value?: any
  onValueChange?: (value: boolean) => void
  children?: React.ReactChild
  type?: "circle" | "square"
  iOSCheckBoxStyles?: StyleProp<ViewStyle> | undefined
  tintColors?: any
}

const CustomCheckbox = ({
  style,
  disabled,
  onPress,
  value,
  onValueChange,
  children,
  type = "square",
  iOSCheckBoxStyles,
  tintColors,
}: CustomCheckboxProps) => {
  return (
    <View style={style}>
      {Platform.OS === "ios" ? (
        <CheckBox
          value={value}
          onValueChange={onValueChange}
          tintColors={tintColors || { true: Colors.easternBlue, false: Colors.whiteSmoke }}
          boxType={type}
          style={[styles.iosCheckbox, iOSCheckBoxStyles]}
          onCheckColor={Colors.white}
          onFillColor={Colors.easternBlue}
          onTintColor={Colors.easternBlue}
          disabled={disabled}
        />
      ) : (
        <CheckBox
          value={value}
          onValueChange={onValueChange}
          tintColors={tintColors || { true: Colors.easternBlue, false: Colors.whiteSmoke }}
          boxType={type}
          disabled={disabled}
        />
      )}
      <TouchableOpacity onPress={onPress} disabled={disabled}>
        {children}
      </TouchableOpacity>
    </View>
  )
}

export default memo(CustomCheckbox)

const styles = StyleSheet.create({
  iosCheckbox: {
    marginTop: 15,
    marginRight: 15,
    width: 20,
    height: 20,
  },
})
