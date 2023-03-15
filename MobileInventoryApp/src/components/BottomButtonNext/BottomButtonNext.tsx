import React from "react"
import { StatusBar, Text, TouchableOpacity, View } from "react-native"
import Colors from "/configs/Colors"
import { getScreenWidth } from "/utils/dimension"
import { Props } from "./types"

const BottomButtonNext = ({ disabled, labels = "Next", isExtraHeight, onPressButton, widthBtn, heightBtn }: Props) => {
  return (
    <View>
      <StatusBar barStyle="dark-content" translucent backgroundColor="rgba(0,0,0,0)" />
      <View
        style={{
          width: widthBtn || getScreenWidth(1),
          height: heightBtn || isExtraHeight ? 110 : 80,
          backgroundColor: "white",
          flexDirection: "row",
          borderTopWidth: 0.2,
        }}
      >
        <View style={{ flex: 1 }}>
          <TouchableOpacity
            onPress={onPressButton}
            disabled={disabled}
            style={{
              flex: 1,
              borderRadius: 30,
              marginVertical: 15,
              justifyContent: "center",
              alignItems: "center",
              marginHorizontal: 20,
              backgroundColor: disabled ? Colors.disableText : Colors.button,
            }}
          >
            <Text style={{ color: "white" }}>{labels}</Text>
          </TouchableOpacity>
        </View>
      </View>
    </View>
  )
}

export default BottomButtonNext
