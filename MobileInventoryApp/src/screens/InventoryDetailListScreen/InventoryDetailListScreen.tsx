import { useFocusEffect, useNavigation, useRoute } from "@react-navigation/native"
import React, { useCallback, useState } from "react"
import { Alert, FlatList, Text, TouchableOpacity, View } from "react-native"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import IconCheck from "react-native-vector-icons/Feather"
import Icon from "react-native-vector-icons/Ionicons"
import MaterialCommunityIcons from "react-native-vector-icons/MaterialCommunityIcons"
import Routes from "/navigators/Routes"
import { BottomButtonNext, Header } from "../../components"
import useGlobalData from "/utils/hook/useGlobalData"
import { get } from "lodash"
import { getDetailInventoryClient, updateDataInventoryClientByQuery } from "/utils/sqlite/tableInventoryClient"
import styles from "./styles"
import { CONDITION_RENDER_TYPE } from "/constant/Constant"

type RenderItemProps = {
  item?: any
  index?: any
}

const InventoryDetailListScreen = () => {
  const { navigate, goBack } = useNavigation()
  const [globalData, setglobalData] = useGlobalData()
  const [initialElements, setinitialElements] = useState(globalData.item[0].dataItemType || [])
  const route = useRoute()
  const idClient = get(route, "params.idClient")
  const isNotGoBack = get(route, "params.isNotGoBack")
  useFocusEffect(
    useCallback(() => {
      getMyStringValue()
    }, [globalData.item[0].dataItemType]),
  )

  const getMyStringValue = () => {
    //saveSQLite
    idClient &&
      getDetailInventoryClient(idClient, (data) => {
        setinitialElements([...globalData.item[0].dataItemType])
      })
  }
  const handleOnPress = () => {
    if (idClient) {
      navigate(Routes.SUBMISSION_SCREEN, { idClient })
    } else {
      console.log("idclient not found")
    }
  }
  const handleOpenScan = () => {
    navigate(Routes.CAMERA_BARCODE, { idClient })
    // navigate({
    //   key: "QRCODE_DETAIL_SCREEN",
    //   name: Routes.SCANCODE_DETAIL_SCREEN,
    //   params: { idClient, conditionRender: null, isEdit: false, parentRowID: null },
    //
    // })
  }
  const handlePressItem = () => {
    navigate({
      key: "InventoryDetailListScreen",
      name: Routes.INVENTORY_DETAIL_SCREEN,
      params: { idClient, conditionRender: null, isEdit: false, parentRowID: null },
    })
  }
  const handleCheckItem = (item?: any, index?: any) => {
    let ar = [item]
    let raw = { chosed: ar }
    let rawGlobalData = { ...globalData, ...raw, index }
    setglobalData(rawGlobalData)
    if (item.conditionRender == CONDITION_RENDER_TYPE.SCANCODE) {
      navigate(Routes.SCANCODE_DETAIL_SCREEN, {
        idClient,
        isEdit: true,
        parentRowID: item.parentRowID,
        indexDataItemType: index,
      })
    } else {
      navigate(Routes.INVENTORY_DETAIL_SCREEN, {
        idClient,
        conditionRender: item.conditionRender,
        isEdit: true,
        parentRowID: item.parentRowID,
        indexDataItemType: index,
      })
    }
  }

  const deleteRow = (index?: any, item?: any) => {
    console.log("item", item)
    Alert.alert("", "Do you want to delete it?", [
      {
        text: "No",
        onPress: () => console.log("Cancel Pressed"),
        style: "cancel",
      },
      {
        text: "Yes",
        onPress: () => handleDeleteRow(index, item),
      },
    ])
  }
  const handleDeleteRow = (index: any, item: any) => {
    initialElements.splice(index, 1)
    initialElements?.forEach((itInitEl: any, indexEl: number) => {
      if (itInitEl?.parentRowID === item?.inventoryRowID) {
        initialElements.splice(indexEl, 1)
      }
    })
    setinitialElements([...initialElements])
    let arr = globalData.item[0].dataItemType
    arr.splice(index, 1)
    arr?.forEach((itInitEl: any, indexEl: number) => {
      if (itInitEl?.parentRowID === item?.inventoryRowID) {
        arr?.splice(indexEl, 1)
      }
    })
    handleUpdateItemTypeSqlite([...arr])
  }

  const handleUpdateItemTypeSqlite = (dataItemType: any) => {
    idClient &&
      updateDataInventoryClientByQuery("dataItemType = ? where id = ?", [JSON.stringify(dataItemType), idClient])
  }

  const renderItem = ({ item, index }: RenderItemProps) => {
    const isHaveParent = item?.parentRowID !== null
    const renderColor = item?.conditionRender === 3 ? "yellow" : "green"
    return (
      <View
        key={index}
        style={{
          backgroundColor: "#fff",
          paddingVertical: 10,
          flexDirection: "row",
          alignItems: "center",
          marginBottom: 2,
        }}
      >
        <TouchableOpacity onPress={() => handleCheckItem(item, index)} style={{ flex: 6, marginLeft: 20 }}>
          <Text style={styles.title}>{`${index + 1} - ${item.itemName || item?.itemTypeName}`}</Text>
        </TouchableOpacity>
        {isHaveParent && <View style={{ width: 16, height: 16, borderRadius: 8, backgroundColor: renderColor }} />}
        {item?.conditionRender === 5 && (
          <View style={{ width: 16, height: 16, borderRadius: 8, backgroundColor: "purple" }} />
        )}
        <TouchableOpacity
          onPress={() => deleteRow(index, item)}
          style={{ flex: 2, alignItems: "flex-end", marginRight: 20}}
        >
          <IconCheck name={"trash-2"} size={20} />
        </TouchableOpacity>
      </View>
    )
  }
  const conditionDisable = () => {
    if (initialElements.length > 0) {
      return false
    } else {
      return true
    }
  }

  const goBackFunc = () => {
    isNotGoBack ? navigate(Routes.HOME_SCREEN) : goBack()
  }
  const renderButtonScan = () => {
    // if (globalData.item[0].isBarScan) {
    return (
      <View style={styles.button}>
        <View style={{ flex: 2 }} />
        <TouchableOpacity onPress={() => handleOpenScan()} style={styles.btnContainer}>
          <MaterialCommunityIcons name={"qrcode-scan"} size={25} color={"#fff"} />
          <Text style={styles.txtButton}>Scan BarCode</Text>
        </TouchableOpacity>
        <View style={{ flex: 2 }} />
      </View>
    )
    // }
  }
  return (
    <View style={styles.container}>
      <Header isGoBack actionGoBack={goBackFunc} labels={"Inventory Collection - ..."} />
      {console.log("initialElements", initialElements)}
      {console.log("idClient list screen", idClient)}
      <KeyboardAwareScrollView
        bounces={false}
        showsVerticalScrollIndicator={false}
        extraScrollHeight={150}
        enableOnAndroid={true}
        style={styles.subContainer}
      >
        <View style={styles.subTitle}>
          <Text style={styles.txtTitle}>Inventory List</Text>
        </View>
        <FlatList data={initialElements} renderItem={renderItem} keyExtractor={(item, index) => `${item}${index}`} />
        <View style={styles.button}>
          <View style={{ flex: 2 }} />
          <TouchableOpacity onPress={() => handlePressItem()} style={styles.btnContainer}>
            <Icon name={"md-add-circle"} size={25} color={"#fff"} />
            <Text style={styles.txtButton}>Add New Item</Text>
          </TouchableOpacity>
          <View style={{ flex: 2 }} />
        </View>
        {renderButtonScan()}
      </KeyboardAwareScrollView>
      <BottomButtonNext disabled={conditionDisable()} onPressButton={() => handleOnPress()} />
    </View>
  )
}

export default InventoryDetailListScreen
