import React, { useState, useEffect } from "react"
import { View, Text, Platform, Modal, StyleSheet, TouchableOpacity, Dimensions, Image } from "react-native"
import CodePush from "react-native-code-push"
import { useSelector } from "react-redux"
import { AppReducerType } from "/redux/reducers"
import styles from "./styles"
import { LOGO_LOGIN } from "../../assets/images"
import * as Progress from "react-native-progress"
import Colors from "/configs/Colors"
import { DEPLOYMENT_KEY, DEPLOYMENT_KEY_ANDROID } from "/constant/Constant"

const CodePushDialog = () => {
  const globalData = useSelector((state: any) => state?.[AppReducerType.GLOBAL_DATA])
  const valueUpdateCodePush = globalData.codePushData
  const [titlePopup, setTitlePopup] = useState("")
  const [showPopup, setShowPopup] = useState(false)
  const [isCheckRequire, setIsCheckRequire] = useState(false)
  const [currentProgress, setCurrentProgress] = useState("0")
  const [showProgress, setShowProgress] = useState(false)
  const [infoUpdate, setInfoUpdate] = useState<{ appVersion: any; label: any }>({ appVersion: "", label: "" })
  const [isInstalled, setIsInstalled] = useState(false)
  const [error, setError] = useState(false)

  const showPopupFunc = (isRequire: any) => {
    setShowPopup(true)
    setIsCheckRequire(isRequire)
  }

  useEffect(() => {
    if (valueUpdateCodePush) {
      showPopupFunc(valueUpdateCodePush.isMandatory)
      const title = valueUpdateCodePush.description || 'Please press "update" for a better experience.'
      setTitlePopup(title)
      setInfoUpdate(valueUpdateCodePush)
    }
  }, [valueUpdateCodePush])

  const getCodePushFunc = async () => {
    setError(false)
    setShowProgress(true)
    setTitlePopup("Loading update please wait a moment...")
    CodePush.sync(
      Platform.select({ ios: DEPLOYMENT_KEY, android: DEPLOYMENT_KEY_ANDROID })
        ? { deploymentKey: Platform.select({ ios: DEPLOYMENT_KEY, android: DEPLOYMENT_KEY_ANDROID }) }
        : {},
      codePushStatusDidChange,
      codePushDownloadDidProgress,
    )
  }

  const codePushStatusDidChange = (syncStatus: any) => {
    switch (syncStatus) {
      case CodePush.SyncStatus.UPDATE_INSTALLED:
        CodePush.allowRestart()
        setIsInstalled(true)
        setTimeout(() => {
          CodePush.restartApp()
        }, 2500)
        break
      case CodePush.SyncStatus.UNKNOWN_ERROR:
        setError(true)
        setShowProgress(false)
        setTitlePopup("An error has occurred, please try again")
        break
      default:
        break
    }
  }

  const codePushDownloadDidProgress = (progress: any) => {
    const { receivedBytes, totalBytes } = progress
    const __currentProgress = parseFloat((receivedBytes / totalBytes).toString()).toFixed(1)
    setCurrentProgress(__currentProgress)
  }

  const closeModalFunc = () => {
    setShowPopup(false)
  }

  const title = isInstalled
    ? "The application will be restarted."
    : `Version ${infoUpdate?.appVersion} - ${infoUpdate?.label}`

  return (
    <Modal transparent visible={showPopup}>
      <View style={styles.container}>
        <View style={styles.body}>
          <View style={styles.view_title}>
            <Image source={LOGO_LOGIN} style={styles.img} resizeMode="contain" />
          </View>
          <View style={styles.view_info}>
            <Text style={styles.txtTitleInfo}>The application has a new version.</Text>
            <Text style={styles.text_info}>{titlePopup}</Text>
          </View>
          <View style={styles.view_action}>
            {isCheckRequire ? (
              <>
                {showProgress ? (
                  <View style={styles.containProgress}>
                    <Progress.Bar
                      progress={+currentProgress}
                      width={250}
                      height={10}
                      color={"#059669"}
                      unfilledColor={"#F3F4F6"}
                      borderWidth={0}
                      useNativeDriver
                    />
                  </View>
                ) : (
                  <View style={styles.view_button}>
                    <TouchableOpacity
                      onPress={getCodePushFunc}
                      style={[
                        styles.button,
                        {
                          backgroundColor: Colors.mainColor,
                          width: 240,
                        },
                      ]}
                    >
                      <Text style={styles.text_button}>{error ? "Try again" : "Updates"}</Text>
                    </TouchableOpacity>
                  </View>
                )}
              </>
            ) : (
              <>
                {showProgress ? (
                  <View style={styles.containProgress}>
                    <Progress.Bar
                      progress={+currentProgress}
                      width={250}
                      height={10}
                      color={"#059669"}
                      unfilledColor={"#F3F4F6"}
                      borderWidth={0}
                      useNativeDriver
                    />
                  </View>
                ) : (
                  <>
                    <View style={styles.view_button}>
                      <TouchableOpacity onPress={closeModalFunc} style={styles.button}>
                        <Text style={[styles.text_button, { color: Colors.mainColor }]}>Later</Text>
                      </TouchableOpacity>
                    </View>
                    <View style={styles.view_button}>
                      <TouchableOpacity
                        onPress={getCodePushFunc}
                        style={[styles.button, { backgroundColor: Colors.mainColor }]}
                      >
                        <Text style={styles.text_button}>{error ? "Try again" : "Update"}</Text>
                      </TouchableOpacity>
                    </View>
                  </>
                )}
              </>
            )}
          </View>
          {showProgress && <Text style={styles.txtInfo}>{title}</Text>}
        </View>
      </View>
    </Modal>
  )
}

export default CodePushDialog
