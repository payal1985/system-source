import { NavigationContainer } from "@react-navigation/native"
import { createStackNavigator } from "@react-navigation/stack"
import React from "react"

import * as Screens from "../screens"
import { navigationRef } from "./_root_navigator"
import AuthStack from "./auth-navigator"
import Routes from "./Routes"
import Main from "./tab-navigator"
import LoadingProgress from "/components/LoadingView/LoadingProgress"

const getActiveRouteName: Function = (state: any) => {
  if (state) {
    const route = state.routes[state.index]
    if (route.state) {
      return getActiveRouteName(route.state)
    }
    return route.name
  }
}

const screenTracking = (state: any) => {
  const currentRouteName = getActiveRouteName(state)
  // eslint-disable-next-line no-console
  console.log("-------CURRENT_ROUTE_NAME------->", currentRouteName)
}

const Stack = createStackNavigator()

const forFade = ({ current }: { current: any }) => ({
  cardStyle: {
    opacity: current.progress,
  },
})

const AppNavigator = () => (
  <NavigationContainer ref={navigationRef} onStateChange={screenTracking}>
    <Stack.Navigator headerMode="none" screenOptions={{ gestureEnabled: false }}>
      <Stack.Screen name={Routes.AUTH} component={AuthStack} />
      {/* <Stack.Screen name={Routes.MAIN} component={Main} /> */}
      <Stack.Screen name={Routes.MORE_SCREEN} component={Screens.MoreScreen} />
      <Stack.Screen name={Routes.INVENTORY_DETAIL_SCREEN} component={Screens.InventoryDetailScreen} />
      <Stack.Screen name={Routes.INVENTORY_RE_LOCATE_SCREEN} component={Screens.InventoryReLocateScreen} />
      <Stack.Screen name={Routes.CAMERA} component={Screens.Camera} />
      <Stack.Screen name={Routes.START_INVENTORY_SCREEN} component={Screens.StartInventoryScreen} />
      <Stack.Screen name={Routes.HOME_SCREEN} component={Screens.HomeScreen} />
      <Stack.Screen name={Routes.INVENTORY_DETAIL_LIST_SCREEN} component={Screens.InventoryDetailListScreen} />
      <Stack.Screen name={Routes.COMPLETED_SCREEN} component={Screens.CompletedScreen} />
      <Stack.Screen name={Routes.SUBMISSION_SCREEN} component={Screens.SubmissionScreen} />
      <Stack.Screen name={Routes.CAMERA_COMPONENT} component={Screens.CameraComponent} />
      <Stack.Screen name={Routes.CAMERA_BARCODE} component={Screens.CameraBarCode} />
      <Stack.Screen name={Routes.CAMERA_CONDITIONS} component={Screens.CameraConditions} />
      <Stack.Screen name={Routes.CAMERA_ADD_MORE_CONDITIONS} component={Screens.CameraAddMoreConditions} />
      <Stack.Screen name={Routes.SCANCODE_DETAIL_SCREEN} component={Screens.ScanCodeDetailScreen} />
      <Stack.Screen name={Routes.UPDATE_DETAIL_INVENTORY_ITEM_SCREEN} component={Screens.UpdateInventoryItemScreen} />
      <Stack.Screen
        name={Routes.LOADING_PROGRESS}
        component={LoadingProgress}
        options={{
          cardStyleInterpolator: forFade,
          cardStyle: { backgroundColor: "transparent" },
        }}
      />
      <Stack.Screen name={Routes.SEARCH_IMAGE_SCREEN} component={Screens.SearchImageScreen} />
      <Stack.Screen name={Routes.UPDATE_INVENTORY_SCREEN} component={Screens.UpdateInventoryScreen} />
      <Stack.Screen name={Routes.UPDATE_DETAIL_INVENTORY_SCREEN} component={Screens.UpdateDetailInventoryScreen} />
      <Stack.Screen name={Routes.ORDER_SCREEN} component={Screens.OrderScreen} />
      <Stack.Screen name={Routes.ORDER_ITEM_LIST_SCREEN} component={Screens.OrderItemListScreen} />
      <Stack.Screen name={Routes.CAMERA_SCAN_ORDER_ITEMS} component={Screens.CameraScanOrderItems} />
    </Stack.Navigator>
  </NavigationContainer>
)

export default AppNavigator
