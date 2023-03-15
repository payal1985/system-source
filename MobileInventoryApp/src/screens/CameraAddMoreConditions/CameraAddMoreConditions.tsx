import React, { useCallback, useEffect, useRef, useState } from "react"
import { Alert, Text, TouchableOpacity, View } from "react-native"
import { RNCamera, TakePictureOptions } from "react-native-camera"
import { BaseHeader } from "/components"
import { TextInput } from "react-native-gesture-handler"
import { BaseBottomSheetCameraAddMoreConditionProps } from "/components/BaseBottomSheetCameraAddMoreCondition/types"
import styles from "./styles"
import _, { cloneDeep, isEmpty } from 'lodash'
interface TakePictureResponse {
  width?: number
  height?: number
  uri?: string
  base64?: string
  exif?: { [name: string]: any }
  pictureOrientation?: number
  deviceOrientation?: number
  itemName?: string
  url?: object[]
}

const CameraAddMoreConditions = ({ onClosed, options: __options, onSelect, type, arrCheckQuantityServer, conditionData }: BaseBottomSheetCameraAddMoreConditionProps) => {
  const options = cloneDeep(__options)
  console.log('options', options)
  const camera = useRef<RNCamera>(null)
  const [conditionTakePic, setconditionTakePic] = useState(true)
  const [, setarrImg] = useState([])
  const [arrImgChose, setarrImgChose] = useState<TakePictureResponse[]>([])
  const [choseItem, setchoseItem] = useState<{ itemName: any } | null>(null)
  const [arrCheckQuatity, setarrCheckQuatity] = useState<any[]>([])
  const [itemChose, setitemChose] = useState(0)
  const [txtQuality, settxtQuality] = useState<string | any>("")
  const [oldQuatity, setoldQuatity] = useState<string | any>("")
  const [countImgServer, setCountImgServer] = useState<any>(0)
  const [countImg, setCountImg] = useState<any[]>([])
  const takePicture = async () => {
    if (camera) {
      const options: TakePictureOptions = {
        quality: 0.3,
        fixOrientation: true, // for Android
        orientation: "portrait",
      }
      const res = await camera.current?.takePictureAsync(options)
      let uri = res?.uri
      let name = uri?.slice(-15)
      res!.name = name

      for (let i = 0; i < arrImgChose.length; i++) {
        if (i == itemChose) {
          if (arrImgChose[i].url!.length < 5) {
            arrImgChose[i].url!.push(res)
          } else {
            setconditionTakePic(false)
          }
        }
      }
      const filterItem = arrImgChose?.filter((__it: any) => __it?.itemName === choseItem?.itemName)
      if (!isEmpty(filterItem) && filterItem?.[0]?.url?.length >= 5) {
        setconditionTakePic(false)
      }
      setCountImg((preState: any) => {
        const checkIndex = preState?.findIndex((it: any) => it?.itemName == choseItem?.itemName)
        if (checkIndex !== -1 && checkIndex != undefined) {
          preState?.splice(checkIndex, 1, choseItem)
        } else {
          preState?.push(choseItem)
        }
        const cloneDeepPreState = _.cloneDeep(preState)
        return cloneDeepPreState
      })
    }

  }

  const getIdByType = (__type: any) => {
    const filterByType = conditionData?.filter((__it: any) => __it?.conditionName === type)
    return _.isEmpty(filterByType) ? '' : filterByType?.[0]?.inventoryItemConditionId
  }
  useEffect(() => {
    setarrImgChose(options.dataCondition || options.conditionData)
    if (options.type == "Damaged" || options.type == "MissingParts") {
      if (options.dataCondition || options.conditionData) {
        let arrDataCondition = options.dataCondition || options.conditionData
        console.log('arrDataCondition', arrDataCondition)
        if (arrDataCondition[0]?.itemName == "All Items") {
          let num = parseInt(options.txtQuantity)
          settxtQuality(num.toString())
          setoldQuatity(num.toString())
        } else if (arrDataCondition.length > 1) {
          let num = options?.dataCondition?.length || options?.conditionData?.length
          settxtQuality(num.toString())
          setoldQuatity(num.toString())
        } else {
          let num = parseInt(options.txtQuantity) < 5 ? (options.dataCondition?.length || options.conditionData?.length) : options.txtQuantity
          settxtQuality(num.toString())
          setoldQuatity(num.toString())
        }
      }
    } else {
      settxtQuality(options.txtQuantity)
      setoldQuatity(options.txtQuantity)
    }
    setCountImg(options?.dataCondition || options?.conditionData)
    setchoseItem(options?.dataCondition?.[0] || options.conditionData?.[0])
  }, [])


  useEffect(() => {
    if (choseItem?.url?.length >= 5) {
      setconditionTakePic(false)
    }
  }, [choseItem])

  const onSelectArrImg = () => {
    options.dataCondition = arrImgChose
    options.txtQuantity = txtQuality
    onSelect(options)
  }
  const onSelectArrImgNotChange = () => {
    options.dataCondition = arrImgChose
    options.txtQuantity = oldQuatity
    onSelect(options)
  }
  const handleChoseItem = (item: any, index: number) => {
    if (arrCheckQuatity.includes(index)) {
      const newIds = arrCheckQuatity.filter((id) => id !== index)
      setarrCheckQuatity(newIds)
    } else {
      const newIds = [...arrCheckQuatity]
      newIds.push(index)
      setarrCheckQuatity(newIds)
    }

    if (choseItem) {
      for (let i = 0; i < arrImgChose.length; i++) {
        if (choseItem.itemName == arrImgChose[i].itemName) {
          arrImgChose[i] = choseItem
        }
      }
      setchoseItem(item)
      setarrImg([])
    } else {
      setchoseItem(item)
    }
    if (countImgServer > 0) {
      setconditionTakePic(false)
    } else {
      setconditionTakePic(true)

    }
    setitemChose(index)
  }
  const cancelSave = () => {
    settxtQuality(oldQuatity)
  }
  const conditionColor = (dataImg: any, index: number) => {
    if (itemChose == index) {
      return "#3399ff"
    } else {
      if (dataImg.url.length < 1) {
        return "white"
      } else {
        return "gray"
      }
    }
  }
  const checkTxtQuality = () => {
    checkQuantityServer(txtQuality)
    if (parseInt(txtQuality) < parseInt(oldQuatity) && parseInt(txtQuality) < 5) {
      if (options.type == "Damaged" || options.type == "MissingParts") {
        Alert.alert("", "Not allow to have less quantity. Please delete item in the main screen", [
          { text: "Close", onPress: () => onSelectArrImgNotChange() },
        ])
      }
    }
    if (parseInt(txtQuality) >= 5 && parseInt(oldQuatity) < 5 && options.type == "Damaged") {
      alertCautionSave()
    }
    if (parseInt(txtQuality) >= 5 && parseInt(oldQuatity) < 5 && options.type == "MissingParts") {
      alertCautionSave()
    }
    if (parseInt(txtQuality) < 5 && parseInt(txtQuality) > parseInt(oldQuatity)) {
      if (options.type == "Damaged" || options.type == "MissingParts") {
        if (parseInt(txtQuality) < 5) {
          let arr = arrImgChose
          let calcuPushItem = txtQuality - oldQuatity
          for (let i = 0; i < calcuPushItem; i++) {
            let lastDataImgStg = arrImgChose[arrImgChose.length - 1].itemName
            let stg = lastDataImgStg!.substr(lastDataImgStg!.length - 1)
            let data = { itemName: `Item${parseInt(stg) + 1}`, url: [] }
            arr.push(data)
          }
        } else {
          let arr = arrImgChose
          let data = { itemName: `All Items`, url: [] }
          arr.push(data)
        }
      }
    }
  }
  const alertCautionSave = () => {
    Alert.alert("", "Only save the first five pictures, the rest will be removed.  Are you sure?", [
      {
        text: "Cancel",
        onPress: () => cancelSave(),
        style: "cancel",
      },
      { text: "Proceed", onPress: () => handleMergeImage() },
    ])
  }
  const handleMergeImage = () => {
    let arrImgAllItem: [] = []
    for (let i = 0; i < arrImgChose.length; i++) {
      let arrUrl: [] = arrImgChose[i].url
      for (let j = 0; j < arrUrl!.length; j++) {
        if (arrImgAllItem.length < 5) {
          arrImgAllItem.push(arrUrl[j])
        }
      }
    }
    setarrImgChose([])
    let data = [{ itemName: `All Items`, url: arrImgAllItem }]
    onSelectArrImgEdit(data)
  }
  const onSelectArrImgEdit = (data: any) => {
    let dataRaw = {
      nameCondition: `${type}`,
      dataCondition: data,
      type: type,
      txtQuantity: txtQuality,
    }
    onSelect(dataRaw)
  }

  useEffect(() => {
    if (countImgServer > 0) {
      setconditionTakePic(false)
    }
  }, [countImgServer])


  useEffect(() => {
    if ((type === 'Damaged' || type === 'MissingParts')) return
    const filterArrCheckQuantity: any = arrCheckQuantityServer?.filter((it: any) => it?.conditionID === getIdByType(type))
    if (!_.isEmpty(filterArrCheckQuantity)) {
      setCountImgServer(filterArrCheckQuantity?.[0]?.representativePhotos?.[0]?.representativePhotoTotalCount || 0)
    }
  }, [arrCheckQuantityServer])

  const checkQuantityServer = useCallback((txtQuality: any) => {
    if (txtQuality < 5) return
    const filterArrCheckQuantity: any = arrCheckQuantityServer?.filter((it: any) => it?.conditionID === getIdByType(type))
    if (!_.isEmpty(filterArrCheckQuantity)) {
      setCountImgServer(filterArrCheckQuantity?.[0]?.representativePhotos?.[0]?.representativePhotoTotalCount || 0)
    }
  }, [arrCheckQuantityServer, type])

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
        <View style={styles.container}>
          <BaseHeader
            titleHeader={`${type} Items`}
            leftIconName={"flash-outline"}
            typeIcon={"help"}
            isGoBack={true}
            textCancel={true}
            onClosed={onClosed}
            onPressButton={() => onSelectArrImg()}
          />

          <View style={{ flex: 7 }}>
            {/*<View style={styles.viewSearch}>*/}
            {/*  <View style={styles.btnSearch}>*/}
            <TextInput
              style={[styles.txtCamera, { color: "gray", fontSize: 17 }]}
              value={txtQuality}
              onBlur={() => checkTxtQuality()}
              // keyboardType="numeric"
              placeholder={`Enter ${type} Quantity`}
              onChangeText={(txt) => settxtQuality(txt)}
            />
            {/*</View>*/}
            {/*</View>*/}
            <View style={styles.viewItemName}>
              {arrImgChose.map((dataImg: any, index: number) => {
                const indexCount = countImg?.findIndex((it: any) => it?.itemName === dataImg?.itemName)
                console.log('countImg', { countImg, indexCount, countImgServer })
                return (
                  <TouchableOpacity
                    onPress={() => handleChoseItem(dataImg, index)}
                    style={[styles.btnItemName, { backgroundColor: conditionColor(dataImg, index) }]}
                  >
                    <Text style={styles.txtItemName}>{`${dataImg?.itemName} (${(countImgServer + (countImg[indexCount]?.url?.length || 0))}/5)`}</Text>
                  </TouchableOpacity>
                )
              })}
            </View>
            <View style={{ flex: 7.5 }}>
              <View style={{ flex: 1.5, justifyContent: "center", alignItems: "center" }}></View>
              <View style={{ flex: 1, justifyContent: "center", alignItems: "center" }}></View>
            </View>
            <View style={{ flex: 1.5 }}></View>
          </View>

          <View style={{ flexDirection: "row", flex: 1, backgroundColor: "rgba(1,1,1,0.4)" }}>
            <View style={{ flex: 3, justifyContent: "center", paddingLeft: "5%" }}></View>

            <View style={{ flex: 4, justifyContent: "center", alignItems: "center", paddingRight: "5%" }}>
              {conditionTakePic && (
                <TouchableOpacity onPress={takePicture} style={styles.capture}>
                  <View style={styles.viewTakePicture}></View>
                </TouchableOpacity>
              )}
            </View>
            <View style={{ flex: 3 }}></View>
          </View>
        </View>
      </RNCamera>
    </View>
  )
}
export default CameraAddMoreConditions
