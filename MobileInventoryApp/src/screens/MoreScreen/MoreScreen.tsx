import React from "react"
import { ScrollView, View } from "react-native"

import { isIPhoneX, responsiveH, responsiveW } from "../../utils/dimension"
const MoreScreen = () => {
  return (
    <View
      style={{
        flex: 1,
        backgroundColor: "#f5f5f5",
      }}
    >
      <ScrollView
        showsVerticalScrollIndicator={false}
        style={{
          backgroundColor: "#F5F5F5",
          borderTopRightRadius: 20,
          borderTopLeftRadius: 20,
          borderColor: "#f5f5f5",
          flex: 1,
          top: responsiveH(-15),
          paddingHorizontal: responsiveW(20),
          paddingVertical: responsiveH(20),
          marginBottom: isIPhoneX ? responsiveH(0) : responsiveH(-30),
        }}
      ></ScrollView>
    </View>
  )
}

export default MoreScreen
