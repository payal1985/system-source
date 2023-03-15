import React, { useCallback, useEffect, useRef, useState } from "react"
import { Alert, Text, TouchableOpacity, View } from "react-native"
import { RNCamera, TakePictureResponse } from "react-native-camera"
import { URL_BASE, SEARCH_IMAGE_TYPE } from "/constant/Constant"
import axios from "axios"
import { BaseHeader } from "/components"
import useLanguage from "/utils/hook/useLanguage"
import MultiLanguage from "/utils/MultiLanguage"
import { TextInput } from "react-native-gesture-handler"
import { useSelector } from "react-redux"
import { AppReducerType } from "/redux/reducers"
import styles from './styles'
import _, { cloneDeep, isEmpty } from "lodash"
import useGlobalData from "/utils/hook/useGlobalData"
import LoadingOverlay from "/components/LoadingView/LoadingOverlay"

type CameraConditionsProps = {
  open?: boolean
  onClosed?: () => void
  flex?: number
  options?: object
  title?: string
  onSelect?: (item: any) => void
  canSearch?: any
  type?: string
  addMore?: any
  InventoryID?: any
  inventoryItemID?: any
  conditionID?: any
  itemTypeId?: string
  conditionRender?: number
  parentRowID?: any
}

const CameraUpdateInventory = ({ InventoryID, conditionID, inventoryItemID, onClosed, onSelect, type, conditionRender, parentRowID, title, arrCheckQuatity }: CameraConditionsProps) => {
  const [language] = useLanguage()
  const camera = useRef<RNCamera>(null)
  const [txtQuality, settxtQuality] = useState<any>("")
  const [conditionTakePic, setconditionTakePic] = useState(false)
  const [disableTxt, setdisableTxt] = useState(true)
  const [arrImg, setarrImg] = useState<(TakePictureResponse | undefined)[]>([])
  const [arrImgChose] = useState<{ url?: any; itemName?: any }[]>([])
  const [choseItem, setchoseItem] = useState<{ url?: any; itemName?: any }>({})
  const [itemChose, setitemChose] = useState(0)
  const [countImg, setCountImg] = useState<any[]>([])
  const userLogin = useSelector((state: any) => state?.[AppReducerType.TOKEN])
  const [countImgServer, setCountImgServer] = useState<any>(0)
  const [dataResImg, setdataResImg] = useState<any>([])
  const [loading, setloading] = useState<any>(false)
  const [globalData] = useGlobalData()
  const clientID = globalData.item[0]?.clientId
  const [hideBtn, setHideBtn] = useState<boolean>(false)
  const [totalImg, setTotalImg] = useState<number>(0)
  const [initHideDone, setInitHideDone] = useState<boolean>(false)

  const checkHideBtnFunc = () => {
    const indexFind = arrCheckQuatity?.findIndex((it: any) => it?.conditionID === conditionID)
    if (indexFind !== -1) {
      const photosCount = type === 'InventoryItemId' ? arrCheckQuatity?.[indexFind]?.conditionData?.[0]?.url?.length : arrCheckQuatity?.[indexFind]?.representativePhotos?.[0]?.url?.length
      setTotalImg(photosCount)
      setInitHideDone(photosCount >= 5)
    }
  }

  useEffect(() => {
    if (totalImg >= 5 || (type === 'InventoryItemId' && (conditionID === 2 || conditionID === 3 || conditionID === 4))) {
      setHideBtn(true)
    }
  }, [totalImg, conditionID])

  useEffect(() => {
    checkHideBtnFunc()
  }, [arrCheckQuatity])

  const takePicture = async () => {
    if (camera) {
      const optionsPhoto = {
        quality: 0.3,
      }
      const res = await camera.current?.takePictureAsync(optionsPhoto)
      setloading(true)
      let uri = res?.uri || ""
      let name: string = uri.slice(-20)
      res!.name = name
      res!.TempPhotoName = name
      // uploadRequest(res)

      uploadRequest(res)
      // let arrConcatCache = []
      // if (type == "Good" || type == "Fair" || type == "Poor") {
      //   if (arrImg.length < 5) {
      //     arrImg.push(res)
      //   } else {
      //     setconditionTakePic(false)
      //   }
      //   choseItem.url = arrImg
      // } else if (type == "Damaged" || type == "MissingParts") {
      //   if (arrImg.length < 5) {
      //     arrConcatCache = choseItem?.url?.concat(res)
      //   } else {
      //     setconditionTakePic(false)
      //   }
      //   choseItem.url = arrConcatCache
      //
      // }
      // if (arrConcatCache.length >= 5 || ((countImgServer + arrConcatCache.length) >= 5)) {
      //   setconditionTakePic(false)
      // }
      // if (arrImg?.length >= 5) {
      //   setconditionTakePic(false)
      // }
      // setCountImg((preState: any) => {
      //   const checkIndex = preState?.findIndex((it: any) => it?.itemName == choseItem?.itemName)
      //   if (checkIndex !== -1 && checkIndex != undefined) {
      //     preState?.splice(checkIndex, 1, choseItem)
      //   } else {
      //     preState?.push(choseItem)
      //   }
      //   const cloneDeepPreState = cloneDeep(preState)
      //   return cloneDeepPreState
      // })
    }
  }
  const uploadRequest = (res: any) => {
    let url = `${URL_BASE}/api/InventoryImages/Add`
    const formData = new FormData()
    let ur1 = {
      uri: res.uri,
      type: "image/jpg",
      name: `${res.name}`,
    }

    formData.append("Photo", ur1)
    formData.append("ClientId", clientID)
    formData.append("InventoryId", InventoryID)
    formData.append("InventoryItemId", inventoryItemID || "")
    formData.append("ConditionId", conditionID || "")
    formData.append("Width", res.width)
    formData.append("Height", res.height)
    axios({
      url: url,
      method: "POST",
      data: formData,
      headers: {
        "Content-Type": "multipart/form-data",
        Authorization: `Bearer ${userLogin?.tokenString}`,
      },
    })
      .then((res) => {
        console.log("ress image", res)
        if (res.status == 200) {
          setloading(false)
          setTotalImg((preState: any) => preState + 1)
          setdataResImg((preState: any) => {
            preState?.push(res?.data)
            return [...preState]
          })
        }
      })
      .catch((error) => {
        setloading(false)
        Alert.alert(`Api error (${error?.status || 0})`)
        console.log("errror", error.response)
      })
  }


  const handleDone = (data: any) => {
    if (!isEmpty(dataResImg)) {
      onSelect && onSelect(dataResImg)
    } else {
      showWarningRequireData()
    }
  }
  const showWarningRequireData = () => {
    Alert.alert("Caution", "Please take a photo", [
      {
        text: "OK",
        onPress: () => console.log("Cancel Pressed"),
        style: "cancel",
      },
    ])
  }

  const showWarningEnoughTakeImg = () => {
    Alert.alert("Caution", "Need at least one photo for each item", [
      {
        text: "OK",
        onPress: () => console.log("Cancel Pressed"),
        style: "cancel",
      },
    ])
  }

  const onSelectArrImg = () => {
    for (let i = 0; i < arrImgChose.length; i++) {
      if (choseItem.itemName == arrImgChose[i].itemName) {
        arrImgChose[i] = choseItem
      }
    }
    let data = {
      nameCondition: `${type}`,
      dataCondition: arrImgChose,
      type: type,
      txtQuantity: txtQuality,
    }
    handleDone(data)
  }
  // const handleChoseItem = (item: any, index: any) => {
  //   if (arrCheckQuatity.includes(index)) {
  //     const newIds = arrCheckQuatity.filter((id) => id !== index)
  //     setarrCheckQuatity(newIds)
  //   } else {
  //     const newIds = [...arrCheckQuatity]
  //     newIds.push(index)
  //     setarrCheckQuatity(newIds)
  //   }

  //   if (choseItem) {
  //     for (let i = 0; i < arrImgChose.length; i++) {
  //       if (choseItem.itemName == arrImgChose[i].itemName) {
  //         arrImgChose[i] = choseItem
  //       }
  //     }
  //     setchoseItem(item)
  //     setarrImg([])
  //   } else {
  //     setchoseItem(item)
  //   }
  //   countImgServer === 0 && setconditionTakePic(true) //fix bug 63
  //   setitemChose(index)
  // }
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




  return (
    <View style={styles.container}>
      <LoadingOverlay visible={loading} />

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
        }}>
        <View style={styles.container}>
          <BaseHeader
            titleHeader={`${title} (${totalImg > 5 ? 5 : totalImg}/5)`}
            leftIconName={"flash-outline"}
            typeIcon={"help"}
            isGoBack={true}
            textCancel={true}
            onClosed={onClosed}
            onPressButton={() => onSelectArrImg()}
            txtQuality={txtQuality}
            hideBtn={initHideDone}
          />
          <View style={{ flex: 7 }}>
            {/*<View style={{ flex: 0.8, backgroundColor: "#fff" }}>*/}
            {/*<View style={styles.btnSearch}>*/}

            {/*</View>*/}
            {/*</View>*/}
            <View style={{ flexDirection: "row", flexWrap: "wrap", alignItems: "flex-start", marginHorizontal: 20 }}>

            </View>
            <View style={{ flex: 7.5 }}>
              <View style={{ flex: 1.5, justifyContent: "center", alignItems: "center" }} />
              <View style={{ flex: 1, justifyContent: "center", alignItems: "center" }} />
            </View>
            <View style={{ flex: 1.5 }}></View>
          </View>

          <View style={{ flexDirection: "row", flex: 1, backgroundColor: "rgba(1,1,1,0.4)" }}>
            <View style={{ flex: 3, justifyContent: "center", paddingLeft: "5%" }} />

            {!hideBtn && <View style={{ flex: 4, justifyContent: "center", alignItems: "center", paddingRight: "5%" }}>
              <TouchableOpacity onPress={takePicture} style={styles.capture}>
                <View style={styles.button} />
              </TouchableOpacity>
            </View>}
            <View style={{ flex: 3 }}></View>
          </View>
        </View>
      </RNCamera>
    </View>
  )
}

export default CameraUpdateInventory
