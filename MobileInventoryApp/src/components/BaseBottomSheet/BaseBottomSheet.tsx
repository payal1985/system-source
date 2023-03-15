import React, { useEffect, useState } from "react"
import { TouchableOpacity, TextInput, Text, View, ScrollView, Alert, FlatList } from "react-native"
import Icon from "react-native-vector-icons/Feather"

import Colors from "/configs/Colors"
import { BaseAlert } from "../../components"
import { fontSizer, responsiveW } from "../../utils/dimension"
import ModalBox from "react-native-modalbox"
import { saveNewItemTypeService, saveNewManufacturerService, saveNewBuildings } from "/redux/progress/service"
import useGlobalData from "/utils/hook/useGlobalData"
import { cloneDeep } from "lodash"
import { ModalFullScreen } from "../ModalFullScreen"
import styles from "./styles"
import { BaseButton } from "../BaseButton"
import { useDispatch } from "react-redux"
import { manufacturer } from "/redux/manufacturer/actions"

type BaseBottomSheetProps = {
  open?: boolean
  onClosed?: () => void
  flex?: number
  options?: object
  title?: string
  onSelect?: (itemTypeData?: object, itemArr?: any) => void
  canSearch?: any
  type?: string
  addMore?: any
  itemTypeId?: string
  onOpened?: () => void
}

const BaseBottomSheet = ({
  open,
  onClosed,
  flex = 0.5,
  options,
  title,
  onSelect,
  canSearch,
  type,
  addMore,
  itemTypeId,
}: BaseBottomSheetProps) => {
  const [itemArr] = useState<any>(options)
  const [newData, setnewData] = useState<any>(false)
  const [arrItem, setarrItem] = useState<any>([])
  const [_, setarr] = useState<any>([])
  const [newItemType, setnewItemType] = useState<any>("")
  const [itemArrFullNoChange] = useState<any>(options)
  const [globalData] = useGlobalData()
  const cloneDeepItemArr = cloneDeep(itemArr)
  const dispatch = useDispatch()

  useEffect(() => {
    setarrItem(cloneDeepItemArr)
  }, [])

  const onChangeFind = (txt: string) => {
    const filterSearch = itemArrFullNoChange?.filter((it: any) =>
      it?.manufacturerName?.toUpperCase()?.includes(txt?.toUpperCase()),
    )
    setarrItem(filterSearch)
  }
  const handleNewdataa = () => {
    setnewData(true)
  }
  const onAddNew = (txt: string) => {
    setnewItemType(txt)
  }
  const handleSaveNewItemType = (itemTypeOption: any) => {
    if (!newItemType?.trim()) {
      Alert.alert('ItemType not empty')
      return
    }
    let body = {
      clientID: globalData.item[0]?.clientId,
      itemTypeName: newItemType,
    }
    saveNewItemTypeService(
      body,
      (res) => {
        if (res.status == 200) {
          let itemTypeData = res.data
          itemTypeData.itemTypeOptions = itemTypeOption
          let arr = arrItem
          arr.push(itemTypeData)
          setarr(arr)
          onSelect && onSelect(itemTypeData)
        } else {
          BaseAlert(res, "Save ItemType")
        }
      },
      (e) => {
        BaseAlert(e, "Save ItemType")
      },
    )
  }
  const handleSaveNewManufacturer = (body: any) => {
    saveNewManufacturerService(
      body,
      (res) => {
        if (res.status == 200) {
          let arr = arrItem
          arr.push(res.data)
          setarr(arr)
          onSelect && onSelect(res.data, itemArr)
        } else {
          BaseAlert(res, "Save New Manufactory")
        }
      },
      (e) => {
        if (e?.message?.includes(409)) {
          console.log('a')
          Alert.alert("Error", 'Company name is existing')
        } else {
          BaseAlert(e)
        }
      }
    )
  }
  const handleSaveNewBuildings = (body: any) => {
    saveNewBuildings(
      body,
      (res) => {
        if (res.status == 200) {
          let arr = arrItem
          arr.push(res.data)
          setarr(arr)
          onSelect && onSelect(res.data, itemArr)
        } else {
          BaseAlert(res, "Save New Buildings")
        }
      },
      (e) => {
        BaseAlert(e, "Save New Buildings")
      },
    )
  }

  const addItemType = () => {
    if (!newItemType?.trim()) {
      Alert.alert("New manufactory is not empty")
      return
    }
    if (type == "itemType") {
      let index = arrItem.findIndex((p: { itemTypeCode: string }) => p.itemTypeCode == "DefaultItemType")
      if (index != -1) {
        let itemTypeOption = arrItem[index].itemTypeOptions
        handleSaveNewItemType(itemTypeOption)
      }
    } else if (type == "Building") {
      let dataBody = {
        clientID: globalData.item[0]?.clientId,
        inventoryBuildingName: newItemType,
        inventoryBuildingDesc: newItemType,
      }
      handleSaveNewBuildings(dataBody)
    } else {
      let dataBody = {
        ManufacturerName: newItemType,
      }
      handleSaveNewManufacturer(dataBody)
      dispatch(manufacturer.request(null))
    }
    setnewData(false)
    setnewItemType(null)

  }

  const getPlaceHolderType = (type: any) => {
    switch (type) {
      case 'Building':
        return 'New Building'
      case 'itemType':
        return 'New Item Type'
      case 'numberNV':
        return 'New Manufactory Options';
      default:
        return 'New value'
    }
  }

  const renderItem = ({ item, index }: any) => {
    const __item: {
      itemTypeCode: string
      itemTypeOptionLineName: string
      name: string
      itemTypeName: string
      clientName: string
      inventoryFloorName: string
      inventoryBuildingName: string
    } = item
    if (__item?.itemTypeCode == "DefaultItemType") {
      return null
    }
    return (
      <TouchableOpacity
        onPress={() => onSelect && onSelect(__item, itemArr)}
        style={{
          flexDirection: "row",
          borderBottomWidth: 1,
          borderTopWidth: 0,
          borderColor: Colors.borderColor,
          alignItems: "center",
          paddingVertical: 15,
          paddingLeft: 0,
        }}
        key={index.toString()}
      >
        {type == "numberNV" && (
          <View style={{ flexDirection: "column" }}>
            <Text style={{ fontSize: fontSizer(15) }}>{`${__item.itemTypeOptionLineName}`}</Text>
          </View>
        )}
        {type == "Floors" && (
          <View style={{ flexDirection: "column" }}>
            <Text style={{ fontSize: fontSizer(15) }}>{`${__item.inventoryFloorName}`}</Text>
          </View>
        )}
        {type == "Building" && (
          <View style={{ flexDirection: "column" }}>
            <Text style={{ fontSize: fontSizer(15) }}>{`${__item.inventoryBuildingName}`}</Text>
          </View>
        )}
        {type == "areaNumber" && (
          <View style={{ flexDirection: "column" }}>
            <Text style={{ fontSize: fontSizer(15) }}>{`${__item.name}`}</Text>
          </View>
        )}

        {type == "itemType" && (
          <View style={{ flexDirection: "column" }}>
            <Text style={{ fontSize: fontSizer(15) }}>{`${__item.itemTypeName}`}</Text>
          </View>
        )}
        {type == "client" && (
          <View style={{ flexDirection: "column" }}>
            <Text style={{ fontSize: fontSizer(15) }}>{`${__item.clientName}`}</Text>
          </View>
        )}
        {type == "Status" && (
          <View style={{ flexDirection: "column" }}>
            <Text style={{ fontSize: fontSizer(15) }}>{`${__item.statusName}`}</Text>
          </View>
        )}
        {type == "Condition" && (
          <View style={{ flexDirection: "column" }}>
            <Text style={{ fontSize: fontSizer(15) }}>{`${__item.name}`}</Text>
          </View>
        )}
      </TouchableOpacity>
    )
  }
  return (
    <>
      <ModalBox
        isOpen={open}
        entry={"top"}
        position={"bottom"}
        swipeToClose={false}
        onClosed={onClosed}
        style={{
          backgroundColor: "#fff",
          flex: flex,
          borderTopLeftRadius: 30,
          borderTopRightRadius: 30,
        }}
      >
        <View style={{ flex: 1 }}>
          <View
            style={{
              borderBottomWidth: 1,
              borderBottomColor: "#efefef",
              padding: responsiveW(20),
              flexDirection: "row",
            }}
          >
            {addMore && (
              <TouchableOpacity
                onPress={() => handleNewdataa()}
                style={{ flex: 1, justifyContent: "center", alignItems: "center" }}
              >
                <Icon name={"plus"} size={20} />
              </TouchableOpacity>
            )}

            <View style={{ flex: 8, justifyContent: "center", alignItems: "center" }}>
              <Text style={{ fontSize: fontSizer(16) }}>
                <Text style={{ fontSize: fontSizer(16), fontWeight: "500" }}>{title}</Text>
              </Text>
            </View>

            <TouchableOpacity onPress={onClosed} style={{ flex: 1, justifyContent: "center", alignItems: "center" }}>
              <Icon name={"x"} size={20} />
            </TouchableOpacity>
          </View>
          {canSearch && (
            <View
              style={{
                borderBottomWidth: 1,
                borderColor: Colors.gray_C4,
                marginHorizontal: responsiveW(20),
                flexDirection: "row",
              }}
            >
              <View style={{ justifyContent: "center", alignItems: "center" }}></View>
              <TextInput
                placeholder={"Search"}
                onChangeText={(text) => onChangeFind(text)}
                style={{
                  height: 40,
                  marginLeft: 10,
                  flex: 1,
                }}
              />
            </View>
          )}
          <FlatList
            style={{ flex: 8, paddingHorizontal: responsiveW(20) }}
            data={arrItem}
            renderItem={(item) => renderItem(item)}
            keyExtractor={(it: any, index: any) => `${index}`}
          />
        </View>
      </ModalBox>

      <ModalFullScreen
        backdrop={true}
        isOpen={newData}
        containerStyle={{ paddingVertical: 16 }}
        onClosed={() => setnewData(false)}
        useNativeDriver={true}
        backdropPressToClose={true}
        style={styles.modalFullScreen}
        element={
          <View style={styles.containerAddNew}>
            <View
              style={{
                borderBottomWidth: 1,
                borderColor: Colors.gray_C4,
                flexDirection: "row",
              }}
            >
              <TextInput
                placeholder={getPlaceHolderType(type)}//type == "itemType" ? "" : ""
                onChangeText={(text) => onAddNew(text)}
                value={newItemType}
                style={{
                  height: 40,
                  flex: 1,
                }}
              />
            </View>
            <View style={styles.rowBtn}>
              <BaseButton
                backgroundColor={Colors.redHeader}
                label={"Add"}
                fontSize={responsiveW(16)}
                width={responsiveW(128)}
                onPress={addItemType}
                marginRight={responsiveW(32)}
              />
              <BaseButton
                backgroundColor={Colors.redHeader}
                label={"Cancel"}
                fontSize={responsiveW(16)}
                width={responsiveW(128)}
                onPress={() => setnewData(false)}
              />
            </View>
          </View>
        }
      />
    </>
  )
}

export default BaseBottomSheet
