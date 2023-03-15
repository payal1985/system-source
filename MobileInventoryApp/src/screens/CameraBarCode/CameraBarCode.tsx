import React, { useEffect, useRef, useState } from "react"
import { TouchableOpacity, View } from "react-native"
import { RNCamera } from "react-native-camera"
import { BaseAlert, BaseHeader } from "/components"
import styles from './styles'
import BarcodeMask from 'react-native-barcode-mask';
import { get } from "lodash"
import Routes from "/navigators/Routes"
import { useFocusEffect, useNavigation, useRoute } from "@react-navigation/native"

type Navigate = {
  key: string
  name: string
  params: any
  merge: boolean
}
function CameraBarCode({ }) {
  const { navigate, goBack } = useNavigation()
  const { renderCamera, setrenderCamera } = useState(true)
  const camera = useRef<RNCamera>(null)
  const route = useRoute()

  const idClient = get(route, "params.idClient")
  const isListScan = get(route, "params.isListScan")

  const handleDone = () => {
    goBack()
  }

  const handleBack = () => {
    goBack()
  }
  const onCodeScan = (e) => {
    let conditionString = e.data
    if (conditionString?.includes("https://ssi.com")) {
      let parts = conditionString.split('=', 2);
      let inventoryItemId = parts[1]
      if (isListScan) {
        navigate(Routes.INVENTORY_RE_LOCATE_SCREEN, { inventoryItemId })
        return
      }
      const navigateSearchImage: Navigate = {
        key: "SCANCODE_DETAIL_SCREEN",
        name: Routes.SCANCODE_DETAIL_SCREEN,
        params: {
          imageInfo: conditionString,
          inventoryItemId,
          idClient, conditionRender: null, isEdit: false, parentRowID: null
        },
        merge: true,
      }
      navigate(navigateSearchImage)
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
        barCodeTypes={[RNCamera.Constants.BarCodeType.code128]}
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
            onPressButton={() => handleDone()}
          />
          <View style={{ flex: 7 }}>

            <BarcodeMask />
          </View>

        </View>
      </RNCamera>

    </View>
  )
}

export default CameraBarCode
