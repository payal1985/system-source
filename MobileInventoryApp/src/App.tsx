import "react-native-gesture-handler"
import React, { useEffect, useRef, useState } from "react"
import { Provider } from "react-redux"
import { PersistGate } from "redux-persist/integration/react"
import { navigationRef } from "/navigators/_root_navigator"
import { NetworkNotifier } from "./feature/NetWork"
import NotificationContext from "./feature/NotificationContext/NotificationContext"
import AppNavigator from "./navigators"
import Routes from "./navigators/Routes"
import { persistor, store } from "./redux/store"
import { closeSqlite, initSqlite } from "./utils/sqlite"
import { initTableInventoryClient } from "./utils/sqlite/tableInventoryClient"
import { getLocation } from "./utils/common"
import Storage from "./helper/Storage"
import LoadingStatePersistToStore from "./components/LoadingView/LoadingStatePersistToStore"
import { setTokenHeader } from "./api"
import { get, isString } from "lodash"
import { AppReducerType } from "./redux/reducers"
import { LogBox, Platform, View } from "react-native"
import { handleRefreshToken } from "./api"
import CodePush from "react-native-code-push"
import { codePushActions } from "/redux/globalData/action"
import { DEPLOYMENT_KEY, DEPLOYMENT_KEY_ANDROID } from "./constant/Constant"
import { CodePushDialog } from "./components"
// global.XMLHttpRequest = global.originalXMLHttpRequest ? global.originalXMLHttpRequest : global.XMLHttpRequest

const timeRefreshToken = 3600 // seconds
const Root = () => {
  const [isOpen, setIsOpen] = useState<boolean>(false)
  const [errMessage, setErrMessage] = useState("")
  const [loadInit, setLoadInit] = useState(true)
  const checkCodePushSync = async () => {
    CodePush.disallowRestart()
    const deploymentKey = Platform.select({ ios: DEPLOYMENT_KEY, android: DEPLOYMENT_KEY_ANDROID })
    CodePush.checkForUpdate(deploymentKey).then((value) => {
      if (value && !value.failedInstall) {
        store.dispatch(codePushActions.request(value))
      }
    })
  }

  useEffect(() => {
    !__DEV__ && checkCodePushSync()
    getLocation()
    LogBox.ignoreAllLogs() //Ignore all log notifications
    const timeout = setTimeout(() => {
      handleNavigate()
      clearTimeout(timeout)
    }, 100)
    refreshTokenWhenNotTouch()
  }, [])
  let timeIntervalRefresh: any
  let count: number = 0
  const refreshTokenWhenNotTouch = () => {
    clearInterval(timeIntervalRefresh)
    timeIntervalRefresh = setInterval(async () => {
      if (count == 2) {
        clearInterval(timeIntervalRefresh)
        count = 0
        return true
      }
      const refreshTk = await Storage.get("@RefreshToken")
      const user: any = await Storage.get("@User")
      if (!!refreshTk) {
        count++
        const rs = await handleRefreshToken(refreshTk, JSON.parse(user))
        const { tokenString, refreshToken } = rs.data
        setTokenHeader(tokenString)
        await Promise.all([Storage.set("@RefreshToken", refreshToken), Storage.set("@TokenUser", tokenString)])
      }
    }, timeRefreshToken * 1000)
    return true
  }

  useEffect(() => {
    handleSqlite()
  }, [])

  const handleSqlite = async () => {
    const db = await initSqlite()
    if (db) {
      initTableInventoryClient(db)
    }
    return () => closeSqlite(db)
  }
  const tokenRef = useRef()
  const onStoreUpdate = () => {
    const stateAuth: any = get(store.getState(), [AppReducerType.TOKEN])
    if (navigationRef.current && tokenRef.current !== stateAuth?.tokenString) {
      tokenRef.current = stateAuth?.tokenString
      if (stateAuth && isString(stateAuth?.tokenString) && stateAuth?.tokenString?.trim() !== "") {
        navigationRef.current?.navigate(Routes.HOME_SCREEN)
      } else {
        navigationRef.current?.navigate(Routes.AUTH)
      }
    }
  }
  //trigger token
  useEffect(() => {
    const unsubscribe = store.subscribe(onStoreUpdate)
    return () => unsubscribe()
  }, [])

  useEffect(() => {
    !!errMessage && setIsOpen(true)
  }, [errMessage])

  const handleNavigate = async () => {
    if (navigationRef.current) {
      const token = await Storage.get("@TokenUser")
      if (token && token?.trim() !== "") {
        setTokenHeader(token)
        navigationRef.current?.navigate(Routes.HOME_SCREEN)
      }
      const timeout = setTimeout(() => {
        setLoadInit(false)
        clearTimeout(timeout)
      }, 100)
    } else {
      setLoadInit(false)
    }
  }
  return (
    <Provider store={store}>
      <View
        style={{ position: "absolute", left: 0, right: 0, top: 0, bottom: 0 }}
        onStartShouldSetResponder={refreshTokenWhenNotTouch}
      />
      <PersistGate loading={<LoadingStatePersistToStore />} persistor={persistor}>
        <NotificationContext>
          <NetworkNotifier>
            <AppNavigator />
          </NetworkNotifier>
        </NotificationContext>
      </PersistGate>
      {loadInit && <LoadingStatePersistToStore />}
      <CodePushDialog />
    </Provider>
  )
}

export default Root
