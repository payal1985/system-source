import { useNavigation } from "@react-navigation/native"
import React, { useRef, useState } from "react"
import { TouchableOpacity, View } from "react-native"
import { RNCamera, TakePictureOptions } from "react-native-camera"
import { BaseHeader } from "/components"
import Routes from "/navigators/Routes"
import useGlobalData from "/utils/hook/useGlobalData"
import { TextInput } from "react-native-gesture-handler"
import styles from "./styles"

function Camera({}) {
  const { navigate } = useNavigation()
  const camera = useRef<RNCamera>(null)
  const [globalData] = useGlobalData()
  const [search, setSearch] = useState("")

  const takePicture = async () => {
    if (camera) {
      const options: TakePictureOptions = {
        quality: 0.9,
        fixOrientation: true, // for Android
        orientation: "portrait",
      }
      const res = await camera.current?.takePictureAsync(options)
    }
  }

  const handleOnPress = () => {
    let lastItem = globalData.item.length - 1
    let item = globalData.item[lastItem],
      arrRaw = { data: [{ name: search, dataChild: [] }] }
    if (item.hasOwnProperty("data")) {
      item.data.push({ name: search, dataChild: [] })
      navigate(Routes.INVENTORY_DETAIL_SCREEN)
    } else {
      let combine = { ...arrRaw, ...item }
      ;(globalData.item[lastItem] = combine), navigate(Routes.INVENTORY_DETAIL_SCREEN)
    }
  }
  return (
    <View style={styles.container}>
      <RNCamera
        ref={camera}
        type={RNCamera.Constants.Type.back}
        captureAudio={false}
        style={{ flex: 1 }}
        ratio={"16:9"}
        onRecordingStart={(e) => console.log("check", e)}
        androidCameraPermissionOptions={{
          title: "Permission to use camera",
          message: "We need your permission to use your camera",
          buttonPositive: "Ok",
          buttonNegative: "Cancel",
        }}
        androidRecordAudioPermissionOptions={{
          title: "Permission to use audio recording",
          message: "We need your permission to use your audio",
          buttonPositive: "Ok",
          buttonNegative: "Cancel",
        }}
      >
        <View style={{ flex: 1 }}>
          <BaseHeader
            titleHeader={"Barcode scan"}
            leftIconName={"flash-outline"}
            typeIcon={"help"}
            isGoBack={true}
            onPressButton={() => handleOnPress()}
          />
          <View style={{ flex: 7 }}>
            <View style={styles.viewSearch}>
              <View style={styles.btnSearch}>
                <TextInput
                  style={[styles.txtCamera, { color: "gray", fontSize: 18 }]}
                  value={search}
                  placeholder={"Tap to manually enter barcode"}
                  onChangeText={(txt) => setSearch(txt)}
                />
              </View>
            </View>
            <View style={{ flex: 8.5 }}>
              <View style={{ flex: 1.5, justifyContent: "center", alignItems: "center" }}></View>
              <View style={{ flex: 1, justifyContent: "center", alignItems: "center" }}></View>
            </View>
            <View style={{ flex: 1.5 }}></View>
          </View>

          <View style={{ flexDirection: "row", flex: 1, backgroundColor: "rgba(1,1,1,0.4)" }}>
            <View style={{ flex: 3, justifyContent: "center", paddingLeft: "5%" }}></View>

            <View style={{ flex: 4, justifyContent: "center", alignItems: "center", paddingRight: "5%" }}>
              <TouchableOpacity onPress={takePicture} style={styles.capture}>
                <View style={styles.viewTakePicture}></View>
              </TouchableOpacity>
            </View>
            <View style={{ flex: 3 }}></View>
          </View>
        </View>
      </RNCamera>
    </View>
  )
}
export default Camera
