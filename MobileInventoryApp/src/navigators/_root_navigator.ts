import { NavigationContainerRef, StackActions } from "@react-navigation/native"
import * as React from "react"

export const navigationRef = React.createRef<NavigationContainerRef>()
export const isMountedRef = React.createRef<NavigationContainerRef>()

export function navigate(name: string, params?: any) {
  navigationRef.current?.navigate(name, params)
}

export function push(args: any) {
  navigationRef.current?.dispatch(StackActions.push(args))
}
export function goBack() {
  navigationRef.current?.goBack()
}

export function getCurrentScreen() {
  return navigationRef.current?.getCurrentRoute()?.name
}
