import { useNavigation, useRoute, useFocusEffect, CommonActions, StackActions } from "@react-navigation/native"

import React, { useCallback, useState, useEffect, useMemo, useRef } from "react"
import {
  Image,
  StyleSheet,
  Text,
  Alert,
  FlatList,
  TouchableOpacity,
  View,
  Modal,
  KeyboardAvoidingView,
  ScrollView,
  Platform,
} from "react-native"
import { TextInput } from "react-native-gesture-handler"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import Icon from "react-native-vector-icons/Ionicons"
import IconFontAwesome from "react-native-vector-icons/FontAwesome5"
import MaterialCommunityIcons from "react-native-vector-icons/MaterialCommunityIcons"
import AsyncStorage from "@react-native-community/async-storage"
import { SelectOption } from "/components/Select/types"
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
  RadioButton,
  BaseButton,
} from "../../components"
import Storage from "/helper/Storage"
import { fontSizer, responsiveH, responsiveW } from "../../utils/dimension"
import IconImage from "react-native-vector-icons/AntDesign"
import useGlobalData from "/utils/hook/useGlobalData"
import {
  SERVER_NAME,
  CONDITION_QUANTITY_TYPE,
  URL_BASE,
  SEARCH_IMAGE_TYPE,
  IMAGE_TYPE,
  getUniqueListBy,
} from "/constant/Constant"
import _, { findIndex, forEach, get, isEqual, min, isEmpty, isNaN, isArray, cloneDeep, isString } from "lodash"
import { getListInventoryClientByQuery, updateDataInventoryClientByQuery } from "/utils/sqlite/tableInventoryClient"
import {
  getItemTypeService,
  getItemOptionSetTypeService,
  getFloorsDataService,
  getBuildDataService,
  removeUserAcceptanceRules,
  getCondition,
} from "/redux/progress/service"
import MultiLanguage from "/utils/MultiLanguage"
import useLanguage from "/utils/hook/useLanguage"
import { getLocation } from "../../utils"
import { useSelector } from "react-redux"
import { AppReducerType } from "/redux/reducers"
import { saveNewManufacturerService } from "/redux/progress/service"
import { useDispatch } from "react-redux"
import { manufacturer } from "/redux/manufacturer/actions"
import { ModalFullScreen } from "/components/ModalFullScreen"
import { goBack } from "/navigators/_root_navigator"

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

// const SERVER_NAME = 'https://systemsource.s3.us-west-2.amazonaws.com/inventory'

const InventoryDetailScreen = () => {
  const { navigate } = useNavigation()
  const [language] = useLanguage()
  const route = useRoute()
  const idClient = get(route, "params.idClient")
  const imageInfo = get(route, "params.imageInfo") // Hard
  const conditionRender = get(route, "params.conditionRender")
  const inventoryId = get(route, "params.inventoryId")
  const isEdit = get(route, "params.isEdit")
  const parentRowID = get(route, "params.parentRowID")
  const [localDataArray, setlocalDataArray] = useState({
    conditionRender: conditionRender,
    isEdit: isEdit,
    parentRowID: parentRowID,
  })
  const [custClient, setcustClient] = useState("")
  const [custBuidingName, setcustBuidingName] = useState("")
  const [custFloor, setcustFloor] = useState("")
  const [custGPS, setcustGPS] = useState("")
  const [modalNumberCode, setModalNumberCode] = useState(false)

  const [globalData, setGlobalDataStore] = useGlobalData()
  const globalDataCache = useRef(cloneDeep(globalData))
  const [Areanumber, setAreanumber] = useState("")
  const [itemTypeTxt, setitemTypeTxt] = useState("")
  const [titleDropDown, settitleDropDown] = useState("Options")
  const [dataItemType, setdataItemType] = useState<any>([])
  const [conditionRenderTitle, setconditionRenderTitle] = useState(false)
  const [modalTypeCamera, setmodalTypeCamera] = useState(false)
  const [modalShowImage, setmodalShowImage] = useState(false)
  const [modalTypeCameraAddMore, setmodalTypeCameraAddMore] = useState(false)
  const [showImageModal, setshowImageModal] = useState<Photos>()
  const [arrCheck, setarrCheck] = useState<any[]>([])
  const [arrCheckQuatity, setarrCheckQuatity] = useState<{ type?: any }[]>([])
  const [dataImageItem, setdataImageItem] = useState({})
  const refIsSave = useRef(false)
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
  const [txtNote, settxtNote] = useState("")
  const [mainPhoto, setMainPhoto] = useState<Photos>()

  const [checkNV, setcheckNV] = useState<SelectOption[]>([])
  const [modalType, setmodalType] = useState(false)
  const [floor, setFloor] = useState<SelectOption[]>([])
  const [buildingData, setbuildingData] = useState<SelectOption[]>([])
  const [itemType, setitemType] = useState<SelectOption[]>([])
  const [disableChoseItemType, setdisableChoseItemType] = useState(true)
  const [itemTypeNew, setItemTypeNew] = useState<{ itemTypeId?: number; clientID?: number }>()
  const [canAddData, setcanAddData] = useState(false)
  const [valuesTextInputTextParagraph, setvaluesTextInputTextParagraph] = useState<string>("")
  const [dataImageInfo, setdataImageInfo] = useState<any>([])
  const [flagCheckSetData, setflagCheckSetData] = useState(false)
  const [flagCheckUpNewImage, setflagCheckUpNewImage] = useState(false)
  const [flagDeleteMainPhoto, setflagDeleteMainPhoto] = useState(false)
  const clientID = globalData.item[0]?.clientId
  const inventoryClientGroupID = globalData.item[0]?.InventoryClientGroupID
  const [errorRequire, setErrorRequire] = useState<any>([])
  const [urlSupportFile, setUrlSupportFile] = useState("")
  const [isMetric, setIsMetric] = useState(false)
  const [isInches, setIsInches] = useState(true)
  const [itemSaveForNote, setItemSaveForNote] = useState<any>(null)
  const [noteTxt, setNoteTxt] = useState<string>("")
  const [isInventoryClientGroupID, setIsInventoryClientGroupID] = useState(false)
  const manufacturerReducer = useSelector((state: any) => state?.[AppReducerType.MANUFACTURER])
  const refDataBuilding = useRef<any>(null)
  const refDataFloor = useRef<any>(null)
  const refManufacturer = useRef<any>(null)
  const refItemArrayQuantity = useRef<any>(null)
  const [dataCondition, setDataCondition] = useState<any>(null)
  const [cacheTotalCountServer, setCacheTotalCountServer] = useState<any>(null)
  const [timeEdited, setTimeEdited] = useState<any>(null)
  const dispatch = useDispatch()

  const handleGetCondition = async () => {
    const rs = await getCondition()
    if (rs?.status === 200) {
      setDataCondition(rs?.data)
    } else {
      Alert.alert(`Error api get condition (${rs?.status || 0})`)
    }
  }

  const getMyStringValue = async () => {
    try {
      const user = await Storage.get("@User")
      const objUser = JSON.parse(`${user}`)
      getListInventoryClientByQuery("status = ? AND userId = ?", ["InProgress", objUser.userId], (data) => {
        if (data) {
          console.log("data", data)
          const filterData = data?.filter((it: any) => it?.id === idClient)
          if (!isEmpty(filterData)) {
            setTimeEdited(filterData?.[0]?.deviceDate)
          }
        }
      })
    } catch (e) {
      // read error
    }
  }

  useEffect(() => {
    getLocation()
    conditionRenderData()
    setIsInventoryClientGroupID(!!inventoryClientGroupID && inventoryClientGroupID >= 2)
    handleGetCondition()
    getMyStringValue()
  }, [])

  const conditionRenderData = () => {
    checkConditionRenderData()
    checkCanEditData()
  }
  const checkConditionRenderData = () => {
    if (
      localDataArray.conditionRender == null &&
      localDataArray.isEdit == false &&
      localDataArray.parentRowID == null
    ) {
      //create new
      handleAddNewCase()
    } else if (
      localDataArray.isEdit != true &&
      (localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ||
        localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER)
    ) {
      // local
      handleSearchImageLocalCase()
    }
  }
  const checkCanEditData = () => {
    if (localDataArray.isEdit) {
      //edit
      handleEditCase()
    }
  }

  useEffect(() => {
    if (
      localDataArray.isEdit != true &&
      (localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ||
        localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER)
    ) {
      //set data search image local
      setdataImageInfo(JSON.stringify(imageInfo.itemTypeOptions))
    }
  }, [flagCheckSetData])

  useEffect(() => {
    if (!imageInfo) return
    const indexManufacturer = imageInfo?.itemTypeOptions?.findIndex(
      (it: any) => it?.itemTypeOptionCode === "Manufacturer",
    )
    if (indexManufacturer !== -1 && indexManufacturer) {
      const itemManufacturer = imageInfo?.itemTypeOptions?.[indexManufacturer]
      const valueReturn = itemManufacturer?.itemTypeOptionReturnValue?.[0]
      const syncBodyManufacturer = {
        itemTypeOptionLineName: valueReturn?.returnValue,
        itemTypeOptionLineID: valueReturn?.returnID,
        itemTypeOptionLineCode: valueReturn?.returnCode,
      }
      refManufacturer.current = syncBodyManufacturer
      const valueManufacturer: any = _.get(itemManufacturer, "itemTypeOptionReturnValue.[0].returnValue", null)
      if (!valueManufacturer) return
      const checkTypeValueManufacturer = isString(valueManufacturer)
        ? valueManufacturer
        : valueManufacturer?.itemTypeOptionLineName
      const checkSomeManufacturer = manufacturerReducer?.data?.some(
        (item: { itemTypeOptionLineName: string }) =>
          item?.itemTypeOptionLineName?.toUpperCase() === checkTypeValueManufacturer?.toUpperCase(),
      )
      if (!checkSomeManufacturer) {
        let dataBody = {
          ManufacturerName: checkTypeValueManufacturer,
        }
        saveNewManufacturerService(dataBody, (res) => {
          if (res.status == 200) {
            dispatch(manufacturer.request(null))
          }
        })
      }
    }
  }, [imageInfo])

  const checkIsRequire = () => {
    setErrorRequire([])
    const dataFilterRequire = dataItemType?.filter((it: any) => it?.isRequired && it?.itemTypeOptionCode !== "Building")
    const checkBlankFields = dataFilterRequire?.some((__it: any) =>
      __it?.itemTypeOptionReturnValue ? !__it?.itemTypeOptionReturnValue[0]?.returnValue : true,
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
    const dataFilterDecimal = dataItemType?.filter((it: any) => it?.fieldType === "Decimal")
    const findIndexNaN = dataFilterDecimal?.findIndex((__it: any) =>
      __it?.itemTypeOptionReturnValue ? isNaN(+__it?.itemTypeOptionReturnValue[0]?.returnValue) : false,
    )
    if (findIndexNaN !== -1 && !isEmpty(dataFilterDecimal)) {
      Alert.alert(
        "Warning",
        `The field type ${dataFilterDecimal[findIndexNaN]?.itemTypeOptionCode} have to be a number`,
      )
      return true
    }
    return false
  }

  const refreshItemTypeOptions = () => {
    if (
      (localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ||
        localDataArray.conditionRender === SEARCH_IMAGE_TYPE.SERVER) &&
      flagCheckUpNewImage == false
    ) {
      let dataItemTypeOptions = itemTypeArr?.itemTypeOptions
      let index = dataItemTypeOptions.find((x: any) => x.itemTypeOptionCode === "TotalCount")
      index.itemTypeOptionReturnValue = null
      setarrCheckQuatity([])
      setitemTypeArr(itemTypeArr)
      setCacheTotalCountServer([])
    }
    setflagDeleteMainPhoto(true)
    setMainPhoto(undefined)
    localDataArray.parentRowID = null
  }


  useEffect(() => {
    const filterTotalCount = dataItemType?.filter((it: any) => it?.itemTypeOptionCode === 'TotalCount')
    if (!isEmpty(filterTotalCount)) {
      filterTotalCount[0].itemTypeOptionReturnValue = arrCheckQuatity
    }
  }, [...[arrCheckQuatity]])

  const handleAddNewCase = () => {
    handleGetDataItemType()
    handleGetFloorsDataService()
    handleGetBuildingDataService()
    getMultipleTitle()
  }
  const handleEditCase = () => {
    let conditionExistEkyc = globalData?.chosed[0] ? globalData?.chosed[0] : []
    onSelectItemTypeEditData(conditionExistEkyc)
    handleGetDataItemType()
    handleGetFloorsDataService()
    handleGetBuildingDataService()
    setItemTypeNew(conditionExistEkyc)
    setData(globalData?.chosed[0])
    setconditionRenderTitle(true)
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
      if (arr[i].valType == 90 && arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "Building") {
        const itemReturnValue = arr[i].itemTypeOptionReturnValue[0]
        setcustBuidingName(itemReturnValue.returnValue)
        refDataBuilding.current = itemReturnValue
      }
      if (arr[i].valType == 3 && arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "Floor") {
        const itemReturnValue = arr[i]?.itemTypeOptionReturnValue[0]
        setcustFloor(itemReturnValue?.returnValue)
        refDataFloor.current = itemReturnValue
      }
      if (arr[i].valType == 30 && arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "GPS") {
        setcustGPS(arr[i].itemTypeOptionReturnValue[0].returnValue)
      }
      if (arr[i].valType == 1 && arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "AreaOrRoom") {
        setAreanumber(arr[i].itemTypeOptionReturnValue[0].returnValue)
      }
      if (
        arr[i].valType == 40 &&
        arr[i]?.itemTypeOptionReturnValue?.length > 0 &&
        isEdit
      ) {
        let arrTotalCountNotExisted = arr[i]?.itemTypeOptionReturnValue?.filter((it: any) => !it?.existedItems)
        setarrCheckQuatity(arrTotalCountNotExisted)
      }
      if (
        arr[i].valType == 90 &&
        arr[i]?.itemTypeOptionReturnValue?.length > 0 &&
        arr[i].itemTypeOptionCode != "Building"
      ) {
        const returnValue = isString(arr[i]?.itemTypeOptionReturnValue[0]?.returnValue)
          ? arr[i]?.itemTypeOptionReturnValue[0]?.returnValue
          : arr[i]?.itemTypeOptionReturnValue[0]?.returnValue.itemTypeOptionLineName
        setcustClient(returnValue)
      }
      if (
        arr[i].valType == 80 &&
        arr[i]?.itemTypeOptionReturnValue != null &&
        arr[i]?.itemTypeOptionReturnValue?.length > 0
      ) {
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
      if (
        arr[i].valType == 100 &&
        arr[i]?.itemTypeOptionReturnValue != null &&
        arr[i]?.itemTypeOptionReturnValue?.length > 0
      ) {
        const value = arr[i].itemTypeOptionReturnValue[0].returnValue
        if (value === "Metric") {
          setIsMetric(true)
          setIsInches(false)
        } else {
          setIsInches(true)
          setIsMetric(false)
        }
      }
      if (arr[i].valType === 40 && arr[i]?.itemTypeOptionCode === "TotalCount" && arr[i]?.itemTypeOptionReturnValue) {
        let arrTotalCount = arr[i]?.itemTypeOptionReturnValue
        let arrTotalCountExisted = arrTotalCount?.filter((it: any) => it?.existedItems)
        const removeDup = getUniqueListBy(arrTotalCountExisted, "conditionID")
        setCacheTotalCountServer(removeDup)
      }
    }
  }
  const handleGetDataItemType = () => {
    let url = `/api/ItemType/GetItemTypes?clientId=${parseInt(clientID)}`
    getItemTypeService(
      url,
      (res) => {
        {
          console.log("res handleGetDataItemType", res)
        }
        if (res.status == 200) {
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
    let url = `${URL_BASE}/api/ItemType/GetItemTypeOptionSet?clientId=${parseInt(clientID)}&itemId=${itemTypeId}` //&clientGroupId=${inventoryClientGroupID}
    getItemOptionSetTypeService(
      url,
      (res) => {
        console.log("res handleGetItemOptionSetTypeService", res)
        if (res.status == 200) {
          let data = res.data[0]
          !data?.inventoryId && delete data.inventoryId
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
      const dataBuilding: any = await AsyncStorage.getItem("@dataBuilding")
      const dataFloor: any = await AsyncStorage.getItem("@dataFloor")
      if (dataBuilding) {
        refDataBuilding.current = JSON.parse(dataBuilding)
      }
      if (dataFloor) {
        refDataFloor.current = JSON.parse(dataFloor)
      }
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
      if (!custBuidingNameRaw && !custFloorRaw && !custAreaNumberRaw) {
        setconditionRenderTitle(false)
      } else {
        setconditionRenderTitle(true)
      }
    } catch (e) {
      // read error
    }
  }

  const onSelectMethod = (item?: any) => {
    const mapManufacturer = {
      itemTypeOptionID: item?.manufacturerId,
      itemTypeOptionId: item?.manufacturerId,
      itemTypeOptionLineCode: item?.manufacturerName,
      itemTypeOptionLineID: item?.manufacturerId,
      itemTypeOptionLineName: item?.manufacturerName,
      manufacturerId: item?.manufacturerId,
      manufacturerName: item?.manufacturerName,
    }
    setcustClient(item.itemTypeOptionLineName || item.name)
    settitleDropDown(item.itemTypeOptionLineName || item.name)
    let arrCheck = itemTypeArr.itemTypeOptions
    for (let i = 0; i < arrCheck.length; i++) {
      if (arrCheck[i].itemTypeOptionCode === "Manufacturer") {
        setcustClient(mapManufacturer.manufacturerName)
        settitleDropDown(mapManufacturer.manufacturerName)
        refManufacturer.current = mapManufacturer
      } else if (arrCheck[i].itemTypeOptionId == mapManufacturer.itemTypeOptionID) {
        arrCheck[i].itemTypeOptionReturnValue = [{ returnValue: mapManufacturer }]
      }
    }
    setModalNumberCode(false)
    checkCompareOnBlur(-1)
  }

  const onSelectArea = (item: any) => {
    setAreanumber(item.name)
  }
  const handleSave = () => {
    saveDataItemType()
    if (isEdit) {
      let lastItem = globalData.item.length - 1
      itemTypeArr.clientID = clientID // doi sau
      let lastIndex = globalData?.index
      globalData.item[lastItem].dataItemType[lastIndex] = itemTypeArr
      setGlobalDataStore(globalData)
      handleSaveSqlite(globalData.item[lastItem].dataItemType)
      refIsSave.current = true
    }
    multiSet()
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
  const _getUserInfo = async () => {
    const user = await Storage.get("@User")
    const objUser = JSON.parse(`${user}`)
    let userId = objUser?.userId
    return userId
  }
  const _getCurrentTime = () => {
    let date = new Date().getDate() //Current Date
    let month = new Date().getMonth() + 1 //Current Month
    let year = new Date().getFullYear() //Current Year
    let hours = new Date().getHours() //Current Hours
    let min = new Date().getMinutes() //Current Minutes
    let sec = new Date().getSeconds() //Current Seconds
    let currentTime = date + "/" + month + "/" + year + " " + hours + ":" + min + ":" + sec
    return currentTime
  }
  const handleOnPress = async (isNavigate = true) => {
    const isRequire = checkIsRequire()
    if (isRequire) {
      Alert.alert("Warning", "Check to make sure you filled out all required fields")
      return
    }
    const isNaN = checkNumberWarning()
    if (isNaN) return
    saveDataItemType()
    multiSet()
    if (
      localDataArray.conditionRender == null &&
      localDataArray.parentRowID == null &&
      localDataArray.isEdit == false
    ) {
      // add new
      let lastItem = globalData.item.length - 1
      let lastIndex = globalData.item[lastItem].dataItemType.length - 1
      let IndexRow = globalData.item[lastItem].dataItemType.length + 1
      itemTypeArr.clientID = clientID
      itemTypeArr.conditionRender = localDataArray.conditionRender
      itemTypeArr.mainImageFull = mainPhoto
      itemTypeArr.mainImage = mainPhoto?.name
      itemTypeArr.createDateTime = _getCurrentTime()
      itemTypeArr.createID = await _getUserInfo()
      itemTypeArr.inventoryRowID = IndexRow // add InventoryAppId = indexRow + 1
      itemTypeArr.parentRowID = null
      itemTypeArr.InventoryItemID = null

      if (isSave) {
        globalData.item[lastItem].dataItemType[lastIndex] = itemTypeArr
        handleSaveSqlite(globalData.item[lastItem].dataItemType)
      } else {
        globalData.item[lastItem].dataItemType.push(itemTypeArr)
        handleSaveSqlite(globalData.item[lastItem].dataItemType)
      }
      isNavigate ? navigate(Routes.INVENTORY_DETAIL_LIST_SCREEN) : null
    } else if (
      localDataArray.isEdit != true &&
      (localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ||
        localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER)
    ) {
      // local
      compareDataItemType()
    }
    if (localDataArray.isEdit == true) {
      //edit
      await AsyncStorage.setItem("@DataInventory", JSON.stringify(globalData.item))
      if (localDataArray.conditionRender == null) {
        itemTypeArr.mainImageFull = mainPhoto
      }
      if (
        (localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ||
          localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER) &&
        localDataArray.parentRowID == null
      ) {
        itemTypeArr.mainImageFull = mainPhoto
        itemTypeArr.parentRowID = null
      }
      handleUpdateSqlite()
      isNavigate ? navigate(Routes.INVENTORY_DETAIL_LIST_SCREEN, { idClient }) : null
    }
    setGlobalDataStore(globalData)
  }

  const compareDataItemType = () => {
    let dataImage = JSON.parse(dataImageInfo) //data receive when searchimage case local/server
    let dataItemType = itemTypeArr?.itemTypeOptions // data itemtype now
    let filteredDataImage = dataImage.filter(
      (item: any) =>
        item.itemTypeOptionCode !== "Building" &&
        item.itemTypeOptionCode !== "GPS" &&
        item.itemTypeOptionCode !== "AreaOrRoom" &&
        item.itemTypeOptionCode !== "Floor" &&
        item.itemTypeOptionCode !== "TotalCount",
    )
    let filteredDataItemType = dataItemType.filter(
      (item: any) =>
        item.itemTypeOptionCode !== "Building" &&
        item.itemTypeOptionCode !== "GPS" &&
        item.itemTypeOptionCode !== "AreaOrRoom" &&
        item.itemTypeOptionCode !== "Floor" &&
        item.itemTypeOptionCode !== "TotalCount",
    )
    let dataItemTypeOptions = JSON.stringify(filteredDataItemType)
    let dataImageCompare = JSON.stringify(filteredDataImage)
    let compare = isEqual(dataImageCompare, dataItemTypeOptions)
    if (compare) {
      if (localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL) {
        itemTypeArr.parentRowID = imageInfo?.inventoryRowID
      } else if (localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER) {
        itemTypeArr.parentRowID = inventoryId
        console.log("Myle in server")
      }
    } else {
      console.log("Myle has changed option")
      if (
        localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ||
        localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER
      ) {
        console.log("Myle set parentRowID to NUll")
        itemTypeArr.parentRowID = null
        itemTypeArr.mainImageFull = mainPhoto
      }
    }
    let IndexRow = globalData.item[globalData.item.length - 1].dataItemType.length + 1
    itemTypeArr.inventoryRowID = IndexRow // add InventoryAppId = indexRow + 1
    let lastItem = globalData.item.length - 1
    itemTypeArr.clientID = clientID
    itemTypeArr.conditionRender = localDataArray.conditionRender
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
    if (localDataArray.conditionRender === SEARCH_IMAGE_TYPE.SERVER) {
      arrCheckQuatity?.map((it: any) => {
        it.existedItems = false
        it.representativePhotos = []
        return it
      })
    }
    const mapArrCheckQuantity = _.cloneDeep(arrCheckQuatity)
    mapArrCheckQuantity?.map((it: any) => {
      if (it?.dataCondition) {
        it.conditionData = it?.dataCondition
        delete it.dataCondition
      }
    })
    for (let i = 0; i < arrItemType.length; i++) {
      if (arrItemType[i].valType == 40) {
        arrItemType[i].itemTypeOptionReturnValue =
          cacheTotalCountServer && localDataArray.conditionRender === SEARCH_IMAGE_TYPE.SERVER
            ? [...cacheTotalCountServer, ...mapArrCheckQuantity]
            : mapArrCheckQuantity
      }
      if (arrItemType[i].valType == 80 && !isEmpty(arrCheck)) {
        let returnValue: { returnValue?: any }[] = (arrItemType[i].itemTypeOptionReturnValue = [])
        for (let j = 0; j < arrCheck.length; j++) {
          let value = arrItemType[i].itemTypeOptionLines[arrCheck[j]]
          returnValue.push({ returnValue: value })
        }
      }
      if (arrItemType[i].valType == 90 && arrItemType[i].itemTypeOptionCode == "Building" && custBuidingName) {
        if (localDataArray.isEdit) {
          arrItemType[i].itemTypeOptionReturnValue = [
            {
              returnValue: custBuidingName,
              returnID: refDataBuilding.current?.returnID,
              returnCode: refDataBuilding.current?.returnCode,
              returnName: refDataBuilding.current?.returnName,
              returnDesc: refDataBuilding.current?.returnDesc,
            },
          ]
        } else {
          arrItemType[i].itemTypeOptionReturnValue = [
            {
              returnValue: custBuidingName,
              returnID: refDataBuilding.current?.inventoryBuildingId,
              returnCode: refDataBuilding.current?.inventoryBuildingCode,
              returnName: refDataBuilding.current?.inventoryBuildingName,
              returnDesc: refDataBuilding.current?.inventoryBuildingDesc,
            },
          ]
        }
      }
      if (arrItemType[i].valType == 3 && arrItemType[i].itemTypeOptionCode == "Floor" && custFloor) {
        // Check again
        if (localDataArray.isEdit) {
          arrItemType[i].itemTypeOptionReturnValue = [
            {
              returnValue: custFloor,
              returnID: refDataFloor.current?.returnID,
              returnCode: refDataFloor.current?.returnCode,
              returnName: refDataFloor.current?.returnName,
              returnDesc: refDataFloor.current?.returnDesc,
            },
          ]
        } else {
          arrItemType[i].itemTypeOptionReturnValue = [
            {
              returnValue: custFloor,
              returnID: refDataFloor.current?.inventoryFloorId,
              returnCode: refDataFloor.current?.inventoryFloorCode,
              returnName: refDataFloor.current?.inventoryFloorName,
              returnDesc: refDataFloor.current?.inventoryFloorDesc,
            },
          ]
        }
      }
      const checkSomeManufacture = refManufacturer.current
        ? Object.values(refManufacturer.current)?.some((it: any) => !!it)
        : false
      if (arrItemType[i].valType == 90 && arrItemType[i].itemTypeOptionCode == "Manufacturer" && checkSomeManufacture) {
        arrItemType[i].itemTypeOptionReturnValue = [
          {
            returnValue: refManufacturer.current?.itemTypeOptionLineName || refManufacturer.current.name,
            returnID: refManufacturer.current?.itemTypeOptionLineID,
            returnCode: refManufacturer.current?.itemTypeOptionLineCode,
            returnName: refManufacturer.current?.itemTypeOptionLineName,
            returnDesc: null,
          },
        ]
      }
      if (arrItemType[i].valType == 30 && arrItemType[i].itemTypeOptionCode == "GPS" && custGPS) {
        arrItemType[i].itemTypeOptionReturnValue = [{ returnValue: custGPS }]
      }
      if (arrItemType[i].valType == 1 && arrItemType[i].itemTypeOptionCode == "AreaOrRoom" && Areanumber) {
        arrItemType[i].itemTypeOptionReturnValue = [{ returnValue: Areanumber }]
      }
      if (arrItemType[i].valType == 100 && isInches && !arrItemType[i].itemTypeOptionReturnValue) {
        arrItemType[i].itemTypeOptionReturnValue = [{ returnValue: "Inches" }]
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
    setitemTypeTxt(item?.itemName || item?.itemTypeName)
    setdataItemType(item?.itemTypeOptions)
    setitemTypeArr(item)
    if (item.parentRowID == null) {
      //add ,edit case
      setMainPhoto(item?.mainImageFull)
    }
    setmodalType(false)
  }
  const onSelectItemType = (item: any) => {
    setItemTypeNew(item)
    setmodalType(false)
    setitemTypeTxt(item.itemTypeName)
    setLoading(true)
    handleGetItemOptionSetTypeService(item.itemTypeId)
    handleResetItemType()
    handleResetData()
  }
  const handleResetData = () => {
    setvaluesTextInputTextParagraph("")
    setarrCheck([])
    setcustClient("")
    setarrCheckQuatity([])
    setIsInches(true)
    setIsMetric(false)
  }
  const handleResetItemType = () => {
    if (
      localDataArray.isEdit != true &&
      (localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ||
        localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER)
    ) {
      let change = {
        conditionRender: null,
        isEdit: false,
        parentRowID: null,
      }
      setlocalDataArray(change)
    }
  }
  const onSelectSharedModal = async (data: any) => {
    if (typeSharedModal == "Building") {
      setcustBuidingName(data.inventoryBuildingName)
      handleGetBuildingDataService()
      refDataBuilding.current = data
      await AsyncStorage.setItem("@custBuidingName", data.inventoryBuildingName)
      await AsyncStorage.setItem("@dataBuilding", JSON.stringify(data))
    } else if (typeSharedModal == "Floors") {
      setcustFloor(data.inventoryFloorName)
      refDataFloor.current = data
      await AsyncStorage.setItem("@custFloor", data.inventoryFloorName)
      await AsyncStorage.setItem("@dataFloor", JSON.stringify(data))
    }

    setopenSharedModal(false)
  }
  const changeItemType = () => {
    setmodalType(true)
  }
  const openModal = (item: any) => {
    const isManufacturer = item?.itemTypeOptionCode === "Manufacturer"
    setModalNumberCode(true)
    isManufacturer ? setcheckNV(manufacturerReducer?.data) : setcheckNV(item.itemTypeOptionLines)
    settitleDropDown(item.itemTypeOptionName)
    setitemTypeId(item.itemTypeId)
  }

  const handleOnEndEditing = (valuesTextInput?: any, item?: any) => {
    let arrCheck = itemTypeArr.itemTypeOptions
    for (let i = 0; i < arrCheck.length; i++) {
      if (arrCheck[i].itemTypeOptionId == item.itemTypeOptionId) {
        arrCheck[i].itemTypeOptionReturnValue = [{ returnValue: valuesTextInput }]
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
      { text: "Yes", onPress: () => removeImg(indexParent, indexChild, indexGrand) },
    ])
  }
  const removeImg = (indexParent: number, indexChild?: any, indexGrand?: any) => {
    let parentArr: { dataCondition: { url?: object[] }[] } = arrCheckQuatity[indexParent]
    let childArr: any = parentArr.dataCondition
      ? parentArr.dataCondition[indexChild].url
      : parentArr.conditionData[indexChild].url
    childArr.splice(indexGrand, 1)
    setarrCheckQuatity([...arrCheckQuatity])
  }
  const handleDeleteItems = (index?: number, dataParent?: any, indexParent?: any, indexChild?: any) => {
    let raw = dataParent?.dataCondition || dataParent?.conditionData
    raw.splice(indexChild, 1)
    if (raw?.length == 0) {
      arrCheckQuatity.splice(indexParent, 1)
    }
    setarrCheckQuatity([...arrCheckQuatity])
  }
  const renderQuantityTxt = (data?: any) => {
    if (data.type == "Damaged" || data.type == "MissingParts") {
      if (data?.dataCondition?.[0]?.itemName == "All Items" || data?.conditionData?.[0]?.itemName == "All Items") {
        return data.txtQuantity
      } else {
        return data?.dataCondition?.length || data?.conditionData?.length
      }
    } else {
      return data.txtQuantity
    }
  }

  const checkNumber = (value: any, key: any) => {
    console.log("key", key)
    if (isNaN(value)) {
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
  const onChangeTextArr = (txt?: any, idx?: any, index?: any, item?: any) => {
    const indexChangeTxt = errorRequire?.findIndex((__er: any) => __er?.itemTypeOptionCode === item?.itemTypeOptionCode)
    if (indexChangeTxt !== -1 && txt && !isEmpty(errorRequire)) {
      setErrorRequire((preState: any) => {
        preState?.splice(indexChangeTxt, 1)
        return preState
      })
    }
    let arr = [...dataItemType]
    if (arr[index].itemTypeOptionReturnValue == null || item?.itemTypeOptionReturnValue == "") {
      arr[index].itemTypeOptionReturnValue = [{ returnValue: txt }]
    } else {
      arr[index].itemTypeOptionReturnValue[idx].returnValue = txt
    }

    setdataItemType(arr)
  }
  const handleSetTxtNote = (txt?: any, index?: any, i?: any, item?: any) => {
    let arr = [...dataItemType]
    if (
      arr[index].itemTypeOptionReturnValue == null ||
      isEmpty(arr[index].itemTypeOptionReturnValue) ||
      localDataArray.conditionRender === SEARCH_IMAGE_TYPE.SERVER ||
      localDataArray.conditionRender === SEARCH_IMAGE_TYPE.LOCAL
    ) {
      let dataTotalCount = arrCheckQuatity[i]
      dataTotalCount.damageNotes = txt
      setarrCheckQuatity([...arrCheckQuatity])
    } else {
      let dataTotalCount = arr[index].itemTypeOptionReturnValue[i]
      dataTotalCount.damageNotes = txt
      setdataItemType([...dataItemType])
    }
  }

  const handleSetTxtNotePerItem = (txt?: any, index?: any, i?: any, item?: any, indexChild?: any) => {
    let arr = [...dataItemType]
    if (
      arr[index].itemTypeOptionReturnValue == null ||
      isEmpty(arr[index].itemTypeOptionReturnValue) ||
      localDataArray.conditionRender === SEARCH_IMAGE_TYPE.SERVER ||
      localDataArray.conditionRender === SEARCH_IMAGE_TYPE.LOCAL
    ) {
      let dataTotalCount: any = arrCheckQuatity[i]
      console.log('dataTotalCount', dataTotalCount)
      if (dataTotalCount?.dataCondition?.[indexChild]) {
        dataTotalCount.dataCondition[indexChild].damageNotes = txt
      } else {
        dataTotalCount.conditionData[indexChild].damageNotes = txt
      }

      setarrCheckQuatity([...arrCheckQuatity])
    } else {
      let dataTotalCount: any = arr[index].itemTypeOptionReturnValue[i]
      console.log('dataTotalCount', dataTotalCount, arr, index)
      if (dataTotalCount?.dataCondition?.[indexChild]) {
        dataTotalCount.dataCondition[indexChild].damageNotes = txt
      } else {
        dataTotalCount.conditionData[indexChild].damageNotes = txt
      }
      setdataItemType([...dataItemType])
    }
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
  const checkCompareOnBlur = (index?: any, item?: any) => {
    let arr = [...dataItemType]
    if (
      arr?.[index]?.itemTypeOptionReturnValue?.[0]?.returnValue <= 0 &&
      item?.fieldType == "Decimal" &&
      index !== -1
    ) {
      Alert.alert("Format wrong, please enter value again")
      arr[index].itemTypeOptionReturnValue = [{ returnValue: null }]
      setdataItemType(arr)
      return
    }
    if (arr?.[index]?.itemTypeOptionReturnValue?.[0]?.returnValue && item?.fieldType == "Decimal" && index !== -1) {
      const numberParse = parseFloat(arr[index].itemTypeOptionReturnValue?.[0]?.returnValue)
      arr[index].itemTypeOptionReturnValue = [{ returnValue: numberParse + "" }]
      setdataItemType(arr)
      return
    }
    if (
      (localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ||
        localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER) &&
      localDataArray?.parentRowID
    ) {
      // if (flagCheckResetQuantity == null) {
      //   setflagCheckResetQuantity(true)
      // } else {
      //   setflagCheckResetQuantity(!flagCheckResetQuantity)
      // }
      refreshItemTypeOptions()
    }
  }
  console.log("dataItemType", dataItemType)
  const checkKeyBoardType = (data: any) => {
    if (data.fieldType == "Decimal") {
      return "decimal-pad"
    }
    return "default"
  }
  const checkKeyType = (data: any) => {
    if (data.fieldType == "Decimal") {
      return "done"
    } else {
      return "default"
    }
  }

  const showImgSupportFile = (supportFile: any) => {
    if (supportFile?.filePath) {
      setUrlSupportFile(`${URL_BASE}${supportFile?.filePath}${supportFile?.fileName}`)
    } else if (supportFile?.fileName) {
      setUrlSupportFile(`${URL_BASE}/${supportFile?.fileName}`)
    } else {
      setUrlSupportFile(supportFile?.desc || "No infomation to show")
    }
  }

  const renderItemTypeFeild = ({ item, index }: RenderItemTypeFeildProps): any => {
    const isError = errorRequire?.some((__er: any) => item?.itemTypeOptionCode === __er?.itemTypeOptionCode)
    const inventorySupportFile = item?.inventorySupportFile
    const hasSupportFile = !!inventorySupportFile
    const isDecimal = item?.fieldType === "Decimal"
    if (item.valType == 1 && !item.isHide && item.itemTypeOptionCode != "Note") {
      if (item?.itemTypeOptionReturnValue == null || item?.itemTypeOptionReturnValue == "") {
        return (
          <View>
            <View style={{ flex: 1 }}>
              <View style={{ marginTop: 20, flex: 1 }}>
                <View style={{ flexDirection: "row", alignItems: "center" }}>
                  <Text style={{ fontSize: 15, marginBottom: 10 }}>
                    {item.itemTypeOptionName}
                    {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
                  </Text>
                  {hasSupportFile && (
                    <TouchableOpacity
                      style={styles.circlePressable}
                      onPress={() => showImgSupportFile(inventorySupportFile)}
                    >
                      <IconFontAwesome name="info-circle" size={16} />
                    </TouchableOpacity>
                  )}
                </View>
                <View style={[styles.btnDefault, { flexDirection: "row", backgroundColor: isError ? "red" : "white" }]}>
                  <View style={{ flex: 9 }}>
                    <TextInput
                      style={styles.txtSize16}
                      keyboardType={checkKeyBoardType(item)}
                      returnKeyType={checkKeyType(item)}
                      onBlur={() => checkCompareOnBlur(index, item)}
                      onChangeText={(txt) => onChangeTextArr(txt, 0, index, item)}
                      placeholder={isDecimal ? "0.00" : ""}
                      autoCapitalize="none"
                    />
                  </View>
                </View>
              </View>
            </View>
          </View>
        )
      } else {
        return (
          <View>
            {item?.itemTypeOptionReturnValue != "" &&
              item?.itemTypeOptionReturnValue?.map((i: any, idx: number) => {
                return (
                  <View style={{ flex: 1 }}>
                    <View style={{ marginTop: 20, flex: 1 }}>
                      <View style={{ flexDirection: "row", alignItems: "center" }}>
                        <Text style={{ fontSize: 15, marginBottom: 10 }}>
                          {item.itemTypeOptionName}
                          {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
                        </Text>
                        {hasSupportFile && (
                          <TouchableOpacity
                            style={styles.circlePressable}
                            onPress={() => showImgSupportFile(inventorySupportFile)}
                          >
                            <IconFontAwesome name="info-circle" size={16} />
                          </TouchableOpacity>
                        )}
                      </View>
                      <View
                        style={[
                          styles.btnDefault,
                          { flexDirection: "row", backgroundColor: isError ? "red" : "white" },
                        ]}
                      >
                        <View style={{ flex: 9 }}>
                          <TextInput
                            style={styles.txtSize16}
                            value={i.returnValue}
                            keyboardType={checkKeyBoardType(item)}
                            returnKeyType={checkKeyType(item)}
                            onBlur={() => checkCompareOnBlur(index, item)}
                            onChangeText={(txt) => onChangeTextArr(txt, idx, index, item)}
                            placeholder={isDecimal ? "0.00" : ""}
                            autoCapitalize="none"
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
        <View style={{ flex: 1, marginTop: 20 }}>
          <View style={{ flexDirection: "row", alignItems: "center" }}>
            <Text style={{ fontSize: 15, marginBottom: 10 }}>
              {item.itemTypeOptionName}
              {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
            </Text>
            {hasSupportFile && (
              <TouchableOpacity style={styles.circlePressable} onPress={() => showImgSupportFile(inventorySupportFile)}>
                <IconFontAwesome name="info-circle" size={16} />
              </TouchableOpacity>
            )}
          </View>
          <TouchableOpacity style={styles.btnDefault} onPress={() => openModal(item)}>
            <Text style={[styles.txtSize16, { marginVertical: 5 }]} maxFontSizeMultiplier={1}>
              {custClient}
            </Text>
            <Icon name={"list"} size={20} />
          </TouchableOpacity>
        </View>
      )
    } else if (item.valType == 90 && !item.isHide) {
      //DropdownPlus
      return (
        <View style={{ flex: 1, marginTop: 20 }}>
          <View style={{ flexDirection: "row", alignItems: "center" }}>
            <Text style={{ fontSize: 15, marginBottom: 10 }}>
              {item.itemTypeOptionName}
              {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
            </Text>
            {hasSupportFile && (
              <TouchableOpacity style={styles.circlePressable} onPress={() => showImgSupportFile(inventorySupportFile)}>
                <IconFontAwesome name="info-circle" size={16} />
              </TouchableOpacity>
            )}
          </View>
          <TouchableOpacity style={styles.btnDefault} onPress={() => openModal(item)}>
            <Text style={[styles.txtSize16, { marginVertical: 5 }]} maxFontSizeMultiplier={1}>
              {custClient}
            </Text>
            <Icon name={"list"} size={20} />
          </TouchableOpacity>
        </View>
      )
    } else if (item.valType == 2 && !item.isHide) {
      //TextParagraph
      return (
        <View style={{ marginTop: 20, flex: 1 }}>
          <View style={{ flexDirection: "row", alignItems: "center" }}>
            <Text style={{ fontSize: 15, marginBottom: 10 }}>
              {item.itemTypeOptionName}
              {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
            </Text>
            {hasSupportFile && (
              <TouchableOpacity style={styles.circlePressable} onPress={() => showImgSupportFile(inventorySupportFile)}>
                <IconFontAwesome name="info-circle" size={16} />
              </TouchableOpacity>
            )}
          </View>
          <View style={[styles.btnDefault, { justifyContent: "flex-start", minHeight: 100 }]}>
            <TextInput
              style={styles.txtSize16}
              multiline
              numberOfLines={4}
              onBlur={() => checkCompareOnBlur(-1)}
              onEndEditing={() => handleOnEndEditing(valuesTextInputTextParagraph, item)}
              value={valuesTextInputTextParagraph}
              onChangeText={(txt) => onTextChangedTxtFeild(item.itemTypeOptionId, txt)}
            />
          </View>
        </View>
      )
    } else if (item.valType == 40 && !item.isHide) {
      //CheckAllApply image
      return (
        <View style={{ flex: 1, marginTop: 20 }}>
          <View style={{ flexDirection: "row", alignItems: "center" }}>
            <Text style={{ fontSize: 15 }}>
              {item.itemTypeOptionName}
              {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
            </Text>
            {hasSupportFile && (
              <TouchableOpacity
                style={[styles.circlePressable, { marginBottom: 0 }]}
                onPress={() => showImgSupportFile(inventorySupportFile)}
              >
                <IconFontAwesome name="info-circle" size={16} />
              </TouchableOpacity>
            )}
          </View>
          {/* <Text style={{ fontSize: 15, flex: 1, color: Colors.textGray }}>
              {`Description: ${item.itemTypeOptionDesc}`}
            </Text> */}
          {console.log("arrCheckQuatity", arrCheckQuatity)}
          {conditionRender === SEARCH_IMAGE_TYPE.SERVER &&
            cacheTotalCountServer?.map((it: any) => {
              const filterConditonById = dataCondition?.filter(
                (__it: any) => __it?.inventoryItemConditionId === it?.conditionID,
              )
              return !isEmpty(it?.representativePhotos?.[0]?.url) ? (
                <View key={it?.conditionID}>
                  <View style={{ marginLeft: 15 }}>
                    <View style={{ flexDirection: "row" }}>
                      <View>
                        <Text style={{ fontSize: 15 }}>{filterConditonById?.[0]?.conditionName}</Text>
                      </View>
                    </View>
                    <FlatList
                      data={it?.representativePhotos?.[0]?.url}
                      contentContainerStyle={{ paddingBottom: 12 }}
                      horizontal={true}
                      keyExtractor={(item, index) => `${index}`}
                      renderItem={({ item, index }) => (
                        <View style={{ marginTop: 15 }}>
                          <View
                            style={{
                              width: 80,
                              height: 80,
                              marginTop: 0,
                              marginHorizontal: 15,
                            }}
                          >
                            <Image
                              source={{ uri: `${item.imageUrl}/${item.name}` }}
                              style={[styles.photo, { backgroundColor: "pink" }]}
                              resizeMode="cover"
                            />
                          </View>
                        </View>
                      )}
                    />
                  </View>
                </View>
              ) : null
            })}
          {arrCheckQuatity.map((data: any, i: number) => {
            // Check View
            let arr = data?.dataCondition || data?.conditionData
            const __data = {
              itemTypeOptionLineCode: data?.type,
              itemTypeOptionID: data?.conditionID,
              itemTypeOptionLineDesc: data?.conditionDescription,
              itemTypeOptionLineName: data?.conditionName,
            }
            const checkTotalImage = cacheTotalCountServer?.filter(
              (__it: any) => __it?.conditionID === data?.conditionID,
            )
            const haveTotalCountServer = checkTotalImage?.[0]?.representativePhotos?.[0]?.representativePhotoTotalCount
            const isDamagesAndMissing = (data?.conditionID === 5 || data?.conditionID === 6) && data?.txtQuantity < 5
            return (
              <View key={i} style={{ marginTop: 10 }}>
                {arr.length > 0 && (
                  <View style={{ flexDirection: "row", alignItems: "center" }}>
                    <TouchableOpacity
                      onPress={() => {
                        handleShowMorePicture(data)
                        refItemArrayQuantity.current = {
                          index,
                          data: __data,
                          item,
                        }
                      }}
                    >
                      <Text style={{ fontSize: 15, flex: 4, marginBottom: 10, color: "blue" }}>
                        {`${data.conditionName} QTY`} {"-"} {renderQuantityTxt(data)}
                      </Text>
                    </TouchableOpacity>
                    {haveTotalCountServer && (
                      <TouchableOpacity
                        onPress={() => {
                          setarrCheckQuatity((preState: any) => {
                            const findIndex = preState?.findIndex((it: any) => it?.conditionID === data?.conditionID)
                            findIndex !== -1 && preState?.splice(findIndex, 1)
                            return [...preState]
                          })
                        }}
                        style={{
                          justifyContent: "center",
                          alignItems: "center",
                          marginLeft: 16,
                          marginBottom: 10,
                        }}
                      >
                        <IconImage name={"delete"} size={15} />
                      </TouchableOpacity>
                    )}
                  </View>
                )}
                {!!data?.damageNotes && (
                  <>
                    <Text style={{ fontSize: 15, marginBottom: 10 }}>
                      {"Note: "}
                      <Text>{data?.damageNotes}</Text>
                    </Text>
                  </>
                )}

                {(isDamagesAndMissing || !haveTotalCountServer) &&
                  arr.map((dataImg: any, indexChild: number) => {
                    return isEmpty(dataImg?.url) ? null : (
                      <View key={indexChild}>
                        <View style={{ marginLeft: 15 }}>
                          <View style={{ flexDirection: "row" }}>
                            <View>
                              <Text style={{ fontSize: 15 }}>
                                {"- "}
                                {dataImg.itemName || dataImg?.itemTypeName}
                              </Text>
                            </View>
                            <TouchableOpacity
                              onPress={() => handleDeleteItems(index, data, i, indexChild)}
                              style={{
                                justifyContent: "center",
                                alignItems: "center",
                                marginLeft: 20,
                                marginBottom: 10,
                              }}
                            >
                              <IconImage name={"delete"} size={15} />
                            </TouchableOpacity>
                          </View>
                          <FlatList
                            data={dataImg.url}
                            contentContainerStyle={{ paddingBottom: 12 }}
                            horizontal={true}
                            keyExtractor={(item, index) => `${index}`}
                            renderItem={({ item, index }) => (
                              <View style={{ marginTop: 15 }}>
                                {(data.type == CONDITION_QUANTITY_TYPE.FAIR ||
                                  data.type == CONDITION_QUANTITY_TYPE.POOR ||
                                  data.type == CONDITION_QUANTITY_TYPE.GOOD ||
                                  dataImg.url.length > 1) && (
                                    <TouchableOpacity
                                      style={{
                                        width: 25,
                                        height: 25,
                                        position: "absolute",
                                        right: 5,
                                        top: -15,
                                      }}
                                      onPress={() => handleDelete(i, indexChild, index)}
                                    >
                                      <MaterialCommunityIcons name={"delete-circle"} size={25} />
                                    </TouchableOpacity>
                                  )}

                                <TouchableOpacity
                                  style={{
                                    width: 80,
                                    height: 80,
                                    marginTop: 0,
                                    marginHorizontal: 15,
                                  }}
                                  onPress={() => handleShowModalImage(item)}
                                  onLongPress={localDataArray.parentRowID == null ? onSetMainPhoto(item) : () => { }}
                                >
                                  <Image
                                    source={{ uri: item.uri }}
                                    style={[styles.photo, item.name === mainPhoto?.name && styles.activePhoto]}
                                    resizeMode="cover"
                                  />
                                </TouchableOpacity>
                              </View>
                            )}
                          />
                          {(data.type == CONDITION_QUANTITY_TYPE.DAMAGED ||
                            data.type == CONDITION_QUANTITY_TYPE.MISSING_PARTS) &&
                            dataImg?.itemName !== "All Items" ? (
                            <View
                              style={{ marginBottom: 32, flexDirection: "row", alignItems: "center", marginLeft: 15 }}
                            >
                              <Text style={{ fontSize: 15, marginBottom: 10 }}>
                                {`${dataImg?.itemName} Note`}
                                <Text style={{ color: "red" }}> *: </Text>
                              </Text>
                              <View
                                style={[
                                  styles.btnDefault,
                                  { flexDirection: "row", backgroundColor: isError ? "red" : "white" },
                                ]}
                              >
                                <View style={{ flex: 9 }}>
                                  <TextInput
                                    style={styles.txtSize16}
                                    onChangeText={(txt) => {
                                      handleSetTxtNotePerItem(txt, index, i, item, indexChild)
                                    }}
                                    placeholder={"Note is required"}
                                    autoCapitalize="words"
                                    value={dataImg?.damageNotes ? dataImg?.damageNotes : ""}
                                  />
                                </View>
                              </View>
                            </View>
                          ) : null}
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
            {!isEmpty(dataCondition) &&
              dataCondition.map((data: any, i?: any) => {
                const __data = {
                  itemTypeOptionLineCode: data?.conditionCode,
                  itemTypeOptionID: data?.inventoryItemConditionId,
                  itemTypeOptionLineDesc: data?.conditionDescription,
                  itemTypeOptionLineName: data?.conditionName,
                }
                const isInclude = arrCheckQuatity?.some((it: any) => it?.conditionID === data?.inventoryItemConditionId)
                return (
                  <TouchableOpacity
                    key={i}
                    onPress={() => {
                      handlePressCheckQuatity(i, __data),
                        (refItemArrayQuantity.current = {
                          index,
                          data: __data,
                          item,
                        })
                    }}
                    style={{
                      backgroundColor: isInclude ? "gray" : "white",
                      borderWidth: 0.4,
                      marginVertical: 5,
                      borderColor: "gray",
                      borderRadius: 18,
                      marginHorizontal: 5,
                    }}
                  >
                    <Text style={{ padding: 10, paddingHorizontal: 15 }}>{__data.itemTypeOptionLineCode}</Text>
                  </TouchableOpacity>
                )
              })}
          </View>
        </View>
      )
    } else if (item.valType == 30 && !item.isHide) {
      //gps
      return (
        <View style={{ flex: 1 }}>
          <View style={{ marginTop: 20, flex: 1 }}>
            <View style={{ flexDirection: "row", alignItems: "center" }}>
              <Text style={{ fontSize: 15, marginBottom: 10 }}>
                {item.itemTypeOptionName}
                {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
              </Text>
              {hasSupportFile && (
                <TouchableOpacity
                  style={styles.circlePressable}
                  onPress={() => showImgSupportFile(inventorySupportFile)}
                >
                  <IconFontAwesome name="info-circle" size={16} />
                </TouchableOpacity>
              )}
            </View>
            <View style={styles.btnDefault}>
              <TextInput
                numberOfLines={3}
                multiline
                onBlur={() => checkCompareOnBlur(-1)}
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
            <View style={{ flex: 1 }}>
              <View style={{ marginTop: 20, flex: 1 }}>
                <View style={{ flexDirection: "row", alignItems: "center" }}>
                  <Text style={{ fontSize: 15, marginBottom: 10 }}>
                    {item.itemTypeOptionName}
                    {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
                  </Text>
                  {hasSupportFile && (
                    <TouchableOpacity
                      style={styles.circlePressable}
                      onPress={() => showImgSupportFile(inventorySupportFile)}
                    >
                      <IconFontAwesome name="info-circle" size={16} />
                    </TouchableOpacity>
                  )}
                </View>
                <View style={[styles.btnDefault, { flexDirection: "row" }]}>
                  <View style={{ flex: 9 }}>
                    <TextInput
                      style={styles.txtSize16}
                      onBlur={() => checkCompareOnBlur(index, item)}
                      onChangeText={(txt) => onChangeTextArr(txt, 0, index, item)}
                    />
                  </View>
                  <TouchableOpacity onPress={() => addMoreTxt(0, index)} style={{ flex: 1 }}>
                    <IconImage name={"plus"} size={20} />
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
                <View style={{ flex: 1 }}>
                  <View style={{ marginTop: 20, flex: 1 }}>
                    <View style={{ flexDirection: "row", alignItems: "center" }}>
                      <Text style={{ fontSize: 15, marginBottom: 10 }}>
                        {item.itemTypeOptionName}
                        {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
                      </Text>
                      {hasSupportFile && (
                        <TouchableOpacity
                          style={styles.circlePressable}
                          onPress={() => showImgSupportFile(inventorySupportFile)}
                        >
                          <IconFontAwesome name="info-circle" size={16} />
                        </TouchableOpacity>
                      )}
                    </View>
                    <View style={[styles.btnDefault, { flexDirection: "row" }]}>
                      <View style={{ flex: 9 }}>
                        <TextInput
                          style={styles.txtSize16}
                          value={i.returnValue}
                          onBlur={() => checkCompareOnBlur(-1)}
                          onChangeText={(txt) => onChangeTextArr(txt, idx, index, item)}
                        />
                      </View>
                      {item?.itemTypeOptionReturnValue.length < parseInt(item.limitMax) && (
                        <TouchableOpacity onPress={() => addMoreTxt(idx, index)} style={{ flex: 1 }}>
                          <IconImage name={"plus"} size={20} />
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
        <View style={{ flex: 1 }}>
          <View style={{ marginTop: 20, flex: 1 }}>
            <View style={{ flexDirection: "row", alignItems: "center" }}>
              <Text style={{ fontSize: 15, marginBottom: 10 }}>
                {item.itemTypeOptionName}
                {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
              </Text>
              {hasSupportFile && (
                <TouchableOpacity
                  style={styles.circlePressable}
                  onPress={() => showImgSupportFile(inventorySupportFile)}
                >
                  <IconFontAwesome name="info-circle" size={16} />
                </TouchableOpacity>
              )}
            </View>
          </View>
          <View style={{ flex: 1, flexDirection: "row", flexWrap: "wrap" }}>
            {arrRaw?.map((data: any, i: number) => {
              return (
                <TouchableOpacity
                  key={i}
                  onPress={() => handlePressCheck(i)}
                  style={{
                    backgroundColor: arrCheck.includes(i) ? "gray" : "white",
                    borderRadius: 20,
                    marginHorizontal: 5,
                    marginTop: 5,
                  }}
                >
                  <Text style={{ padding: 10, paddingHorizontal: 15 }}>{data.itemTypeOptionLineName}</Text>
                </TouchableOpacity>
              )
            })}
          </View>
        </View>
      )
    } else if (item.valType == 100 && !item.isHide) {
      let arrRaw = item.itemTypeOptionLines
      return (
        <View style={{ flex: 1 }}>
          <View style={{ marginTop: 20, flex: 1 }}>
            <View style={{ flexDirection: "row", alignItems: "center" }}>
              <Text style={{ fontSize: 15, marginBottom: 10 }}>
                {item.itemTypeOptionName}
                {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
              </Text>
              {hasSupportFile && (
                <TouchableOpacity
                  style={styles.circlePressable}
                  onPress={() => showImgSupportFile(inventorySupportFile)}
                >
                  <IconFontAwesome name="info-circle" size={16} />
                </TouchableOpacity>
              )}
            </View>
          </View>
          <View style={{ flex: 1, flexDirection: "row" }}>
            {arrRaw.map((data: any, i: number) => {
              return (
                <View style={[styles.containerRowUnit, { marginRight: i == 0 ? 48 : 0 }]}>
                  <Text style={styles.txtUnit}>{data.itemTypeOptionLineName}</Text>
                  <RadioButton
                    value={data?.itemTypeOptionLineID == 14 ? isInches : isMetric}
                    onValueChange={(value: any) => {
                      if (data?.itemTypeOptionLineID === 14) {
                        if (!value) return
                        setIsInches(value)
                        value === true && setIsMetric(false)
                        onChangeTextArr(value ? "Inches" : "Metric", 0, index, item)
                      } else {
                        if (!value) return
                        setIsMetric(value)
                        value === true && setIsInches(false)
                        onChangeTextArr(value ? "Metric" : "Inches", 0, index, item)
                      }
                    }}
                  />
                </View>
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
  const handlePressCheckQuatity = async (_: any, data: any) => {
    const userInfo: any = await Storage.get("@User")
    const parseJsonUserInfo = await JSON.parse(userInfo)
    const isConditionOne = ["Good", "Fair", "Poor"].includes(data?.itemTypeOptionLineCode)
    const isConditionTwo = ["Damaged", "MissingParts"].includes(data?.itemTypeOptionLineCode)
    if (isConditionOne && parseJsonUserInfo?.inventory_user_accept_rules_reqd?.includes(1)) {
      Alert.alert(
        "Condition",
        "At least 1 photo per condition type is required. 5 is the maximum number of photos per item.",
        [
          {
            text: "Accept",
            onPress: async () => {
              const rs: any = await removeUserAcceptanceRules({ userId: parseJsonUserInfo?.userId, rules: "1" })
              if (rs?.status === 200) {
                const userInfoNew = {
                  ...parseJsonUserInfo,
                  inventory_user_accept_rules_reqd: parseJsonUserInfo?.inventory_user_accept_rules_reqd?.replace(
                    "1,",
                    "",
                  ),
                }
                await Storage.set("@User", JSON.stringify(userInfoNew))
                setconditionsType(data.itemTypeOptionLineCode)
                setmodalTypeCamera(true)
              } else {
                Alert.alert(`Error api (${rs?.status || 0})`)
              }
            },
          },
        ],
      )
    } else if (isConditionTwo && parseJsonUserInfo?.inventory_user_accept_rules_reqd?.includes(2)) {
      Alert.alert(
        "Condition",
        "1 photo that clearly shows the damaged or missing parts is required for each damaged or missing parts item. If the quantity of damaged or missing items is over 5 for the same product type, you may take 1 representative photo for all of them.",
        [
          {
            text: "Accept",
            onPress: async () => {
              const rs: any = await removeUserAcceptanceRules({ userId: parseJsonUserInfo?.userId, rules: "2" })
              if (rs?.status === 200) {
                const userInfoNew = {
                  ...parseJsonUserInfo,
                  inventory_user_accept_rules_reqd: parseJsonUserInfo?.inventory_user_accept_rules_reqd?.replace(
                    "2,",
                    "",
                  ),
                }
                await Storage.set("@User", JSON.stringify(userInfoNew))
                setconditionsType(data.itemTypeOptionLineCode)
                setmodalTypeCamera(true)
              } else {
                Alert.alert(`Error api (${rs?.status || 0})`)
              }
            },
          },
        ],
      )
    } else {
      setconditionsType(data.itemTypeOptionLineCode)
      setmodalTypeCamera(true)
    }
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
    checkCompareOnBlur(-1)
  }

  const funcSaveImage = (item: any) => {
    for (let i = 0; i < arrCheckQuatity.length; i++) {
      if (arrCheckQuatity[i].type == item.type) {
        arrCheckQuatity.splice(i, 1)
      }
    }
    arrCheckQuatity.push(item)
    setmodalTypeCamera(false)
  }

  const nextWithNote = async () => {
    const { index, data, item } = refItemArrayQuantity.current || {}
    modalTypeCameraAddMore ? funcUpdateImg(itemSaveForNote) : funcSaveImage(itemSaveForNote)
    const indexQuantity = arrCheckQuatity?.findIndex((it: any) => it?.type === data?.itemTypeOptionLineCode)
    handleSetTxtNote(noteTxt, index, indexQuantity, item)
    setItemSaveForNote(null)
    setNoteTxt("")
  }

  const checkImage = async (item: any) => {
    const filterDataCondition = dataCondition?.filter((it: any) => it?.conditionCode === item?.type)
    const __itemNew = {
      conditionName: item?.nameCondition,
      txtQuantity: item?.txtQuantity,
      conditionID: filterDataCondition?.[0]?.inventoryItemConditionId,
      dataCondition: item?.dataCondition,
      type: item?.type,
    }
    if (
      (item?.type === CONDITION_QUANTITY_TYPE.DAMAGED || item?.type === CONDITION_QUANTITY_TYPE.MISSING_PARTS) &&
      item?.txtQuantity >= 5
    ) {
      setItemSaveForNote(__itemNew)
      return
    }

    funcSaveImage(__itemNew)
  }

  const funcUpdateImg = async (item: any) => {
    for (let i = 0; i < arrCheckQuatity.length; i++) {
      if (arrCheckQuatity[i].type == item.type) {
        arrCheckQuatity.splice(i, 1)
      }
    }
    await arrCheckQuatity.push(item)
    setmodalTypeCameraAddMore(false)
    setflagCheckUpNewImage(true) //if upload new then no delete image when edit itemtype data (case 3,4)
  }

  const getUpdateImage = async (item: any) => {
    const filterDataCondition = dataCondition?.filter((it: any) => it?.conditionCode === item?.type)
    const __itemNew = {
      conditionName: item?.nameCondition,
      txtQuantity: item?.txtQuantity,
      conditionID: filterDataCondition?.[0]?.inventoryItemConditionId,
      dataCondition: item?.dataCondition,
      type: item?.type,
    }
    if (
      (item?.type === CONDITION_QUANTITY_TYPE.DAMAGED || item?.type === CONDITION_QUANTITY_TYPE.MISSING_PARTS) &&
      item?.txtQuantity >= 5
    ) {
      setItemSaveForNote(__itemNew)
      return
    }

    funcUpdateImg(item)
  }

  const addMoreTxt = (_: any, index: number) => {
    let arr = [...dataItemType]
    let arrayReturnValue = arr[index].itemTypeOptionReturnValue
    if (arrayReturnValue) {
      arrayReturnValue?.push({ returnValue: "" })
      setdataItemType(arr)
    } else {
      arr[index].itemTypeOptionReturnValue = []
      arr[index].itemTypeOptionReturnValue.push({ returnValue: "" }, { returnValue: "" })
      setdataItemType(arr)
    }
  }
  const handleOpenModal = (type: any) => {
    console.log('dataItemType', dataItemType)
    settypeSharedModal(type)
    settitleSharedModal(type)
    if (type == "Building") {
      setdataSharedModal(buildingData)
      setcanAddData(false)
      setopenSharedModal(true)
    } else if (type == "Floors") {
      const floorsFilter = cloneDeep(floor)?.filter((it: any) => it?.inventoryFloorId !== 0)
      setdataSharedModal(floorsFilter)
      setcanAddData(false)
      setopenSharedModal(true)
    }
  }

  const checkShowRedStart = (key: string) => {
    const indexFind = dataItemType?.findIndex((value: any) => value?.itemTypeOptionCode === key)
    if (indexFind != -1) {
      return dataItemType[indexFind]?.isRequired
    }
    return false
  }

  const onPressSearch = () => {
    navigate(Routes.SEARCH_IMAGE_SCREEN, { itemTypeId: itemTypeNew?.itemTypeId, clientId: clientID, idClient })
  }
  const handleSaveDataLocation = async (type: any) => {
    if (type == "gps") {
      await AsyncStorage.setItem("@custGPS", custGPS)
    } else {
      // arearoomnumber
      await AsyncStorage.setItem("@custAreaNumber", Areanumber)
    }
  }
  const getAndShowLocation = async () => {
    await getLocation()
    setTimeout(async () => {
      const custGPSRaw = await AsyncStorage.getItem("@custGPS")
      custGPSRaw && setcustGPS(custGPSRaw)
    }, 500)
  }
  const renderChoseTitle = () => {
    return (
      <>
        <View style={styles.choseTitleContainer}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              {/*Building Name {checkShowRedStart("Building") && <Text style={{ color: "red" }}>*</Text>}*/}
              Building Name <Text style={{ color: "red" }}>*</Text>
            </Text>
            <TouchableOpacity style={styles.btnDefault} onPress={() => handleOpenModal("Building")}>
              <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
                {custBuidingName}
              </Text>
              <Icon name={"list"} size={20} />
            </TouchableOpacity>
          </View>
        </View>
        <View style={styles.choseTitleContainer}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              {/*Floor {checkShowRedStart("Floor") && <Text style={{ color: "red" }}>*</Text>}*/}
              Floor <Text style={{ color: "red" }}>*</Text>
            </Text>
            <TouchableOpacity style={styles.btnDefault} onPress={() => handleOpenModal("Floors")}>
              <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
                {custFloor}
              </Text>
              <Icon name={"list"} size={20} />
            </TouchableOpacity>
          </View>
        </View>
        <View style={styles.choseTitleContainer}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              {/*GPS{checkShowRedStart("GPS") && <Text style={{ color: "red" }}>*</Text>}*/}
              GPS <Text style={{ color: "red" }}>*</Text>
            </Text>
            <View style={styles.btnDefault}>
              <TextInput
                style={styles.txtSize16}
                numberOfLines={2}
                onBlur={() => handleSaveDataLocation("gps")}
                maxFontSizeMultiplier={1}
                onChangeText={(text) => setcustGPS(text)}
              >
                {custGPS}
              </TextInput>
              <TouchableOpacity onPress={getAndShowLocation}>
                <Icon name={"location"} size={20} />
              </TouchableOpacity>
            </View>
          </View>
        </View>

        <View style={styles.choseTitleContainer}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              {/*Area or Room number {checkShowRedStart("AreaOrRoom") && <Text style={{ color: "red" }}>*</Text>}*/}
              Area or Room number <Text style={{ color: "red" }}>*</Text>
            </Text>
            <View style={styles.btnDefault}>
              <TextInput
                style={styles.txtSize16}
                maxFontSizeMultiplier={1}
                onBlur={() => handleSaveDataLocation("areaNumber")}
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
  ////
  const renderTitle = () => {
    return (
      <>
        <View style={{ marginTop: 12 }}>
          <Text style={{ fontStyle: "italic" }}>Time created: {timeEdited}</Text>
          <View style={styles.containerRow}>
            <Text style={styles.txtTitleOps}>{`Building : ${custBuidingName}`} </Text>
            <TouchableOpacity onPress={() => setconditionRenderTitle(false)}>
              <MaterialCommunityIcons name={"pencil-outline"} size={25} color={Colors.black} />
            </TouchableOpacity>
          </View>
          <View style={styles.containerRow}>
            <Text style={styles.txtTitleOps}>{`Floor : ${custFloor} `} </Text>
            <TouchableOpacity onPress={() => setconditionRenderTitle(false)}></TouchableOpacity>
          </View>
          <View style={styles.containerRow}>
            <Text style={styles.txtTitleOps}>{`GPS : ${custGPS}`}</Text>
            <TouchableOpacity onPress={() => setconditionRenderTitle(false)}></TouchableOpacity>
          </View>
          <View style={styles.containerRow}>
            <Text style={styles.txtTitleOps}>{`Area or Room number : ${Areanumber}`}</Text>
            <TouchableOpacity onPress={() => setconditionRenderTitle(false)}></TouchableOpacity>
          </View>
        </View>
      </>
    )
  }

  const validateRequired = () => {
    const filterDamage: any = arrCheckQuatity?.filter((it: any) => it?.type === "Damaged")
    const filterMissingPart: any = arrCheckQuatity?.filter((it: any) => it?.type === "MissingParts")
    const checkNotesDamages = filterDamage?.[0]?.dataCondition ? !filterDamage?.[0]?.dataCondition?.some((_it: any) => !_it?.damageNotes) : !filterDamage?.[0]?.conditionData?.some((_it: any) => !_it?.damageNotes)
    const checkNoteMissingparts = filterMissingPart?.[0]?.dataCondition ? !filterMissingPart?.[0]?.dataCondition?.some((_it: any) => !_it?.damageNotes) : !filterMissingPart?.[0]?.conditionData?.some((_it: any) => !_it?.damageNotes)
    const checkNoteDamage =
      isEmpty(filterDamage) ||
      !!filterDamage?.[0]?.damageNotes ||
      checkNotesDamages
    const checkNoteMissingParts =
      isEmpty(filterMissingPart) ||
      !!filterMissingPart?.[0]?.damageNotes ||
      checkNoteMissingparts
    if (
      dataItemType?.length > 0 &&
      arrCheckQuatity?.length > 0 &&
      custBuidingName &&
      custFloor &&
      custGPS &&
      Areanumber &&
      checkNoteDamage &&
      checkNoteMissingParts
      // &&isNoteHaveValue
    ) {
      return false
    } else {
      return true
    }
  }
  const renderChoseItemType = () => {
    return (
      <View style={{ flex: 1 }}>
        <View style={{ marginTop: 20, flex: 1 }}>
          <Text style={{ fontSize: 15, flex: 4, marginBottom: 10 }}>
            Item Type <Text style={{ color: "red" }}>*</Text>
          </Text>
          <TouchableOpacity disabled={disableChoseItemType} style={styles.btnDefault} onPress={() => changeItemType()}>
            <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
              {itemTypeTxt}
            </Text>
            <Icon name={"list"} size={20} />
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
    if ((!mainPhoto || (mainPhoto && mainPhoto.isAuto)) && !!arrCheckQuatity?.length && !isEdit) {
      const goodCondition = arrCheckQuatity.filter((i: any) => i?.type === CONDITION_QUANTITY_TYPE.GOOD)
      const fairCondition = arrCheckQuatity.filter((i: any) => i?.type === CONDITION_QUANTITY_TYPE.FAIR)
      const poorCondition = arrCheckQuatity.filter((i: any) => i?.type === CONDITION_QUANTITY_TYPE.POOR)
      const damagedCondition = arrCheckQuatity.filter((i: any) => i?.type === CONDITION_QUANTITY_TYPE.DAMAGED)
      const missingPartCondition = arrCheckQuatity.filter((i: any) => i?.type === CONDITION_QUANTITY_TYPE.MISSING_PARTS)
      const arrCheckQuatitySorted: any = [
        ...goodCondition,
        ...fairCondition,
        ...poorCondition,
        ...damagedCondition,
        ...missingPartCondition,
      ]
      const mainPhoto = arrCheckQuatitySorted[0]?.dataCondition[0]?.url[0]
      if (
        localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ||
        localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER
      ) {
        if (flagDeleteMainPhoto) {
          setMainPhoto({ ...mainPhoto, isAuto: true })
        }
      } else {
        // case add new / edit
        setMainPhoto({ ...mainPhoto, isAuto: true })
      }
    }
  }, [JSON.stringify(arrCheckQuatity), JSON.stringify(mainPhoto)])
  const ListHeaderComponent = useMemo((): any => {
    const uri =
      localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL
        ? localDataArray.isEdit
          ? itemTypeArr?.mainImageFull?.uri
          : imageInfo?.mainImageFull?.uri
        : localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER
          ? localDataArray.isEdit
            ? `${SERVER_NAME}/${clientID}/${itemTypeArr?.mainPhoto || itemTypeArr?.mainImage}`
            : `${SERVER_NAME}/${clientID}/${imageInfo?.mainPhoto || imageInfo?.mainImage}`
          : null

    return (
      !!dataItemType?.length && (
        <View>
          {localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER ||
            localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ? (
            <View style={{}}>
              <Text style={{ fontSize: fontSizer(16), fontWeight: "bold", fontStyle: "italic" }}>Main Photo </Text>
              <View
                style={{
                  width: 80,
                  height: 80,
                  marginTop: 10,
                  marginHorizontal: 15,
                }}
              >
                <Image source={{ uri }} style={[styles.photo, styles.activePhoto]} resizeMode="cover" />
              </View>
            </View>
          ) : null}
        </View>
      )
    )
  }, [dataItemType, itemTypeNew])
  return (
    <View style={styles.container}>
      <Header
        isSaved={true}
        handleSave={() => handleOnPress()}
        isGoBack
        actionGoBack={() => {
          !refIsSave.current && setGlobalDataStore(globalDataCache.current)
          goBack()
        }}
        conditionRender={localDataArray.conditionRender}
        labels={"Inventory Collection"}
      />
      <LoadingOverlay visible={loading} />
      <Text style={styles.noteTxt}>Note: * is required fields</Text>
      <KeyboardAvoidingView
        behavior={Platform.OS === "ios" ? "padding" : "height"}
        // bounces={false}
        // showsVerticalScrollIndicator={false}
        // extraScrollHeight={150}
        // enableOnAndroid={true}
        style={styles.subContainer}
      >
        <ScrollView showsVerticalScrollIndicator={false}>
          {!conditionRenderTitle ? renderChoseTitle() : renderTitle()}
          {renderChoseItemType()}
          <TouchableOpacity onPress={() => onPressSearch()} style={styles.buttonSearchImg}>
            <Text style={{ fontSize: fontSizer(16), fontWeight: "bold", color: "blue" }}>Search Photo </Text>
          </TouchableOpacity>
          <FlatList
            ListHeaderComponent={localDataArray.parentRowID != null ? ListHeaderComponent : null}
            data={dataItemType}
            style={{ marginBottom: 100 }}
            renderItem={(item) => renderItemTypeFeild(item)}
            extraData={[...errorRequire]}
            keyExtractor={(item, index) => `${index}`}
          />
        </ScrollView>
      </KeyboardAvoidingView>
      <BottomButtonNext disabled={validateRequired()} onPressButton={() => handleOnPress()} />

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
          canSearch={true}
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
          // isShowChooseMainPhoto={(localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL || localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER) && flagDeleteMainPhoto == false ? false : isEdit ? false : true }
          isShowChooseMainPhoto={localDataArray.parentRowID == null ? true : false}
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
          addMore={isInventoryClientGroupID}
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
          conditionRender={localDataArray.conditionRender}
          parentRowID={localDataArray.parentRowID}
          title={"Chose Item Type number?"}
          type={conditionsType}
          onSelect={(item) => checkImage(item)}
          onClosed={() => setmodalTypeCamera(false)}
          onOpened={() => setmodalTypeCamera(true)}
          arrCheckQuantityServer={cacheTotalCountServer}
          conditionData={dataCondition}
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
          arrCheckQuantityServer={cacheTotalCountServer}
          conditionData={dataCondition}
        />
      )}
      <Modal visible={!!urlSupportFile} onRequestClose={() => setUrlSupportFile("")} transparent animationType="slide">
        <TouchableOpacity activeOpacity={1} style={styles.containerViewModal} onPressOut={() => setUrlSupportFile("")}>
          {urlSupportFile?.includes(URL_BASE) ? (
            <Image source={{ uri: urlSupportFile }} style={styles.imgSupportFile} />
          ) : (
            <View style={styles.containerContent}>
              <Text style={[styles.txtDesc, { fontWeight: "bold" }]}>{"Photo Rules:"}</Text>
              <Text style={styles.txtDesc}>{urlSupportFile?.toString()}</Text>
            </View>
          )}
        </TouchableOpacity>
      </Modal>

      <ModalFullScreen
        backdrop={true}
        isOpen={!!itemSaveForNote}
        containerStyle={{ paddingVertical: 16 }}
        onClosed={() => {
          setItemSaveForNote(null)
          setNoteTxt("")
        }}
        useNativeDriver={true}
        backdropPressToClose={true}
        style={styles.modalFullScreen}
        element={
          <View style={styles.containerAddNew}>
            <Text style={{ fontSize: 16 }}>
              {itemSaveForNote?.type === "Damaged" ? "Describe damage:" : "Describe missing parts:"}
            </Text>
            <View
              style={{
                borderBottomWidth: 1,
                borderColor: Colors.gray_C4,
                flexDirection: "row",
              }}
            >
              <TextInput
                placeholder={"Note is required"}
                onChangeText={(text) => setNoteTxt(text)}
                value={noteTxt}
                multiline
                numberOfLines={4}
                style={{
                  minHeight: 40,
                  flex: 1,
                }}
              />
            </View>
            <View style={styles.rowBtn}>
              <BaseButton
                backgroundColor={Colors.redHeader}
                label={"Cancel"}
                fontSize={responsiveW(16)}
                width={responsiveW(128)}
                onPress={() => setItemSaveForNote(null)}
                marginRight={responsiveW(32)}
              />
              <BaseButton
                backgroundColor={Colors.redHeader}
                label={"Save"}
                fontSize={responsiveW(16)}
                width={responsiveW(128)}
                onPress={nextWithNote}
                disabled={!noteTxt}
              />
            </View>
          </View>
        }
      />
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
  },
  circlePressable: {
    marginLeft: 8,
    marginBottom: 10,
  },
  imgSupportFile: {
    width: 200,
    height: 200,
    borderRadius: 10,
    backgroundColor: "white",
  },
  containerViewModal: {
    flex: 1,
    backgroundColor: "#0009",
    alignItems: "center",
    justifyContent: "center",
  },
  button: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    flexDirection: "row",
    paddingVertical: 5,
  },
  btnContainer: {
    alignItems: "center",
    flexDirection: "row",
    backgroundColor: Colors.button,
    paddingHorizontal: responsiveW(20),
    borderRadius: 30,
    marginTop: 16,
  },
  txtButton: {
    paddingVertical: responsiveH(10),
    fontSize: fontSizer(16),
    marginLeft: responsiveW(10),
    color: "#fff",
  },
  containerRow: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
  },
  containerRowUnit: {
    flexDirection: "row",
    flex: 1,
    alignItems: "flex-start",
  },
  txtUnit: {
    fontSize: fontSizer(16),
  },
  noteTxt: {
    fontSize: 16,
    fontWeight: "bold",
    color: Colors.redHeader,
    marginLeft: 16,
    marginTop: 8,
  },
  modalFullScreen: {
    alignSelf: "center",
    justifyContent: "center",
    backgroundColor: "transparent",
  },
  containerAddNew: {
    backgroundColor: "white",
    marginHorizontal: responsiveW(20),
    padding: responsiveW(16),
    overflow: "hidden",
  },
  rowBtn: {
    flexDirection: "row",
    justifyContent: "center",
    marginTop: responsiveW(16),
  },
  containerContent: {
    backgroundColor: Colors.white,
    padding: 16,
    borderRadius: 8,
    alignItems: "center",
  },
  txtDesc: {
    fontSize: 16,
  },
})
export default InventoryDetailScreen
