import React, { useCallback, useEffect, useMemo, useState } from "react"
import { View, FlatList, Text, Image, TouchableOpacity, ScrollView, Alert } from "react-native"
import _, { get, forEach, isObject, isEmpty } from "lodash"
import styles from "./styles"
import { getCondition, getSubmitted } from "/redux/progress/service"
import LoadingOverlay from "/components/LoadingView/LoadingOverlay"
import Icon from "react-native-vector-icons/MaterialIcons"
import { URL_BASE } from "../../constant/Constant"
import { BaseBottomSheetShowImage, BaseBottomSheet } from "../../components"
import AntIcon from 'react-native-vector-icons/AntDesign'
import { Colors } from "/configs"
interface SubmittedProps {
  dataItemType: any
}

const Submitted = (props: SubmittedProps) => {
  const { dataItemType } = props
  const [loading, setLoading] = useState(true)
  const [dataRow, setDataRow] = useState<any[]>([])
  const [modalShowImage, setModalShowImage] = useState(false)
  const [infoImageModal, setInfoImageModal] = useState({ name: "", uri: "" })
  const [valueQRCode, setValueQrCode] = useState([])
  const [detailInfo, setDetailInfo] = useState<any>(null)
  const clientId = dataItemType?.clientId
  const deviceDate = dataItemType?.deviceDate
  const clientGroupId = dataItemType?.InventoryClientGroupID
  const [dataCondition, setDataCondition] = useState([])
  const handleGetCondition = async () => {
    const rs = await getCondition()
    if (rs?.status === 200) {
      setDataCondition(rs?.data)
    } else {
      Alert.alert(`Error api get condition (${rs?.status || 0})`)
    }

  }
  const showImageZoom = (value: any) => {
    setModalShowImage(true)
    setInfoImageModal(value)
  }

  const setValueQRCodeFunc = (value: any) => {
    if (isEmpty(value)) {
      Alert.alert('No QR code to show')
    } else {
      setValueQrCode(value)
    }
  }

  const getNameById = useCallback((id: any) => {
    const filterByID: any = dataCondition?.filter((it: any) => it?.inventoryItemConditionId === id)
    return _.isEmpty(filterByID) ? '' : filterByID?.[0]?.conditionName
  }, [dataCondition])
  const renderItemFunc = useCallback(({ item }: any) => {
    const returnValue: { itemTypeOptionLineName: string } = get(item, "itemTypeOptionReturnValue[0].returnValue", null)
    const itemTypeCode = item?.itemTypeOptionCode
    const arrImg = item?.itemTypeOptionReturnValue
    if (itemTypeCode === "Custom-Modular-AV") {
      const valueCustomModular = get(item, 'itemTypeOptionReturnValue', null)
      return <View style={styles.containerQRow}>
        <Text style={styles.singleItem}>{`${itemTypeCode}:`}</Text>
        {valueCustomModular?.map((it: any, index: number) => <Text>{index === valueCustomModular?.length - 1 ? `${it?.returnValue}` : `${it?.returnValue}, `} </Text>)}
      </View>
    }
    return itemTypeCode === "TotalCount" ? (
      <View style={styles.containerItem}>
        <Text style={styles.singleItem}>{`${itemTypeCode}:`}</Text>
        {arrImg?.map((it: any) => {
          let arrayUrl: any[] = []
          let arrBarCode: any[] = []
          forEach(it?.representativePhotos?.[0].url, (it) => {
            arrayUrl = arrayUrl?.concat(`${it?.imageUrl}/${it?.name}`)
          })
          forEach(it?.conditionData, (__it) => {
            arrBarCode = arrBarCode?.concat(`${__it?.inventoryItemBarcodes?.[0]}`)
          })
          const firstItemQrCode = it?.conditionData?.[0]?.inventoryItemBarcodes?.[0]
          console.log('arrBarCode', arrBarCode)
          return (
            <View>
              <View style={styles.rowList}>
                <Text style={{ marginTop: 8 }}>{`* ${getNameById(it?.conditionID)} - ${it?.txtQuantity}`}</Text>
              </View>
              <View style={styles.rowList}>
                {arrayUrl?.map((urlItem: any) => {
                  return (
                    <TouchableOpacity
                      onPress={() => showImageZoom({ name: urlItem?.toString(), uri: urlItem?.toString() })}
                    >
                      <Image
                        key={`${urlItem?.toString()}`}
                        source={{ uri: urlItem?.toString() }}
                        style={styles.imgItem}
                      />
                    </TouchableOpacity>
                  )
                })}
              </View>
              <View style={styles.rowList}>
                {arrBarCode?.map((urlItem: any) => {
                  return (
                    <TouchableOpacity
                      onPress={() => setValueQRCodeFunc(urlItem)}
                    >
                      {/* <Image
                        key={`${urlItem?.toString()}`}
                        source={{ uri: urlItem?.toString() }}
                        style={styles.imgItem}
                      /> */}
                      <AntIcon name="barcode" size={64} style={[styles.imgItem, { backgroundColor: 'transparent' }]} />
                    </TouchableOpacity>
                  )
                })}
              </View>
            </View>
          )
        })}
      </View>
    ) : (
      <View style={styles.containerRow}>
        <Text style={styles.singleItem}>{`${itemTypeCode}:`}</Text>
        <Text>{isObject(returnValue) ? returnValue?.itemTypeOptionLineName : returnValue}</Text>
      </View>
    )
  }, [dataCondition])

  const getIndexItemByKey = (itemTypeOptions: any[], key: string) => {
    const indexFind = itemTypeOptions?.findIndex((it: any) => it?.itemTypeOptionCode === key)
    return indexFind
  }

  const getValueByIndex = (itemTypeOptions: any, indexValue: number) => {
    const valueByIndex: { itemTypeOptionLineName: string } =
      indexValue === -1 || indexValue === undefined
        ? "Empty"
        : get(itemTypeOptions[indexValue], "itemTypeOptionReturnValue[0].returnValue", "Empty")
    const valueFinal = isObject(valueByIndex) ? valueByIndex?.itemTypeOptionLineName : valueByIndex
    return valueFinal
  }

  const getValueTotalCount = (itemTypeOptions: any, indexValue: number) => {
    const valueByIndex: any =
      indexValue === -1 || indexValue === undefined
        ? "Empty"
        : get(itemTypeOptions[indexValue], "itemTypeOptionReturnValue", "Empty")
    const countTotal =
      (!valueByIndex || valueByIndex === "Empty")
        ? "Empty"
        : valueByIndex?.reduce(
          (total: number, currentValue: any) => total + currentValue?.txtQuantity,
          0,
        )
    return countTotal
  }

  const setValueItemType = (filterItemTypeOption: any, itemTypeQrCode: any) => {
    setDetailInfo(filterItemTypeOption)
  }

  const renderDataRow = useCallback(({ item }: any) => {
    const { itemTypeOptions } = item
    const partNumberIndex = getIndexItemByKey(itemTypeOptions, "PartNumber")
    const buildingIndex = getIndexItemByKey(itemTypeOptions, "Building")
    const floorIndex = getIndexItemByKey(itemTypeOptions, "Floor")
    const roomIndex = getIndexItemByKey(itemTypeOptions, "AreaOrRoom")
    const quantityIndex = getIndexItemByKey(itemTypeOptions, "TotalCount")
    const valuePartNumber = getValueByIndex(itemTypeOptions, partNumberIndex)
    const valueBuilding = getValueByIndex(itemTypeOptions, buildingIndex)
    const valueFloor = getValueByIndex(itemTypeOptions, floorIndex)
    const valueRoom = getValueByIndex(itemTypeOptions, roomIndex)
    const valueQuantity = getValueTotalCount(itemTypeOptions, quantityIndex)
    const filterItemTypeOptions = itemTypeOptions?.filter((it: any) => !isEmpty(it?.itemTypeOptionReturnValue))
    return (
      <View style={styles.rowMultiField}>
        <Text
          style={[styles.singleItemRow, {color: Colors.blue}]}
          onPress={() => setValueItemType(filterItemTypeOptions, item?.inventoryQRCode)}
        >
          {valuePartNumber}
        </Text>
        <Text style={styles.singleItemRow}>{`${valueBuilding} - ${valueFloor} - ${valueRoom}`}</Text>
        <Text style={[styles.singleItemRow, { marginRight: 0 }]}>{valueQuantity}</Text>
      </View>
    )
  }, [])

  const getSubmittedFunc = async () => {
    const body = {
      clientId,
      inventoryAppId: deviceDate,
      clientGroupId,
    }
    const { results } = await getSubmitted(body)
    if (results) {
      setDataRow(results)
    }
    setLoading(false)
  }

  useEffect(() => {
    getSubmittedFunc()
    handleGetCondition()
  }, [])
  const arrayValueQrCode: any[] = []
  if (valueQRCode) {
    arrayValueQrCode.push({ clientName: `${URL_BASE}/${valueQRCode}` })
  }
  return (
    <>
      <ScrollView style={styles.scrollViewHidden} showsVerticalScrollIndicator={false}>
        {loading ? (
          <LoadingOverlay visible={true} />
        ) : detailInfo ? (
          <FlatList
            ListHeaderComponent={() => {
              return (
                <View style={styles.rowMultiField}>
                  <TouchableOpacity onPress={() => setValueItemType(null, null)}>
                    <Icon name={"chevron-left"} size={32} color={"#000"} />
                  </TouchableOpacity>
                  <Text style={styles.headerFlatList}>
                    {dataItemType?.dataItemType?.[0]?.itemName || dataItemType?.dataItemType?.[0]?.itemTypeName}
                  </Text>
                  <View />
                </View>
              )
            }}
            keyExtractor={(item, index) => `${index}`}
            data={detailInfo}
            renderItem={renderItemFunc}
            showsVerticalScrollIndicator={false}
            scrollEnabled={false}
          />
        ) : (
          <>
            <View style={styles.rowMultiField}>
              <Text style={styles.txtFieldRow}>Part number</Text>
              <Text style={styles.txtFieldRow}>Locations</Text>
              <Text style={styles.txtFieldRow}>Quantity</Text>
            </View>
            <FlatList keyExtractor={(it, index) => `${it}${index}`} data={dataRow} renderItem={renderDataRow} />
          </>
        )}
      </ScrollView>

      {!isEmpty(valueQRCode) && <BaseBottomSheet
        open={!isEmpty(valueQRCode)}
        options={arrayValueQrCode}
        flex={0.6}
        title={"Choose QR Code"}
        type={"client"}
        onSelect={(item: any) => showImageZoom({ uri: item?.clientName, name: item?.clientName })}
        onClosed={() => setValueQrCode([])}
      />}

      {modalShowImage && (
        <BaseBottomSheetShowImage
          open={modalShowImage}
          options={infoImageModal}
          flex={0.9}
          onClosed={() => setModalShowImage(false)}
          onOpened={() => setModalShowImage(true)}
          value={true}
        />
      )}
    </>
  )
}
export default Submitted
