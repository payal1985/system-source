import React from "react"
import { Text, Dimensions, View, ViewStyle, TextPropTypes } from "react-native"
import Colors from "/configs/Colors"
import { fontSizer } from "/utils"

import TouchableWithoutTwiceClick from "./TouchableWithoutTwichClick"

type BaseButtonProps = {
  style?: ViewStyle
  label?: string
  onPress?: () => void
  borderRadius?: number
  width?: number | string
  backgroundColor?: string
  height?: number
  color?: string
  fontWeight?: "100" | "600" | "normal" | "bold" | "200" | "300" | "400" | "500" | "700" | "800" | "900" | undefined
  marginTop?: number
  marginLeft?: number
  marginRight?: number
  marginBottom?: number
  disabled?: boolean
  paddingBottom?: number
  fontSize?: number
}

const BaseButton = ({
  style,
  label,
  onPress,
  borderRadius = 50,
  width = Dimensions.get("window").width - 40,
  backgroundColor = Colors.baseColor,
  height = 46,
  color = "white",
  fontWeight = "600",
  marginTop,
  marginLeft,
  marginRight,
  marginBottom = 10,
  disabled = false,
  paddingBottom,
  fontSize = fontSizer(13),
}: BaseButtonProps) => {
  return (
    <View
      style={[
        {
          borderRadius,
          height,
          width,
          marginTop,
          marginBottom,
          marginLeft,
          marginRight,
          backgroundColor: disabled ? "#6666" : backgroundColor,
          alignItems: "center",
          justifyContent: "center",
          borderBottomColor: "red",
        },
        style,
      ]}
    >
      <TouchableWithoutTwiceClick
        disabled={disabled}
        style={{
          borderRadius,
          height,
          width,
          alignItems: "center",
          justifyContent: "center",
          paddingBottom: paddingBottom ? paddingBottom : 0,
        }}
        onPress={onPress}
      >
        <Text
          style={{
            color,
            fontSize,
            fontWeight,
          }}
        >
          {label}
        </Text>
      </TouchableWithoutTwiceClick>
    </View>
  )
}

export default BaseButton
