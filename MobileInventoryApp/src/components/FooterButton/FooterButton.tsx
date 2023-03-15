import React from "react"
import { View } from "react-native"
import { heightPercentageToDP } from "/utils"
import { Props } from "./types"

const FooterButton = ({ children, background, height }: Props) => {
  return (
    <View
      style={{
        alignItems: "center",
        height: height || heightPercentageToDP("10"),
        justifyContent: "center",
        backgroundColor: background || "#fff",
      }}
    >
      {children}
    </View>
  )
}

export default FooterButton
