import {
  BottomTabDescriptorMap,
  BottomTabNavigationEventMap,
} from "@react-navigation/bottom-tabs/lib/typescript/src/types"
import { NavigationHelpers, ParamListBase, TabNavigationState } from "@react-navigation/native"
import React, { useEffect, useState } from "react"
import { Image, ImageSourcePropType, Keyboard, Platform, Text, TouchableOpacity, View } from "react-native"

import { responsiveH } from "/utils"
import useLanguage from "/utils/hook/useLanguage"

import styles from "./styles"

const BottomTabBar = ({
  state,
  descriptors,
  navigation,
}: {
  state: TabNavigationState<ParamListBase>
  descriptors: BottomTabDescriptorMap
  navigation: NavigationHelpers<ParamListBase, BottomTabNavigationEventMap>
}) => {
  const [language] = useLanguage()
  const [isOpen, setIsOpen] = useState<boolean>(false)
  const showModal = () => setIsOpen(true)

  const focusedOptions = descriptors[state.routes[state.index].key].options

  if (focusedOptions.tabBarVisible === false) {
    return null
  }
  const [showTab, setShowTab] = useState(true)

  const _keyboardDidShow = () => {
    setShowTab(false)
  }

  const _keyboardDidHide = () => {
    setShowTab(true)
  }

  useEffect(() => {
    Keyboard.addListener("keyboardDidShow", _keyboardDidShow)
    Keyboard.addListener("keyboardDidHide", _keyboardDidHide)

    return () => {
      Keyboard.removeListener("keyboardDidShow", _keyboardDidShow)
      Keyboard.removeListener("keyboardDidHide", _keyboardDidHide)
    }
  }, [])
  return (
    <View style={{ flexDirection: "row" }}>
      {showTab &&
        state.routes.map((route: any, index: Number) => {
          const { options } = descriptors[route.key]
          const label =
            options.tabBarLabel !== undefined
              ? options.tabBarLabel
              : options.title !== undefined
                ? options.title
                : route.name

          const isFocused = state.index === index

          const onPress = () => {
            const event = navigation.emit({
              type: "tabPress",
              target: route.key,
              canPreventDefault: true,
            })

            if (!isFocused && !event.defaultPrevented) {
              route.name === "Budget" ? showModal() : navigation.navigate(route.name)
            }
          }

          const onLongPress = () => {
            navigation.emit({
              type: "tabLongPress",
              target: route.key,
            })
          }

          return (
            <TouchableOpacity
              key={index.toString()}
              accessibilityRole="button"
              accessibilityState={isFocused ? { selected: true } : {}}
              accessibilityLabel={options.tabBarAccessibilityLabel}
              testID={options.tabBarTestID}
              onPress={onPress}
              onLongPress={onLongPress}
              style={{
                flex: 1,
                alignItems: "center",
                justifyContent: "center",
                backgroundColor: "white",
                paddingBottom: responsiveH(25),
                paddingTop: responsiveH(20),
              }}
            >
              <Image
                source={options.tabBarIcon as ImageSourcePropType}
                style={[styles.img, isFocused && { tintColor: "green" }]}
                resizeMode="contain"
              />
              <Text style={[styles.label, isFocused && styles.labelFocus]}>{label}</Text>
            </TouchableOpacity>

          )
        })}
    </View>
  )
}

export default BottomTabBar
