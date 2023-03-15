import React, { ReactElement, useCallback, useEffect, useState } from "react"
import { StyleSheet, Text, View } from "react-native"
import NetInfo, { NetInfoState } from "@react-native-community/netinfo"

import Colors from "/configs/Colors"
import { NetworkNotifierContext, NetworkNotifierListener } from "./NetworkNotifierContext"
import { getScreenWidth } from "/utils"
import Routes from "/navigators/Routes"
import { navigationRef } from "/navigators/_root_navigator"

interface Props {
  children: ReactElement
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  network: {
    position: "absolute",
    backgroundColor: Colors.opacityBlack,
    top: 0,
    bottom: 0,
    right: 0,
    left: 0,
    justifyContent: "center",
    alignItems: "center",
  },
  text: {
    color: Colors.black,
    textAlign: "center",
    paddingHorizontal: 20,
    width: getScreenWidth(1) * 0.9,
    backgroundColor: "#FFF",
    paddingVertical: 30,
    borderRadius: 10,
  },
})

const NetworkNotifier = (props: Props) => {
  const [connected, setConnect] = useState<boolean>(true)
  const [visible, setVisible] = useState<boolean>(true)
  const listeners: Array<NetworkNotifierListener> = []

  /**
   * Listener network change
   */
  const netInfoSubscription = useCallback(
    (state: NetInfoState) => {
      if (connected !== state.isConnected) {
        setConnect(state.isConnected)

        if (navigationRef?.current?.getCurrentRoute()?.name === "Login") {
        } else if (navigationRef?.current?.getCurrentRoute()?.name === "Splash") {
          navigationRef.current?.navigate(Routes.AUTH)
        } else {
          navigationRef.current?.navigate(Routes.HOME_SCREEN, { screen: Routes.HOME })
        }
      }

      // Notify all listeners
      for (const listener of listeners) {
        listener(state.isConnected, state)
      }
    },
    [connected, listeners],
  )

  useEffect(() => {
    const unsubscribe = NetInfo.addEventListener(netInfoSubscription)

    return () => unsubscribe()
  }, [netInfoSubscription])

  const renderNetwork = () => {
    return (
      <View style={styles.network}>
        <Text style={styles.text}>{"Not connected network. Please check again"}</Text>
      </View>
    )
  }

  /**
   * Add a network listener
   *
   * @param listener The listener to add
   */
  const addListener = (listener: NetworkNotifierListener) => {
    listeners.push(listener)
  }

  /**
   * Remove a network listener
   *
   * @param listener The listener to remove
   */
  const removeListener = (listener: NetworkNotifierListener) => {
    const index = listeners.indexOf(listener)
    if (index >= 0) {
      listeners.splice(index, 1)
    }
  }

  const showNotify = (state: boolean) => setVisible(state)

  return (
    <View style={styles.container}>
      <NetworkNotifierContext.Provider
        value={{
          addNetworkChangedListener: addListener,
          removeNetworkChangedListener: removeListener,
          showNotify: showNotify,
        }}
      >
        {props.children}
        {!connected && visible && renderNetwork()}
      </NetworkNotifierContext.Provider>
    </View>
  )
}

export default NetworkNotifier
