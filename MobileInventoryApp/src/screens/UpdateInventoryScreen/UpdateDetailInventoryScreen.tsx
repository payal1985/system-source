import { useNavigation, useRoute } from "@react-navigation/native"

import React, { useCallback, useState, useEffect, useMemo, useRef } from "react"
import { Image, StyleSheet, Text, Alert, FlatList, TouchableOpacity, View, Modal } from "react-native"
import { TextInput } from "react-native-gesture-handler"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import Icon from "react-native-vector-icons/Ionicons"
import IconFontAwesome from "react-native-vector-icons/FontAwesome5"
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
  BaseBottomSheetCameraUpdateInventory
} from "../../components"
import { fontSizer, responsiveH, responsiveW } from "../../utils/dimension"
import IconImage from "react-native-vector-icons/AntDesign"
import MaterialIcons from "react-native-vector-icons/MaterialIcons"
import useGlobalData from "/utils/hook/useGlobalData"
import { SERVER_NAME, URL_BASE, SEARCH_IMAGE_TYPE, CONDITION_QUANTITY_TYPE, update_arr, getUniqueListBy } from "/constant/Constant"
import { findIndex, forEach, get, isEmpty, isNaN, cloneDeep, isString } from "lodash"
import { updateDataInventoryClientByQuery } from "/utils/sqlite/tableInventoryClient"
import {
  getItemTypeService,
  getItemOptionSetTypeService,
  getFloorsDataService,
  getBuildDataService,
  getInventoryItemInfo,
  getStatusGroup,
  updateInventoryItem,
  getCondition,
  inventoryImageDelete
} from "/redux/progress/service"
import useLanguage from "/utils/hook/useLanguage"
import { getLocation } from "../../utils"
import _ from "lodash"
import { useSelector } from "react-redux"
import { AppReducerType } from "/redux/reducers"
import { saveNewManufacturerService } from "/redux/progress/service"
import { useDispatch } from "react-redux"
import { manufacturer } from "/redux/manufacturer/actions"
import ModalList from './ModalList'
import MaterialCommunityIcons from "react-native-vector-icons/MaterialCommunityIcons";

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

const UpdateDetailInventoryScreen = () => {
  const { navigate } = useNavigation()
  const [language] = useLanguage()
  const route = useRoute()
  const idClient = get(route, "params.idClient")
  const conditionRender = get(route, "params.conditionRender")
  const inventoryId = get(route, "params.inventoryId")
  const isEdit = get(route, "params.isEdit")
  const parentRowID = get(route, "params.parentRowID")
  const inventoryItemIdRaw = get(route, "params.inventoryItemId")
  const totalQuantity = get(route, "params.totalQuantity")
  const [localDataArray, setlocalDataArray] = useState({
    conditionRender: conditionRender,
    isEdit: isEdit,
    parentRowID: parentRowID,
  })
  const [custClient, setcustClient] = useState("")
  const [custBuidingName, setcustBuidingName] = useState("")
  const [custFloor, setcustFloor] = useState("")
  const [custGPS, setcustGPS] = useState("")
  const [custStatus, setcustStatus] = useState("")
  const [statusData, setstatusData] = useState("")
  const [modalNumberCode, setModalNumberCode] = useState(false)
  const [imageInfo, setimageInfo] = useState<any>("")
  const [noteItem, setnoteItem] = useState("")
  const [dataDropdownCondition, setdataDropdownCondition] = useState([
    { name: "Good" },
    { name: "Fair" },
    { name: "Poor" },
    { name: "Damaged" },
    { name: "MissingParts" },
  ])
  const [custCondition, setcustCondition] = useState("")

  const [globalData] = useGlobalData()

  const [Areanumber, setAreanumber] = useState("")
  const [itemTypeTxt, setitemTypeTxt] = useState("")
  const [titleDropDown, settitleDropDown] = useState("Options")
  const [dataItemType, setdataItemType] = useState<any>([])
  const [conditionRenderTitle, setconditionRenderTitle] = useState(false)
  const [modalTypeCamera, setmodalTypeCamera] = useState(false)
  const [modalTypeCameraUpdate, setmodalTypeCameraUpdate] = useState(false)
  const [modalShowImage, setmodalShowImage] = useState(false)
  const [modalTypeCameraAddMore, setmodalTypeCameraAddMore] = useState(false)
  const [showImageModal, setshowImageModal] = useState<Photos>()
  const [arrCheck, setarrCheck] = useState<any[]>([])
  const [arrCheckQuatity, setarrCheckQuatity] = useState<{ type?: any }[]>([])
  const [dataImageItem, setdataImageItem] = useState({})
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
  const [isInches, setIsInches] = useState(false)
  const [isMetric, setIsMetric] = useState(false)
  const [isInventoryClientGroupID, setIsInventoryClientGroupID] = useState(false)
  const manufacturerReducer = useSelector((state: any) => state?.[AppReducerType.MANUFACTURER])
  const refDataBuilding = useRef<any>(null)
  const refDataFloor = useRef<any>(null)
  const refManufacturer = useRef<any>(null)
  const [isOpenModal, setIsOpenModal] = useState(false)
  const [arrayTestUpdate, setarrayTestUpdate] = useState(update_arr)
  const [dataCondition, setDataCondition] = useState<any>(null)
  const [idxAddItemPhoto, setidxAddItemPhoto] = useState<any>(null)
  const [InventoryID, setInventoryID] = useState<any>(null)
  const [conditionID, setconditionID] = useState<any>(null)
  const [quantityByID, setQuantityByID] = useState<any>([])
  const dispatch = useDispatch()


  useEffect(() => {
    handleGetCondition()
    setimageInfo({ ...globalData?.chosed[0], clientId: clientID })
    setInventoryID(globalData?.chosed[0].inventoryId)

  }, [])
  const handleGetCondition = async () => {
    const rs = await getCondition()
    if (rs?.status === 200) {
      await setDataCondition(rs?.data)
      if (isEdit) {
        handleSetData({ ...globalData?.chosed[0], clientId: clientID })
        // await   setimageInfo(arrayTestUpdate[0])
        // await  handleSetData(arrayTestUpdate[0])
      } else {
        handleGetInventoryItemInfo()
      }
    } else {
      Alert.alert(`Error api get condition (${rs?.status || 0})`)
    }

  }
  const handleGetStatus = (dataInventory) => {
    let url = `${URL_BASE}/api/Status/GetStatusGroup?statusType=General`
    getStatusGroup(
      url,
      (res) => {
        if (res.status == 200) {
          setstatusData(res.data)

          let dataStatus = res.data
          let statusId = isEdit ? globalData?.chosed[0].statusId : dataInventory.statusId
          for (let i = 0; i < dataStatus.length; i++) {
            if (dataStatus[i].statusId === statusId) {
              setcustStatus(dataStatus[i])
            }
          }
        } else {
          setLoading(false)
          BaseAlert(res, "Get handleGetStatus")
        }
      },
      (e) => {
        setLoading(false)
        BaseAlert(e, "Get handleGetStatus")
      },
    )
  }
  const handleGetInventoryItemInfo = () => {
    getInventoryItemInfo(
      inventoryItemIdRaw,
      // 1919,
      (res) => {
        setimageInfo(res.data[0])
        handleSetData(res.data[0])
        setInventoryID(res.data[0].inventoryId)

      },
      (e) => {
        Alert.alert("Error", "Invalid Barcode", [
          {
            text: "OK",
            onPress: () => navigate(Routes.INVENTORY_DETAIL_LIST_SCREEN),

            style: "cancel",
          },
        ])
      },
      idClient,
    )
  }
  const handleSetData = (imageInfoRaw) => {
    let dataInventory = imageInfoRaw
    setflagCheckSetData(!flagCheckSetData)
    handleGetStatus(dataInventory)
    onSelectItemTypeEditData(dataInventory)
    handleGetDataItemType()
    getMultipleTitle() // get 4 data location
    handleGetFloorsDataService()
    handleGetBuildingDataService()
    setData(dataInventory)
    setItemTypeNew(dataInventory)
    setnoteItem(dataInventory.noteForItem)
    setcustCondition(dataInventory.condition)
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
      const valueManufacturer: string = _.get(
        itemManufacturer,
        "itemTypeOptionReturnValue.[0].returnValue.itemTypeOptionLineName",
        null,
      )
      if (!valueManufacturer) return
      const checkSomeManufacturer = manufacturerReducer?.data?.some(
        (item: { itemTypeOptionLineName: string }) =>
          item?.itemTypeOptionLineName?.toUpperCase() === valueManufacturer?.toUpperCase(),
      )
      if (!checkSomeManufacturer) {
        let dataBody = {
          ManufacturerName: valueManufacturer,
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
    }
    setflagDeleteMainPhoto(true)
    setMainPhoto(undefined)
    localDataArray.parentRowID = null
  }

  const setData = (data: any) => {
    console.log("datadatadata", data)
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
      if (arr[i].valType == 40 && arr[i]?.itemTypeOptionReturnValue?.length > 0) {
        if (!isEdit) {
          setcustCondition(arr[i]?.itemTypeOptionReturnValue[0]?.nameCondition)
        }
      }
      if (arr[i].valType == 90 && arr[i]?.itemTypeOptionReturnValue?.length > 0) {
        const returnValue = isString(arr[i]?.itemTypeOptionReturnValue[0]?.returnValue)
          ? arr[i]?.itemTypeOptionReturnValue[0]?.returnValue
          : arr[i]?.itemTypeOptionReturnValue[0]?.returnValue.itemTypeOptionLineName
        setcustClient(returnValue)
      }
      if (
        arr[i].valType == 80 &&
        arr[i]?.itemTypeOptionReturnValue != null &&
        arr[i]?.itemTypeOptionReturnValue.length > 0
      ) {
        let arrChek = []
        let arrPick = arr[i]?.itemTypeOptionReturnValue
        let arrOptionLines = arr[i].itemTypeOptionLines
        for (let k = 0; k < arrOptionLines.length; k++) {
          for (let z = 0; z < arrPick.length; z++) {
            if (arrPick[z].returnValue?.trim() == arrOptionLines[k].itemTypeOptionLineName?.trim()) {
              arrChek.push(k)
            }
          }
        }
        setarrCheck(arrChek)
      }
      let arrayMerge: any[] = []
      if (arr[i].valType == 40 && arr[i]?.itemTypeOptionReturnValue?.length > 0) {
        arr[i]?.itemTypeOptionReturnValue?.forEach((it: any) => {
          const findIndex = arrayMerge?.findIndex((__it: any) => __it?.conditionID === it?.conditionID)
          if (findIndex !== -1) {
            arrayMerge[findIndex].txtQuantity = arrayMerge?.[findIndex]?.txtQuantity + it?.txtQuantity

          } else {
            arrayMerge?.push(it)
          }
        })
        setQuantityByID(arrayMerge)
        const removeDup = getUniqueListBy(arr[i]?.itemTypeOptionReturnValue, 'conditionID')
        setarrCheckQuatity(removeDup)
      }
      if (arr[i].valType == 100) {
        arr[i]?.itemTypeOptionReturnValue?.[0]?.returnValue === "Metric" ? setIsMetric(true) : setIsInches(true)
      }
    }
  }
  console.log('conditionID', quantityByID)
  const handleGetDataItemType = () => {
    let url = `/api/ItemType/GetItemTypes?clientId=${parseInt(clientID)}`
    getItemTypeService(
      url,
      (res) => {

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
      manufacturerName: item?.manufacturerName
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
    checkCompareOnBlur()
  }

  const onSelectArea = (item: any) => {
    setAreanumber(item.name)
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
    saveDataItemType()
    if (
      localDataArray.conditionRender == null &&
      localDataArray.parentRowID == null &&
      localDataArray.isEdit == false
    ) {
      // add new
      let lastItem = globalData.item.length - 1
      let lastIndex = globalData.item[lastItem].dataItemType.length - 1
      let IndexRow = globalData.item[lastItem].dataItemType.length + 1
      itemTypeArr.conditionRender = 5
      itemTypeArr.updateDateTime = _getCurrentTime()
      itemTypeArr.inventoryRowID = IndexRow // add InventoryAppId = indexRow + 1
      itemTypeArr.parentRowID = null
      itemTypeArr.statusId = custStatus.statusId
      itemTypeArr.noteForItem = noteItem
      itemTypeArr.condition = custCondition
      itemTypeArr.InventoryItemID = inventoryItemIdRaw
      if (isSave) {
        globalData.item[lastItem].dataItemType[lastIndex] = itemTypeArr
        handleSaveSqlite(globalData.item[lastItem].dataItemType)
      } else {
        globalData.item[lastItem].dataItemType.push(itemTypeArr)
        handleSaveSqlite(globalData.item[lastItem].dataItemType)
      }
      isNavigate ? navigate(Routes.INVENTORY_DETAIL_LIST_SCREEN) : null
    }
    if (localDataArray.isEdit == true) {
      //edit
      await AsyncStorage.setItem("@DataInventory", JSON.stringify(globalData.item))
      itemTypeArr.statusId = custStatus.statusId
      itemTypeArr.noteForItem = noteItem
      itemTypeArr.condition = custCondition
      const rs = await updateInventoryItem(itemTypeArr)
      if (rs?.status === 200) {
        navigate(Routes.UPDATE_INVENTORY_SCREEN)
        Alert.alert("Update inventory item successful")
      } else {
        Alert.alert(`Error Api Update (${rs?.status || 0})`)
      }
    }
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
    // itemTypeArr.mainPhoto = mainPhoto?.name
    itemTypeArr.mainImage = mainPhoto?.name
    for (let i = 0; i < arrItemType.length; i++) {
      if (arrItemType[i].valType == 40) {
        arrItemType[i].itemTypeOptionReturnValue = arrCheckQuatity
      }
      if (arrItemType[i].valType == 90 && arrItemType[i].itemTypeOptionCode == "Building" && custBuidingName) {
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
      if (arrItemType[i].valType == 3 && arrItemType[i].itemTypeOptionCode == "Floor" && custFloor) {
        // Check again
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
      if (
        arrItemType[i].valType == 90 &&
        arrItemType[i].itemTypeOptionCode == "Manufacturer" &&
        refManufacturer.current
      ) {
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
    } else if (typeSharedModal == "Status") {
      setcustStatus(data)
    } else if (typeSharedModal == "Condition") {
      setcustCondition(data.name)
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

  useEffect(() => {
    let arr = [...dataItemType]
    const indexFindModular = arr?.findIndex((it: any) => it?.itemTypeOptionCode === "Custom-Modular-AV")
    if (indexFindModular !== -1) {
      let arrCheckCache = []
      for (let j = 0; j < arrCheck.length; j++) {
        arrCheckCache?.push({
          returnValue: arr[indexFindModular].itemTypeOptionLines[arrCheck[j]],
        })
      }
      arr[indexFindModular].itemTypeOptionReturnValue = arrCheckCache
      setdataItemType(arr)
    }
  }, [arrCheck?.length])
  const onTextChangedTxtFeild = (_: any, txt: string) => {
    setvaluesTextInputTextParagraph(txt)
  }
  const checkCompareOnBlur = () => {
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
  const checkKeyBoardType = (data: any) => {
    if (data.fieldType == "Decimal") {
      return "number-pad"
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
      setUrlSupportFile(`${URL_BASE}${supportFile?.desc}`)
    }
  }



  const handleDeleteItems = (index?: number, dataParent?: any, indexParent?: any, indexChild?: any) => {
    let raw = dataParent.dataCondition
    raw.splice(indexChild, 1)
    if (raw?.length == 0) {
      arrCheckQuatity.splice(indexParent, 1)
    }
    setarrCheckQuatity([...arrCheckQuatity])
  }

  const removeImg = (item: any, indexParent: number, indexChild?: any) => {
    let parentArr: any = arrCheckQuatity[indexParent]
    let childArr: any = parentArr?.representativePhotos[0].url
    childArr?.splice(indexChild, 1)
    setarrCheckQuatity([...arrCheckQuatity])
    handleRemoveImage(item)
  }
  const handleRemoveImage = async (item) => {
    const rs = await inventoryImageDelete({ inventoryImageId: item.inventoryItemImgId || item.inventoryImageId })
  }
  const handleDelete = (item?: any, indexChild?: any, indexParent?: any,) => {
    if (arrCheckQuatity[indexParent]?.representativePhotos[0].url?.length <= 1) {
      Alert.alert('Not allow delete last photo!')
      return
    }
    Alert.alert("Delete Image", "Are you sure you wish to delete this Image?", [
      {
        text: "No",
        onPress: () => console.log("Cancel Pressed"),
        style: "cancel",
      },
      { text: "Yes", onPress: () => removeImg(item, indexParent, indexChild) },
    ])
  }


  const renderItemTypeFeild = ({ item, index }: RenderItemTypeFeildProps): any => {
    const inventorySupportFile = item?.inventorySupportFile
    const hasSupportFile = !!inventorySupportFile
    const isDecimal = item?.fieldType === "Decimal"
    if (item.valType == 1 && !item.isHide) {
      if (item?.itemTypeOptionReturnValue == null || item?.itemTypeOptionReturnValue == "") {
        return (
          <View>
            <View style={{ flex: 1 }}>
              <View style={{ marginTop: 20, flex: 1 }}>
                <View style={{ flexDirection: "row", alignItems: "center" }}>
                  <Text style={{ fontSize: 15, marginBottom: 10 }}>{item.itemTypeOptionName}</Text>
                  {hasSupportFile && (
                    <TouchableOpacity
                      style={styles.circlePressable}
                      onPress={() => showImgSupportFile(inventorySupportFile)}
                    >
                      <IconFontAwesome name="info-circle" size={16} />
                    </TouchableOpacity>
                  )}
                </View>
                <View style={[styles.btnDefault]}>
                  <View style={{ flex: 9 }}>
                    <TextInput
                      style={styles.txtSize16}
                      keyboardType={checkKeyBoardType(item)}
                      returnKeyType={checkKeyType(item)}
                      onBlur={() => checkCompareOnBlur()}
                      onChangeText={(txt) => onChangeTextArr(txt, 0, index, item)}
                      placeholder={isDecimal ? "0.00" : ""}
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
                        <Text style={{ fontSize: 15, marginBottom: 10 }}>{item.itemTypeOptionName}</Text>
                        {hasSupportFile && (
                          <TouchableOpacity
                            style={styles.circlePressable}
                            onPress={() => showImgSupportFile(inventorySupportFile)}
                          >
                            <IconFontAwesome name="info-circle" size={16} />
                          </TouchableOpacity>
                        )}
                      </View>
                      <View style={[styles.btnDefault]}>
                        <View style={{ flex: 9 }}>
                          <TextInput
                            style={styles.txtSize16}
                            value={i.returnValue}
                            keyboardType={checkKeyBoardType(item)}
                            returnKeyType={checkKeyType(item)}
                            onBlur={() => checkCompareOnBlur()}
                            onChangeText={(txt) => onChangeTextArr(txt, idx, index, item)}
                            placeholder={isDecimal ? "0.00" : ""}
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
          <Text style={{ fontSize: 15, marginBottom: 10 }}>{item.itemTypeOptionName}</Text>
          <View style={styles.btnDefault} onPress={() => openModal(item)}>
            <Text style={[styles.txtSize16, { marginVertical: 5 }]} maxFontSizeMultiplier={1}>
              {custClient}
            </Text>
          </View>
        </View>
      )
    } else if (item.valType == 90 && !item.isHide) {
      //DropdownPlus
      return (
        <View style={{ flex: 1, marginTop: 20 }}>
          <View style={{ flexDirection: "row", alignItems: "center" }}>
            <Text style={{ fontSize: 15, marginBottom: 10 }}>{item.itemTypeOptionName}</Text>
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
          <Text style={{ fontSize: 15, marginBottom: 10 }}>{item.itemTypeOptionName}</Text>
          <View style={[styles.btnDefault, { justifyContent: "flex-start", minHeight: 100 }]}>
            <TextInput
              style={styles.txtSize16}
              multiline
              numberOfLines={4}
              onBlur={() => checkCompareOnBlur()}
              onEndEditing={() => handleOnEndEditing(valuesTextInputTextParagraph, item)}
              value={valuesTextInputTextParagraph}
              onChangeText={(txt) => {
                onTextChangedTxtFeild(item.itemTypeOptionId, txt)
                onChangeTextArr(txt, 0, index, item)
              }}
            />
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
                <Text style={{ fontSize: 15, flex: 4, marginBottom: 10 }}>{item.itemTypeOptionName}</Text>
                <View style={[styles.btnDefault, { flexDirection: "row" }]}>
                  <View style={{ flex: 9 }}>
                    <TextInput
                      style={styles.txtSize16}
                      onBlur={() => checkCompareOnBlur()}
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
                    <Text style={{ fontSize: 15, flex: 4, marginBottom: 10 }}>{item.itemTypeOptionName}</Text>
                    <View style={[styles.btnDefault, { flexDirection: "row" }]}>
                      <View style={{ flex: 9 }}>
                        <TextInput
                          style={styles.txtSize16}
                          value={i.returnValue}
                          onBlur={() => checkCompareOnBlur()}
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
            <Text style={{ fontSize: 15, flex: 4, marginBottom: 10 }}>{item.itemTypeOptionName}</Text>
          </View>
          <View style={{ flex: 1, flexDirection: "row" }}>
            {arrRaw.map((data: any, i: number) => {
              return (
                <TouchableOpacity
                  key={i}
                  onPress={() => handlePressCheck(i)}
                  style={{
                    backgroundColor: arrCheck.includes(i) ? Colors.lightGray : "white",
                    borderRadius: 20,
                    marginHorizontal: 5,
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
    setshowImageModal({ ...data, name: data?.name || data?.imageName })
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

  const checkImage = async (arrItem: any) => {
    for (let i = 0; i < arrCheckQuatity.length; i++) {
      if (i == idxAddItemPhoto) {
        let arrayPhotos = arrCheckQuatity[i].representativePhotos[0].url
        arrItem?.forEach((it: any) => {
          arrayPhotos.push(it)
        })
      }
    }
    setarrCheckQuatity([...arrCheckQuatity])
    setmodalTypeCameraUpdate(false)

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
      arrayReturnValue?.push({ returnValue: "" })
      setdataItemType(arr)
    } else {
      arr[index].itemTypeOptionReturnValue = []
      arr[index].itemTypeOptionReturnValue.push({ returnValue: "" }, { returnValue: "" })
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
      const floorsFilter = cloneDeep(floor)?.filter((it: any) => it?.inventoryFloorId !== 0)
      setdataSharedModal(floorsFilter)
      setcanAddData(false)
      setopenSharedModal(true)
    } else if (type == "Status") {
      setdataSharedModal(statusData)
      setcanAddData(false)
      setopenSharedModal(true)
    } else if (type == "Condition") {
      setdataSharedModal(dataDropdownCondition)
      setcanAddData(false)
      setopenSharedModal(true)
    }
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



  const renderChoseItemType = () => {
    return (
      <View style={{ flex: 1 }}>
        <View style={{ marginTop: 30, flex: 1 }}>
          <Text style={{ fontSize: 15, flex: 4, marginBottom: 10 }} maxFontSizeMultiplier={1}>
            {`Inventory ID: ${imageInfo?.inventoryId || ""}`}
          </Text>
          <Text style={{ fontSize: 15, flex: 4, marginBottom: 10 }} maxFontSizeMultiplier={1}>
            {`Item Name: ${imageInfo?.itemTypeName || ""}`}
          </Text>
          <Text style={{ fontSize: 15, flex: 4, marginBottom: 10 }} maxFontSizeMultiplier={1}>
            {`Warranty Years: ${imageInfo?.warrantyYears || ""}`}
          </Text>
          <TouchableOpacity onPress={() => setIsOpenModal(true)}>
            <Text style={{ fontSize: 15, flex: 4, marginBottom: 10, color: Colors.blue }} maxFontSizeMultiplier={1}>
              {`Update Available Quantity: ${totalQuantity || "null"}`}
            </Text>
          </TouchableOpacity>
        </View>
      </View>
    )
  }

  const onChangeCheckBoxSetMainPhoto = useCallback(
    (value) => {
      setMainPhoto(value ? showImageModal : undefined)
      imageInfo.mainPhoto = value ? showImageModal?.name : null
      imageInfo.mainImage = value ? showImageModal?.name : null
      setimageInfo({ ...imageInfo })
    },
    [showImageModal],
  )
  const openUpdateImage = (it, idx) => {
    setmodalTypeCameraUpdate(true)
    setconditionID(it.conditionID)
    setidxAddItemPhoto(idx)
  }
  const ListHeaderComponent = useMemo((): any => {
    const uri = `${SERVER_NAME}/${clientID}/${imageInfo?.mainPhoto || imageInfo?.mainImage
      }`
    return (
      <View>
        {(imageInfo?.mainPhoto || imageInfo?.mainImage) && <View style={{ marginTop: 10 }}>
          <Text style={{ fontSize: fontSizer(16), fontWeight: "bold", fontStyle: "italic" }}>Main Photo </Text>
          <View
            style={{
              width: 80,
              height: 80,
              marginTop: 10,
              marginHorizontal: 15,
              backgroundColor: "pink",
            }}
          >
            <Image source={{ uri }} style={[styles.photo, styles.activePhoto]} resizeMode="cover" />
          </View>
        </View>}
        {
          console.log("arrCheckQuatity", arrCheckQuatity)
        }
        <View style={{ marginTop: 20 }}>
          {
            arrCheckQuatity.length > 0 &&
            <Text style={{ fontSize: fontSizer(16), fontWeight: "bold", fontStyle: "italic" }}>Representative Photos</Text>
          }

          {
            arrCheckQuatity?.map((it: any, indexParent: any) => {
              const dataArr = it?.representativePhotos?.[0]?.url
              const filterConditonById = dataCondition?.filter((__it: any) => __it?.inventoryItemConditionId === it?.conditionID)
              const filterCondition = quantityByID?.filter((_it: any) => _it?.conditionID === it?.conditionID)
              const arrayIdNotRepretativePhotos = [2, 3, 4]
              const isNotRepresentativePhotos = arrayIdNotRepretativePhotos.includes(it?.conditionID)
              return <View key={it?.conditionID}>
                <View style={{ marginTop: 10 }}>

                  <View style={{ flexDirection: "row" }}>
                    <TouchableOpacity style={{ flexDirection: 'row', justifyContent: 'center', alignItems: 'center' }} onPress={() => openUpdateImage(it, indexParent)}>
                      <Text style={{ fontSize: 15, color: Colors.black }}>
                        {`${filterConditonById?.[0]?.conditionName} - QTY: ${filterCondition?.[0]?.txtQuantity}`}
                      </Text>
                      <View style={{ paddingHorizontal: 2, marginBottom: 2 }}>
                        <MaterialIcons name={"add-photo-alternate"} size={32} color={Colors.blue} />
                      </View>

                    </TouchableOpacity>
                  </View>
                  <FlatList
                    data={dataArr}
                    contentContainerStyle={{ paddingBottom: 12 }}
                    horizontal={true}
                    keyExtractor={(item, index) => `${index}`}
                    renderItem={({ item, index }) => {
                      const isMainPhoto = (item?.name || item?.imageName) === (imageInfo?.mainPhoto || imageInfo?.mainImage)
                      const styleMainPhoto = isMainPhoto ? styles.activePhoto : {}
                      return <View style={{ marginTop: 15, }}>
                        <TouchableOpacity
                          onLongPress={() => {
                            imageInfo.mainPhoto = item?.name || item?.imageName
                            imageInfo.mainImage = item?.name || item?.imageName
                            setMainPhoto({ ...item, name: item?.name || item?.imageName })
                            setimageInfo({ ...imageInfo })
                          }}
                          onPress={() => handleShowModalImage({ ...item, uri: `${SERVER_NAME}/${clientID}/${item.imageName || item.name}` })}
                          style={{
                            width: 80,
                            height: 80,
                            marginTop: 0,
                            marginHorizontal: 15,
                            flexDirection: "row",
                          }}
                        >
                          <Image
                            source={{ uri: `${SERVER_NAME}/${clientID}/${item.imageName || item.name}` }}
                            style={[styles.photo, { backgroundColor: 'pink', ...styleMainPhoto }]}
                            resizeMode="cover"
                          />
                          <TouchableOpacity
                            style={{
                              width: 25,
                              height: 25,
                              position: "absolute",
                              right: -15,
                              top: -8,
                            }}
                            onPress={() => isMainPhoto ? Alert.alert('You are deleting the main Photo. Please replace before deleting it') : handleDelete(item, index, indexParent)}
                          >
                            <MaterialCommunityIcons
                              name={"delete-circle"}
                              size={20}
                            />
                          </TouchableOpacity>
                        </TouchableOpacity>

                      </View>
                    }

                    }
                  />
                </View>
              </View>
            })
          }
        </View>
      </View>
    )
  }, [dataItemType, itemTypeNew, arrCheckQuatity, imageInfo?.mainPhoto])
  return (
    <View style={styles.container}>
      <Header
        // isSaved={true}
        // handleSave={() => (dataItemType?.length > 0 ? handleSave() : null)}
        isGoBack
        conditionRender={localDataArray.conditionRender}
        labels={"Update Inventory"}
      />
      <LoadingOverlay visible={loading} />
      <KeyboardAwareScrollView
        bounces={false}
        showsVerticalScrollIndicator={false}
        extraScrollHeight={150}
        enableOnAndroid={true}
        style={styles.subContainer}
      >
        {renderChoseItemType()}

        <FlatList
          ListHeaderComponent={ListHeaderComponent}
          data={dataItemType}
          style={{ marginBottom: 100 }}
          renderItem={(item) => renderItemTypeFeild(item)}
          extraData={[...errorRequire, imageInfo]}
          keyExtractor={(item, index) => `${index}`}

        />
      </KeyboardAwareScrollView>
      <BottomButtonNext labels="Update" onPressButton={() => {
        if (imageInfo?.mainPhoto || imageInfo?.mainImage) {
          handleOnPress()
        } else {
          Alert.alert('You do not have main photo, please choose main photo before update')
        }
      }} />

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
          onClosed={() => setmodalShowImage(false)}
          onOpened={() => setmodalShowImage(true)}
          onValueChange={onChangeCheckBoxSetMainPhoto}
          value={(mainPhoto?.name || imageInfo?.mainPhoto || imageInfo?.mainImage) === showImageModal?.name}
          isShowChooseMainPhoto
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
      {modalTypeCameraUpdate && (
        <BaseBottomSheetCameraUpdateInventory
          open={modalTypeCameraUpdate}
          options={itemType}
          flex={1}
          conditionRender={localDataArray.conditionRender}
          parentRowID={localDataArray.parentRowID}
          title={`New photo: ${InventoryID}`}
          InventoryID={InventoryID}
          inventoryItemID={inventoryItemIdRaw}
          conditionID={conditionID}
          type={"InventoryId"}
          onSelect={(item) => checkImage(item)}
          onClosed={() => setmodalTypeCameraUpdate(false)}
          onOpened={() => setmodalTypeCameraUpdate(true)}
          conditionData={dataCondition}
          arrCheckQuatity={arrCheckQuatity}
        />
      )}

      <Modal visible={!!urlSupportFile} onRequestClose={() => setUrlSupportFile("")} transparent animationType="slide">
        <TouchableOpacity activeOpacity={1} style={styles.containerViewModal} onPressOut={() => setUrlSupportFile("")}>
          <Image source={{ uri: urlSupportFile }} style={styles.imgSupportFile} />
        </TouchableOpacity>
      </Modal>
      <ModalList clientId={clientID} inventoryID={imageInfo?.inventoryId} isOpenModal={isOpenModal} setIsOpenModal={setIsOpenModal} navigate={navigate} />
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
  containerRowUnit: {
    flexDirection: "row",
    flex: 1,
    alignItems: "flex-start",
  },
  txtUnit: {
    fontSize: fontSizer(16),
  },
})
export default UpdateDetailInventoryScreen
