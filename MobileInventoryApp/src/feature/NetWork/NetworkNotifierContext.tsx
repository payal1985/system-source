import { NetInfoState } from "@react-native-community/netinfo"
import React from "react"

export type NetworkNotifierListener = (isConnected: boolean, state: NetInfoState) => void

export interface NetworkNotifierContextType {
  addNetworkChangedListener: (listener: NetworkNotifierListener) => void
  removeNetworkChangedListener: (listener: NetworkNotifierListener) => void
  showNotify: (visible: boolean) => void
}

export const NetworkNotifierContext = React.createContext<NetworkNotifierContextType>({
  addNetworkChangedListener: () => {},
  removeNetworkChangedListener: () => {},
  showNotify: () => {},
})
