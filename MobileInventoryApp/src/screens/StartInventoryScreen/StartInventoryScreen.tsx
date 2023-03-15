import { useNavigation, useRoute } from "@react-navigation/native"
import React, { useEffect, useState } from "react"
import { Switch, Text, TouchableOpacity, View } from "react-native"
import Colors from "/configs/Colors"
import Routes from "/navigators/Routes"
import { useSelector } from "react-redux"

import { BaseBottomSheet, BaseAlert, BottomButtonNext, Header } from "../../components"
import Icon from "react-native-vector-icons/Ionicons"

import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import DateTimePickerModal from "react-native-modal-datetime-picker"
import moment from "moment"
import { SelectOption } from "/components/Select/types"
import useGlobalData from "/utils/hook/useGlobalData"
import { insertDataInventoryClient, STATUS_CLIENT } from "/utils/sqlite/tableInventoryClient"
import { getClientDataService } from "/redux/progress/service"
import LoadingOverlay from "/components/LoadingView/LoadingOverlay"
import styles from "./styles"

import { get } from "lodash"
import Storage from "/helper/Storage"
import { AppReducerType } from "/redux/reducers"

let dateActive = new Date()

const StartInventoryScreen = () => {
  const { navigate } = useNavigation()
  const navigation = useNavigation()
  const route = useRoute()
  const idInventory = get(route, "params.idInventory")
  const isScanList = get(route, "params.isScanList")
  const isOrderScreen = get(route, "params.isOrderScreen")
  const isUpdateInventory = get(route, "params.isUpdateInventory")
  const [custDate, setcustDate] = useState("")
  const [custClient, setcustClient] = useState("")
  const [clientChose, setclientChose] = useState<{ clientID?: any; inventory_Client_Group_ID?: any }>({})
  const [globalData, setglobalData] = useGlobalData()
  const [loading, setLoading] = useState<boolean>(true)

  const [typeDate, settypeDate] = useState("")
  const [isDatePickerVisible, setDatePickerVisibility] = useState(false)
  const [isEnabled, setisEnabled] = useState(false)
  const [modalNumberCode, setModalNumberCode] = useState(false)
  const [checkNV, setcheckNV] = useState<SelectOption[]>([])
  const userLogin = useSelector((state: any) => state?.[AppReducerType.TOKEN])

  const getClientData = () => {
    getClientDataService(
      (res) => {
        if (res.status == 200) {
          setcheckNV(res.data)
          setLoading(false)
        } else {
          setLoading(false)
          BaseAlert(res, "Get ClientData")
        }
      },
      (e) => {
        setLoading(false)
        BaseAlert(e, "Get ClientData")
      },
      userLogin?.userId,
    )
  }
  useEffect(() => {
    if (custDate) {
      let rawDateTime = custDate
      let dateTime = parseInt(rawDateTime.replace(/[^0-9.]/g, ""))
    }
  }, [custDate])

  useEffect(() => {
    const unsubscribe = navigation.addListener("focus", () => {
      const yourDate = new Date()
      let dateNew = moment(yourDate, "MM/DD/YYYY")
      let dataT = dateNew.format("MM/DD/YYYY")
      let hours = new Date().getHours() //To get the Current Hours
      let min = new Date().getMinutes()
      let finalMin = min > 9 ? min : "0" + min
      let hourNow = `${hours}:${finalMin}`
      setcustDate(`${dataT}  ${hourNow}`)
      setcustClient("")
      setLoading(true)
      setcheckNV([])
      setclientChose({})
      getClientData()
      setisEnabled(false)
      setModalNumberCode(false)
      setDatePickerVisibility(false)
    })
    return unsubscribe
  }, [navigation, route?.params])

  const handleConfirm = (date: any) => {
    dateActive = date
    let raw = moment(date, "MM/DD/YYYY HH:mm")
    let dataT = raw.format("MM/DD/YYYY HH:mm")
    setcustDate(dataT)
    setDatePickerVisibility(false)
  }
  const hideDatePicker = () => {
    setDatePickerVisibility(false)
  }

  const showDatePicker = (type: any) => {
    setDatePickerVisibility(true)
    settypeDate(type)
  }

  const onSelectMethod = (item: any) => {
    setcustClient(item.clientName)
    setclientChose(item)
    setModalNumberCode(false)
  }

  const handleOnPress = () => {
    if (isScanList) {
      navigate(Routes.INVENTORY_RE_LOCATE_SCREEN, { idClient: clientChose?.clientID })
      return
    }
    if (isUpdateInventory || isOrderScreen) {
      navigate(isOrderScreen ? Routes.ORDER_SCREEN : Routes.UPDATE_INVENTORY_SCREEN, { idClient: clientChose?.clientID })
      let data = [
        {
          deviceDate: custDate,
          client: custClient,
          clientId: clientChose?.clientID,
          status: STATUS_CLIENT.IN_PROGRESS,
          dataItemType: [],
          isBarScan: isEnabled,
          InventoryClientGroupID: clientChose?.inventory_Client_Group_ID,
        },
      ]
      let raw = { item: data }
      setglobalData(raw)
      return
    }
    handleSaveSqlite((id: string | number) => {
      let data = [
        {
          deviceDate: custDate,
          client: custClient,
          clientId: clientChose?.clientID,
          status: STATUS_CLIENT.IN_PROGRESS,
          dataItemType: [],
          isBarScan: isEnabled,
          InventoryClientGroupID: clientChose?.inventory_Client_Group_ID,
          id: id,
        },
      ]
      let raw = { item: data }
      setglobalData(raw)
      navigate(Routes.INVENTORY_DETAIL_LIST_SCREEN, { idClient: id, isNotGoBack: true })
    })
  }

  const handleSaveSqlite = async (cb: (id: string | number) => void) => {
    try {
      const clientId = parseInt(clientChose?.clientID, 10)
      const InventoryClientGroupID = parseInt(clientChose?.inventory_Client_Group_ID)
      const user = await Storage.get("@User")
      const objUser = JSON.parse(`${user}`)
      if (idInventory) {
        const data = [
          objUser?.userId,
          custDate,
          custClient,
          clientId,
          STATUS_CLIENT.IN_PROGRESS,
          isEnabled ? 1 : 0,
          idInventory,
          "",
          InventoryClientGroupID,
        ]
        insertDataInventoryClient(data, cb)
      }
    } catch (e) {
      console.log("error", e)
    }
  }
  const conditionDisable = () => {
    if (custDate && custClient) {
      return false
    } else {
      return true
    }
  }

  return (
    <View style={styles.container}>
      <Header isGoBack labels={isScanList ? 'Re-Locate Inventory' : isUpdateInventory ? 'Update Inventory' : "Inventory Collection"} />
      <LoadingOverlay visible={loading} />

      <KeyboardAwareScrollView
        bounces={false}
        showsVerticalScrollIndicator={false}
        extraScrollHeight={150}
        enableOnAndroid={true}
        style={styles.mainContainer}
      >
        {!(isScanList || isUpdateInventory || isOrderScreen) && <Text style={styles.txtContainer}>{" New Inventory"}</Text>}

        {!(isUpdateInventory || isScanList || isOrderScreen) && <View style={{ flexDirection: "row" }}>
          <View style={styles.txtOptions}>
            <Text style={styles.subTxtOptiops}>Date of Activity</Text>
            <TouchableOpacity style={styles.btnCamera} onPress={() => showDatePicker(true)}>
              <Text style={styles.txtValue}>{custDate}</Text>
              {/*<Icon name={"calendar"} size={20} />*/}
            </TouchableOpacity>
          </View>
          <DateTimePickerModal
            isVisible={isDatePickerVisible}
            mode="datetime"
            //locale="en_GB" // Use "en_GB" here
            cancelTextIOS={"Cancel"}
            confirmTextIOS={"Confirm"}
            display={"spinner"}
            onConfirm={handleConfirm}
            onCancel={() => hideDatePicker()}
            minimumDate={new Date(1900, 1, 1)}
            date={dateActive}
          />
        </View>}

        <View style={styles.content}>
          <View style={styles.txtOptions}>
            <Text style={styles.subTxtOptiops}>
              Client <Text style={styles.colorRequired}>*</Text>
            </Text>
            <TouchableOpacity
              disabled={checkNV?.length == 0 ? true : false}
              style={[styles.btnCamera]}
              onPress={() => setModalNumberCode(true)}
            >
              <Text style={styles.txtValue}>{custClient}</Text>
              <Icon name={"list"} size={20} />
            </TouchableOpacity>
          </View>
        </View>

        <View style={styles.content}>
          {/* <View style={styles.txtOptions}>
            <Text style={styles.subTxtOptiops}>
              Need barcode scan <Text style={styles.colorRequired}>*</Text>
            </Text>

            <Switch
              trackColor={{ false: "#767577", true: Colors.button }}
              thumbColor={isEnabled ? "white" : "#f4f3f4"}
              ios_backgroundColor="#3e3e3e"
              onValueChange={() => setisEnabled(!isEnabled)}
              value={isEnabled}
            />
          </View> */}
        </View>
      </KeyboardAwareScrollView>
      <BottomButtonNext disabled={conditionDisable()} onPressButton={() => handleOnPress()} />
      {modalNumberCode && (
        <BaseBottomSheet
          open={modalNumberCode}
          options={checkNV}
          flex={0.6}
          title={"Choose client"}
          type={"client"}
          onSelect={(item) => onSelectMethod(item)}
          onClosed={() => setModalNumberCode(false)}
          onOpened={() => setModalNumberCode(true)}
        />
      )}
    </View>
  )
}
export default StartInventoryScreen
