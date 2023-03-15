import { useNavigation } from "@react-navigation/native"
import React from "react"
import { StatusBar, Text, TouchableOpacity, View } from "react-native"
import Entypo from "react-native-vector-icons/Entypo"
import Ionicons from "react-native-vector-icons/Ionicons"
import Colors from "/configs/Colors"

import Routes from "/navigators/Routes"
import { getScreenWidth } from "/utils/dimension"
import { Props } from "./types"
import Icon from "react-native-vector-icons/MaterialIcons"

const Header = ({
  leftHandle,
  isGoBack,
  isSaved,
  handleSave,
  labels,
  isExtraHeight,
  conditionRender,
  actionGoBack,
}: Props) => {
  const { navigate, goBack } = useNavigation()

  const leftPress = () => {
    if (isGoBack) {
      actionGoBack ? actionGoBack() : goBack()
    } else if (leftHandle) {
      leftHandle()
    } else if (conditionRender == 2) {
      goBack()
    } else navigate(Routes.HOME_SCREEN, { screen: Routes.HOME })
  }
  const navigateHome = () => {
    navigate(Routes.HOME_SCREEN, { screen: Routes.HOME })
  }

  return (
    <View>
      <StatusBar barStyle="dark-content" translucent backgroundColor="rgba(0,0,0,0)" />
      <View
        style={{
          width: getScreenWidth(1),
          height: isExtraHeight ? 120 : 90,
          backgroundColor: Colors.mainColor,
          paddingTop: 10,
          borderBottomLeftRadius: 15,
          borderBottomRightRadius: 15,
        }}
      >
        <View
          style={{
            flexDirection: "row",
            flex: 1,
          }}
        >
          <TouchableOpacity
            onPress={leftPress}
            style={{ flex: 1.5, justifyContent: "center", marginTop: 20 }}
            activeOpacity={1}
          >
            {isGoBack ? (
              <Icon name={"chevron-left"} size={40} color={"#fff"} />
            ) : (
              <Entypo name="menu" size={30} color="#FFF" />
            )}
          </TouchableOpacity>
          <View style={{ flex: 6, justifyContent: "center", alignItems: "center" }}>
            <Text
              style={{
                fontWeight: "500",
                fontSize: 22,
                color: "#fff",
                marginTop: 15,
              }}
              maxFontSizeMultiplier={1}
              numberOfLines={1}
              children={labels}
            />
          </View>
          <TouchableOpacity
            onPress={() => {
              if (isSaved) {
                handleSave && handleSave()
              } else {
                navigateHome()
              }
            }}
            style={{ flex: 1.5, justifyContent: "center", alignItems: "center", marginTop: 20 }}
          >
            {isSaved ? (
              <Icon name={"save"} size={28} color={"#fff"} />
            ) : (
              <Ionicons name={"home"} size={24} color={"#fff"} />
            )}
          </TouchableOpacity>
        </View>
      </View>
    </View>
  )
}

export default Header
