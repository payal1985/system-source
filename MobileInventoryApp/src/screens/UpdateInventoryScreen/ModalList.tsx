import React, { useCallback, useEffect, useState } from "react"
import { StyleSheet, View, FlatList, Alert, Image, TouchableOpacity, Text, ActivityIndicator, ScrollView } from "react-native"
import { ModalFullScreen } from "../../components/ModalFullScreen"
import { getInventoryItemInfo, searchSimpleInventoryItem } from "../../redux/progress/service"
import { Colors } from "/configs"
import { SERVER_NAME } from "/constant/Constant"
import IconFeather from "react-native-vector-icons/Feather"
import { width } from "/utils"
import { BaseBottomSheetShowImage } from "../../components"
import Routes from "/navigators/Routes"
import { isArray, isEmpty } from "lodash"
import { useFocusEffect } from "@react-navigation/native"
import moment from "moment"

const ModalList = ({ clientId, inventoryID, isOpenModal, setIsOpenModal, navigate }: any) => {
  const [data, setData] = useState<any[]>([])
  const [page, setPage] = useState<number>(1)
  const [isLoadMore, setIsLoadMore] = useState(false)
  const [isRefresh, setIsRefresh] = useState(false)
  const [infoImageModal, setInfoImageModal] = useState<any>(null)
  const [infoImageModalChild, setInfoImageModalChild] = useState<any>(null)
  const [infoDetail, setInfoDetail] = useState<any>(null)
  let onEndReachedCalledDuringMomentum = true
  const getSimpleInventory = async (page: number, isLoadMore: boolean) => {
    const body = {
      currentPage: page,
      itemsPerPage: 10,
      clientId,
      inventoryID,
    }
    const rs = await searchSimpleInventoryItem(body)
    if (rs?.status === 200) {
      setData(isLoadMore ? (preState) => [...preState, ...rs?.data] : rs?.data)
    } else {
      Alert.alert(`Error Api (${rs?.status || 0})`)
    }
    setIsLoadMore(false)
    setIsRefresh(false)
  }

  useEffect(() => {
    if (inventoryID) {
      getSimpleInventory(page, page !== 1)
    }
  }, [page, inventoryID])

  useFocusEffect(useCallback(() => {
    if (inventoryID) {
      getSimpleInventory(1, false)
    }
  }, [inventoryID]))
  const handleLoadMore = () => {
    setPage((preState: number) => preState + 1)
    setIsLoadMore(true)
  }

  const onRefresh = () => {
    page === 1 ? getSimpleInventory(1, false) : setPage(1)
    setIsRefresh(true)
  }

  const onPressItem = (inventoryItemId: any) => {
    setIsOpenModal(false)
    navigate(Routes.UPDATE_DETAIL_INVENTORY_ITEM_SCREEN, { clientId, inventoryItemId, inventoryID })
  }


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


  const getInventoryItemInfoFunc = async (id: any) => {
    await getInventoryItemInfo(id, (rs: any) => {
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
    }, (err: any) => {
      Alert.alert(`Inventory Item ID invalid (${err?.status})`)
    }, clientId)
  }

  const renderItem = ({ item, index }: any) => {
    const uri = `${SERVER_NAME}/${clientId}/${item?.mainImage}`
    return (
      <View style={styles.containerItemRow} key={`${index}`}>
        <TouchableOpacity onPress={() => setInfoImageModal({ uri: uri, name: item?.mainImage })}>
          <Image source={{ uri: uri }} style={styles.img} />
        </TouchableOpacity>
        <TouchableOpacity style={{ flex: 1 }} onPress={() => onPressItem(item?.inventoryItemID)}>
          <Text style={[styles.txtListSearch, { color: Colors.blue }]}>
            {/*{`${item?.inventoryItemID} -- ${item?.condition} -- ${item?.itemTypeName}`}*/}
            {`${item?.inventoryItemID} -- ${(item?.condition ? "--" + item?.condition : "")} -- ${item?.itemTypeName}`}
          </Text>
          <Text style={styles.txtListSearch}>{`${item?.building} - Floor: ${item?.floor} - Room: ${item?.room}`}</Text>
          <TouchableOpacity onPress={() => getInventoryItemInfoFunc(item?.inventoryItemID)}>
            <Text numberOfLines={3} style={styles.txtListSearch}>{`Notes: ${item?.notes || ""}`}</Text>
          </TouchableOpacity>
        </TouchableOpacity>
      </View>
    )
  }
  const keysInfo = infoDetail ? Object.keys(infoDetail) : null
  const valuesInfo: any = infoDetail ? Object.values(infoDetail) : null
  const indexMainPhoto = keysInfo?.findIndex((it: any) => it === "mainPhoto" || it === "mainImage")
  return (
    <ModalFullScreen
      backdrop={true}
      isOpen={isOpenModal}
      onClosed={() => {
        setIsOpenModal(false)
      }}
      useNativeDriver={true}
      backdropPressToClose={true}
      entry={"bottom"}
      position={"bottom"}
      style={styles.modalFullScreenWhite}
      swipeToClose={false}
      element={
        <>
          <View style={styles.containerContent}>
            <TouchableOpacity style={styles.closeIcon} onPress={() => setIsOpenModal(false)}>
              <IconFeather name={"x"} size={20} />
            </TouchableOpacity>
            <FlatList
              data={data}
              renderItem={renderItem}
              onEndReachedThreshold={0.1}
              onEndReached={() => {
                if (!onEndReachedCalledDuringMomentum) {
                  handleLoadMore()
                  onEndReachedCalledDuringMomentum = true
                }
              }}
              onMomentumScrollBegin={() => {
                onEndReachedCalledDuringMomentum = false
              }}
              ItemSeparatorComponent={() => {
                return <View style={styles.containerSeparator} />
              }}
              ListFooterComponent={() =>
                isLoadMore ? <ActivityIndicator size={32} color={Colors.black} style={{ marginTop: 16 }} /> : null
              }
              showsVerticalScrollIndicator={false}
              refreshing={isRefresh}
              onRefresh={onRefresh}
            />
          </View>
          {!!infoImageModal && (
            <BaseBottomSheetShowImage
              open={!!infoImageModal}
              options={infoImageModal}
              onClosed={() => setInfoImageModal(null)}
              flex={0.9}
              value={true}
            />
          )}
          <ModalFullScreen
            backdrop={true}
            isOpen={!!infoDetail}
            onClosed={() => {
              setInfoDetail(null)
            }}
            useNativeDriver={true}
            backdropPressToClose={true}
            entry={"bottom"}
            position={"bottom"}
            style={styles.modalFullScreenWhite}
            swipeToClose={false}
            element={
              <>
                <ScrollView style={styles.containerModal} showsVerticalScrollIndicator={false}>
                  <View style={styles.containerRow}>
                    <View />
                    <TouchableOpacity onPress={() => setInfoDetail(null)}>
                      <IconFeather name={"x"} size={20} />
                    </TouchableOpacity>
                  </View>
                  {indexMainPhoto && indexMainPhoto !== -1 && (
                    <TouchableOpacity
                      style={{ width: 50, height: 50, marginBottom: 16 }}
                      onPress={() => {
                        setInfoImageModalChild({
                          name: valuesInfo?.[indexMainPhoto]?.toString(),
                          uri: `${SERVER_NAME}/${clientId}/${valuesInfo?.[indexMainPhoto]?.toString()}`,
                        })
                      }}
                    >

                      <Image
                        source={{ uri: `${SERVER_NAME}/${clientId}/${valuesInfo?.[indexMainPhoto]?.toString()}` }}
                        style={styles.img}
                      />
                    </TouchableOpacity>
                  )}
                  <View style={{ paddingBottom: 32 }}>
                    {keysInfo?.map((it: string, index: number) => {
                      if (valuesInfo?.[index]?.toString() === 'hide_txt_value') return null
                      if (it === "itemTypeOptions" || it == "mainImage" || it == "mainPhoto") return null
                      return (
                        <View style={styles.rowDetail}>
                          <Text style={styles.singleItem}>{`${it?.toString()}: `}</Text>
                          <Text>{valuesInfo?.[index]?.toString() || ""}</Text>
                        </View>
                      )
                    })}
                  </View>
                </ScrollView>
                {!!infoImageModalChild && (
                  <BaseBottomSheetShowImage
                    open={!!infoImageModalChild}
                    options={infoImageModalChild}
                    onClosed={() => setInfoImageModalChild(null)}
                    flex={0.9}
                    value={true}
                  />
                )}
              </>
            }
          />
        </>
      }
    />
  )
}
const styles = StyleSheet.create({
  modalFullScreenWhite: {
    backgroundColor: "#fff",
    borderTopLeftRadius: 30,
    borderTopRightRadius: 30,
    flex: 0.9,
  },
  containerContent: {
    padding: 16,
    paddingBottom: 32,
  },
  containerItemRow: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "flex-start",
  },
  img: {
    width: 64,
    height: 64,
    marginRight: 12,
    borderRadius: 4,
    backgroundColor: "pink",
  },
  container: {
    flex: 1,
  },
  title: {
    fontSize: 16,
    flex: 4,
  },
  txtListSearch: {
    fontSize: 16,
  },
  closeIcon: {
    alignSelf: "flex-end",
  },
  containerSeparator: {
    height: 2,
    width: width,
    backgroundColor: Colors.borderColor,
    marginVertical: 12,
  },
  containerModal: {
    padding: 16,
  },
  containerRow: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
  },
  rowDetail: {
    flexDirection: "row",
    alignItems: "center",
    marginTop: 16,
  },
  singleItem: {
    marginRight: 16,
    fontWeight: "bold",
    textTransform: "uppercase",
  },
})
export default ModalList
