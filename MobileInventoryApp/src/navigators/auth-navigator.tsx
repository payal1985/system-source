import { createStackNavigator } from "@react-navigation/stack"
import React from "react"

import { Login } from "../screens"
import Routes from "./Routes"
import LoadingProgress from "/components/LoadingView/LoadingProgress"

const Stack = createStackNavigator()
const forFade = ({ current }: { current: any }) => ({
  cardStyle: {
    opacity: current.progress,
  },
})
const AuthStack = () => (
  <Stack.Navigator initialRouteName={Routes.LOGIN} headerMode="none" screenOptions={{ gestureEnabled: false }}>
    <Stack.Screen name={Routes.LOGIN} component={Login} />
    <Stack.Screen
      name={Routes.LOADING_PROGRESS}
      component={LoadingProgress}
      options={{
        cardStyleInterpolator: forFade,
        cardStyle: { backgroundColor: "transparent" },
      }}
    />
  </Stack.Navigator>
)

export default AuthStack
