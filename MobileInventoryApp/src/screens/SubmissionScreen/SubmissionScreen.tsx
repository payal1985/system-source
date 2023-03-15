import { useNavigation, useRoute } from "@react-navigation/native"
import React, { useEffect, useState } from "react"
import { Switch, Text, TouchableOpacity, View, Alert, TextInput } from "react-native"
import Colors from "/configs/Colors"
import { BottomButtonNext, Header } from "../../components"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import ModalBox from "react-native-modalbox"
import IconCheck from "react-native-vector-icons/Feather"
import Routes from "/navigators/Routes"
import useGlobalData from "/utils/hook/useGlobalData"
import AsyncStorage from "@react-native-community/async-storage"
import { uploadProgressService } from "/redux/progress/service"
import { hideLoadingProgress, showLoadingProgress } from "/navigators/appNavigate"
import { getDetailInventoryClient, STATUS_CLIENT, updateDataInventoryClientByQuery } from "/utils/sqlite/tableInventoryClient"
import { get, isArray, isEmpty } from "lodash"
import { useSelector } from "react-redux"
import styles from "./styles"
import Storage from "/helper/Storage"

type tokenState = {
  token: {
    userId: number
  }
}

const SubmissionScreen = () => {
  const { navigate } = useNavigation()
  const route = useRoute()
  const idClient = get(route, "params.idClient")
  const [isEnabled, setIsEnabled] = useState(false)
  const toggleSwitch = () => setIsEnabled((previousState) => !previousState)
  const [isEnabled1, setIsEnabled1] = useState(false)
  const toggleSwitch1 = () => setIsEnabled1((previousState) => !previousState)
  const [isEnabled2, setIsEnabled2] = useState(false)
  const toggleSwitch2 = () => setIsEnabled2((previousState) => !previousState)
  const [modalInfo, setModalInfo] = useState(false)
  const [data, setdata] = useState(null)
  const [globalData, setglobalData] = useGlobalData()
  const navigator = useNavigation()
  const userId = useSelector((state: tokenState) => state.token.userId)
  const [emailSubmit, setEmailSubmit] = useState('')

  useEffect(() => {
    getData()
  }, [])
  const handleOnPressAfterSubmit = async (STATUS_CLIENT: { STATUS_CLIENT: string }) => {
    try {
      let arr1 = globalData.item[0]
      arr1.status = STATUS_CLIENT.toString()
      await AsyncStorage.removeItem("@DataNewItemType")
      navigate(Routes.HOME_SCREEN)
      handleSaveSqlite(arr1, STATUS_CLIENT.toString())
    } catch (e) {
      // save error
    }
  }
  const handleShowErrorMessage = (res: any) => {
    Alert.alert("Error", res?.data ? `${res?.data} (${res?.status || 0})` : `Error api (${res?.status || 0})`, [
      {
        text: "OK",
        onPress: () => console.log("Cancel Pressed"),
        style: "cancel",
      },
    ])
  }
  const conditionHandleSubmit = async () => {
    try {
      if (data) {
        let arr1 = globalData.item[0]
        handleSubmitData(arr1, true)
      } else {
        let arr1 = globalData.item

        handleSubmitData(arr1, false)
      }
    } catch (e) {
      // save error
    }
  }


  const handleFakeTotalCount = () => {
    let arr = {
      nameCondition: "Good",
      txtQuality: "1",
      type: "Good",
      dataCondition: []
    }
    return arr
  }
  const handleSubmitWithProgress = async (dataMainJson: any, mainJson: any, clientId: number, STATUS_CLIENT: any) => {
    const userLogin: any = await Storage.get("@UserLogin")
    const parseUserLogin = JSON.parse(userLogin)
    const loginName = parseUserLogin?.loginName
    const dataMainJsonTemp = dataMainJson.map((item: any) => ({
      ...item,
      LoginUserID: userId,
      EmailUser: isEnabled ? loginName : null,
      ApproverEmail: emailSubmit || null,
      dataItemType: item?.dataItemType.map((i: any) => ({
        ...i,
        itemTypeOptions: i?.itemTypeOptions.map((j: any) => ({
          itemTypeOptionId: j?.itemTypeOptionId,
          itemTypeOptionCode: j?.itemTypeOptionCode,
          itemTypeOptionName: j?.itemTypeOptionName,
          itemTypeOptionReturnValue: isArray(j?.itemTypeOptionReturnValue)
            ? j?.itemTypeOptionReturnValue.map((k: any) => {
              const temp = !!k?.dataCondition
                ? i.conditionRender == 5 ? handleFakeTotalCount() : {
                  ...k,
                  dataCondition: (isArray(k?.dataCondition) ? k?.dataCondition : [k?.dataCondition] || []).map((h: any) => ({
                    ...h,
                    url: (h?.url || []).map((l: any) => ({
                      width: l?.width,
                      height: l?.height,
                      name: l?.name,
                      TempPhotoName: l?.TempPhotoName,
                    })),
                  })),
                }
                : { ...k }
              return temp
            })
            : j?.itemTypeOptionReturnValue,
        })),
      })),
    }))
    console.log("JSONdataMainJsonTemp", JSON.stringify(dataMainJsonTemp))
    showLoadingProgress(navigator, { progress: 1 })
    uploadProgressService(
      dataMainJsonTemp,
      clientId, // pass client id
      (progress: number) => {
        showLoadingProgress(navigator, { progress }, true)
      },
      (res) => {
        console.log('resresres', res)
        hideLoadingProgress(navigator)
        if (res.status == 200) {
          // Success
          handleOnPressAfterSubmit(STATUS_CLIENT.SUBMITTED)
        } else if (res.status == 400) {
          // Invalid data
          handleOnPressAfterSubmit(STATUS_CLIENT.INVALID_DATA)
        } else if (res.status == 409) {
          //Existed submission
          handleOnPressAfterSubmit(STATUS_CLIENT.EXISTED_SUBMISSON)
          Alert.alert('Announce', res?.data?.detail || 'Existed submission')
        } else {
          // ERROR
          handleShowErrorMessage(res)
        }
      },
      async () => {
        hideLoadingProgress(navigator)
        const jsonError: any = await AsyncStorage.getItem('JSON_ERROR')
        if (isEmpty(jsonError)) {
          await AsyncStorage.setItem('JSON_ERROR', JSON.stringify(dataMainJsonTemp))
        } else {
          const parseData = await JSON.parse(jsonError)
          const dataFinal = parseData?.concat(dataMainJsonTemp)
          await AsyncStorage.setItem('JSON_ERROR', JSON.stringify(dataFinal))
        }
        handleShowErrorMessage({})
      },
    )
  }

  const handleSubmitData = async (mainJson: any, statusData: any) => {
    try {
      let body = mainJson
      body.clientId = parseInt(body.clientId)
      let dataMainJson = statusData ? [body] : body

      let clientId = globalData.item[0]?.clientId

      handleSubmitWithProgress(dataMainJson, mainJson, clientId, STATUS_CLIENT)
    } catch (e) {
      console.log("e", e)
    }
  }
  const handleSaveSqlite = (arr1: any, status: string) => {
    //saveSqlite
    if (idClient) {
      const dataItemType = JSON.stringify(arr1?.dataItemType)
      updateDataInventoryClientByQuery("deviceDate = ?, status = ?,  client = ?, dataItemType = ?, LoginUserID = ? where id = ? ", [
        arr1?.deviceDate,
        status,
        arr1?.client,
        dataItemType,
        userId,
        idClient,
      ])
    }
  }
  const getData = async () => {
    try {
      let data = await AsyncStorage.getItem("@DataInventory")
      if (data) {
        let raw = JSON.parse(data)
        setdata(raw)
      }
    } catch (e) {
      // save error
    }
  }
  const openReview = () => {
    setModalInfo(true)
  }
  return (
    <View style={styles.container}>
      <Header isGoBack labels={"Inventory Collection"} />
      <KeyboardAwareScrollView
        bounces={false}
        showsVerticalScrollIndicator={false}
        extraScrollHeight={150}
        enableOnAndroid={true}
        style={styles.keyboardAwreScrollView}
      >
        <Text style={styles.txtTitle}>Complete Submission</Text>
        <View style={styles.optionsChose}>
          <View style={styles.boxEmailSub}>
            <Text style={styles.emailSub}>Email Submission</Text>
            <TextInput
              style={styles.none}
              value={emailSubmit}
              onChangeText={(txt: string) => setEmailSubmit(txt)}
              placeholder={'Enter email'}
              multiline
              numberOfLines={2}
            />
          </View>
          <TouchableOpacity onPress={() => openReview()} style={styles.reviewSub}>
            <Text style={styles.txtReviewSub}>Review Submission</Text>
          </TouchableOpacity>
        </View>

        <View style={styles.content}>
          <View style={styles.boxContent}>
            <View style={styles.boxSwitch}>
              <Text style={styles.boxTxt}>Email me a copy</Text>
              <Switch
                trackColor={{ false: "#aaa9ab", true: Colors.button }}
                thumbColor={isEnabled ? "#fff" : "#fff"}
                ios_backgroundColor="#3e3e3e"
                onValueChange={toggleSwitch}
                value={isEnabled}
              />
            </View>
          </View>
          <View style={styles.boxContent}>
            <View style={styles.boxSwitch}>
              <Text style={styles.boxTxt}>Sync after tapping Submit</Text>
              <Switch
                trackColor={{ false: "#767577", true: Colors.button }}
                thumbColor={isEnabled1 ? "#fff" : "#fff"}
                ios_backgroundColor="#3e3e3e"
                onValueChange={toggleSwitch1}
                value={isEnabled1}
              />
            </View>
          </View>
          <View style={styles.boxContent}>
            <View style={styles.boxSwitch}>
              <Text style={styles.boxTxt}>Show PDF after sync</Text>
              <Switch
                trackColor={{ false: "#767577", true: Colors.button }}
                thumbColor={isEnabled2 ? "#fff" : "#fff"}
                ios_backgroundColor="#3e3e3e"
                onValueChange={toggleSwitch2}
                value={isEnabled2}
              />
            </View>
          </View>
        </View>
      </KeyboardAwareScrollView>
      <BottomButtonNext labels={"Submit"} onPressButton={() => conditionHandleSubmit()} />
      <ModalBox
        transparent={true}
        isOpen={modalInfo}
        entry={"bottom"}
        position={"bottom"}
        swipeToClose={true}
        onClosed={() => setModalInfo(false)}
        style={styles.modalStyle}
      >
        <View style={styles.txtModal}>
          <TouchableOpacity onPress={() => setModalInfo(false)} style={styles.btnIconX}>
            <IconCheck name={"x"} size={25} color={Colors.button} />
          </TouchableOpacity>
          <View style={styles.viewReviewSub}>
            <Text style={{ fontSize: 18 }}>Review Submission</Text>
          </View>
          <View style={{ flex: 1 }}></View>
        </View>
        <View style={{ flex: 1 }}>
          <View style={styles.containerChose}>
            <Text style={styles.txtNameScreen}>Start of Day</Text>
            <View style={styles.boxSubmiss}>
              <Text style={styles.txtKey}>Activity</Text>
              <Text style={styles.txtValue}>11/7/2021</Text>
            </View>
            <View style={styles.boxSubmiss}>
              <Text style={styles.txtKey}>Time of Activity</Text>
              <Text style={styles.txtValue}>07:07</Text>
            </View>
            <View style={styles.boxSubmiss}>
              <Text style={styles.txtKey}>Client</Text>
              <Text style={styles.txtValue}>Toyota</Text>
            </View>
            <View style={styles.boxSubmiss}>
              <Text style={styles.txtKey}>Building Name</Text>
              <Text style={styles.txtValue}>CGGG</Text>
            </View>
            <View style={styles.boxSubmiss}>
              <Text style={styles.txtKey}>Floor</Text>
              <Text style={styles.txtValue}>Captured</Text>
            </View>
            <Text style={styles.inventoryCollection}>Inventory Collection</Text>
            <View style={styles.viewBarCode}>
              <Text style={styles.txtKey}>Barcode</Text>
              <Text style={styles.barCode}>Barcode</Text>
            </View>
          </View>
        </View>
      </ModalBox>
    </View>
  )
}
export default SubmissionScreen
