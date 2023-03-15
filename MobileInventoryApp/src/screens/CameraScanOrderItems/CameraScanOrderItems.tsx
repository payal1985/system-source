import React, { useEffect, useRef, useState } from "react"
import { View, Text } from "react-native"
import { RNCamera } from "react-native-camera"
import { BaseHeader } from "/components"
import styles from './styles'
import BarcodeMask from 'react-native-barcode-mask';
import { get } from "lodash"
import Routes from "/navigators/Routes"
import { useNavigation, useRoute } from "@react-navigation/native"

type Navigate = {
  key: string
  name: string
  params: any
  merge: boolean
}
function CameraScanOrderItems({ }) {
  const { navigate, goBack } = useNavigation()
  const camera = useRef<RNCamera>(null)
  const route = useRoute()
  // const dataOrderItems = get(route, "params.dataOrderItems")
  const [dataOrderItems, setdataOrderItems] = useState(get(route, "params.dataOrderItems"))
  const [nameOrderItem, setnameOrderItem] = useState("")

  useEffect(() => {
    // onCodeScan()
  }, [])

  const handleDone = () => {
    // navigate(Routes.ORDER_ITEM_LIST_SCREEN, { dataOrderItems:dataOrderItems , isNotGoBack:true })
    navigate({
      key: "ORDER_ITEM_LIST_SCREEN",
      name: Routes.ORDER_ITEM_LIST_SCREEN,
      params: { dataOrderItems: dataOrderItems, goBackFromScan: true },

    })
  }

  const handleBack = () => {
    goBack()
  }
  const onCodeScan = (e: any) => {
    let conditionString = e.data
    // let conditionString = "https://ssi.com/id=2"
    let arrRaw = dataOrderItems
    if (conditionString?.includes("https://ssi.com")) {
      let parts = conditionString.split('=', 2);
      let orderItemID = parts[1]
      for (let i = 0; i < arrRaw.length; i++) {
        if (arrRaw[i].orderItemID == orderItemID) {
          arrRaw[i].matchScan = true
          setnameOrderItem(arrRaw[i].orderItemID)
        }
      }
      setdataOrderItems(arrRaw)
      // navigate(Routes.ORDER_ITEM_LIST_SCREEN, { dataOrderItems:dataOrderItems , goBackFromScan:true })
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
        // barCodeTypes={arrayType}
        // onGoogleVisionBarcodesDetected={(e) => onCodeScan(e) }
        // googleVisionBarcodeType={RNCamera.Constants.GoogleVisionBarcodeDetection.BarcodeType.CODE_128}
        // barCodeTypes={[RNCamera.Constants.BarCodeType.code128]}
        onBarCodeRead={(e) => onCodeScan(e)}
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
        }}>
        <View style={styles.container}>
          <BaseHeader
            titleHeader={"Barcode scan"}
            leftIconName={"flash-outline"}
            typeIcon={"help"}
            isGoBack={true}
            textCancel={true}
            onClosed={() => handleBack()}
            // onPressButton={() => onCodeScan()}
            onPressButton={() => handleDone()}
          />


          <View style={{ flex: 6 }}>
            {
              nameOrderItem ?
                <View style={{ margin: 20, justifyContent: 'center', alignItems: 'center', flexDirection: 'row' }}>
                  <Text style={{ color: 'white', fontSize: 20 }}>
                    {`Congratulations on a successful scan, Order Item ID ${nameOrderItem}`}
                  </Text>
                </View> : null
            }

            <BarcodeMask />
          </View>

        </View>
      </RNCamera>

    </View>
  )
}

export default CameraScanOrderItems
