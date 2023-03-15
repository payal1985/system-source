import { useFocusEffect, useNavigation, useRoute } from "@react-navigation/native"
import { useCallback, useState } from "react"
import { Alert } from "react-native"
import Routes from "/navigators/Routes"
import useGlobalData from "/utils/hook/useGlobalData"
import { get } from "lodash"
import { getDetailInventoryClient, updateDataInventoryClientByQuery } from "/utils/sqlite/tableInventoryClient"
import _ from "lodash"

const InventoryDetailListScreenIndex = () => {
  const { navigate } = useNavigation()
  const [globalData, setglobalData] = useGlobalData()
  const [initialElements, setinitialElements] = useState(globalData.item[0].dataItemType || [])
  const route = useRoute()
  const idClient = get(route, "params.idClient")

  useFocusEffect(
    useCallback(() => {
      getMyStringValue()
    }, []),
  )

  const getMyStringValue = () => {
    idClient &&
      getDetailInventoryClient(idClient, (data) => {
        setinitialElements([...initialElements])
      })
  }
  const handleOnPress = () => {
    if (idClient) {
      navigate(Routes.SUBMISSION_SCREEN, { idClient })
    } else {
      console.log("idclient not found")
    }
  }

  const handlePressItem = () => {
    if (globalData.item[0].isBarScan) {
      navigate(Routes.CAMERA_BARCODE, { idClient })
    } else {
      navigate({
        key: "InventoryDetailListScreen",
        name: Routes.INVENTORY_DETAIL_SCREEN,
        params: { idClient, conditionRender: null, isEdit: false, parentRowID: null },
      })
    }
  }
  const handleCheckItem = (item?: any, index?: any) => {
    let ar = [item]
    let raw = { chosed: ar }
    let rawGlobalData = { ...globalData, ...raw, index }
    setglobalData(rawGlobalData)
    navigate(Routes.INVENTORY_DETAIL_SCREEN, {
      idClient,
      conditionRender: item.conditionRender,
      isEdit: true,
      parentRowID: item.parentRowID,
      indexDataItemType: index,
    })
  }

  const deleteRow = (index?: any, item?: any) => {
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
  const handleDeleteRow = (index: any) => {
    initialElements.splice(index, 1)
    setinitialElements([...initialElements])
    let arr = globalData.item[0].dataItemType
    arr.splice(index, 1)
    handleUpdateItemTypeSqlite([...arr])
  }

  const handleUpdateItemTypeSqlite = (dataItemType: any) => {
    idClient &&
      updateDataInventoryClientByQuery("dataItemType = ? where id = ?", [JSON.stringify(dataItemType), idClient])
  }

  const conditionDisable = () => {
    if (initialElements.length > 0) {
      return false
    } else {
      return true
    }
  }
  return {
    globalData,
    initialElements,
    handleCheckItem,
    deleteRow,
    handlePressItem,
    conditionDisable,
    handleOnPress,
  }
}
export default InventoryDetailListScreenIndex
