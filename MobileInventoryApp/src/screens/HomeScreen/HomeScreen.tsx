import { useFocusEffect, useNavigation } from "@react-navigation/native"
import React, { useCallback, useMemo, useState, useEffect, useRef } from "react"
import { FlatList, Image, Alert, Text, TouchableOpacity, View, Linking } from "react-native"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import ModalBox from "react-native-modalbox"
import IconImage from "react-native-vector-icons/FontAwesome5"
import FontAwesome from "react-native-vector-icons/FontAwesome"
import Routes from "/navigators/Routes"
import Icon from "react-native-vector-icons/AntDesign"

import { getScreenWidth } from "../../utils/dimension"
import AsyncStorage from "@react-native-community/async-storage"
import useGlobalData from "/utils/hook/useGlobalData"
import { logout } from "/redux/logout/actions"
import { manufacturer } from "/redux/manufacturer/actions"
import LoadingOverlay from "/components/LoadingView/LoadingOverlay"
import { BaseButton } from "/components"
import {
  deleteDataInventoryClient,
  getListInventoryClientByQuery,
  STATUS_CLIENT,
  STATUS_CLIENT_ID,
  STATUS_CLIENT_TITLE,
  insertDataInventoryClient
} from "/utils/sqlite/tableInventoryClient"
import { get, isEmpty } from "lodash"
import Storage from "/helper/Storage"
import { useDispatch } from "react-redux"
import styles from "./styles"
import moment from "moment"
import Submitted from "./Submitted"
import SwipeOut from 'react-native-swipeout'
import { Colors } from "/configs"

type RenderItemProps = {
  item?: any
  index?: number
}
type Item = {
  client?: any
  deviceDate?: any
  dataItemType?: any[]
}

let clientInventorySite = "https://ssidb-test.systemsource.com/inventory/"

const HomeScreen = () => {
  const { navigate, goBack } = useNavigation()
  const [, setglobalData] = useGlobalData()

  const [modalErr, setmodalErr] = useState(false)
  const [dataItemType, setdataItemType] = useState<Item>()
  const [openModalStatus, setOpenModalStatus] = useState(false)
  const [status, setStatus] = useState("InProgress")
  const [exampleState, setExampleState] = useState([])
  const [indexDelete, setIndexDelete] = useState<any>("")
  const [loading, setLoading] = useState<boolean>(false)
  const [tableID, setTableID] = useState<number>(STATUS_CLIENT_ID.IN_PROGRESS)
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
      const statusIdTable: number = renderStatusId()
      setTableID(statusIdTable)
    }, [status]),
  )

  const getUserAndPassIntoLink = async () => {
    try {
      const user = await Storage.get("@User")
      const objUser = JSON.parse(`${user}`)
      clientInventorySite = clientInventorySite + `authentication/${objUser?.userId}`
    } catch (error) {
      console.log("error", error)
    }
  }

  useEffect(() => {
    getUserAndPassIntoLink()
    getManufactoryFunc()
  }, [])

  const getManufactoryFunc = () => {
    dispatch(manufacturer.request(null))
  }

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
            console.log('data',data)
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

  const onSubmitDeleteAll = async () => {
    Alert.alert("Delete All Submission", "Are you sure you wish to delete all this submission", [
      {
        text: "No",
        onPress: () => console.log("Cancel Pressed"),
        style: "cancel",
      },
      { text: "Yes", onPress: () => onDeleteAll() },
    ])
  }

  const onDeleteAll = async () => {
    exampleState?.forEach((it: any) => {
      handleDeleteSqlite(get(it, 'id'))
    })
    setExampleState([])
    await AsyncStorage.setItem('@DataInventory', JSON.stringify([]))
    navigate(Routes.HOME_SCREEN)
  }

  const handleDeleteSqlite = (idClient?: string) => {
    idClient && deleteDataInventoryClient(idClient)
  }
  const swipeOutBtn = [
    {
      text: 'Delete',
      backgroundColor: Colors.redHeader,
      onPress: () => {
        handleDelete()
      }

    }
  ]
  const renderItem = ({ item, index }: RenderItemProps): any => {
    if (status == item?.status) {
      return (
        <SwipeOut close={true} style={styles.containerSwipeOut} onOpen={() => {
          setdataItemType(item)
          setIndexDelete(index)
        }} right={swipeOutBtn}>
          <TouchableOpacity key={item.id} onPress={() => openModal(item, index)} style={styles.item}>
            <View style={{ flex: 0.5, marginHorizontal: 15, marginTop: 5 }}>
              <IconImage name={"laptop-code"} size={20} />
            </View>
            <View style={{ flex: 5 }}>
              <Text style={styles.title}>{item?.client ? item?.client : "Client"}</Text>
              <Text style={[styles.subTitle, { marginTop: 5 }]}>{`${moment(item?.deviceDate).format(
                "DD/MM/YYYY hh:mm a z",
              )}`}</Text>
              <Text style={[styles.subTitle, { marginTop: 5 }]}>{item?.status}</Text>
            </View>
          </TouchableOpacity>
        </SwipeOut>
      )
    }
  }
  const renderStatus = () => {
    if (status == "InProgress") {
      return STATUS_CLIENT_TITLE.IN_PROGRESS
    } else if (status == "Submitted") {
      return STATUS_CLIENT_TITLE.SUBMITTED
    } else if (status == "InvalidData") {
      return STATUS_CLIENT_TITLE.FAILED_SUBMISSION
    } else if (status == "ExistedSubmission") {
      return STATUS_CLIENT_TITLE.EXISTED_SUBMISSION
    }
  }

  const renderStatusId = () => {
    switch (status) {
      case "InProgress":
        return STATUS_CLIENT_ID.IN_PROGRESS
      case "Submitted":
        return STATUS_CLIENT_ID.SUBMITTED
      case "InvalidData":
        return STATUS_CLIENT_ID.FAILED_SUBMISSION
      default:
        return STATUS_CLIENT_ID.SUBMITTED
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

  const handleSaveSqlite = async (cb: (id: string | number) => void) => {
    try {
      const date = moment().format("MM/DD/YYYY HH:mm")
      let raw = moment(date, "MM/DD/YYYY HH:mm")
      let dataT = raw.format("MM/DD/YYYY HH:mm")
      const clientFirst: any = await AsyncStorage.getItem("CLIENT_ID")
      const parseClientFirst = await JSON.parse(clientFirst)
      const { clientID, inventory_Client_Group_ID } = parseClientFirst || {}
      const clientId = parseInt(clientID, 10)
      const InventoryClientGroupID = parseInt(inventory_Client_Group_ID)
      const user = await Storage.get("@User")
      const objUser = JSON.parse(`${user}`)
      const data = [
        objUser?.userId,
        dataT,
        parseClientFirst?.clientName,
        clientId,
        STATUS_CLIENT.IN_PROGRESS,
        0,
        1,
        "",
        InventoryClientGroupID,
      ]
      insertDataInventoryClient(data, cb)
    } catch (e) {
      console.log("error", e)
    }
  }

  const handleNewClient = async (isScanList: boolean = false, isUpdateInventory: boolean = false, isOrderScreen: boolean = false) => {
    removeFew()
    const date = moment().format("MM/DD/YYYY HH:mm")
    let raw = moment(date, "MM/DD/YYYY HH:mm")
    let dataT = raw.format("MM/DD/YYYY HH:mm")
    const clientFirst: any = await AsyncStorage.getItem("CLIENT_ID")
    const parseClientFirst = await JSON.parse(clientFirst)
    if (!isEmpty(parseClientFirst)) {
      const { clientID, inventory_Client_Group_ID } = parseClientFirst || {}
      if (isScanList) {
        navigate(Routes.INVENTORY_RE_LOCATE_SCREEN, { idClient: clientID })
        return
      }
      if (isUpdateInventory || isOrderScreen) {
        navigate(isOrderScreen ? Routes.ORDER_SCREEN : Routes.UPDATE_INVENTORY_SCREEN, { idClient: clientID })
        let data = [
          {
            deviceDate: dataT,
            client: parseClientFirst?.clientName,
            clientId: clientID,
            status: STATUS_CLIENT.IN_PROGRESS,
            dataItemType: [],
            isBarScan: false,
            InventoryClientGroupID: inventory_Client_Group_ID,
          },
        ]
        let raw = { item: data }
        setglobalData(raw)
        return
      }
      handleSaveSqlite((id: string | number) => {
        let data = [
          {
            deviceDate: dataT,
            client: parseClientFirst?.clientName,
            clientId: clientID,
            status: STATUS_CLIENT.IN_PROGRESS,
            dataItemType: [],
            isBarScan: false,
            InventoryClientGroupID: inventory_Client_Group_ID,
            id: id,
          },
        ]
        console.log('handleSaveSqlite',data)
        let raw = { item: data }
        setglobalData(raw)
        navigate(Routes.INVENTORY_DETAIL_LIST_SCREEN, { idClient: id, isNotGoBack: true })
      })
    } else {
      navigate(Routes.START_INVENTORY_SCREEN, { idInventory: "1", isScanList, isUpdateInventory, isOrderScreen })
    }
  }

  const handleGoToInventorySite = () => {
    Linking.openURL(clientInventorySite)
  }
  const renderLogout = useMemo(() => {
    return (
      <TouchableOpacity onPress={handleLogout} style={styles.touchLogout}>
        <Text style={{ fontSize: 18, color: "blue" }}>Logout</Text>
      </TouchableOpacity>
    )
  }, [])

  return (
    <View style={styles.container}>
      <View style={{ width: getScreenWidth(1), height: 50 }}></View>
      <KeyboardAwareScrollView
        bounces={false}
        showsVerticalScrollIndicator={false}
        extraScrollHeight={150}
        enableOnAndroid={true}
        style={{ flex: 8 }}
      >
        <View style={styles.content}>
          <View style={styles.viewLeft}>
            <TouchableOpacity onPress={() => handleGoToInventorySite()} style={[styles.btnLeft, { flexDirection: 'row', }]}>
              <Text style={{ justifyContent: 'center', fontSize: 18, color: "blue", alignItems: 'center', marginTop: 5, textDecorationLine: 'underline' }}>Client Site</Text>
            </TouchableOpacity>
            <View style={styles.logoOut}></View>
            {renderLogout}
          </View>
          <View style={{ justifyContent: "center", alignItems: "center", marginVertical: 15 }}>
            <BaseButton
              backgroundColor={"#BE3030"}
              label={"Create New Inventory"}
              onPress={() => handleNewClient()}
              fontSize={14}
              width={"70%"}
            />
            <BaseButton
              backgroundColor={"#BE3030"}
              label={"Update Inventory"}
              onPress={() => handleNewClient(false, true)}
              fontSize={14}
              width={"70%"}
            />
            <BaseButton
              backgroundColor={"#BE3030"}
              label={"Re-Locate Inventory"}
              onPress={() => handleNewClient(true)}
              fontSize={14}
              width={"70%"}
            />
            <BaseButton
              backgroundColor={"#BE3030"}
              label={"Order Inventory"}
              onPress={() => handleNewClient(false, false, true)}
              fontSize={14}
              width={"70%"}
            />
          </View>
          <Text style={styles.inventoryCollection}>Inventory Collection</Text>
        </View>

        <View style={{ flex: 1, paddingVertical: 10, marginTop: 20, flexDirection: 'row', alignItems: 'center' }}>
          <TouchableOpacity
            style={{ flexDirection: "row", alignItems: "center" }}
            onPress={() => setOpenModalStatus(true)}
          >
            <Text style={{ fontSize: 18, color: "#000", marginLeft: 10 }}>{renderStatus()}</Text>
            <Image
              source={require("../../assets/images/down-arrow.png")}
              style={{ height: 16, width: 16, marginTop: 2, marginLeft: 5 }}
            />
          </TouchableOpacity>
          <TouchableOpacity
            style={styles.doneBtn}
            disabled={status != STATUS_CLIENT.IN_PROGRESS}
            onPress={onSubmitDeleteAll}
          >
            <Text style={[styles.txtBtn, { color: status == STATUS_CLIENT.IN_PROGRESS ? Colors.blue : Colors.gray_80 }]}>Delete All</Text>
          </TouchableOpacity>
        </View>

        <FlatList
          data={exampleState}
          extraData={exampleState}
          renderItem={(item: any) => renderItem(item)}
          keyExtractor={(item: any, index: number) => `${index}`}
        />
      </KeyboardAwareScrollView>
      <ModalBox
        isOpen={modalErr}
        entry={"bottom"}
        position={"bottom"}
        onClosed={() => setmodalErr(false)}
        style={[styles.mainModal, status === STATUS_CLIENT.SUBMITTED && { flex: 0.9 }]}
        swipeToClose={status !== STATUS_CLIENT.SUBMITTED}
      >
        {status !== STATUS_CLIENT.SUBMITTED ? (
          <View style={{ flex: 1 }}>
            <View style={{ flex: 0.1, flexDirection: "row", marginTop: 0 }}>
              <View style={{ flex: 3 }} />
              <View style={styles.border}></View>
              <View style={{ flex: 3 }} />
            </View>
            <View style={{ flex: 1, marginTop: 10, marginHorizontal: 15 }}>
              <Text style={{ fontWeight: "bold", fontSize: 18 }}>{dataItemType?.client}</Text>
              <Text style={{ fontSize: 14, marginTop: 10 }}>{`${moment(dataItemType?.deviceDate).format(
                "DD/MM/YYYY hh:mm a z",
              )}`}</Text>
              <View style={{ marginTop: 40 }}>
                <TouchableOpacity style={{ flexDirection: "row" }} onPress={() => handleFillout()}>
                  <Icon name={"edit"} size={25} color={"gray"} />
                  <Text style={styles.txtProgress}> Edit</Text>
                </TouchableOpacity>
              </View>
              <View style={{ marginTop: 20 }}>
                <TouchableOpacity style={{ flexDirection: "row" }} onPress={() => handleDelete()}>
                  <Icon name={"delete"} size={25} color={"gray"} />
                  <Text style={styles.txtProgress}>Delete</Text>
                </TouchableOpacity>
              </View>
            </View>
          </View>
        ) : (
          <Submitted dataItemType={dataItemType} />
        )}
      </ModalBox>
      <ModalBox
        transparent={true}
        isOpen={openModalStatus}
        entry={"bottom"}
        position={"bottom"}
        swipeToClose={true}
        onClosed={() => setOpenModalStatus(false)}
        style={styles.mainModal}
      >
        <View style={{ flex: 1 }}>
          <View style={{ flex: 0.1, flexDirection: "row", marginTop: 0 }}>
            <View style={{ flex: 3 }} />
            <View style={styles.border}></View>
            <View style={{ flex: 3 }} />
          </View>
          <View style={{ flex: 1, marginTop: 10, marginHorizontal: 15 }}>
            <Text style={{ fontWeight: "bold", fontSize: 18 }}>Select status</Text>

            <View style={{ flexDirection: "row", marginTop: 20 }}>
              <TouchableOpacity style={styles.btnSelect} onPress={() => changeStatus("ExistedSubmission")}>
                <FontAwesome
                  name={status == "ExistedSubmission" ? "check-circle-o" : "circle-o"}
                  size={25}
                  color={"gray"}
                />
                <Text style={styles.txtProgress}>{STATUS_CLIENT_TITLE.EXISTED_SUBMISSION}</Text>
              </TouchableOpacity>
              <TouchableOpacity style={styles.btnSelect} onPress={() => changeStatus("InvalidData")}>
                <FontAwesome name={status == "InvalidData" ? "check-circle-o" : "circle-o"} size={25} color={"gray"} />
                <Text style={styles.txtProgress}>{STATUS_CLIENT_TITLE.FAILED_SUBMISSION}</Text>
              </TouchableOpacity>
            </View>
            <View style={{ flexDirection: "row", marginTop: 20 }}>
              <TouchableOpacity style={styles.btnSelect} onPress={() => changeStatus("InProgress")}>
                <FontAwesome name={status == "InProgress" ? "check-circle-o" : "circle-o"} size={25} color={"gray"} />
                <Text style={styles.txtProgress}>{STATUS_CLIENT_TITLE.IN_PROGRESS}</Text>
              </TouchableOpacity>
              <TouchableOpacity style={styles.btnSelect} onPress={() => changeStatus("Submitted")}>
                <FontAwesome name={status == "Submitted" ? "check-circle-o" : "circle-o"} size={25} color={"gray"} />
                <Text style={styles.txtProgress}>{STATUS_CLIENT_TITLE.SUBMITTED}</Text>
              </TouchableOpacity>
            </View>
          </View>
        </View>
      </ModalBox>
      <LoadingOverlay visible={loading} />
    </View>
  )
}
export default HomeScreen
