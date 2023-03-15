import { useFocusEffect, useNavigation } from "@react-navigation/native"
import React, { useCallback, useState } from "react"
import { FlatList, Image, Alert, Text, TouchableOpacity, View } from "react-native"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import ModalBox from "react-native-modalbox"
import IconImage from "react-native-vector-icons/FontAwesome5"
import FontAwesome from "react-native-vector-icons/FontAwesome"
import Routes from "/navigators/Routes"
import Icon from "react-native-vector-icons/AntDesign"

import { getScreenWidth } from "../../utils/dimension"
import AsyncStorage from "@react-native-community/async-storage"
import useGlobalData from "/utils/hook/useGlobalData"
import {
  deleteDataInventoryClient,
  getListInventoryClientByQuery,
  STATUS_CLIENT,
} from "/utils/sqlite/tableInventoryClient"
import { get } from "lodash"
import styles from './styles'
type Item = {
  client?: any
  deviceDate?: any
  time?: any
  date?: any
}
const InProgressScreen = () => {
  const { navigate, goBack } = useNavigation()
  const [, setglobalData] = useGlobalData()

  const [modalErr, setmodalErr] = useState(false)
  const [dataItemType, setdataItemType] = useState<Item>()
  const [openModalStatus, setOpenModalStatus] = useState(false)
  const [status, setStatus] = useState("InProgress")
  const [exampleState, setExampleState] = useState([])
  const [indexDelete, setIndexDelete] = useState<any>("")

  const openModal = (item?: any, index?: any) => {
    setmodalErr(true)
    setdataItemType(item.item)
    setIndexDelete(index)
  }

  useFocusEffect(
    useCallback(() => {
      setExampleState([])
      getMyStringValue()
    }, [status]),
  )

  const getMyStringValue = async () => {
    try {
      getListInventoryClientByQuery(
        "status = ?",
        [status == STATUS_CLIENT.IN_PROGRESS ? STATUS_CLIENT.IN_PROGRESS : STATUS_CLIENT.SUBMITTED],
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
    let data = JSON.stringify(arr)
    AsyncStorage.setItem("@DataInventory", data)
    handleDeleteSqlite(get(itemDelete[0], "id"))
    navigate(Routes.HOME_SCREEN)
  }

  const handleDeleteSqlite = (idClient?: string) => {
    idClient && deleteDataInventoryClient(idClient)
  }
  const renderItem = (item: any): any => {
    if (status == item.item.status) {
      return (
        <TouchableOpacity
          key={item.id}
          onPress={() => openModal(item)}
          style={styles.item}
          disabled={item.item.status == "Submited" ? true : false}
        >
          <View style={{ flex: 0.5, marginHorizontal: 15, marginTop: 5 }}>
            <IconImage name={"laptop-code"} size={20} />
          </View>
          <View style={{ flex: 5 }}>
            <Text style={styles.title}>{item.item.client ? item.item.client : "Client"}</Text>
            <Text style={[styles.subTitle, { marginTop: 5 }]}>{`${item.item.date} ${item.item.time}`}</Text>
            <Text style={[styles.subTitle, { marginTop: 5 }]}>{item.item.status}</Text>
          </View>
        </TouchableOpacity>
      )
    }
  }
  const removeFew = async () => {
    const keys = ["@custBuidingName", "@custFloor", "@custGPS"]
    try {
      await AsyncStorage.multiRemove(keys)
    } catch (e) {
      // remove error
    }
  }

  const renderStatus = () => {
    if (status == "InProgress") {
      return "In Progress"
    } else if (status == "Submited") {
      return "Submited"
    }
  }

  return (
    <View
      style={{
        flex: 1,
        backgroundColor: "#f1f0f6",
      }}
    >
      <View style={{ width: getScreenWidth(1), height: 50 }}></View>
      <KeyboardAwareScrollView
        bounces={false}
        showsVerticalScrollIndicator={false}
        extraScrollHeight={150}
        enableOnAndroid={true}
        style={{ flex: 8 }}
      >
        <View
          style={{
            flex: 1,
            justifyContent: "center",
          }}
        >
          <View style={{ flex: 1, padding: 10, flexDirection: "row" }}>
            <TouchableOpacity onPress={() => goBack()} style={{ flex: 2, justifyContent: "center" }}>
              <Icon name={"left"} size={25} color={"blue"} />
            </TouchableOpacity>
            <View style={{ flex: 8 }}></View>
          </View>
          <Text style={{ marginLeft: 10, fontSize: 26, color: "#000", fontWeight: "bold", margin: 5 }}>
            Inventory Collection
          </Text>
        </View>
        <View style={{ flex: 1, paddingVertical: 10, marginTop: 20 }}>
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
        </View>

        <FlatList
          data={exampleState}
          extraData={exampleState}
          renderItem={(item) => renderItem(item)}
          keyExtractor={(item: any) => item.time}
        />
      </KeyboardAwareScrollView>
      <ModalBox
        transparent={true}
        isOpen={modalErr}
        entry={"bottom"}
        position={"bottom"}
        swipeToClose={true}
        onClosed={() => setmodalErr(false)}
        style={styles.mainModal}
      >
        <View style={{ flex: 1 }}>
          <View style={{ flex: 0.1, flexDirection: "row", marginTop: 0 }}>
            <View style={{ flex: 3 }} />
            <View style={styles.border}></View>
            <View style={{ flex: 3 }} />
          </View>
          <View style={{ flex: 1, marginTop: 10, marginHorizontal: 15 }}>
            <Text style={{ fontWeight: "bold", fontSize: 18 }}>{dataItemType?.client}</Text>
            <Text style={{ fontSize: 14, marginTop: 10 }}>{`${dataItemType?.date}  ${dataItemType?.time} `}</Text>
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
            <View style={{ marginTop: 40, flexDirection: "row", justifyContent: "space-around" }}>
              <TouchableOpacity style={{ flexDirection: "row" }} onPress={() => changeStatus("InProgress")}>
                <FontAwesome name={status == "InProgress" ? "check-circle-o" : "circle-o"} size={25} color={"gray"} />
                <Text style={styles.txtProgress}> In Progress</Text>
              </TouchableOpacity>
              <TouchableOpacity style={{ flexDirection: "row" }} onPress={() => changeStatus("Submited")}>
                <FontAwesome name={status == "Submited" ? "check-circle-o" : "circle-o"} size={25} color={"gray"} />
                <Text style={styles.txtProgress}> Submited</Text>
              </TouchableOpacity>
            </View>
          </View>
        </View>
      </ModalBox>
    </View>
  )
}

export default InProgressScreen
