import { useFocusEffect, useNavigation } from "@react-navigation/native"
import { useCallback, useState } from "react"
import { Alert } from "react-native"
import Routes from "/navigators/Routes"

import AsyncStorage from "@react-native-community/async-storage"
import useGlobalData from "/utils/hook/useGlobalData"
import { logout } from "/redux/logout/actions"
import {
  deleteDataInventoryClient,
  getListInventoryClientByQuery,
  STATUS_CLIENT,
} from "/utils/sqlite/tableInventoryClient"
import { get } from "lodash"
import Storage from "/helper/Storage"
import { useDispatch } from "react-redux"
type Item = {
  client?: any
  deviceDate?: any
}
const HomeScreenIndex = () => {
  const { navigate, goBack } = useNavigation()
  const [, setglobalData] = useGlobalData()

  const [modalErr, setmodalErr] = useState(false)
  const [dataItemType, setdataItemType] = useState<Item>()
  const [openModalStatus, setOpenModalStatus] = useState(false)
  const [status, setStatus] = useState("InProgress")
  const [exampleState, setExampleState] = useState([])
  const [indexDelete, setIndexDelete] = useState<any>("")
  const [loading, setLoading] = useState<boolean>(false)

  const dispatch = useDispatch()

  const openModal = (item: any, index?: any) => {
    setmodalErr(true)
    setdataItemType(item)
    setIndexDelete(index)
  }

  useFocusEffect(
    useCallback(() => {
      setExampleState([])
      getMyStringValue()
    }, [status]),
  )
  const checkedStatusValueSubmission = (status: string, STATUS_CLIENT: any): void => {
    switch (status) {
      case STATUS_CLIENT.IN_PROGRESS:
        return STATUS_CLIENT.IN_PROGRESS
      case STATUS_CLIENT.SUBMITTED:
        return STATUS_CLIENT.SUBMITTED
      case STATUS_CLIENT.INVALID_DATA:
        return STATUS_CLIENT.INVALID_DATA
      case STATUS_CLIENT.EXISTED_SUBMISSON:
        return STATUS_CLIENT.EXISTED_SUBMISSON
      default:
      case STATUS_CLIENT.IN_PROGRESS:
    }
  }
  const getMyStringValue = async () => {
    try {
      const user = await Storage.get("@User")
      const objUser = JSON.parse(`${user}`)
      getListInventoryClientByQuery(
        "status = ? AND userId = ?",
        [checkedStatusValueSubmission(status, STATUS_CLIENT), objUser.userId],
        (data) => {
          if (data) {
            setExampleState(data.reverse())
          }
        },
      )
    } catch (e) {
      // read error
    }
  }
  const changeStatus = (i: string) => {
    if (i !== status) {
      setStatus(i)
    }
    setOpenModalStatus(false)
  }

  const handleFillout = () => {
    setmodalErr(false)
    let ar = [dataItemType]
    let raw: any = { item: ar }
    setglobalData(raw)
    raw?.item[0]?.id && navigate(Routes.INVENTORY_DETAIL_LIST_SCREEN, { idClient: raw?.item[0]?.id })
  }
  const handleDelete = () => {
    setmodalErr(false)
    Alert.alert("Delete Submission", "Are you sure you wish to delete this submission", [
      {
        text: "No",
        onPress: () => console.log("Cancel Pressed"),
        style: "cancel",
      },
      { text: "Yes", onPress: () => onDeleted() },
    ])
  }

  const onDeleted = () => {
    let arr = exampleState
    const itemDelete: any = arr.splice(indexDelete, 1)
    setExampleState([...arr])
    let data = JSON.stringify(arr)
    AsyncStorage.setItem("@DataInventory", data)
    handleDeleteSqlite(get(itemDelete[0], "id"))
    navigate(Routes.HOME_SCREEN)
  }

  const handleDeleteSqlite = (idClient?: string) => {
    idClient && deleteDataInventoryClient(idClient)
  }
  const renderStatus = () => {
    if (status == "InProgress") {
      return "In Progress"
    } else if (status == "Submitted") {
      return "Submitted"
    } else if (status == "InvalidData") {
      return "Submission Fail"
    } else if (status == "ExistedSubmission") {
      return "Existed Submission"
    }
  }

  const handleLogoutError = (err: any) => {
    setLoading(false)
  }
  const handleLogoutSuccess = async () => {
    setLoading(false)
  }
  const handleLogout = () => {
    Alert.alert("", "Are you sure you want to sign out?", [
      {
        text: "No",
        style: "cancel",
      },
      {
        text: "Yes",
        onPress: async () => {
          setLoading(true)
          const refreshToken = await Storage.get("@RefreshToken")
          dispatch(
            logout.request({
              refreshToken,
              cbError: handleLogoutError,
              cbSuccess: handleLogoutSuccess,
            }),
          )
        },
      },
    ])
  }
  const removeFew = async () => {
    const keys = ["@custBuidingName", "@custFloor", "@custGPS", "@custAreaNumber"]
    try {
      await AsyncStorage.multiRemove(keys)
    } catch (e) {
      // remove error
    }
  }
  const handleNewClient = () => {
    removeFew()
    navigate(Routes.START_INVENTORY_SCREEN, { idInventory: "1" })
  }
  return {
    status,
    exampleState,
    modalErr,
    dataItemType,
    openModalStatus,
    loading,
    openModal,
    handleLogout,
    goBack,
    handleNewClient,
    setOpenModalStatus,
    renderStatus,
    changeStatus,
    handleFillout,
    handleDelete,
    setmodalErr,
  }
}
export default HomeScreenIndex
