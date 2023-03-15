import { useFocusEffect, useNavigation, useRoute } from "@react-navigation/native"
import { useState, useCallback, useRef, useEffect, useMemo } from "react"
import { Alert } from "react-native"
import Routes from "/navigators/Routes"
import { cloneDeep, get, isArray, isEmpty, isEqual } from "lodash"
import { getInventoryItemInfo, updateLocations, getInventoryInfo, getCondition } from "/redux/progress/service"
import DataLocationsService from "./DataLocationService"
import { getLocation } from "/utils"
import AsyncStorage from "@react-native-community/async-storage"
import { searchSimpleInventory } from "/redux/progress/service"
import { useGlobalData } from "/utils/hook"

export const useDetailLIstInventory = () => {
  const { navigate, goBack } = useNavigation()
  const [inventoryItemList, setInventoryItemList] = useState([])
  const [inventoryItemListID, setInventoryItemListID] = useState([])
  const route = useRoute()
  const idClient = get(route, "params.idClient")
  const inventoryItemId = get(route, "params.inventoryItemId")
  const [buildingDataState, setBuildingDataState] = useState<any>(null)
  const [floorDataState, setFloorDataState] = useState<any>(null)
  const [itemTypeState, setItemTypeState] = useState<any>(null)
  const [gPSValue, setGPSValue] = useState<any>(null)
  const [areaNumber, setAreaNumber] = useState<any>(null)
  const [canAddData, setCanAddData] = useState<any>(false)
  const [titleModalType, setTitleModalType] = useState<any>(null)
  const [dataModalType, setDataModalType] = useState<any>(null)
  const [openModalType, setOpenModalType] = useState<any>(false)
  const [typeModalType, setTypeModalType] = useState<any>(null)
  const [modalBox, setModalBox] = useState<any>(false)
  const [infoDetail, setInfoDetail] = useState<any>(null)
  const [detailShow, setDetailShow] = useState(false)
  const [valueInventory, setValueInventory] = useState<any>(null)
  const [isSearchList, setIsSearchList] = useState(false)
  const [listSearch, setListSearch] = useState<any>([])
  const [valueToolTip, setValueToolTip] = useState(null)
  const [noDoneBtn, setNoDoneBtn] = useState(false)
  const refBuildingData = useRef<any>({ buildingName: "ALL", buildingID: 0 })
  const refFloorData = useRef<any>({ floorName: "ALL", floorID: 0 })
  const refItemTypeData = useRef<any>({ itemTypeName: "ALL", itemTypeID: 0 })
  const refConditionData = useRef<any>({ itemTypeName: "ALL", itemTypeID: 0 })
  const [modalShowImage, setModalShowImage] = useState<any>(false)
  const [modalShowImageOut, setModalShowImageOut] = useState<any>(false)
  const [infoImageModal, setInfoImageModal] = useState<any>(null)
  const refAreaNumber = useRef<any>(null)
  const [textSearch, setTextSearch] = useState<any>(null)
  const [listChoose, setListChoose] = useState<any>([])
  const { buildingData, floorData, itemTypesData, handleGetBuildingDataService } = DataLocationsService(idClient)
  const [globalData, setglobalData] = useGlobalData()
  const [currentPage, setCurrentPage] = useState(1)
  const [isLoadMore, setIsLoadMore] = useState(false)
  const [conditionData, setConditionData] = useState([])
  const [conditionDataState, setConditionDataState] = useState<any>(null)
  const refFlatList = useRef<any>(null)
  const searchList = () => {
    setModalBox(true)
    setIsSearchList(true)
    setCurrentPage(1)
    setTextSearch(null)
    refBuildingData.current = { buildingName: "ALL", buildingID: 0 }
    refFloorData.current = { floorName: "ALL", floorID: 0 }
    refItemTypeData.current = { itemTypeName: "ALL", itemTypeID: 0 }
    refConditionData.current = { itemTypeName: "ALL", itemTypeID: 0 }
    setBuildingDataState(refBuildingData.current)
    setFloorDataState(refFloorData.current)
    setItemTypeState(refItemTypeData.current)
    setConditionDataState(refConditionData.current)
    searchInventory(1, false, "", 0)
  }

  const closeModal = () => {
    setModalBox(false)
    setIsSearchList(false)
  }

  const getConditionDataFunc = async () => {
    const rs = await getCondition()
    if (rs?.status === 200) {
      rs?.data?.map((it: any) => {
        ; (it.clientId = idClient), (it.itemTypeId = it?.inventoryItemConditionId), (it.name = it?.conditionName)
        it.itemTypeName = it?.conditionName
      })
      setConditionData(rs?.data)
    }
  }

  const handleOnPress = async () => {
    const gPSValueSplit = gPSValue?.split("/")
    const body = {
      clientID: idClient,
      locations: {
        Building: {
          ID: buildingDataState?.buildingID,
          name: buildingDataState?.buildingName,
        },
        Floor: {
          ID: floorDataState?.floorID,
          name: floorDataState?.floorName,
        },
        GPS: {
          latitude: gPSValueSplit[0],
          longitude: gPSValueSplit[1],
        },
        RoomNumber: areaNumber,
      },
      inventoryItemID: inventoryItemListID,
    }
    const rs = await updateLocations(body)
    if (rs?.status === 200) {
      setModalBox(false)
      setInventoryItemList([])
      setInventoryItemListID([])
      setListSearch(null)
      setAreaNumber(null)
      setBuildingDataState(null)
      setFloorDataState(null)
      setGPSValue(null)
    } else {
      Alert.alert(`Something wrong! Check again (${rs?.status || 0})`)
    }
  }

  const handleOpenScan = () => {
    navigate(Routes.CAMERA_BARCODE, { idClient, isListScan: true })
  }

  const funcGetInfoInventory = useCallback(
    async (inventoryItemId: any, totalQuantity?: any) => {
      const body = {
        clientId: idClient,
        inventoryId: inventoryItemId,
      }
      const rs = await getInventoryInfo(body)
      if (rs?.status === 200) {
        const rsData = rs?.data?.[0]
        handleCheckItem(rsData, totalQuantity)
      } else {
        Alert.alert(`Inventory Item ID invalid (${rs?.status || 0})`)
      }
    },
    [idClient, inventoryItemList, inventoryItemListID],
  )

  const getValueByCode = (item: any, code: any) => {
    const itemByCode = item?.filter((it: any) => it?.itemTypeOptionCode === code)
    if (isEmpty(itemByCode)) {
      return "hide_txt_value"
    } else {
      if (code === "Custom-Modular-AV") {
        return itemByCode?.[0]?.itemTypeOptionReturnValue
      }
      return itemByCode?.[0]?.itemTypeOptionReturnValue?.[0]?.returnValue
    }
  }

  const renderValueArr = (value: any) => {
    if (isArray(value)) {
      let cacheTxt = ""
      value?.forEach((it: any, index: number) => {
        cacheTxt += `${it?.returnValue}` + (index === value?.length - 1 ? "" : `, `)
      })
      return cacheTxt
    } else {
      return value?.[0]?.returnValue
    }
  }

  const funcGetInventoryInfo = async (id: any) => {
    const body = {
      clientId: idClient,
      inventoryId: id,
    }
    const rs = await getInventoryInfo(body)
    if (rs?.status === 200) {
      const rsData = rs?.data?.[0]
      const itemTypeOptionsCache = rsData?.itemTypeOptions
      const formatBody = {
        "Item Type Name": rsData?.itemTypeName,
        "Warranty Years": rsData?.warrantyYears,
        mainPhoto: rsData?.mainPhoto || rsData?.mainImage,
        Condition: rsData?.condition,
        Building: getValueByCode(itemTypeOptionsCache, "Building"),
        Floor: getValueByCode(itemTypeOptionsCache, "Floor"),
        GPS: getValueByCode(itemTypeOptionsCache, "GPS"),
        Room: getValueByCode(itemTypeOptionsCache, "AreaOrRoom"),
        Tag: getValueByCode(itemTypeOptionsCache, "Tag"),
        Description: getValueByCode(itemTypeOptionsCache, "Description"),
        Note: getValueByCode(itemTypeOptionsCache, "Note"),
        Manufacturer: getValueByCode(itemTypeOptionsCache, "Manufacturer"),
        "Custom Modular-AV": renderValueArr(getValueByCode(itemTypeOptionsCache, "Custom-Modular-AV")),
        "Part Number": getValueByCode(itemTypeOptionsCache, "PartNumber"),
        "Frame Finish": getValueByCode(itemTypeOptionsCache, "FrameFinish"),
        "Base Finish": getValueByCode(itemTypeOptionsCache, "BaseFinish"),
        "Back Finish": getValueByCode(itemTypeOptionsCache, "BackFinish"),
        "Seat Finish": getValueByCode(itemTypeOptionsCache, "SeatFinish"),
        Unit: getValueByCode(itemTypeOptionsCache, "Unit"),
        Width: getValueByCode(itemTypeOptionsCache, "Width"),
        Height: getValueByCode(itemTypeOptionsCache, "Height"),
        "Seat Height": getValueByCode(itemTypeOptionsCache, "SeatHeight"),
      }
      setInfoDetail(formatBody)
      setDetailShow(true)
    } else {
      Alert.alert(`Inventory Item ID invalid (${rs?.status || 0})`)
    }
  }

  useFocusEffect(
    useCallback(() => {
      if (!inventoryItemId) return
      funcGetInfoInventory(inventoryItemId)
    }, [inventoryItemId]),
  )

  useEffect(() => {
    if (isSearchList) {
      refAreaNumber.current = areaNumber
    }
  }, [areaNumber])

  const onSelectSharedModal = async (data: any) => {
    if (typeModalType == "Building") {
      const bodyBuilding = { buildingName: data.inventoryBuildingName, buildingID: data.inventoryBuildingId }
      setBuildingDataState(bodyBuilding)
      isSearchList && (refBuildingData.current = bodyBuilding)
      handleGetBuildingDataService()
    } else if (typeModalType == "Floors") {
      const bodyFloor = { floorName: data.inventoryFloorName, floorID: data.inventoryFloorId }
      setFloorDataState(bodyFloor)
      isSearchList && (refFloorData.current = bodyFloor)
    } else if (typeModalType === "Condition") {
      const bodyConditionData: any = { itemTypeName: data?.itemTypeName, itemTypeID: data?.itemTypeId }
      setConditionDataState(bodyConditionData)
      isSearchList && (refConditionData.current = bodyConditionData)
    } else {
      const bodyItemType = { itemTypeName: data?.itemTypeName, itemTypeID: data?.itemTypeId }
      setItemTypeState(bodyItemType)
      isSearchList && (refItemTypeData.current = bodyItemType)
    }
    setOpenModalType(false)
  }
  const handleOpenModal = (type: any) => {
    setTypeModalType(type)
    setTitleModalType(type)
    if (type == "Building") {
      isSearchList
        ? setDataModalType([
          {
            clientId: idClient,
            inventoryBuildingCode: "ALL",
            inventoryBuildingDesc: "ALL",
            inventoryBuildingId: 0,
            inventoryBuildingName: "ALL",
          },
          ...buildingData,
        ])
        : setDataModalType(buildingData)
      setCanAddData(true)
    } else if (type == "Floors") {
      const floorsFilter = cloneDeep(floorData)?.filter((it: any) => it?.inventoryFloorId !== 0)
      isSearchList
        ? setDataModalType([
          {
            clientId: idClient,
            inventoryFloorCode: "ALL",
            inventoryFloorDesc: "ALL",
            inventoryFloorId: 0,
            inventoryFloorName: "ALL",
          },
          ...floorsFilter,
        ])
        : setDataModalType(floorsFilter)
      setCanAddData(false)
    } else if (type === "Condition") {
      setDataModalType([{ clientId: idClient, itemTypeName: "ALL", name: "ALL", itemTypeId: 0 }, ...conditionData])
    } else {
      isSearchList
        ? setDataModalType([
          {
            clientId: idClient,
            itemTypeId: 0,
            itemTypeName: "ALL",
          },
          ...itemTypesData,
        ])
        : setDataModalType(itemTypesData)
      setCanAddData(false)
    }
    setOpenModalType(true)
  }

  const addItemType = useCallback(() => {
    getInventoryItemInfo(
      valueInventory,
      (rs) => {
        const rsData = rs?.data?.[0]
        setInventoryItemList((preSta) => preSta?.concat({ ...rsData, inventoryItemId: valueInventory }))
        setInventoryItemListID((preSta) => preSta?.concat(valueInventory))
      },
      (err) => {
        Alert.alert(`Inventory Item ID invalid (${err?.status || 0})`)
      },
      idClient,
    )
  }, [valueInventory])

  const ConditionDisableBtnDone = useCallback(() => {
    if (buildingDataState?.buildingName && floorDataState?.floorName && areaNumber && gPSValue) {
      return false
    }
    return true
  }, [buildingDataState, gPSValue, floorDataState, areaNumber])

  const getAndShowLocation = async () => {
    await getLocation()
    setTimeout(async () => {
      const GPSValueSync = await AsyncStorage.getItem("@custGPS")
      GPSValueSync && setGPSValue(GPSValueSync)
    }, 500)
  }

  const searchInventory = async (
    currentPage: any,
    isLoadMore: boolean,
    txtSearch?: string,
    itemTypeID?: number,
    conditionID?: number,
    buildingID?: number,
    floorID?: number,
  ) => {
    const body = {
      clientId: idClient,
      itemTypeId: itemTypeID ?? itemTypeState?.itemTypeID,
      searchString: txtSearch ?? textSearch,
      conditionID: conditionID ?? conditionDataState.itemTypeID,
      buildingID: buildingID ?? buildingDataState.buildingID,
      floorID: floorID ?? floorDataState?.floorID,
      room: areaNumber || "",
      itemsPerPage: 10,
      currentPage,
    }
    const rs = await searchSimpleInventory(body)
    if (rs?.status === 200) {
      setListSearch(isLoadMore ? (preState: any) => [...preState, ...rs?.data] : rs?.data)
    } else {
      Alert.alert(`Inventory Item Not Found (${rs?.status || 0})`)
    }
    setIsLoadMore(false)
  }

  const handleLoadMore = () => {
    setCurrentPage((preState) => preState + 1)
  }

  useEffect(() => {
    if (currentPage === 1) return
    setIsLoadMore(true)
    searchInventory(currentPage, true)
  }, [currentPage])

  const chooseItemToAdd = (__inventoryItemId: any, totalQuantity: any) => {
    funcGetInfoInventory(__inventoryItemId, totalQuantity)
    setModalBox(false)
    setIsSearchList(false)
  }

  const removeAll = () => {
    setInventoryItemList([])
    setInventoryItemListID([])
  }

  const showImageZoom = (value: any, isOut?: any) => {
    if (isOut) {
      setModalShowImageOut(true)
    } else {
      setModalShowImage(true)
    }
    setInfoImageModal(value)
  }

  useEffect(() => {
    let filterSearchList: any = []
    let cloneDeepListSearch = cloneDeep(listSearch)
    inventoryItemList?.forEach(async (it: any) => {
      filterSearchList = cloneDeepListSearch?.filter(
        (itSearch: any) => itSearch?.inventoryItemId !== it?.inventoryItemId,
      )
      cloneDeepListSearch = filterSearchList
    })
  }, [inventoryItemList, listSearch])

  useEffect(() => {
    getConditionDataFunc()
  }, [])

  const chooseFunc = (isAdd: boolean, id: string) => {
    if (isAdd) {
      setListChoose((preState: any[]) => {
        const preStateNew: any = preState.concat(id)
        return [...preStateNew]
      })
    } else {
      setListChoose((preState: any[]) => {
        const indexId = preState?.findIndex((it: any) => it === id)
        preState.splice(indexId, 1)
        return [...preState]
      })
    }
  }

  const handleCheckItem = (item?: any, totalQuantity?: any) => {
    let ar = [item]
    let raw = { chosed: ar }
    let rawGlobalData = { ...globalData, ...raw }
    setglobalData(rawGlobalData)
    navigate(Routes.UPDATE_DETAIL_INVENTORY_SCREEN, {
      idClient,
      isEdit: true,
      parentRowID: item.parentRowID,
      totalQuantity: totalQuantity,
    })
  }

  return {
    buildingDataState,
    floorDataState,
    gPSValue,
    areaNumber,
    inventoryItemList,
    modalBox,
    openModalType,
    dataModalType,
    canAddData,
    titleModalType,
    typeModalType,
    detailShow,
    isSearchList,
    itemTypeState,
    listSearch,
    idClient,
    valueToolTip,
    textSearch,
    infoDetail,
    noDoneBtn,
    infoImageModal,
    modalShowImage,
    modalShowImageOut,
    listChoose,
    isLoadMore,
    refFlatList,
    refItemTypeData,
    refBuildingData,
    refFloorData,
    conditionDataState,
    refConditionData,
    addItemType,
    handleOpenScan,
    handleOpenModal,
    setGPSValue,
    getAndShowLocation,
    setAreaNumber,
    goBack,
    setDetailShow,
    setModalBox,
    ConditionDisableBtnDone,
    handleOnPress,
    onSelectSharedModal,
    setOpenModalType,
    setValueInventory,
    searchList,
    closeModal,
    searchInventory,
    chooseItemToAdd,
    setValueToolTip,
    setTextSearch,
    removeAll,
    setInfoDetail,
    setNoDoneBtn,
    showImageZoom,
    setModalShowImage,
    setModalShowImageOut,
    chooseFunc,
    handleLoadMore,
    setCurrentPage,
    setItemTypeState,
    funcGetInventoryInfo,
    setIsSearchList,
    setBuildingDataState,
    setFloorDataState,
    setConditionDataState,
  }
}

export default useDetailLIstInventory
