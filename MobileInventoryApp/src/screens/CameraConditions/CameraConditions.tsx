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
import styles from "./styles"
import _, { cloneDeep, isEmpty } from "lodash"

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
  itemTypeId?: string
  conditionRender?: number
  parentRowID?: any
  arrCheckQuantityServer?: any
  conditionData?: any
}

const CameraConditions = ({
  onClosed,
  onSelect,
  type,
  conditionRender,
  parentRowID,
  arrCheckQuantityServer,
  conditionData,
}: CameraConditionsProps) => {
  console.log('arrCheckQuantityServer', arrCheckQuantityServer)
  const [language] = useLanguage()
  const camera = useRef<RNCamera>(null)
  const [txtQuality, settxtQuality] = useState<any>("")
  const [conditionTakePic, setconditionTakePic] = useState(false)
  const [disableTxt, setdisableTxt] = useState(true)
  const [arrImg, setarrImg] = useState<(TakePictureResponse | undefined)[]>([])
  const [arrImgChose] = useState<{ url?: any; itemName?: any }[]>([])
  const [choseItem, setchoseItem] = useState<{ url?: any; itemName?: any }>({})
  const [arrCheckQuatity, setarrCheckQuatity] = useState<any[]>([])
  const [itemChose, setitemChose] = useState(0)
  const [countImg, setCountImg] = useState<any[]>([])
  const userLogin = useSelector((state: any) => state?.[AppReducerType.TOKEN])
  const [countImgServer, setCountImgServer] = useState<any>(0)
  const refConditionTakePic = useRef(false)

  useEffect(() => {
    console.log("choseItem", choseItem)
    if (choseItem?.url?.length >= 5) {
      setconditionTakePic(false)
      refConditionTakePic.current = false
    }
  }, [{ ...choseItem }])

  const takePicture = async () => {
    if (camera && refConditionTakePic.current) {
      const optionsPhoto = {
        quality: 0.3,
      }
      const res = await camera.current?.takePictureAsync(optionsPhoto)
      let uri = res?.uri || ""
      let name: string = uri.slice(-20)
      res!.name = name
      res!.TempPhotoName = name
      await uploadRequest(res)
      let arrConcatCache: any = []
      if (type == "Good" || type == "Fair" || type == "Poor") {
        if (arrImg.length < 5) {
          arrImg.push(res)
        } else {
          setconditionTakePic(false)
          refConditionTakePic.current = false
        }
        choseItem.url = arrImg
      } else if (type == "Damaged" || type == "MissingParts") {
        if (arrImg.length < 5) {
          arrConcatCache = await choseItem?.url?.concat(res)
        } else {
          setconditionTakePic(false)
          refConditionTakePic.current = false
        }
        choseItem.url = arrConcatCache
      }
      if (arrConcatCache?.length >= 5 || countImgServer + arrConcatCache?.length >= 5) {
        setconditionTakePic(false)
        refConditionTakePic.current = false
      }
      if (arrImg?.length >= 5) {
        setconditionTakePic(false)
        refConditionTakePic.current = false
      }
      setCountImg((preState: any) => {
        const checkIndex = preState?.findIndex((it: any) => it?.itemName == choseItem?.itemName)
        if (checkIndex !== -1 && checkIndex != undefined) {
          preState?.splice(checkIndex, 1, choseItem)
        } else {
          preState?.push(choseItem)
        }
        const cloneDeepPreState = cloneDeep(preState)
        return cloneDeepPreState
      })
      setchoseItem({ ...choseItem })
    }
  }

  const uploadRequest = (res: any) => {
    let url = `${URL_BASE}/api/ItemType/UploadTempPhotos`

    const formData = new FormData()
    let ur1 = {
      uri: res.uri,
      type: "image/jpg",
      name: `${res.name}`,
    }

    formData.append("photos", ur1)
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
      })
      .catch((error) => {
        console.log("errror", error.response)
      })
  }
  const checkNumber = () => {
    const numberParse = parseInt(txtQuality)
    settxtQuality(numberParse + "")
    if (!isNaN(txtQuality) && parseInt(txtQuality) > 0) {
      checkTxtQuality()
      checkQuantityServer(txtQuality)
    } else {
      Alert.alert(
        MultiLanguage("Only numbers are accepted", language.textStatic),
        "",
        [
          {
            text: MultiLanguage("OK", language.textStatic),
            onPress: () => { },
            style: "cancel",
          },
        ],
        { cancelable: false },
      )
    }
  }
  const checkTxtQuality = () => {
    if (txtQuality && arrImgChose.length < 1) {
      if (countImgServer === 0) {
        setconditionTakePic(true)
        refConditionTakePic.current = true
      }
      if (type == "Damaged" || type == "MissingParts") {
        if (parseInt(txtQuality) < 5) {
          let arr = arrImgChose

          for (let i = 0; i < txtQuality; i++) {
            let dataItem = { itemName: `Item${i + 1}`, url: [] }
            arr.push(dataItem)
          }
          handleChoseItem({ itemName: `Item1`, url: [] }, 0)
        } else {
          let arr = arrImgChose
          let data = { itemName: `All Items`, url: [] }
          arr.push(data)
          handleChoseItem(data, 0)
        }
      } else {
        let arr = arrImgChose
        let data = { itemName: `All Items`, url: [] }
        arr.push(data)
        handleChoseItem(data, 0)
      }
    }
    setdisableTxt(!disableTxt)
  }
  const handleDone = (data: any) => {
    let totalUrl = data?.dataCondition[0]?.url
    const checkSomeImgEmpty = data?.dataCondition?.some((it: any) => isEmpty(it?.url))
    if (checkSomeImgEmpty && (type === "Damaged" || type === "MissingParts") && data?.txtQuantity < 5) {
      showWarningEnoughTakeImg()
      return
    }
    if (!isEmpty(totalUrl) || countImgServer > 0) {
      // if (totalUrl?.length == 0 && conditionRender != SEARCH_IMAGE_TYPE.LOCAL && conditionRender != SEARCH_IMAGE_TYPE.SERVER ) {
      //   showWarningRequireData()
      // }
      if (totalUrl?.length === 0 && !parentRowID) {
        showWarningRequireData()
      } else {
        onSelect && onSelect(data)
      }
    } else {
      showWarningRequireData()
    }
  }
  const showWarningRequireData = () => {
    Alert.alert("Caution", "Need at least one picture", [
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
  const handleChoseItem = (item: any, index: any) => {
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
    if (countImgServer === 0) {
      setconditionTakePic(true)
      refConditionTakePic.current = true
    }
    setitemChose(index)
  }

  useEffect(() => {
    if (countImgServer > 0) {
      setconditionTakePic(false)
      refConditionTakePic.current = false
    }
  }, [countImgServer])

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
  const getIdByType = (__type: any) => {
    const filterByType = conditionData?.filter((__it: any) => __it?.conditionCode === type)
    return _.isEmpty(filterByType) ? "" : filterByType?.[0]?.inventoryItemConditionId
  }
  useEffect(() => {
    if (type === "Damaged" || type === "MissingParts") return
    const filterArrCheckQuantity: any = arrCheckQuantityServer?.filter(
      (it: any) => it?.conditionID === getIdByType(type),
    )
    if (!isEmpty(filterArrCheckQuantity)) {
      setCountImgServer(filterArrCheckQuantity?.[0]?.representativePhotos?.[0]?.representativePhotoTotalCount || 0)
    }
  }, [arrCheckQuantityServer])

  const checkQuantityServer = useCallback(
    (txtQuality: any) => {
      if (txtQuality < 5) return
      const filterArrCheckQuantity: any = arrCheckQuantityServer?.filter(
        (it: any) => it?.conditionID === getIdByType(type),
      )
      if (!isEmpty(filterArrCheckQuantity)) {
        setCountImgServer(filterArrCheckQuantity?.[0]?.representativePhotos?.[0]?.representativePhotoTotalCount || 0)
      }
    },
    [arrCheckQuantityServer, type],
  )

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
            txtQuality={txtQuality}
            hideBtn={isEmpty(arrImgChose)}
          />
          <View style={{ flex: 7 }}>
            {/*<View style={{ flex: 0.8, backgroundColor: "#fff" }}>*/}
            {/*<View style={styles.btnSearch}>*/}
            <TextInput
              style={[styles.txtCamera, { color: "gray", fontSize: 17 }]}
              value={txtQuality}
              editable={disableTxt}
              onBlur={() => checkNumber()}
              keyboardType="number-pad"
              returnKeyType="done"
              placeholder={`Enter ${type} Quantity`}
              onChangeText={(txt) => settxtQuality(txt)}
            />
            {/*</View>*/}
            {/*</View>*/}
            <View style={{ flexDirection: "row", flexWrap: "wrap", alignItems: "flex-start", marginHorizontal: 20 }}>
              {arrImgChose.map((dataImg: any, index: number) => {
                const indexCount = countImg?.findIndex((it: any) => it?.itemName === dataImg?.itemName)
                return (
                  <TouchableOpacity
                    onPress={() => handleChoseItem(dataImg, index)}
                    style={[styles.btnItemName, { backgroundColor: conditionColor(dataImg, index) }]}
                  >
                    <Text style={{ padding: 10, paddingHorizontal: 15, color: "black" }}>{`${dataImg.itemName} (${+countImgServer + (countImg[indexCount]?.url?.length || 0)
                      }/5)`}</Text>
                  </TouchableOpacity>
                )
              })}
            </View>
            <View style={{ flex: 7.5 }}>
              <View style={{ flex: 1.5, justifyContent: "center", alignItems: "center" }} />
              <View style={{ flex: 1, justifyContent: "center", alignItems: "center" }} />
            </View>
            <View style={{ flex: 1.5 }}></View>
          </View>

          <View style={{ flexDirection: "row", flex: 1, backgroundColor: "rgba(1,1,1,0.4)" }}>
            <View style={{ flex: 3, justifyContent: "center", paddingLeft: "5%" }} />

            <View style={{ flex: 4, justifyContent: "center", alignItems: "center", paddingRight: "5%" }}>
              {conditionTakePic && (
                <TouchableOpacity onPress={takePicture} style={styles.capture}>
                  <View style={styles.button} />
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

export default CameraConditions
