import React, { useEffect, useRef, useState } from "react"
import { StyleSheet, View, Text, TouchableOpacity, TextInput, Alert, Image, FlatList } from "react-native"
import { BaseAlert, BaseBottomSheet, BaseBottomSheetCameraUpdateInventory, BottomButtonNext, Header } from "/components"
import LoadingOverlay from "/components/LoadingView/LoadingOverlay"
import { Colors } from "/configs"
import DataLocationsService from "../InventoryReLocateScreen/DataLocationsService"
import _, { isEmpty } from 'lodash'
import { fontSizer, responsiveH, responsiveW } from "/utils"
import Icon from "react-native-vector-icons/Ionicons"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import {
  getInventoryItemInfo, getStatusGroup, inventoryImageDelete, updateInventoryItemDetail, getCondition,
} from "/redux/progress/service"
import { URL_BASE, SERVER_NAME } from "/constant/Constant"
import { navigationRef } from "/navigators/_root_navigator"
import Storage from "/helper/Storage"
import MaterialCommunityIcons from "react-native-vector-icons/MaterialCommunityIcons";
import MaterialIcons from "react-native-vector-icons/MaterialIcons";
import moment from "moment"




export const UpdateInventoryItemScreen = ({ route }: any) => {
  const clientId = _.get(route, 'params.clientId')
  const inventoryItemId = _.get(route, 'params.inventoryItemId')
  const inventoryID = _.get(route, 'params.inventoryID')
  const [loading, setLoading] = useState(true)

  const { buildingData, floorData, handleGetBuildingDataService } = DataLocationsService(clientId)
  const [buildingName, setBuildingName] = useState("")
  const [floor, setFloor] = useState<any>("")
  const [areaNumber, setAreaNumber] = useState("")
  const [status, setStatus] = useState<any>("")
  const [statusId, setStatusId] = useState("")
  const [note, setNote] = useState("")
  const [condition, setCondition] = useState("")
  const [openSharedModal, setOpenSharedModal] = useState(false)
  const [dataSharedModal, setDataSharedModal] = useState<any>(null)
  const [canAddData, setCanAddData] = useState(false)
  const [titleSharedModal, setTitleSharedModal] = useState<any>(null)
  const [typeSharedModal, setTypeSharedModal] = useState<any>(null)
  const [dataConditionDropDown, setDataConditionDropDown] = useState<any>(null)
  const [itemTypeArr, setItemTypeArr] = useState<any>()
  const [modalTypeCameraUpdate, setmodalTypeCameraUpdate] = useState(false)
  const [arrCheckQuatity, setarrCheckQuatity] = useState<{ type?: any }[]>([])
  const [idxAddItemPhoto, setidxAddItemPhoto] = useState<any>(null)
  const [inventoryItemID, setinventoryItemID] = useState<any>(null)
  const [conditionID, setConditionID] = useState<any>(null)
  const [dataCondition, setDataCondition] = useState<any>([])
  const refDataBuilding = useRef<any>(null)
  const refDataFloor = useRef<any>(null)
  const getInventoryItem = () => {
    getInventoryItemInfo(inventoryItemId, (rs) => {
      const rsData = rs.data[0]
      setData(rsData)
      setStatusId(rsData?.statusId)
      setNote(rsData?.noteForItem)
      setCondition(rsData?.condition || 'ALL')
      setItemTypeArr(rsData)
      setLoading(false)
    }, (e) => {
      Alert.alert(`Err Api (${e?.status})`),
        setLoading(false)
    }, clientId)
  }


  useEffect(() => {
    if (!inventoryItemId) return
    getInventoryItem()
  }, [inventoryItemId])

  const getStatus = async () => {
    let url = `${URL_BASE}/api/Status/GetStatusGroup?statusType=General`
    getStatusGroup(
      url,
      (res: any) => {
        if (res.status == 200) {
          setStatus(res.data)
        } else {
          BaseAlert(res, "Get handleGetStatus")
        }
      },
      (e: any) => {
        BaseAlert(e, "Get handleGetStatus")
      },
    )
  }
  const handleGetCondition = async () => {
    const rs = await getCondition()
    if (rs?.status === 200) {
      const mapData = rs?.data?.map((it: any) => {
        it.name = it?.conditionCode
        it.itemTypeId = it?.inventoryItemConditionId
        it.itemTypeName = it?.conditionCode
        it.clientId = clientId
        return it
      })
      setDataConditionDropDown(mapData)
      setDataCondition(rs?.data)
    } else {
      Alert.alert(`Error api get condition (${rs?.status || 0})`)
    }

  }
  useEffect(() => {
    getStatus()
    handleGetCondition()
  }, [])



  const setData = (data: any) => {
    let arr = data.itemTypeOptions
    for (let i = 0; i < arr.length; i++) {
      if (arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "Building") {
        const itemReturnValue = arr[i].itemTypeOptionReturnValue[0]
        setBuildingName(itemReturnValue.returnValue)
      }
      if (arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "Floor") {
        const itemReturnValue = arr[i]?.itemTypeOptionReturnValue[0]
        setFloor(itemReturnValue?.returnValue)
      }
      if (arr[i].itemTypeOptionReturnValue && arr[i].itemTypeOptionCode == "AreaOrRoom") {
        setAreaNumber(arr[i].itemTypeOptionReturnValue[0].returnValue)
      }
      if (arr[i].valType == 40 && arr[i]?.itemTypeOptionReturnValue?.length > 0) {
        setarrCheckQuatity(arr[i]?.itemTypeOptionReturnValue)
      }
    }
  }
  const handleOpenModal = (type: any) => {
    setTypeSharedModal(type)
    setTitleSharedModal(type)
    if (type == "Building") {
      setDataSharedModal(buildingData)
      setCanAddData(true)
      setOpenSharedModal(true)
    }
    else if (type == "Floors") {
      const floorsFilter = _.cloneDeep(floorData)?.filter((it: any) => it?.inventoryFloorId !== 0)
      setDataSharedModal(floorsFilter)
      setCanAddData(false)
      setOpenSharedModal(true)
    }
    else if (type == "Status") {
      setDataSharedModal(status)
      setCanAddData(false)
      setOpenSharedModal(true)
    }
    else if (type == "Condition") {
      setDataSharedModal([{ clientId: clientId, itemTypeName: "ALL", name: "ALL", itemTypeId: 0 }, ...dataConditionDropDown])
      setCanAddData(false)
      setOpenSharedModal(true)
    }
  }

  const onSelectSharedModal = (item: any) => {
    if (typeSharedModal == "Building") {
      setBuildingName(item.inventoryBuildingName)
      handleGetBuildingDataService()
      refDataBuilding.current = item
    } else if (typeSharedModal == "Floors") {
      setFloor(item.inventoryFloorName)
      refDataFloor.current = item
    }
    else if (typeSharedModal == "Status") {
      setStatusId(item?.statusId)
    } else if (typeSharedModal == "Condition") {
      setCondition(item?.itemTypeName)
    }
    setOpenSharedModal(false)
  }

  const getStatusName = () => {
    if (isEmpty(status)) {
      return ''
    }
    const indexStatus = status?.findIndex((it: any) => it?.statusId === statusId)
    return indexStatus !== -1 ? status[indexStatus]?.statusName : ''
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

  const renderChoseTitle = () => {
    return (
      <>
        <Text style={styles.txtTitleChose}>
          Warranty Remaining: {renderExpiredDate(itemTypeArr)}
        </Text>
        <View style={styles.choseTitleContainer}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              Building Name
            </Text>
            <TouchableOpacity style={[styles.btnDefault, { backgroundColor: '#FFFF' }]} onPress={() => handleOpenModal("Building")}>
              <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
                {buildingName}
              </Text>
              <Icon name={"list"} size={20} />
            </TouchableOpacity>
          </View>
        </View>
        <View style={styles.choseTitleContainer}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              Floor
            </Text>
            <TouchableOpacity style={[styles.btnDefault, { backgroundColor: '#FFFF' }]} onPress={() => handleOpenModal("Floors")}>
              <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
                {floor}
              </Text>
              <Icon name={"list"} size={20} />
            </TouchableOpacity>
          </View>
        </View>
        <View style={styles.choseTitleContainer}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              Area or Room number
            </Text>
            <View style={[styles.btnDefault, { backgroundColor: '#FFFF' }]}>
              <TextInput
                style={styles.txtSize16}
                maxFontSizeMultiplier={1}
                onChangeText={(text) => setAreaNumber(text)}
              >
                {areaNumber}
              </TextInput>
            </View>
          </View>
        </View>

        <View style={styles.choseTitleContainer}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              Status
            </Text>
            <TouchableOpacity style={[styles.btnDefault, { backgroundColor: '#FFFF' }]} onPress={() => handleOpenModal("Status")}>
              <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
                {getStatusName()}
              </Text>
              <Icon name={"list"} size={20} />
            </TouchableOpacity>
          </View>
        </View>
        <View style={styles.choseTitleContainer}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              Condition
            </Text>
            <TouchableOpacity style={[styles.btnDefault, { backgroundColor: '#FFFF' }]} onPress={() => handleOpenModal("Condition")}>
              <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
                {condition}
              </Text>
              <Icon name={"list"} size={20} />
            </TouchableOpacity>
          </View>
        </View>

        <View style={[styles.choseTitleContainer, { marginBottom: responsiveW(16) }]}>
          <View style={styles.subChoseTitleContainer}>
            <Text style={styles.txtTitleChose}>
              Note For Item
            </Text>
            <View style={[styles.btnDefault, { backgroundColor: '#FFFF' }]}>
              <TextInput
                style={styles.txtSize16}
                maxFontSizeMultiplier={1}
                onChangeText={(text) => setNote(text)}
              >
                {note}
              </TextInput>
            </View>
          </View>
        </View>
      </>
    )
  }
  const saveDataItemType = () => {
    let arrItemType = itemTypeArr.itemTypeOptions
    if (statusId) {
      itemTypeArr.statusId = statusId
    }
    if (note) {
      itemTypeArr.noteForItem = note
    }
    if (condition) {
      const getIdCondition = dataCondition?.filter((it: any) => it?.conditionCode === condition)
      itemTypeArr.condition = condition
      itemTypeArr.conditionId = getIdCondition?.[0]?.inventoryItemConditionId || 0
    }
    if (clientId) {
      itemTypeArr.clientId = clientId
    }
    for (let i = 0; i < arrItemType.length; i++) {
      if (arrItemType[i].valType == 90 && arrItemType[i].itemTypeOptionCode == "Building" && buildingName) {
        arrItemType[i].itemTypeOptionReturnValue = [
          {
            returnValue: buildingName,
            returnID: refDataBuilding.current?.inventoryBuildingId,
            returnCode: refDataBuilding.current?.inventoryBuildingCode,
            returnName: refDataBuilding.current?.inventoryBuildingName,
            returnDesc: refDataBuilding.current?.inventoryBuildingDesc,
          },
        ]
      }
      if (arrItemType[i].valType == 3 && arrItemType[i].itemTypeOptionCode == "Floor" && floor) {
        arrItemType[i].itemTypeOptionReturnValue = [
          {
            returnValue: floor,
            returnID: refDataFloor.current?.inventoryFloorId,
            returnCode: refDataFloor.current?.inventoryFloorCode,
            returnName: refDataFloor.current?.inventoryFloorName,
            returnDesc: refDataFloor.current?.inventoryFloorDesc,
          },
        ]
      }
      if (arrItemType[i].valType == 1 && arrItemType[i].itemTypeOptionCode == "AreaOrRoom" && areaNumber) {
        arrItemType[i].itemTypeOptionReturnValue = [{ returnValue: areaNumber }]
      }
    }
  }
  const handleOnPress = async () => {
    saveDataItemType()
    const user = await Storage.get("@User")
    const objUser = JSON.parse(`${user} `)
    let userId = objUser?.userId
    const rs = await updateInventoryItemDetail({ userId: userId, inventoryItem: itemTypeArr })
    if (rs?.status === 200) {
      navigationRef.current?.goBack()
      Alert.alert("Update inventory item successful")
    } else {
      Alert.alert(`Error Api Update (${rs?.status || 0})`)
    }
  }
  const checkImage = async (arrItem: any) => {
    for (let i = 0; i < arrCheckQuatity.length; i++) {
      if (i == idxAddItemPhoto) {
        let arrayPhotos = arrCheckQuatity[i].conditionData[0].url
        arrItem?.forEach((it: any) => {
          arrayPhotos.push(it)
        })

      }
    }
    setarrCheckQuatity([...arrCheckQuatity])
    setmodalTypeCameraUpdate(false)

  }
  const openUpdateImage = (it: any, idx: any) => {
    setinventoryItemID(it?.conditionData[0].inventoryItemId)
    setmodalTypeCameraUpdate(true)
    setidxAddItemPhoto(idx)
    setConditionID(it?.conditionID)
  }
  const handleDelete = (item?: any, indexChild?: any, indexParent?: any,) => {
    if (arrCheckQuatity[indexParent]?.conditionData[0].url?.length <= 1) {
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
  const removeImg = (item: any, indexParent: number, indexChild?: any) => {
    let parentArr: any = arrCheckQuatity[indexParent]
    let childArr: any = parentArr?.conditionData[0].url
    childArr.splice(indexChild, 1)
    setarrCheckQuatity([...arrCheckQuatity])
    handleRemoveImage(item)
  }
  const handleRemoveImage = async (item: any) => {
    const rs = await inventoryImageDelete({ inventoryImageId: item.inventoryItemImgId || item.inventoryImageId })
  }
  return <View style={styles.container}>
    <Header
      isGoBack
      labels={"Update Inventory Item"}
    />
    <LoadingOverlay visible={loading} />
    <KeyboardAwareScrollView
      bounces={false}
      showsVerticalScrollIndicator={false}
      extraScrollHeight={150}
      enableOnAndroid={true}
      style={styles.subContainer}
    >
      {
        arrCheckQuatity?.map((it: any, indexParent: any) => {
          if (it?.conditionID === 2 || it?.conditionID === 3 || it?.conditionID === 4) return
          const filterConditonById = dataCondition?.filter((__it: any) => __it?.inventoryItemConditionId === it?.conditionID)
          const dataArr = it?.conditionData?.[0]?.url
          return <View key={it?.conditionID}>
            <View style={{ marginTop: 10 }}>

              <View style={{ flexDirection: "row" }}>
                <TouchableOpacity style={{ flexDirection: 'row', justifyContent: 'center', alignItems: 'center' }} onPress={() => openUpdateImage(it, indexParent)}>
                  <Text style={{ fontSize: 15, color: Colors.black, }}>
                    {filterConditonById?.[0]?.conditionName}
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
                keyExtractor={(item, index) => `${index} `}
                renderItem={({ item, index }) => {
                  const checkImg = item?.imageUrl ? `${item.imageUrl} /${item?.clientId}/${item?.imageName} ` : `${SERVER_NAME} /${clientId}/${item.name} `
                  return <View style={{ marginTop: 15, }}>
                    <View
                      style={{
                        width: 80,
                        height: 80,
                        marginTop: 0,
                        marginHorizontal: 15,
                        flexDirection: "row"
                      }}
                    >
                      <Image
                        source={{ uri: checkImg }}
                        style={[styles.photo, {
                          flex: 1,
                          borderRadius: 10, backgroundColor: 'pink'
                        }]}
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
                        onPress={() => handleDelete(item, index, indexParent)}
                      >
                        <MaterialCommunityIcons
                          name={"delete-circle"}
                          size={20}
                        />
                      </TouchableOpacity>
                    </View>

                  </View>
                }}
              />
            </View>
          </View>
        })
      }
      {renderChoseTitle()}

    </KeyboardAwareScrollView>
    {openSharedModal && (
      <BaseBottomSheet
        open={openSharedModal}
        options={dataSharedModal}
        flex={0.6}
        addMore={canAddData}
        title={titleSharedModal}
        type={typeSharedModal}
        onSelect={(item) => onSelectSharedModal(item)}
        onClosed={() => setOpenSharedModal(false)}
        onOpened={() => setOpenSharedModal(true)}
      />
    )}
    {modalTypeCameraUpdate && (
      <BaseBottomSheetCameraUpdateInventory
        open={modalTypeCameraUpdate}
        flex={1}
        title={`New photo: ${inventoryItemID} `}
        onSelect={(item) => checkImage(item)}
        type={"InventoryItemId"}
        inventoryItemID={inventoryItemID}
        onClosed={() => setmodalTypeCameraUpdate(false)}
        onOpened={() => setmodalTypeCameraUpdate(true)}
        conditionData={[]}
        arrCheckQuatity={arrCheckQuatity}
        conditionID={conditionID}
        InventoryID={inventoryID}
      />
    )}
    <BottomButtonNext labels="Update" onPressButton={() => handleOnPress()} />
  </View>

}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: Colors.white
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
    backgroundColor: Colors.lightGray,
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
  subContainer: {
    padding: responsiveW(20),
    paddingTop: responsiveW(5),
  },
})
