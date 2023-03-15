import { createStackNavigator } from "@react-navigation/stack"
import React from "react"

import * as Screens from "../screens"
import Routes from "./Routes"
import Main from "./tab-navigator"

const Stack = createStackNavigator()

const AppStack = () => (
  <Stack.Navigator headerMode="none" screenOptions={{ gestureEnabled: false }}>
    <Stack.Screen name={Routes.HOME} component={Screens.Login} />
    <Stack.Screen name={Routes.MAIN} component={Main} />
  </Stack.Navigator>
)

export default AppStack
