import { useNavigation } from "@react-navigation/native"
import React, { useState } from "react"
import { Platform, StatusBar, Text, TouchableOpacity, View } from "react-native"
import Icon from "react-native-vector-icons/Ionicons"
import MaterialIcons from "react-native-vector-icons/MaterialIcons"

import Routes from "/navigators/Routes"
import { getScreenWidth, statusBarHeight } from "/utils/dimension"
import { isIPhoneX } from "/utils/dimension"
import Colors from "/configs/Colors"

import { BaseModal } from "/components"
import { Props } from "./types"

const BaseHeader = (props: Props) => {
  const {
    leftHandle,
    test,
    textCancel,
    onClosed,
    onPressButton,
    isGoBack,
    titleHeader,
    typeIcon,
    bottomExtend,
    autoOpen,
    leftIconName,
    hideBtn,
  } = props
  const { navigate, goBack } = useNavigation()
  const [modalAccount, setModalAccount] = useState(autoOpen || false)

  const leftPress = () => {
    if (isGoBack && test == false) {
      goBack()
    } else if (test) {
      navigate(Routes.HOME_SCREEN, { screen: Routes.HOME })
    } else if (textCancel) {
      onClosed && onClosed()
    } else navigate(Routes.HOME_SCREEN, { screen: Routes.HOME })
  }

  const handleDone = () => {
    onPressButton && onPressButton()
  }

  return (
    <View>
      <BaseModal open={modalAccount} flex={0.4} onClosed={() => setModalAccount(false)} />
      <StatusBar barStyle="dark-content" translucent backgroundColor="rgba(0,0,0,0)" />
      <View
        style={{
          width: getScreenWidth(1),
          height: isIPhoneX ? (bottomExtend ? 155 : 100) : bottomExtend ? 140 : 100,
          backgroundColor: Colors.mainColor,
        }}
      >
        <View
          style={{
            flexDirection: "row",
            justifyContent: "space-between",
            alignItems: "center",
            height: "100%",
            paddingHorizontal: 20,
            paddingTop: Platform.OS === "ios" ? statusBarHeight - 15 : statusBarHeight - 5,
            width: getScreenWidth(1),
          }}
        >
          {isGoBack ? (
            <TouchableOpacity onPress={() => leftPress()} style={{ zIndex: 5 }} activeOpacity={1}>
              {textCancel ? (
                <Text style={{ color: "#fff", fontSize: 17 }}>{"Cancel"}</Text>
              ) : isGoBack || leftHandle ? (
                <Icon name={leftIconName} size={25} style={{}} color="#FFF" />
              ) : (
                <MaterialIcons name={leftIconName} size={25} style={{}} color="#FFF" />
              )}
            </TouchableOpacity>
          ) : (
            <View style={{ width: 30, height: 30 }}></View>
          )}
          <View
            style={{
              alignItems: "center",
                flex:6
            }}
          >
            <Text style={{ color: "#FFFFFF", fontSize: 18, }} numberOfLines={2}>
              {titleHeader}
            </Text>
          </View>
          {!hideBtn ? <TouchableOpacity onPress={() => handleDone()}>
            {typeIcon == "help" ? (
              <Text style={{ color: "#fff", fontSize: 17 }}>Done</Text>
            ) : (
              <Icon name="headphones-alt" size={25} style={{}} color="white" onPress={() => setModalAccount(true)} />
            )}
          </TouchableOpacity> : <View />}
        </View>
      </View>
    </View>
  )
}

export default BaseHeader
