import { useNavigation, useRoute } from "@react-navigation/native"

import { useCallback, useState, useEffect } from "react"
import AsyncStorage from "@react-native-community/async-storage"
import { SelectOption } from "/components/Select/types"
import Routes from "/navigators/Routes"
import { BaseAlert } from "../../components"
import Storage from "/helper/Storage"
import useGlobalData from "/utils/hook/useGlobalData"
import { URL_BASE, SEARCH_IMAGE_TYPE, CONDITION_QUANTITY_TYPE } from "/constant/Constant"
import { findIndex, forEach, get, isEqual, min, isEmpty, isNaN } from "lodash"
import { updateDataInventoryClientByQuery } from "/utils/sqlite/tableInventoryClient"
import {
  getItemTypeService,
  getItemOptionSetTypeService,
  getFloorsDataService,
  getBuildDataService,
} from "/redux/progress/service"
import MultiLanguage from "/utils/MultiLanguage"
import useLanguage from "/utils/hook/useLanguage"
import { getLocation } from "../../utils"
import { Alert } from "react-native"

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
const InventoryDetailScreenIndex = () => {
  const { navigate } = useNavigation()
  const [language] = useLanguage()
  const route = useRoute()
  const idClient = get(route, "params.idClient")
  const imageInfo = get(route, "params.imageInfo")
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

  const [globalData] = useGlobalData()

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
  const [itemTypeArr, setitemTypeArr] = useState<{
    itemTypeOptions?: any
    clientID?: any
    mainImage?: any
    mainImageFull?: Photos
    conditionRender?: any
    inventoryRowID?: any
    parentRowID?: any
    createDateTime?: any
    createID?: any
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
  const [flagCheckResetQuantity, setflagCheckResetQuantity] = useState<any>(null)
  const [flagCheckUpNewImage, setflagCheckUpNewImage] = useState(false)
  const [flagDeleteMainPhoto, setflagDeleteMainPhoto] = useState(false)
  const clientID = globalData.item[0]?.clientId
  const inventoryClientGroupID = globalData.item[0]?.InventoryClientGroupID
  const [errorRequire, setErrorRequire] = useState<any>([])
  const [urlSupportFile, setUrlSupportFile] = useState("")
  const [isInventoryClientGroupID, setIsInventoryClientGroupID] = useState(false)

  useEffect(() => {
    getLocation()
    conditionRenderData()
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
  useEffect(() => {
    if (
      localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL &&
      flagCheckResetQuantity != null &&
      flagCheckUpNewImage == false
    ) {
      let dataItemTypeOptions = itemTypeArr?.itemTypeOptions
      let index = dataItemTypeOptions.find((x: any) => x.itemTypeOptionCode === "TotalCount")
      index.itemTypeOptionReturnValue = null
      setarrCheckQuatity([])
      setitemTypeArr(itemTypeArr)
    }
  }, [flagCheckResetQuantity])

  useEffect(() => {
    if (flagCheckResetQuantity) {
      setflagDeleteMainPhoto(true)
      setMainPhoto(undefined)
      localDataArray.parentRowID = null
    }
  }, [flagCheckResetQuantity])

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

  const onChangeCheckBoxSetMainPhoto = useCallback(
    (value) => {
      setMainPhoto(value ? showImageModal : undefined)
    },
    [showImageModal],
  )

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
      if (arr[i].valType == 1 && arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "Building") {
        setcustBuidingName(arr[i].itemTypeOptionReturnValue[0].returnValue)
      }
      if (arr[i].valType == 3 && arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "Floor") {
        setcustFloor(arr[i]?.itemTypeOptionReturnValue[0]?.returnValue)
      }
      if (arr[i].valType == 30 && arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "GPS") {
        setcustGPS(arr[i].itemTypeOptionReturnValue[0].returnValue)
      }
      if (arr[i].valType == 1 && arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "AreaOrRoom") {
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
    console.log("itemTypeId handleGetItemOptionSetTypeService", itemTypeId)
    let url = `${URL_BASE}/api/ItemType/GetItemTypeOptionSet?clientId=${parseInt(
      clientID,
    )}&itemId=${itemTypeId}`//&clientGroupId=${inventoryClientGroupID}
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
      if (arrCheck[i].itemTypeOptionId == item.itemTypeOptionId) {
        arrCheck[i].itemTypeOptionReturnValue = [{ returnValue: item }]
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
    console.log("currentTime", currentTime)
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
      itemTypeArr.createID = _getUserInfo()
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
    } else if (
      localDataArray.isEdit != true &&
      (localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ||
        localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER)
    ) {
      // local
      compareDataItemType()
      // isNavigate ? navigate(Routes.INVENTORY_DETAIL_LIST_SCREEN) : null
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
      }
      handleUpdateSqlite()
      isNavigate ? navigate(Routes.INVENTORY_DETAIL_LIST_SCREEN, { idClient }) : null
    }
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
      }
    } else {
      if (
        localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ||
        localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER
      ) {
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

    for (let i = 0; i < arrItemType.length; i++) {
      if (arrItemType[i].valType == 40) {
        arrItemType[i].itemTypeOptionReturnValue = arrCheckQuatity
      }
      if (arrItemType[i].valType == 80) {
        let returnValue: { returnValue?: any }[] = (arrItemType[i].itemTypeOptionReturnValue = [])
        for (let j = 0; j < arrCheck.length; j++) {
          let value = arrItemType[i].itemTypeOptionLines[arrCheck[j]]
          returnValue.push({ returnValue: value })
        }
      }
      if (arrItemType[i].valType == 1 && arrItemType[i].itemTypeOptionCode == "Building" && custBuidingName) {
        arrItemType[i].itemTypeOptionReturnValue = [{ returnValue: custBuidingName }]
      }
      if (arrItemType[i].valType == 3 && arrItemType[i].itemTypeOptionCode == "Floor" && custBuidingName) {
        arrItemType[i].itemTypeOptionReturnValue = [{ returnValue: custFloor }]
      }
      if (arrItemType[i].valType == 30 && arrItemType[i].itemTypeOptionCode == "GPS" && custGPS) {
        arrItemType[i].itemTypeOptionReturnValue = [{ returnValue: custGPS }]
      }
      if (arrItemType[i].valType == 1 && arrItemType[i].itemTypeOptionCode == "AreaOrRoom" && Areanumber) {
        arrItemType[i].itemTypeOptionReturnValue = [{ returnValue: Areanumber }]
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
    setIsInventoryClientGroupID(!!inventoryClientGroupID && inventoryClientGroupID >= 2)
    setitemTypeTxt(item?.itemName)
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
    setitemTypeTxt(item.itemName)
    setIsInventoryClientGroupID(!!inventoryClientGroupID && inventoryClientGroupID >= 2)
    setLoading(true)
    handleGetItemOptionSetTypeService(item.itemTypeId)
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
    setitemTypeId(item.itemTypeId)
  }

  const handleOnEndEditing = (valuesTextInput?: any, item?: any) => {
    console.log("item", item)
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
    let childArr: any = parentArr.dataCondition[indexChild].url
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
        return data.txtQuantity
      } else {
        return data.dataCondition.length
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
            onPress: () => {},
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
    if (arr[index].itemTypeOptionReturnValue == null) {
      arr[index].itemTypeOptionReturnValue = [{ returnValue: txt }]
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
    if (
      localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ||
      localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER
    ) {
      if (flagCheckResetQuantity == null) {
        setflagCheckResetQuantity(true)
      } else {
        setflagCheckResetQuantity(!flagCheckResetQuantity)
      }
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
    } else {
      setUrlSupportFile(`${URL_BASE}/${supportFile?.fileName}`)
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
      setdataSharedModal(floor)
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
      await AsyncStorage.setItem("@custAreaNumber", Areanumber)
    }
  }
  const validateRequired = () => {
    if (dataItemType?.length > 0 && arrCheckQuatity?.length > 0) {
      return false
    } else {
      return true
    }
  }
  return {
    errorRequire,
    custClient,
    valuesTextInputTextParagraph,
    arrCheckQuatity,
    localDataArray,
    mainPhoto,
    custGPS,
    arrCheck,
    dataItemType,
    itemTypeNew,
    Areanumber,
    custBuidingName,
    custFloor,
    disableChoseItemType,
    itemTypeTxt,
    showImageModal,
    imageInfo,
    loading,
    conditionRenderTitle,
    modalNumberCode,
    checkNV,
    titleDropDown,
    openSharedModal,
    dataSharedModal,
    canAddData,
    titleSharedModal,
    typeSharedModal,
    modalShowImage,
    modalType,
    itemType,
    isInventoryClientGroupID,
    modalTypeCamera,
    conditionsType,
    modalTypeCameraAddMore,
    dataImageItem,
    urlSupportFile,
    itemTypeArr,
    itemTypeId,
    handleOnPress,
    handleShowModalImage,
    handlePressCheckQuatity,
    addMoreTxt,
    handlePressCheck,
    handleOpenModal,
    handleSaveDataLocation,
    onPressSearch,
    checkImage,
    getUpdateImage,
    showImgSupportFile,
    checkKeyBoardType,
    checkKeyType,
    checkCompareOnBlur,
    onChangeTextArr,
    handleOnEndEditing,
    openModal,
    handleShowMorePicture,
    renderQuantityTxt,
    handleDeleteItems,
    onTextChangedTxtFeild,
    onSetMainPhoto,
    setcustGPS,
    handleDelete,
    setmodalShowImage,
    setmodalTypeCamera,
    setmodalTypeCameraAddMore,
    setopenSharedModal,
    setAreanumber,
    handleSave,
    setconditionRenderTitle,
    onSelectMethod,
    onSelectSharedModal,
    onSelectArea,
    onSelectItemType,
    setUrlSupportFile,
    setmodalType,
    changeItemType,
    setModalNumberCode,
    onChangeCheckBoxSetMainPhoto,
    validateRequired,
  }
}
export default InventoryDetailScreenIndex
