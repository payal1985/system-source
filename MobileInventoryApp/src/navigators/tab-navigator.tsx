import { createBottomTabNavigator } from "@react-navigation/bottom-tabs"
import React from "react"
import * as IMG from "/assets/images"
import useLanguage from "/utils/hook/useLanguage"

import { HomeScreen, InventoryDetailListScreen } from "../screens"
import BottomTabBar from "./bottomTabBar"
import Routes from "./Routes"
import { View, Text } from "react-native"

const Tab = createBottomTabNavigator()
export interface PropsTabBottom {
  name: string
  component: React.ComponentType<any>
  image: any
  displayName: string
}
export const TAB_BOTTOM = (textStatic: any) => {
  return [
    {
      displayName: "Home",
      component: HomeScreen,
      image: IMG.IC_WALLET,
      name: Routes.HOME_SCREEN,
    },

  ]
}

const TabComponent = (props: PropsTabBottom, index: number) => {
  return (
    <Tab.Screen
      key={index.toString()}
      name={props.name}
      component={props.component}
      options={{
        tabBarLabel: props.displayName,
        tabBarIcon: props.image,
      }}
    />
  )
}

const TabBottomNav = () => {
  const [language] = useLanguage()
  return (
    <Tab.Navigator initialRouteName={Routes.HOME_SCREEN} tabBar={(props) => <BottomTabBar {...props} />}>
      {TAB_BOTTOM(language.textStatic).map((item, index) => TabComponent(item, index))}
    </Tab.Navigator>
  )
}

export default TabBottomNav
