import {useNavigation, useRoute, useFocusEffect} from "@react-navigation/native"

import React, {useCallback, useState, useEffect, useMemo, useRef} from "react"
import {Image, StyleSheet, Text, Alert, FlatList, TouchableOpacity, View} from "react-native"
import {TextInput} from "react-native-gesture-handler"
import {KeyboardAwareScrollView} from "react-native-keyboard-aware-scroll-view"
import Icon from "react-native-vector-icons/Ionicons"
import MaterialCommunityIcons from "react-native-vector-icons/MaterialCommunityIcons"
import AsyncStorage from "@react-native-community/async-storage"
import {SelectOption} from "/components/Select/types"
import Colors from "/configs/Colors"
import Routes from "/navigators/Routes"
import LoadingOverlay from "/components/LoadingView/LoadingOverlay"
import {
  BaseBottomSheet,
  BaseBottomSheetCamera,
  BaseBottomSheetCameraAddMoreCondition,
  BaseBottomSheetShowImage,
  BottomButtonNext,
  BaseAlert,
  Header,
} from "../../components"
import {fontSizer, responsiveH, responsiveW} from "../../utils/dimension"
import IconImage from "react-native-vector-icons/AntDesign"
import useGlobalData from "/utils/hook/useGlobalData"
import {CONDITION_QUANTITY_TYPE, URL_BASE, SEARCH_IMAGE_TYPE} from "/constant/Constant"
import {findIndex, forEach, get, isEqual, min, isEmpty, isNaN} from "lodash"
import {updateDataInventoryClientByQuery} from "/utils/sqlite/tableInventoryClient"
import {
  getItemTypeService,
  getItemOptionSetTypeService,
  getFloorsDataService,
  getBuildDataService,
} from "/redux/progress/service"
import MultiLanguage from "/utils/MultiLanguage"
import useLanguage from "/utils/hook/useLanguage"

type Photos =
  | {
  TempPhotoName: string
  height: number
  name: string
  width: string
  uri: string
  isAuto: string
}
  | undefined

type RenderItemTypeFeildProps = {
  item: any
  index: any
}

const InventoryDetailScreen = () => {
  const {navigate} = useNavigation()
  const [language] = useLanguage()
  const route = useRoute()
  const idClient = get(route, "params.idClient")
  const imageInfo = get(route, "params.imageInfo")
  const conditionRender = get(route, "params.conditionRender")
  const inventoryId = get(route, "params.inventoryId")
  const canEditData = get(route, "params.canEditData")
  const [localDataArray, setlocalDataArray] = useState([{
    "conditionRender": conditionRender,
    "canEditData": canEditData
  }])
  const [custClient, setcustClient] = useState("")
  const [custBuidingName, setcustBuidingName] = useState("")
  const [custFloor, setcustFloor] = useState("")
  const [custGPS, setcustGPS] = useState("")
  const [modalNumberCode, setModalNumberCode] = useState(false)

  const [globalData] = useGlobalData()

  const [Areanumber, setAreanumber] = useState("")
  const [itemTypeTxt, setitemTypeTxt] = useState("")
  const [titleDropDown, settitleDropDown] = useState("Options")
  const [dataItemType, setdataItemType] = useState<any>([])
  const [valuesTextInput, setvaluesTextInput] = useState([])
  const [conditionRenderTitle, setconditionRenderTitle] = useState(false)
  const [modalTypeCamera, setmodalTypeCamera] = useState(false)
  const [modalShowImage, setmodalShowImage] = useState(false)
  const [modalTypeCameraAddMore, setmodalTypeCameraAddMore] = useState(false)
  const [showImageModal, setshowImageModal] = useState<Photos>()
  const [arrCheck, setarrCheck] = useState<any[]>([])
  const [arrCheckQuatity, setarrCheckQuatity] = useState<{ type?: any }[]>([])
  const [dataImageItem, setdataImageItem] = useState({})
  const [dataImageRaw, setdataImageRaw] = useState(imageInfo)
  const [itemTypeArr, setitemTypeArr] = useState<{
    itemTypeOptions?: any
    clientID?: any
    mainImage?: any
    mainImageFull?: Photos
    conditionRender?: any
    inventoryRowID?: any
    parentRowID?: any
  }>({})
  const [itemTypeId, setitemTypeId] = useState("")
  const [conditionsType, setconditionsType] = useState("") //conditions pas camera screen
  const [isSave, setIsSave] = useState(false)
  const [loading, setLoading] = useState<boolean>(true)
  const [openSharedModal, setopenSharedModal] = useState<boolean>(false)
  const [dataSharedModal, setdataSharedModal] = useState<any[]>([])
  const [typeSharedModal, settypeSharedModal] = useState("")
  const [titleSharedModal, settitleSharedModal] = useState("")
  const [mainPhoto, setMainPhoto] = useState<Photos>()

  const [checkNV, setcheckNV] = useState<SelectOption[]>([])
  const [, setmodalArea] = useState(false)
  const [modalType, setmodalType] = useState(false)
  const [floor, setFloor] = useState<SelectOption[]>([])
  const [buildingData, setbuildingData] = useState<SelectOption[]>([])
  const [itemType, setitemType] = useState<SelectOption[]>([])
  const [disableChoseItemType, setdisableChoseItemType] = useState(true)
  const [itemTypeNew, setItemTypeNew] = useState<{ itemTypeID?: number; clientID?: number }>()
  const [canAddData, setcanAddData] = useState(false)
  const [valuesTextInputTextParagraph, setvaluesTextInputTextParagraph] = useState<string>("")
  const [dataImageInfo, setdataImageInfo] = useState([])
  const [flagCheckSetData, setflagCheckSetData] = useState(false)
  const [flagCheckResetQuantity, setflagCheckResetQuantity] = useState<any>(null)
  const [flagCheckUpNewImage, setflagCheckUpNewImage] = useState(false)
  const [flagDeleteMainPhoto, setflagDeleteMainPhoto] = useState(false)
  const [clientID, setclientID] = useState(globalData.item[0]?.clientId)
  const refHeight: any = useRef()
  const [colorHeight, setColorHeight] = useState<string>('')
  const [checkError, setCheckError] = useState<any>()
  const [errorRequire, setErrorRequire] = useState<any>([])


  useEffect(() => {
    console.log("localDataArray", localDataArray)
    setlocalDataArray()
    conditionRenderData()
  }, [])
  useEffect(() => {
    if (conditionRender == SEARCH_IMAGE_TYPE.LOCAL || conditionRender == SEARCH_IMAGE_TYPE.SERVER) { //set data search image local
      setdataImageInfo(JSON.stringify(imageInfo.itemTypeOptions))
    }
  }, [flagCheckSetData])

  const checkIsRequire = () => {
    setErrorRequire([])
    const dataFilterRequire = dataItemType?.filter((it: any) => it?.isRequired && it?.itemTypeOptionCode !== 'Building')
    const checkBlankFields = dataFilterRequire?.some((__it: any) =>
      __it?.itemTypeOptionReturnValue ? !__it?.itemTypeOptionReturnValue[0]?.returnValue : true
    )
    if (checkBlankFields) {
      forEach(dataFilterRequire, (__item) => {
        if (__item?.itemTypeOptionReturnValue && !__item?.itemTypeOptionReturnValue[0]?.returnValue) {
          setErrorRequire((preState: any[]) => preState?.concat(__item))
        } else if (!__item?.itemTypeOptionReturnValue) {
          setErrorRequire((preState: any[]) => preState?.concat(__item))
        }
      })
    }
    return checkBlankFields
  }
  const checkNumberWarning = () => {
    const dataFilterDecimal = dataItemType?.filter((it: any) => it?.fieldType === 'Decimal')
    const findIndexNaN = dataFilterDecimal?.findIndex((__it: any) => __it?.itemTypeOptionReturnValue ? isNaN(+__it?.itemTypeOptionReturnValue[0]?.returnValue) : false)
    if (findIndexNaN !== -1 && !isEmpty(dataFilterDecimal)) {
      Alert.alert('Warning', `The field type ${dataFilterDecimal[findIndexNaN]?.itemTypeOptionCode} have to be a number`)
      return true
    }
    return false
  }
  useEffect(() => {
    if (conditionRender == SEARCH_IMAGE_TYPE.LOCAL && flagCheckResetQuantity != null && flagCheckUpNewImage == false) {
      let dataItemTypeOptions = itemTypeArr?.itemTypeOptions
      let index = dataItemTypeOptions.find(x => x.itemTypeOptionCode === 'TotalCount')
      index.itemTypeOptionReturnValue = null
      setarrCheckQuatity([])
      setitemTypeArr(itemTypeArr)
    }
  }, [flagCheckResetQuantity])
  useEffect(() => {
    if (flagCheckResetQuantity) {
      setflagDeleteMainPhoto(true)
      setMainPhoto(null)
    }
  }, [flagCheckResetQuantity]);

  const conditionRenderData = () => {
    checkConditionRenderData()
    checkCanEditData()

  }
  const checkConditionRenderData = () => {
    if (conditionRender == 1) { //create new
      handleAddNewCase()
    } else if (conditionRender == SEARCH_IMAGE_TYPE.LOCAL || conditionRender == SEARCH_IMAGE_TYPE.SERVER) { // local
      handleSearchImageLocalCase()
    }
  }
  const checkCanEditData = () => {
    if (canEditData) { //edit
      console.log("edit data")
      handleEditCase()
    }
  }

  const handleAddNewCase = () => {
    handleGetDataItemType()
    handleGetFloorsDataService()
    handleGetBuildingDataService()
    getMultipleTitle()
  }
  const handleEditCase = () => {
    let conditionExistEkyc = globalData?.chosed[0] ? globalData?.chosed[0] : []
    console.log("conditionExistEkyc edit", conditionExistEkyc)
    onSelectItemTypeEditData(conditionExistEkyc)
    handleGetDataItemType()
    // getMultipleTitle()
    handleGetFloorsDataService()
    handleGetBuildingDataService()
    setData(globalData?.chosed[0])
  }
  const handleSearchImageLocalCase = () => {
    let conditionExistEkyc = imageInfo
    setflagCheckSetData(!flagCheckSetData)
    onSelectItemTypeEditData(conditionExistEkyc)
    handleGetDataItemType()
    getMultipleTitle() // get 4 data location
    handleGetFloorsDataService()
    handleGetBuildingDataService()
    setData(conditionExistEkyc)
    setItemTypeNew(conditionExistEkyc)


  }
  const setData = (data: any) => {
    let arr = data.itemTypeOptions
    for (let i = 0; i < arr.length; i++) {
      if (arr[i].valType == 2 && arr[i].itemTypeOptionReturnValue) {
        setvaluesTextInputTextParagraph(arr[i].itemTypeOptionReturnValue[0].returnValue)
      }
      if (arr[i].valType == 1 && arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == 'Building' && conditionRender != 1) {
        setcustBuidingName(arr[i].itemTypeOptionReturnValue[0].returnValue)
      }
      if (arr[i].valType == 3 && arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "Floor" && conditionRender != 1) {
        setcustFloor(arr[i]?.itemTypeOptionReturnValue[0]?.returnValue)
      }
      if (arr[i].valType == 30 && arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "GPS" && conditionRender != 1) {
        setcustGPS(arr[i].itemTypeOptionReturnValue[0].returnValue)
      }
      if (arr[i].valType == 1 && arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "AreaOrRoom" && conditionRender != 1) {
        setAreanumber(arr[i].itemTypeOptionReturnValue[0].returnValue)
      }
      if (arr[i].valType == 40 && arr[i]?.itemTypeOptionReturnValue?.length > 0) {
        setarrCheckQuatity(arr[i]?.itemTypeOptionReturnValue)
      }
      if (arr[i].valType == 90 && arr[i]?.itemTypeOptionReturnValue?.length > 0) {
        setcustClient(arr[i]?.itemTypeOptionReturnValue[0]?.returnValue.itemTypeOptionLineName)
      }
      if (arr[i].valType == 80 && arr[i]?.itemTypeOptionReturnValue.length > 0) {
        let arrChek = []
        let arrPick = arr[i]?.itemTypeOptionReturnValue
        let arrOptionLines = arr[i].itemTypeOptionLines
        for (let k = 0; k < arrOptionLines.length; k++) {
          for (let z = 0; z < arrPick.length; z++) {
            if (arrPick[z].returnValue?.itemTypeOptionLineID == arrOptionLines[k].itemTypeOptionLineID) {
              arrChek.push(k)
            }
          }
        }
        setarrCheck(arrChek)
      }
    }
  }
  const handleGetDataItemType = () => {
    let url = `/api/ItemType/GetItemTypes?clientId=${parseInt(clientID)}`
    getItemTypeService(
      url,
      (res) => {
        if (res.status == 200) {
          console.log('list of itemtype:', res.data)
          setitemType(res.data)
          setdisableChoseItemType(false)
        } else {
          BaseAlert(res, "Get DataItemType")
        }
      },
      (e) => {
        BaseAlert(e, "Get DataItemType")
      },
    )
  }
  const handleGetFloorsDataService = () => {
    getFloorsDataService(
      (res) => {
        if (res.status == 200) {
          setFloor(res.data)
        } else {
          setLoading(false)
          BaseAlert(res, "Get Floors")
        }
      },
      (e) => {
        setLoading(false)
        BaseAlert(e, "Get Floors")
      },
    )
  }
  const handleGetBuildingDataService = () => {
    let url = `/api/InventoryBuildings/GetBuildings?clientId=${parseInt(clientID)}`
    getBuildDataService(
      url,
      (res) => {
        if (res.status == 200) {
          setLoading(false)
          setbuildingData(res.data)
        } else {
          setLoading(false)
          BaseAlert(res, "Get Building")
        }
      },
      (e) => {
        setLoading(false)
        BaseAlert(e, "Get Building")
      },
    )
  }
  const handleGetItemOptionSetTypeService = (itemTypeId: any) => {
    let url = `${URL_BASE}/api/ItemType/GetItemTypeOptionSet?clientId=${parseInt(
      clientID,
    )}&itemId=${itemTypeId}`
    getItemOptionSetTypeService(
      url,
      (res) => {
        console.log("res", res)
        if (res.status == 200) {
          let data = res.data[0]
          setdataItemType(data.itemTypeOptions)
          setitemTypeArr(data)
          setLoading(false)
        } else {
          setLoading(false)
          BaseAlert(res, "Get ItemOptionsSetType")
        }
      },
      (e) => {
        setLoading(false)
        BaseAlert(e, "Get ItemOptionsSetType")
      },
    )
  }
  const getMultipleTitle = async () => {
    try {
      let custBuidingNameRaw = await AsyncStorage.getItem("@custBuidingName")
      let custFloorRaw = await AsyncStorage.getItem("@custFloor")
      let custGPSRaw = await AsyncStorage.getItem("@custGPS")
      let custAreaNumberRaw = await AsyncStorage.getItem("@custAreaNumber")
      if (custBuidingNameRaw) {
        setcustBuidingName(custBuidingNameRaw)
      }
      if (custFloorRaw) {
        setcustFloor(custFloorRaw)
      }
      if (custGPSRaw) {
        setcustGPS(custGPSRaw)
      }

      if (custAreaNumberRaw) {
        setAreanumber(custAreaNumberRaw)
      }
      if (!custBuidingNameRaw && !custFloorRaw && !custGPSRaw && !custAreaNumberRaw) {
        setconditionRenderTitle(false)
      } else {
        setconditionRenderTitle(true)
      }
    } catch (e) {
      // read error
    }
  }

  const onSelectMethod = (item?: any) => {
    setcustClient(item.itemTypeOptionLineName || item.name)
    settitleDropDown(item.itemTypeOptionLineName || item.name)
    let arrCheck = itemTypeArr.itemTypeOptions
    for (let i = 0; i < arrCheck.length; i++) {
      if (arrCheck[i].itemTypeOptionID == item.itemTypeOptionID) {
        arrCheck[i].itemTypeOptionReturnValue = [{returnValue: item}]
      }
    }
    setModalNumberCode(false)
    checkCompareOnBlur()

  }

  const onSelectArea = (item: any) => {
    setAreanumber(item.name)
    setmodalArea(false)
  }
  const handleSave = () => {
    saveDataItemType()
    if (isSave) {
      let lastItem = globalData.item.length - 1
      itemTypeArr.clientID = clientID // doi sau
      let lastIndex = globalData.item[lastItem].dataItemType.length - 1
      globalData.item[lastItem].dataItemType[lastIndex] = itemTypeArr
      handleSaveSqlite(globalData.item[lastItem].dataItemType)
    } else {
      let lastItem = globalData.item.length - 1
      itemTypeArr.clientID = clientID // doi sau
      globalData.item[lastItem].dataItemType.push(itemTypeArr)
      handleSaveSqlite(globalData.item[lastItem].dataItemType)
    }
    multiSet()
    setIsSave(true)
    mesageSaveSuccess()
  }
  const mesageSaveSuccess = () => {
    Alert.alert("", "Save success", [
      {
        text: "OK",
        onPress: () => console.log("Cancel Pressed"),
        style: "cancel",
      },
    ])
  }
  const handleOnPress = async (isNavigate = true) => {
    // const isRequire =  checkIsRequire()
    // if (isRequire) {
    //   Alert.alert('Warning', 'Check to make sure you filled out all required fields')
    //   return
    // }
    // const isNaN = checkNumberWarning()
    // if (isNaN) return
    saveDataItemType()
    multiSet()
    if (conditionRender == 1) { // add new

      let lastItem = globalData.item.length - 1
      let lastIndex = globalData.item[lastItem].dataItemType.length - 1
      let IndexRow = globalData.item[lastItem].dataItemType.length + 1
      itemTypeArr.clientID = clientID
      itemTypeArr.conditionRender = conditionRender
      itemTypeArr.mainImageFull = mainPhoto
      itemTypeArr.mainImage = mainPhoto?.name

      itemTypeArr.inventoryRowID = IndexRow // add InventoryAppId = indexRow + 1
      itemTypeArr.parentRowID = null

      if (isSave) {
        globalData.item[lastItem].dataItemType[lastIndex] = itemTypeArr
        handleSaveSqlite(globalData.item[lastItem].dataItemType)
      } else {
        globalData.item[lastItem].dataItemType.push(itemTypeArr)
        handleSaveSqlite(globalData.item[lastItem].dataItemType)
      }
      isNavigate ? navigate(Routes.INVENTORY_DETAIL_LIST_SCREEN) : null
    } else if (conditionRender == SEARCH_IMAGE_TYPE.LOCAL || conditionRender == SEARCH_IMAGE_TYPE.SERVER) { // local
      compareDataItemType()

      // isNavigate ? navigate(Routes.INVENTORY_DETAIL_LIST_SCREEN) : null
    } else if (conditionRender == 2 && canEditData == true) { //edit
      await AsyncStorage.setItem("@DataInventory", JSON.stringify(globalData.item))
      itemTypeArr.mainImageFull = mainPhoto
      handleUpdateSqlite()
      isNavigate ? navigate(Routes.INVENTORY_DETAIL_LIST_SCREEN, {idClient}) : null
    }
  }

  // const syncNavigate = async (isNavigate = true) => {

  // }

  // useEffect(() => {
  //   const indexError = checkError?.findIndex((it:any) => it?.isError)
  //   console.log('checkError',checkError)
  //   if (indexError !== -1){
  //     return
  //   }
  //   syncNavigate()
  // },[checkError])

  const compareDataItemType = () => {
    let dataImage = JSON.parse(dataImageInfo) //data receive when searchimage case local/server
    let dataItemType = itemTypeArr?.itemTypeOptions // data itemtype now
    let filteredDataImage = dataImage.filter(item =>
      item.itemTypeOptionCode !== 'Building' &&
      item.itemTypeOptionCode !== 'GPS' &&
      item.itemTypeOptionCode !== 'AreaOrRoom' &&
      item.itemTypeOptionCode !== 'Floor' &&
      item.itemTypeOptionCode !== 'TotalCount'
    )
    let filteredDataItemType = dataItemType.filter(item =>
      item.itemTypeOptionCode !== 'Building' &&
      item.itemTypeOptionCode !== 'GPS' &&
      item.itemTypeOptionCode !== 'AreaOrRoom' &&
      item.itemTypeOptionCode !== 'Floor' &&
      item.itemTypeOptionCode !== 'TotalCount'
    )
    let dataItemTypeOptions = JSON.stringify(filteredDataItemType)
    let dataImageCompare = JSON.stringify(filteredDataImage)
    let compare = isEqual(dataImageCompare, dataItemTypeOptions)
    if (compare) {
      if (conditionRender == SEARCH_IMAGE_TYPE.LOCAL) {
        itemTypeArr.parentRowID = imageInfo?.inventoryRowID
      } else if (conditionRender == SEARCH_IMAGE_TYPE.SERVER) {
        itemTypeArr.parentRowID = inventoryId
      }
    } else {
      if (conditionRender == SEARCH_IMAGE_TYPE.LOCAL || conditionRender == SEARCH_IMAGE_TYPE.SERVER) {
        itemTypeArr.parentRowID = null
        itemTypeArr.mainImageFull = mainPhoto
      }
    }
    let IndexRow = globalData.item[globalData.item.length - 1].dataItemType.length + 1
    itemTypeArr.inventoryRowID = IndexRow // add InventoryAppId = indexRow + 1
    let lastItem = globalData.item.length - 1
    itemTypeArr.clientID = clientID
    itemTypeArr.conditionRender = conditionRender

    let lastIndex = globalData.item[lastItem].dataItemType.length - 1

    if (isSave) {
      globalData.item[lastItem].dataItemType[lastIndex] = itemTypeArr
      handleSaveSqlite(globalData.item[lastItem].dataItemType)
    } else {
      globalData.item[lastItem].dataItemType.push(itemTypeArr)
      handleSaveSqlite(globalData.item[lastItem].dataItemType)
    }
    navigate(Routes.INVENTORY_DETAIL_LIST_SCREEN)
  }
  const handleSaveSqlite = (dataItemType: any) => {
    idClient &&
    updateDataInventoryClientByQuery("dataItemType = ? where id = ?", [JSON.stringify(dataItemType), idClient])
  }
  const handleUpdateSqlite = () => {
    const index = findIndex(globalData?.item, (e: { id: any }) => e?.id === idClient)
    if (index !== -1) {
      const dataItemType = globalData?.item[index]?.dataItemType
      idClient &&
      updateDataInventoryClientByQuery("dataItemType = ? where id = ?", [JSON.stringify(dataItemType), idClient])
    }
  }
  const saveDataItemType = () => {
    let arrItemType = itemTypeArr.itemTypeOptions
    itemTypeArr.mainImage = mainPhoto?.name
    // if(itemTypeArr.conditionRender != SEARCH_IMAGE_TYPE.LOCAL || itemTypeArr.conditionRender != SEARCH_IMAGE_TYPE.SERVER ){
    //   console.log("3333")
    //   itemTypeArr.conditionRender = conditionRender
    // }
    for (let i = 0; i < arrItemType.length; i++) {
      if (arrItemType[i].valType == 40) {
        arrItemType[i].itemTypeOptionReturnValue = arrCheckQuatity
      }
      if (arrItemType[i].valType == 80) {
        let returnValue: { returnValue?: any }[] = (arrItemType[i].itemTypeOptionReturnValue = [])
        for (let j = 0; j < arrCheck.length; j++) {
          let value = arrItemType[i].itemTypeOptionLines[arrCheck[j]]
          returnValue.push({returnValue: value})
        }
      }
      if (arrItemType[i].valType == 1 && arrItemType[i].itemTypeOptionCode == "Building" && custBuidingName) {
        arrItemType[i].itemTypeOptionReturnValue = [{returnValue: custBuidingName}]
      }
      if (arrItemType[i].valType == 3 && arrItemType[i].itemTypeOptionCode == "Floor" && custBuidingName) {
        arrItemType[i].itemTypeOptionReturnValue = [{returnValue: custFloor}]
      }
      if (arrItemType[i].valType == 30 && arrItemType[i].itemTypeOptionCode == "GPS" && custGPS) {
        arrItemType[i].itemTypeOptionReturnValue = [{returnValue: custGPS}]
      }
      if (arrItemType[i].valType == 1 && arrItemType[i].itemTypeOptionCode == "AreaOrRoom" && Areanumber) {
        arrItemType[i].itemTypeOptionReturnValue = [{returnValue: Areanumber}]
      }
    }
  }
  const multiSet = async () => {
    const firstPair = ["@custBuidingName", custBuidingName]
    const secondPair = ["@custFloor", custFloor]
    const thirdPair = ["@custGPS", custGPS]
    const fourPair = ["@custAreaNumber", Areanumber]
    try {
      await AsyncStorage.multiSet([firstPair, secondPair, thirdPair, fourPair])
    } catch (e) {
      console.log("e", e)
      //save error
    }
  }

  const onSelectItemTypeEditData = (item: any) => {
    setitemTypeTxt(item?.itemName)
    setdataItemType(item?.itemTypeOptions)
    setitemTypeArr(item)
    // setMainPhoto({ name: item?.mainImage })
    if (item.conditionRender == 1 || item.conditionRender == 2) {
      setMainPhoto(item?.mainImageFull)
    }
    setmodalType(false)
  }
  const onSelectItemType = (item: any) => {
    setItemTypeNew(item)
    setmodalType(false)
    setitemTypeTxt(item.itemName)
    setLoading(true)
    handleGetItemOptionSetTypeService(item.itemTypeID)
  }
  const onSelectSharedModal = async (data: any) => {
    if (typeSharedModal == "Building") {
      setcustBuidingName(data.inventoryBuildingName)
      await AsyncStorage.setItem("@custBuidingName", data.inventoryBuildingName)
    } else if (typeSharedModal == "Floors") {
      setcustFloor(data.inventoryFloorName)
      await AsyncStorage.setItem("@custFloor", data.inventoryFloorName)
    }

    setopenSharedModal(false)
  }
  const changeItemType = () => {
    setmodalType(true)
  }
  const openModal = (item: any) => {
    setModalNumberCode(true)
    setcheckNV(item.itemTypeOptionLines)
    settitleDropDown(item.itemTypeOptionName)
    // settypeOptionsId(item.itemTypeOptionID)
    setitemTypeId(item.itemTypeID)
  }
  const onTextChanged = (txt: any) => {
    setvaluesTextInput(txt)
  }
  const handleOnEndEditing = (valuesTextInput?: any, item?: any) => {
    let arrCheck = itemTypeArr.itemTypeOptions
    for (let i = 0; i < arrCheck.length; i++) {
      if (arrCheck[i].itemTypeOptionID == item.itemTypeOptionID) {
        arrCheck[i].itemTypeOptionReturnValue = [{returnValue: valuesTextInput}]
      }
    }
  }

  const handleShowMorePicture = (data?: any) => {
    setdataImageItem(data)
    setmodalTypeCameraAddMore(true)
  }
  const handleDelete = (indexParent?: any, indexChild?: any, indexGrand?: any) => {
    Alert.alert("Delete Image", "Are you sure you wish to delete this Image?", [
      {
        text: "No",
        onPress: () => console.log("Cancel Pressed"),
        style: "cancel",
      },
      {text: "Yes", onPress: () => removeImg(indexParent, indexChild, indexGrand)},
    ])
  }
  const removeImg = (indexParent: number, indexChild?: any, indexGrand?: any) => {
    let parentArr: { dataCondition: { url?: object[] }[] } = arrCheckQuatity[indexParent]
    let childArr = parentArr.dataCondition[indexChild].url
    childArr.splice(indexGrand, 1)
    setarrCheckQuatity([...arrCheckQuatity])
  }
  const handleDeleteItems = (index?: number, dataParent?: any, indexParent?: any) => {
    let raw = dataParent.dataCondition
    raw.splice(index, 1)
    if (raw?.length == 0) {
      arrCheckQuatity.splice(indexParent, 1)
    }
    setarrCheckQuatity([...arrCheckQuatity])
  }
  const renderQuantityTxt = (data?: any) => {
    if (data.type == "Damaged" || data.type == "MissingParts") {
      if (data.dataCondition[0].itemName == "All Items") {
        return data.txtQuality
      } else {
        return data.dataCondition.length
      }
    } else {
      return data.txtQuality
    }
  }

  const checkNumber = (value: any, key: any) => {
    console.log('key', key)
    if (isNaN(value)) {
      Alert.alert(
        MultiLanguage("Only numbers are accepted", language.textStatic),
        "",
        [
          {
            text: MultiLanguage("OK", language.textStatic),
            onPress: () => {
            },
            style: "cancel",
          },
        ],
        {cancelable: false},
      )
    }
  }
  const onChangeTextArr = (txt?: any, idx?: any, index?: any, item?: any) => {
    const indexChangeTxt = errorRequire?.findIndex((__er: any) => __er?.itemTypeOptionCode === item?.itemTypeOptionCode)
    if (indexChangeTxt !== -1 && txt && !isEmpty(errorRequire)) {
      setErrorRequire((preState: any) => {
        preState?.splice(indexChangeTxt, 1)
        return preState
      })
    }
    let arr = [...dataItemType]
    if (arr[index].itemTypeOptionReturnValue == null) {
      arr[index].itemTypeOptionReturnValue = [{returnValue: txt}]
    } else {
      arr[index].itemTypeOptionReturnValue[idx].returnValue = txt
    }

    setdataItemType(arr)
  }

  const onSetMainPhoto = useCallback(
    (item) => () => {
      setMainPhoto(item)
    },
    [],
  )
  const onTextChangedTxtFeild = (_: any, txt: string) => {
    setvaluesTextInputTextParagraph(txt)
  }
  const checkCompareOnBlur = () => {
    // const indexDecimal = dataItemType?.findIndex((it:any) => it?.fieldType === 'Decimal')
    // if (indexDecimal !== -1){
    //   checkNumber(dataItemType[indexDecimal]?.itemTypeOptionReturnValue[0].returnValue,dataItemType[indexDecimal]?.itemTypeOptionCode)
    // }
    if (flagCheckResetQuantity == null) {
      setflagCheckResetQuantity(true)
    } else {
      setflagCheckResetQuantity(!flagCheckResetQuantity)
    }
  }
  const checkKeyBoardType = (data: any) => {
    if (data.fieldType == "Decimal") {
      return 'number-pad'
    }
    return 'default'

  }
  const checkKeyType = (data: any) => {
    if (data.fieldType == "Decimal") {
      return 'done'
    } else {
      return 'default'
    }
  }
  // const checkErrorFlowKey = (key: any) => {
  //   const indexRequire = checkError?.findIndex((it: any) => it?.key === key) //itemTypeID
  //   let isError = false
  //   if (indexRequire !== undefined && indexRequire !== -1){
  //     isError =  checkError[indexRequire]?.isError
  //   }
  //   return isError ? 'red' : null
  // }
  const renderItemTypeFeild = ({item, index}: RenderItemTypeFeildProps): any => {
    const isError = errorRequire?.some((__er: any) => item?.itemTypeOptionCode === __er?.itemTypeOptionCode)
    if (item.valType == 1 && !item.isHide) {
      if (item?.itemTypeOptionReturnValue == null) {
        return (
          <View>
            <View style={{flex: 1}}>
              <View style={{marginTop: 20, flex: 1}}>
                <Text style={{fontSize: 15, flex: 4, marginBottom: 10}}>
                  {item.itemTypeOptionName}
                  {item.isRequired ? <Text style={{color: "red"}}> *</Text> : null}
                </Text>
                <View
                  style={[styles.btnDefault, {flexDirection: "row", backgroundColor: isError ? 'red' : 'transparent'}]}>
                  <View style={{flex: 9}}>
                    <TextInput style={styles.txtSize16}
                               keyboardType={checkKeyBoardType(item)}
                               returnKeyType={checkKeyType(item)}
                               onBlur={() => checkCompareOnBlur()}
                               onChangeText={(txt) => onChangeTextArr(txt, 0, index, item)}/>
                  </View>
                </View>
              </View>
            </View>
          </View>
        )
      } else {
        return (
          <View>
            {item?.itemTypeOptionReturnValue?.map((i: any, idx: number) => {
              return (
                <View style={{flex: 1}}>
                  <View style={{marginTop: 20, flex: 1}}>
                    <Text style={{fontSize: 15, flex: 4, marginBottom: 10}}>
                      {item.itemTypeOptionName}
                      {item.isRequired ? <Text style={{color: "red"}}> *</Text> : null}
                    </Text>
                    <View style={[styles.btnDefault, {
                      flexDirection: "row",
                      backgroundColor: isError ? 'red' : 'transparent'
                    }]}>
                      <View style={{flex: 9}}>
                        <TextInput
                          style={styles.txtSize16}
                          value={i.returnValue}
                          onFocus={() => {
                            if (item?.fieldType === 'Decimal' && !+i.returnValue && !isNaN(+i.returnValue)) {
                              onChangeTextArr('', idx, index, item?.itemTypeOptionCode)
                            }
                          }}
                          keyboardType={checkKeyBoardType(item)}
                          returnKeyType={checkKeyType(item)}
                          onBlur={() => checkCompareOnBlur()}
                          onChangeText={(txt) => onChangeTextArr(txt, idx, index, item)}
                        />
                      </View>
                    </View>
                  </View>
                </View>
              )
            })}
          </View>
        )
      }
    } else if (item.valType == 3 && !item.isHide) {
      //Dropdown
      return (
        <View style={{flex: 1, marginTop: 20}}>
          <Text style={{fontSize: 15, marginBottom: 10,}}>
            {item.itemTypeOptionName}
            {item.isRequired ? <Text style={{color: "red"}}> *</Text> : null}
          </Text>
          <TouchableOpacity style={styles.btnDefault} onPress={() => openModal(item)}>
            <Text style={[styles.txtSize16, {marginVertical: 5}]} maxFontSizeMultiplier={1}>
              {custClient}
            </Text>
            <Icon name={"list"} size={20}/>
          </TouchableOpacity>
        </View>
      )
    } else if (item.valType == 90 && !item.isHide) {
      //DropdownPlus
      return (
        <View style={{flex: 1, marginTop: 20}}>
          <Text style={{fontSize: 15, marginBottom: 10,}}>
            {item.itemTypeOptionName}
            {item.isRequired ? <Text style={{color: "red"}}> *</Text> : null}
          </Text>
          <TouchableOpacity style={styles.btnDefault} onPress={() => openModal(item)}>
            <Text style={[styles.txtSize16, {marginVertical: 5}]} maxFontSizeMultiplier={1}>
              {custClient}
            </Text>
            <Icon name={"list"} size={20}/>
          </TouchableOpacity>
        </View>
      )
    } else if (item.valType == 2 && !item.isHide) {
      //TextParagraph
      return (
        <View style={{marginTop: 20, flex: 1}}>
          <Text style={{fontSize: 15, marginBottom: 10,}}>
            {item.itemTypeOptionName}
            {item.isRequired ? <Text style={{color: "red"}}> *</Text> : null}
          </Text>
          <View style={[styles.btnDefault, {justifyContent: "flex-start", minHeight: 100}]}>
            <TextInput
              style={styles.txtSize16}
              multiline
              numberOfLines={4}
              onBlur={() => checkCompareOnBlur()}
              onEndEditing={() => handleOnEndEditing(valuesTextInputTextParagraph, item)}
              value={valuesTextInputTextParagraph}
              onChangeText={(txt) => onTextChangedTxtFeild(item.itemTypeOptionID, txt)}
            />
          </View>
        </View>
      )
    } else if (item.valType == 40 && !item.isHide) {
      //CheckAllApply image
      return (
        <View style={{flex: 1, marginTop: 20}}>
          <View>
            <Text style={{fontSize: 15, flex: 1}}>
              {"Conditions Quantity"}
              {item.isRequired ? <Text style={{color: "red"}}> *</Text> : null}
            </Text>

            {arrCheckQuatity.map((data: any, i: number) => {
              let arr = data?.dataCondition
              return (
                <View key={i} style={{marginTop: 10}}>
                  {arr.length > 0 && (
                    <TouchableOpacity onPress={() => handleShowMorePicture(data)}>
                      <Text style={{fontSize: 15, flex: 4, marginBottom: 10, color: "blue"}}>
                        {`${data.nameCondition} QTY`} {"-"} {renderQuantityTxt(data)}
                      </Text>
                    </TouchableOpacity>
                  )}
                  {arr.map((dataImg: any, indexChild: number) => {
                    return (
                      <View key={indexChild}>
                        <View style={{marginLeft: 15}}>
                          <View style={{flexDirection: "row"}}>
                            <View>
                              <Text style={{fontSize: 15, marginBottom: 10}}>
                                {"- "}
                                {dataImg.itemName}
                              </Text>
                            </View>
                            <TouchableOpacity
                              onPress={() => handleDeleteItems(index, data, i)}
                              style={{
                                justifyContent: "center",
                                alignItems: "center",
                                marginLeft: 20,
                                marginBottom: 10,
                              }}
                            >
                              <IconImage name={"delete"} size={15}/>
                            </TouchableOpacity>
                          </View>
                          <FlatList
                            data={dataImg.url}
                            contentContainerStyle={{paddingBottom: 12}}
                            horizontal={true}
                            renderItem={({item, index}) => (
                              <View style={{}}>
                                <TouchableOpacity
                                  style={{marginTop: 10}}
                                  onPress={() => handleDelete(i, indexChild, index)}
                                >
                                  <MaterialCommunityIcons
                                    name={"delete-circle"}
                                    size={25}
                                    style={{
                                      position: "absolute",
                                      right: 5,
                                      top: -5,
                                    }}
                                  />
                                </TouchableOpacity>
                                <TouchableOpacity
                                  style={{
                                    width: 80,
                                    height: 80,
                                    marginTop: 10,
                                    marginHorizontal: 15,
                                  }}
                                  onPress={() => handleShowModalImage(item)}

                                  onLongPress={(conditionRender == SEARCH_IMAGE_TYPE.LOCAL || conditionRender == SEARCH_IMAGE_TYPE.SERVER) && flagDeleteMainPhoto == false ? null : canEditData ? null : onSetMainPhoto(item)}
                                >
                                  <Image
                                    source={{uri: item.uri}}
                                    style={[styles.photo, item.name === mainPhoto?.name && styles.activePhoto]}
                                    resizeMode="cover"
                                  />
                                </TouchableOpacity>
                              </View>
                            )}
                          />
                        </View>
                      </View>
                    )
                  })}
                </View>
              )
            })}
            <View
              style={{
                flexDirection: "row",
                flexWrap: "wrap",

                marginTop: 20,
              }}
            >
              {item.itemTypeOptionLines.map((data: any, i?: any) => {
                return (
                  <TouchableOpacity
                    key={i}
                    onPress={() => handlePressCheckQuatity(i, data)}
                    style={{
                      backgroundColor: arrCheckQuatity.includes(i) ? "gray" : "white",
                      borderWidth: 0.4,
                      marginVertical: 5,
                      borderColor: "gray",
                      borderRadius: 18,
                      marginHorizontal: 5,
                    }}
                  >
                    <Text style={{padding: 10, paddingHorizontal: 15}}>{data.itemTypeOptionLineCode}</Text>
                  </TouchableOpacity>
                )
              })}
            </View>
          </View>
        </View>
      )
    } else if (item.valType == 30 && !item.isHide) {
      //gps
      return (
        <View style={{flex: 1}}>
          <View style={{marginTop: 20, flex: 1}}>
            <Text style={{fontSize: 15, flex: 4, marginBottom: 10}}>
              {item.itemTypeOptionName}
              {item.isRequired ? <Text style={{color: "red"}}> *</Text> : null}
            </Text>
            <View style={styles.btnDefault}>
              <TextInput
                numberOfLines={3}
                multiline
                onBlur={() => checkCompareOnBlur()}
                style={styles.txtSize16}
                value={custGPS}
                onChangeText={(txt) => setcustGPS(txt)}
              />
            </View>
          </View>
        </View>
      )
    } else if (item.valType == 50 && !item.isHide) {
      //TextBoxPlus
      if (item?.itemTypeOptionReturnValue == null) {
        return (
          <View>
            <View style={{flex: 1}}>
              <View style={{marginTop: 20, flex: 1}}>
                <Text style={{fontSize: 15, flex: 4, marginBottom: 10}}>
                  {item.itemTypeOptionName}
                  {item.isRequired ? <Text style={{color: "red"}}> *</Text> : null}
                </Text>
                <View style={[styles.btnDefault, {flexDirection: "row"}]}>
                  <View style={{flex: 9}}>
                    <TextInput style={styles.txtSize16}
                               onBlur={() => checkCompareOnBlur()}
                               onChangeText={(txt) => onChangeTextArr(txt, 0, index, item)}/>
                  </View>
                  <TouchableOpacity onPress={() => addMoreTxt(0, index)} style={{flex: 1}}>
                    <IconImage name={"plus"} size={20}/>
                  </TouchableOpacity>
                </View>
              </View>
            </View>
          </View>
        )
      } else {
        return (
          <View>
            {item?.itemTypeOptionReturnValue.map((i: any, idx: number) => {
              return (
                <View style={{flex: 1}}>
                  <View style={{marginTop: 20, flex: 1}}>
                    <Text style={{fontSize: 15, flex: 4, marginBottom: 10}}>
                      {item.itemTypeOptionName}
                      {item.isRequired ? <Text style={{color: "red"}}> *</Text> : null}
                    </Text>
                    <View style={[styles.btnDefault, {flexDirection: "row"}]}>
                      <View style={{flex: 9}}>
                        <TextInput
                          style={styles.txtSize16}
                          value={i.returnValue}
                          onBlur={() => checkCompareOnBlur()}
                          onChangeText={(txt) => onChangeTextArr(txt, idx, index, item)}
                        />
                      </View>
                      {item?.itemTypeOptionReturnValue.length < parseInt(item.limitMax) && (
                        <TouchableOpacity onPress={() => addMoreTxt(idx, index)} style={{flex: 1}}>
                          <IconImage name={"plus"} size={20}/>
                        </TouchableOpacity>
                      )}
                    </View>
                  </View>
                </View>
              )
            })}
          </View>
        )
      }
    } else if (item.valType == 80 && !item.isHide) {
      //RadioPlus
      let arrRaw = item.itemTypeOptionLines
      return (
        <View style={{flex: 1}}>
          <View style={{marginTop: 20, flex: 1}}>
            <Text style={{fontSize: 15, flex: 4, marginBottom: 10}}>
              {item.itemTypeOptionName}
              {item.isRequired ? <Text style={{color: "red"}}> *</Text> : null}
            </Text>
          </View>
          <View style={{flex: 1, flexDirection: "row"}}>
            {arrRaw.map((data: any, i: number) => {
              return (
                <TouchableOpacity
                  key={i}
                  onPress={() => handlePressCheck(i)}
                  style={{
                    backgroundColor: arrCheck.includes(i) ? "gray" : "white",
                    borderRadius: 20,
                    marginHorizontal: 5,
                  }}
                >
                  <Text style={{padding: 10, paddingHorizontal: 15}}>{data.itemTypeOptionLineName}</Text>
                </TouchableOpacity>
              )
            })}
          </View>
        </View>
      )
    }
  }
  const handleShowModalImage = (data: any) => {
    setmodalShowImage(true)
    setshowImageModal(data)
  }
  const handlePressCheckQuatity = (_: any, data: any) => {
    setconditionsType(data.itemTypeOptionLineCode)
    setmodalTypeCamera(true)
  }
  const handlePressCheck = (item: number) => {

    if (arrCheck.includes(item)) {
      const newIds = arrCheck.filter((id) => id !== item)
      setarrCheck(newIds)
    } else {
      const newIds = [...arrCheck]
      newIds.push(item)
      setarrCheck(newIds)
    }
    checkCompareOnBlur()
  }
  const checkImage = async (item: any) => {
    for (let i = 0; i < arrCheckQuatity.length; i++) {
      if (arrCheckQuatity[i].type == item.type) {
        arrCheckQuatity.splice(i, 1)
      }
    }
    await arrCheckQuatity.push(item)
    // const mainPhoto = item.dataCondition[0]?.url[0]
    // setMainPhoto(mainPhoto ? mainPhoto : undefined)
    setmodalTypeCamera(false)
  }

  const getUpdateImage = async (item: any) => {
    for (let i = 0; i < arrCheckQuatity.length; i++) {
      if (arrCheckQuatity[i].type == item.type) {
        arrCheckQuatity.splice(i, 1)
      }
    }
    await arrCheckQuatity.push(item)

    setmodalTypeCameraAddMore(false)
    setflagCheckUpNewImage(true) //if upload new then no delete image when edit itemtype data (case 3,4)
  }

  const addMoreTxt = (_: any, index: number) => {
    let arr = [...dataItemType]
    let arrayReturnValue = arr[index].itemTypeOptionReturnValue
    if (arrayReturnValue) {
      arrayReturnValue?.push({returnValue: ""})
      setdataItemType(arr)
    } else {
      arr[index].itemTypeOptionReturnValue = []
      arr[index].itemTypeOptionReturnValue.push({returnValue: ""}, {returnValue: ""})
      setdataItemType(arr)
    }
  }
  const handleOpenModal = (type: any) => {
    settypeSharedModal(type)
    settitleSharedModal(type)
    if (type == "Building") {
      setdataSharedModal(buildingData)
      setcanAddData(true)
      setopenSharedModal(true)
    } else if (type == "Floors") {
      setdataSharedModal(floor)
      setcanAddData(false)
      setopenSharedModal(true)
    }
  }

  // const onPressSearch = useCallback(() => {
  //   console.log('Myle in InventoryDetailScreen - ItemtypeId: ', itemTypeNew?.itemTypeID)
  //   navigate(Routes.SEARCH_IMAGE_SCREEN, { itemTypeId: itemTypeNew?.itemTypeID, clientId: itemTypeNew?.clientID })
  // }, [JSON.stringify(itemTypeNew)])
  const onPressSearch = () => {
    navigate(Routes.SEARCH_IMAGE_SCREEN, {itemTypeId: itemTypeNew?.itemTypeID, clientId: clientID, idClient})
  }
  const handleSaveDataLocation = async (type: any) => {
    if (type == 'gps') {
      await AsyncStorage.setItem("@custGPS", custGPS)
    } else { // arearoomnumber
      await AsyncStorage.setItem("@custAreaNumber", Areanumber)
    }

  }
  const renderChoseTitle = () => {
    return (
      <>
        <View style={styles.choseTitleContainer}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              Building Name <Text style={{color: "red"}}>*</Text>
            </Text>
            <TouchableOpacity style={styles.btnDefault} onPress={() => handleOpenModal("Building")}>
              <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
                {custBuidingName}
              </Text>
              <Icon name={"list"} size={20}/>
            </TouchableOpacity>
          </View>
        </View>
        <View style={styles.choseTitleContainer}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              Floor <Text style={{color: "red"}}>*</Text>
            </Text>
            <TouchableOpacity style={styles.btnDefault} onPress={() => handleOpenModal("Floors")}>
              <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
                {custFloor}
              </Text>
              <Icon name={"list"} size={20}/>
            </TouchableOpacity>
          </View>
        </View>
        <View style={styles.choseTitleContainer}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              GPS <Text style={{color: "red"}}>*</Text>
            </Text>
            <View style={styles.btnDefault}>
              <TextInput
                style={styles.txtSize16}
                numberOfLines={2}
                onBlur={() => handleSaveDataLocation('gps')}
                maxFontSizeMultiplier={1}
                onChangeText={(text) => setcustGPS(text)}
              >
                {custGPS}
              </TextInput>
              <Icon name={"location"} size={20}/>
            </View>
          </View>
        </View>

        <View style={styles.choseTitleContainer}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              Area or Room number <Text style={{color: "red"}}>*</Text>
            </Text>
            <View style={styles.btnDefault}>
              <TextInput
                style={styles.txtSize16}
                maxFontSizeMultiplier={1}
                onBlur={() => handleSaveDataLocation('areaNumber')}

                onChangeText={(text) => setAreanumber(text)}
              >
                {Areanumber}
              </TextInput>
            </View>
          </View>
        </View>
      </>
    )
  }
  const renderTitle = () => {
    return (
      <TouchableOpacity onPress={() => setconditionRenderTitle(false)} style={{marginTop: 20}}>
        <Text style={styles.txtTitleOps}>{`Building : ${custBuidingName}`} </Text>
        <Text style={styles.txtTitleOps}>{`Floor : ${custFloor} `} </Text>
        <Text style={styles.txtTitleOps}>{`GPS : ${custGPS}`}</Text>
        <Text style={styles.txtTitleOps}>{`Area or Room number : ${Areanumber}`}</Text>
      </TouchableOpacity>
    )
  }

  const validateRequired = () => {
    if (dataItemType?.length > 0 && arrCheckQuatity?.length > 0) {
      return false
    } else {
      return true
    }
  }
  const renderChoseItemType = () => {
    return (
      <View style={{flex: 1}}>
        <View style={{marginTop: 20, flex: 1}}>
          <Text style={{fontSize: 15, flex: 4, marginBottom: 10}}>
            Item Type <Text style={{color: "red"}}>*</Text>
          </Text>
          <TouchableOpacity disabled={disableChoseItemType} style={styles.btnDefault} onPress={() => changeItemType()}>
            <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
              {itemTypeTxt}
            </Text>
            <Icon name={"list"} size={20}/>
          </TouchableOpacity>
        </View>
      </View>
    )
  }

  const onChangeCheckBoxSetMainPhoto = useCallback(
    (value) => {
      setMainPhoto(value ? showImageModal : undefined)
    },
    [showImageModal],
  )

  useEffect(() => {
    if ((!mainPhoto || (mainPhoto && mainPhoto.isAuto)) && !!arrCheckQuatity?.length && !canEditData) {
      const goodCondition = arrCheckQuatity.filter((i: any) => i?.type === CONDITION_QUANTITY_TYPE.GOOD)
      const fairCondition = arrCheckQuatity.filter((i: any) => i?.type === CONDITION_QUANTITY_TYPE.FAIR)
      const poorCondition = arrCheckQuatity.filter((i: any) => i?.type === CONDITION_QUANTITY_TYPE.POOR)
      const damagedCondition = arrCheckQuatity.filter((i: any) => i?.type === CONDITION_QUANTITY_TYPE.DAMAGED)
      const missingPartCondition = arrCheckQuatity.filter((i: any) => i?.type === CONDITION_QUANTITY_TYPE.MISSING_PARTS)
      const arrCheckQuatitySorted: { dataCondition: any }[] = [
        ...goodCondition,
        ...fairCondition,
        ...poorCondition,
        ...damagedCondition,
        ...missingPartCondition,
      ]
      const mainPhoto = arrCheckQuatitySorted[0]?.dataCondition[0]?.url[0]
      if (conditionRender == SEARCH_IMAGE_TYPE.LOCAL || conditionRender == SEARCH_IMAGE_TYPE.SERVER) {
        console.log("flag", flagDeleteMainPhoto)
        if (flagDeleteMainPhoto) {
          setMainPhoto({...mainPhoto, isAuto: true})
        }
      } else { // case add new / edit
        console.log('myle in setMainPhoto')
        setMainPhoto({...mainPhoto, isAuto: true})
      }

    }
  }, [JSON.stringify(arrCheckQuatity), JSON.stringify(mainPhoto)])
  const ListHeaderComponent = useMemo((): any => {
    const uri = conditionRender == SEARCH_IMAGE_TYPE.LOCAL ? imageInfo.mainImageFull.uri : conditionRender == SEARCH_IMAGE_TYPE.SERVER ? `${URL_BASE}/${imageInfo?.mainPhoto}` : null
    return (
      !!dataItemType?.length && conditionRender != 2 && (
        <View>

          {
            conditionRender == SEARCH_IMAGE_TYPE.SERVER || conditionRender == SEARCH_IMAGE_TYPE.LOCAL ?
              <View
                style={{}}>
                <Text style={{fontSize: fontSizer(16), fontWeight: 'bold', fontStyle: 'italic'}}>Main Photo </Text>
                <View
                  style={{
                    width: 80,
                    height: 80,
                    marginTop: 10,
                    marginHorizontal: 15,
                  }}
                >
                  <Image
                    source={{uri}}
                    style={[styles.photo, styles.activePhoto]}
                    resizeMode="cover"
                  />
                </View>
              </View> : null
          }

        </View>
      )
    )
  }, [dataItemType, itemTypeNew])
  return (
    <View style={styles.container}>
      <Header
        isSaved={true}
        handleSave={() => (dataItemType?.length > 0 ? handleSave() : null)}
        isGoBack
        conditionRender={conditionRender}
        labels={"Inventory Collection"}
      />
      {
        console.log("localDataArray", localDataArray)
      }
      <LoadingOverlay visible={loading}/>
      <KeyboardAwareScrollView
        bounces={false}
        showsVerticalScrollIndicator={false}
        extraScrollHeight={150}
        enableOnAndroid={true}
        style={styles.subContainer}
      >

        {!conditionRenderTitle ? renderChoseTitle() : renderTitle()}
        {renderChoseItemType()}
        <TouchableOpacity
          onPress={() => onPressSearch()}
          style={styles.buttonSearchImg}>
          <Text style={{fontSize: fontSizer(16), fontWeight: 'bold', color: 'blue'}}>Search Photo </Text>
        </TouchableOpacity>
        <FlatList
          ListHeaderComponent={flagDeleteMainPhoto == false ? ListHeaderComponent : null}
          data={dataItemType}
          style={{marginBottom: 100}}
          renderItem={(item) => renderItemTypeFeild(item)}
          extraData={[...errorRequire]}
        />
      </KeyboardAwareScrollView>
      <BottomButtonNext disabled={validateRequired()} onPressButton={() => handleOnPress()}/>
      {/*<BottomButtonNext  onPressButton={() => handleOnPress()} />*/}

      {modalNumberCode && (
        <BaseBottomSheet
          open={modalNumberCode}
          options={checkNV}
          flex={0.6}
          addMore={true}
          title={titleDropDown}
          type={"numberNV"}
          itemTypeId={itemTypeId}
          onSelect={(item) => onSelectMethod(item)}
          onClosed={() => setModalNumberCode(false)}
          onOpened={() => setModalNumberCode(true)}
        />
      )}
      {openSharedModal && (
        <BaseBottomSheet
          open={openSharedModal}
          options={dataSharedModal}
          flex={0.6}
          addMore={canAddData}
          title={titleSharedModal}
          type={typeSharedModal}
          // onSelect={(item) => onSelectFloor(item)}
          onSelect={(item) => onSelectSharedModal(item)}
          onClosed={() => setopenSharedModal(false)}
          onOpened={() => setopenSharedModal(true)}
        />
      )}

      {modalShowImage && (
        <BaseBottomSheetShowImage
          open={modalShowImage}
          options={showImageModal}
          flex={0.9}
          title={"Image"}
          type={"areaNumber"}
          onSelect={(item) => onSelectArea(item)}
          onClosed={() => setmodalShowImage(false)}
          onOpened={() => setmodalShowImage(true)}
          // isShowChooseMainPhoto={flagDeleteMainPhoto == false ? true : false }
          isShowChooseMainPhoto={(conditionRender == SEARCH_IMAGE_TYPE.LOCAL || conditionRender == SEARCH_IMAGE_TYPE.SERVER) && flagDeleteMainPhoto == false ? false : canEditData ? false : true}
          // isShowChooseMainPhoto={true}
          onValueChange={onChangeCheckBoxSetMainPhoto}
          value={mainPhoto?.name === showImageModal!.name}
        />
      )}
      {modalType && (
        <BaseBottomSheet
          open={modalType}
          options={itemType}
          flex={0.6}
          addMore={true}
          title={"ItemTypes"}
          type={"itemType"}
          onSelect={(item) => onSelectItemType(item)}
          onClosed={() => setmodalType(false)}
          onOpened={() => setmodalType(true)}
        />
      )}
      {modalTypeCamera && (
        <BaseBottomSheetCamera
          open={modalTypeCamera}
          options={itemType}
          flex={1}
          conditionRender={conditionRender}
          title={"Chose Item Type number?"}
          type={conditionsType}
          onSelect={(item) => checkImage(item)}
          onClosed={() => setmodalTypeCamera(false)}
          onOpened={() => setmodalTypeCamera(true)}
        />
      )}
      {modalTypeCameraAddMore && (
        <BaseBottomSheetCameraAddMoreCondition
          open={modalTypeCameraAddMore}
          options={dataImageItem}
          flex={1}
          title={"Chose Item Type number?"}
          type={conditionsType}
          onSelect={(item) => getUpdateImage(item)}
          onClosed={() => setmodalTypeCameraAddMore(false)}
          onOpened={() => setmodalTypeCameraAddMore(true)}
        />
      )}
    </View>
  )
}
const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#f5f5f5",
  },
  subContainer: {
    flex: 8,
    padding: responsiveW(20),
    paddingTop: responsiveW(5),
  },
  choseTitleContainer: {
    flexDirection: "row",
    marginTop: 20,
  },
  subChoseTitleContainer: {
    marginTop: 20,
    flex: 1,
    flexDirection: "row",
    alignItems: "center",
  },
  txtTitleChose: {
    fontSize: 15,
    flex: 4,
  },
  btnDefault: {
    alignItems: "center",
    justifyContent: "space-between",
    flexDirection: "row",
    backgroundColor: "#FFFFFF",
    paddingHorizontal: responsiveW(18),
    borderRadius: 10,
    borderWidth: 1,
    borderColor: "#D6D6D6",
    flex: 3,
  },
  txtSize16: {
    paddingVertical: responsiveH(15),
    fontSize: fontSizer(16),
    flex: 1,
    marginRight: responsiveW(10),
  },
  txtTitleOps: {
    fontSize: 20,
    marginVertical: 5,
    fontWeight: "bold",
  },

  textFilter: {
    paddingVertical: 5,
    paddingHorizontal: 15,
    borderWidth: 0.4,
    borderRadius: 15,
    marginRight: 12,

    // marginVertical: 6,
  },
  btnCustom: {
    alignItems: "center",
    backgroundColor: "#FFFFFF",
    borderRadius: 30,
    borderWidth: 1,
    borderColor: "#D6D6D6",
    flex: 1,
    textAlign: "center",
    marginHorizontal: 5,
  },
  txtCustom: {
    paddingVertical: responsiveH(5),
    fontSize: fontSizer(16),
    flex: 1,
    marginRight: responsiveW(10),
  },
  activePhoto: {
    borderWidth: 2.5,
    borderColor: Colors.red,
  },
  photo: {
    flex: 1,
    borderRadius: 10,
  },
  buttonSearchImg: {
    borderColor: "#D6D6D6",
    height: 40,
    // marginTop:5,
    width: 170,
    alignItems: "center",
    flexDirection: "row",
  }
})

export default InventoryDetailScreen
