import { useFocusEffect, useNavigation, useRoute } from "@react-navigation/native"
import { useState, useCallback, useRef, useEffect, useMemo } from "react"
import { Alert } from "react-native"
import Routes from "/navigators/Routes"
import { cloneDeep, get, isArray, isEmpty, isEqual, isObject } from "lodash"
import { getInventoryItemInfo, updateLocations, getInventoryInfo, getCondition } from "/redux/progress/service"
import DataLocationsService from "./DataLocationsService"
import { getLocation } from "/utils"
import AsyncStorage from "@react-native-community/async-storage"
import { searchInventoryItem } from "/redux/progress/service"
import moment from "moment"

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
  const [checkBox, setCheckBox] = useState(false)
  const [modalShowImage, setModalShowImage] = useState<any>(false)
  const [modalShowImageOut, setModalShowImageOut] = useState<any>(false)
  const [infoImageModal, setInfoImageModal] = useState<any>(null)
  const refAreaNumber = useRef<any>(null)
  const [textSearch, setTextSearch] = useState<any>(null)
  const [autoShow, setAutoShow] = useState<boolean>(false)
  const [cacheListSearch, setCacheListSearch] = useState<any>([])
  const [listChoose, setListChoose] = useState<any>([])
  const [currentPage, setCurrentPage] = useState(1)
  const [isLoadMore, setIsLoadMore] = useState(false)
  const [conditionData, setConditionData] = useState([])
  const [conditionDataState, setConditionDataState] = useState<any>(null)


  const { buildingData, floorData, itemTypesData, handleGetBuildingDataService } = DataLocationsService(idClient)
  const refScrollView = useRef<any>(null)

  const nextFunc = () => {
    setModalBox(true)
    setBuildingDataState(null)
    setFloorDataState(null)
    setAreaNumber(null)
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

  useEffect(() => {
    getConditionDataFunc()
  }, [])

  const searchList = () => {
    setModalBox(true)
    setIsSearchList(true)
    refConditionData.current = { itemTypeName: "ALL", itemTypeID: 0 }
    setBuildingDataState(refBuildingData.current)
    setFloorDataState(refFloorData.current)
    setItemTypeState(refItemTypeData.current)
    setConditionDataState(refConditionData.current)
    setCurrentPage(1)
    searchInventory(1, false)
  }

  const closeModal = () => {
    setModalBox(false)
    setIsSearchList(false)
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
      setAutoShow(true)
      setTimeout(() => {
        setAutoShow(false)
      }, 1500)
    } else {
      Alert.alert(`Something wrong! Check again (${rs?.status || 0})`)
    }
  }

  const handleOpenScan = () => {
    navigate(Routes.CAMERA_BARCODE, { idClient, isListScan: true })
  }

  const funcGetInfoInventory = useCallback(
    (inventoryItemId: any) => {
      getInventoryItemInfo(
        inventoryItemId,
        (rs) => {
          const rsData = rs?.data?.[0]
          setInventoryItemList((preSta) => preSta?.concat({ ...rsData, inventoryItemId }))
          setInventoryItemListID((preSta) => preSta?.concat(inventoryItemId))
        },
        (err) => {
          Alert.alert(`Inventory Item ID invalid (${err?.status || 0})`)
        },
        idClient,
      )
    },
    [idClient, inventoryItemList, inventoryItemListID],
  )

  const getValueByCode = (item: any, code: any) => {
    const itemByCode = item?.filter((it: any) => it?.itemTypeOptionCode === code)
    if (isEmpty(itemByCode)) {
      return 'hide_txt_value'
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

  const renderExpiredDate = (value: any) => {
    const poOrderDate = value?.itemTypeAdditionalOption?.poOrderDate
    if (poOrderDate) {
      const dateUsed = moment().diff(poOrderDate, 'days')
      const daysLeft = +value?.warrantyYears * 365 - dateUsed
      const formatString = daysLeft < 0 ? 'Expired' : `${Number.parseInt(daysLeft / 30 + '')} ${daysLeft / 30 >= 2 ? 'months' : 'month'} ${Number.parseInt(daysLeft % 30 + '')} ${daysLeft % 30 > 1 ? 'days' : 'day'
        } `
      return formatString
    } else {
      return ''
    }
  }


  const funcGetDetailInventoryItem = async (inventoryItemId: any) => {
    await getInventoryItemInfo(
      inventoryItemId,
      (rs: any) => {
        const rsData = rs?.data?.[0]
        const itemTypeOptionsCache = rsData?.itemTypeOptions
        const formatBody = {
          "Item Type Name": rsData?.itemTypeName,
          "Warranty": renderExpiredDate(rsData),
          mainPhoto: rsData?.mainPhoto || rsData?.mainImage,
          Condition: rsData?.condition,
          "Note For Item": rsData?.noteForItem,
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
          "Unit": getValueByCode(itemTypeOptionsCache, "Unit"),
          "Width": getValueByCode(itemTypeOptionsCache, "Width"),
          "Height": getValueByCode(itemTypeOptionsCache, "Height"),
          "Seat Height": getValueByCode(itemTypeOptionsCache, "SeatHeight")
        }
        setInfoDetail(formatBody)
        setDetailShow(true)
      },
      (err: any) => {
        Alert.alert(`Inventory Item ID invalid (${err?.status})`)
      },
      idClient,
    )
  }

  useFocusEffect(
    useCallback(() => {
      if (!inventoryItemId) return
      funcGetInfoInventory(inventoryItemId)
    }, [inventoryItemId]),
  )

  const deleteRow = (index?: any) => {
    Alert.alert("", "Do you want to delete it?", [
      {
        text: "No",
        onPress: () => console.log("Cancel Pressed"),
        style: "cancel",
      },
      {
        text: "Yes",
        onPress: () => handleDeleteRow(index),
      },
    ])
  }

  useEffect(() => {
    if (isSearchList) {
      refAreaNumber.current = areaNumber
    }
  }, [areaNumber])

  const handleDeleteRow = (index: any) => {
    setInventoryItemList((preState) => {
      preState?.splice(index, 1)
      return [...preState]
    })
  }

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

  const getValueItemOptions = (item: any) => {
    return get(item, "itemTypeOptionReturnValue.[0].returnValue")
  }
  const renderTxtLocations = (item: any) => {
    const building = item?.itemTypeOptions?.filter((it: any) => it?.itemTypeOptionCode === "Building")?.[0]
    const floor = item?.itemTypeOptions?.filter((it: any) => it?.itemTypeOptionCode === "Floor")?.[0]
    const room = item?.itemTypeOptions?.filter((it: any) => it?.itemTypeOptionCode === "AreaOrRoom")?.[0]
    return `${getValueItemOptions(building)} - Floor: ${getValueItemOptions(floor)} - Room: ${getValueItemOptions(
      room,
    )}`
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

  const conditionDisable = useCallback(() => {
    if (inventoryItemList.length > 0) {
      return false
    } else {
      return true
    }
  }, [inventoryItemList])

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

  const handleLoadMore = () => {
    setCurrentPage((preState) => preState + 1)
  }

  const searchInventory = async (currentPage: number, isLoadMore: boolean) => {
    const body = {
      clientId: idClient,
      buildingId: buildingDataState?.buildingID,
      floorId: floorDataState?.floorID,
      room: areaNumber || "",
      itemTypeId: itemTypeState?.itemTypeID,
      searchString: textSearch,
      itemsPerPage: 10,
      currentPage,
      conditionID: conditionDataState.itemTypeID,
    }
    const rs = await searchInventoryItem(body)
    if (rs?.status === 200) {
      setListSearch(isLoadMore ? (preState: any) => [...preState, ...rs?.data] : rs?.data)
    } else {
      Alert.alert(`Inventory Item Not Found (${rs?.status || 0})`)
    }
    setInventoryItemListID([])
    setListChoose([])
    setIsLoadMore(false)
  }

  useEffect(() => {
    if (currentPage === 1) return
    setIsLoadMore(true)
    searchInventory(currentPage, true)
  }, [currentPage])

  const chooseItemToAdd = (__inventoryItemId: any) => {
    funcGetInfoInventory(__inventoryItemId)
    setModalBox(false)
    setIsSearchList(false)
    chooseFunc(false, __inventoryItemId)
  }

  const addAllItem = () => {
    listChoose?.forEach((it: any) => {
      chooseItemToAdd(it)
    })
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
    isEmpty(inventoryItemList) ? setCacheListSearch(listSearch) : setCacheListSearch(filterSearchList)
  }, [inventoryItemList, listSearch])

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
  const selectAllFunc = (isAdd: boolean) => {
    cacheListSearch?.forEach((it: any) => {
      const checkSome = listChoose?.some((itChoose: any) => itChoose === it?.inventoryItemId)
      if (checkSome && isAdd) return
      chooseFunc(isAdd, it?.inventoryItemId)
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
    listSearch: cacheListSearch,
    idClient,
    valueToolTip,
    checkBox,
    textSearch,
    infoDetail,
    noDoneBtn,
    infoImageModal,
    modalShowImage,
    autoShow,
    modalShowImageOut,
    listChoose,
    refScrollView,
    isLoadMore,
    refItemTypeData,
    refBuildingData,
    refFloorData,
    conditionDataState,
    refConditionData,
    addItemType,
    renderTxtLocations,
    deleteRow,
    handleOpenScan,
    handleOpenModal,
    setGPSValue,
    getAndShowLocation,
    setAreaNumber,
    goBack,
    setDetailShow,
    conditionDisable,
    nextFunc,
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
    addAllItem,
    setCheckBox,
    setTextSearch,
    removeAll,
    setInfoDetail,
    setNoDoneBtn,
    showImageZoom,
    setModalShowImage,
    setAutoShow,
    setModalShowImageOut,
    chooseFunc,
    setListChoose,
    selectAllFunc,
    funcGetDetailInventoryItem,
    getValueByCode,
    renderValueArr,
    setCurrentPage,
    handleLoadMore,
    setIsSearchList,
    setBuildingDataState,
    setFloorDataState,
    setItemTypeState,
    setConditionDataState,
    renderExpiredDate,
  }
}

export default useDetailLIstInventory
