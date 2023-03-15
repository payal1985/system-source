import { useNavigation } from "@react-navigation/native"
import React, { useState, useEffect, useRef, useCallback } from "react"
import { FlatList, Text, TouchableOpacity, View } from "react-native"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import IconImage from "react-native-vector-icons/FontAwesome5"
import Routes from "/navigators/Routes"
import { getListOrder } from "/redux/progress/service"
import { getScreenWidth } from "../../utils/dimension"
import LoadingOverlay from "/components/LoadingView/LoadingOverlay"
import { BaseAlert, Header } from "/components"
import Storage from "/helper/Storage"
import styles from "./styles"
import moment from "moment"
import { useGlobalData } from "/utils/hook"
import { isEmpty } from "lodash"
import { useFocusEffect } from '@react-navigation/native'

type RenderItemProps = {
  item?: any
  index?: number
}
type Item = {
  client?: any
  deviceDate?: any
  dataItemType?: any[]
}


const OrderScreen = () => {
  const { navigate } = useNavigation()
  const [orderList, setorderList] = useState([])
  const [globalData] = useGlobalData()

  const [assignedToMe, setassignedToMe] = useState([])
  const [assignedToEveryOne, setassignedToEveryOne] = useState([])
  const [loading, setLoading] = useState<boolean>(false)


  useFocusEffect(useCallback(() => {
    getListOrderData()
  }, []))
  
  const getListOrderData = async () => {
    const user = await Storage.get("@User")
    const objUser = JSON.parse(`${user}`)
    let body = {
      "clientId": globalData?.item[0]?.clientId,
      "statusID": 5,
    }
    getListOrder(
      body,
      (res) => {
        console.log("res getListOrderData", res)
        if (res.status == 200) {
          let data = res.data
          setorderList(data)
          let assignedToMeRaw = data.filter((x: any) => x.assignedToID == objUser?.userId)
          let assignedToEveryOneRaw = data.filter((x: any) => x.assignedToID !== objUser?.userId)
          setassignedToMe(assignedToMeRaw)
          setassignedToEveryOne(assignedToEveryOneRaw)
        } else {
          BaseAlert(res, "Error get data order")
        }
      },
      (e) => {
        BaseAlert(e, "Error get data order")
      },
    )
  }

  const navigateOrderItemList = (orderID: any) => {
    navigate(Routes.ORDER_ITEM_LIST_SCREEN, { orderID: orderID, })
  }

  const renderItem = ({ item, index }: RenderItemProps): any => {
    console.log('item', item)
    return (
      <View style={styles.containerSwipeOut} >
        <TouchableOpacity key={item.id} onPress={() => navigateOrderItemList(item.orderID)} style={[styles.item, { paddingHorizontal: 16 }]}>
          {/* <View style={{ flex: 0.5, marginHorizontal: 15, marginTop: 5 }}>
            <IconImage name={"laptop-code"} size={20} />
          </View> */}
          <View style={{ flex: 5 }}>
            <Text style={styles.title}>Order ID: {item.orderID}</Text>
            <Text style={styles.title}>Request ID: {item?.requestID}</Text>
            <Text style={[styles.subTitle, { marginTop: 5 }]}>{`Created Date: ${moment(item?.createDateTime).format(
              "DD/MM/YYYY hh:mm a z",
            )}`}</Text>
            <Text style={[styles.subTitle, { marginTop: 5 }]}>{`Required Date: ${moment(item?.createDateTime).format(
              "DD/MM/YYYY hh:mm a z",
            )}`}</Text>
            <Text style={[styles.subTitle, { marginTop: 5 }]}>{item?.status}</Text>
          </View>
        </TouchableOpacity>
      </View>
    )
  }



  return (
    <View style={styles.container}>
      <Header isGoBack labels="Order Inventory" />
      <KeyboardAwareScrollView
        bounces={false}
        showsVerticalScrollIndicator={false}
        extraScrollHeight={150}
        enableOnAndroid={true}
        style={{ flex: 8 }}
      >
        {!isEmpty(assignedToMe) &&
          <>
            <View style={{ flex: 1, paddingVertical: 10, flexDirection: 'row', alignItems: 'center' }}>
              <View style={{ marginTop: 30 }}>
                <Text style={styles.inventoryCollection}>My Orders</Text>
              </View>
            </View>
            <FlatList
              data={assignedToMe}
              extraData={assignedToMe}
              renderItem={(item: any) => renderItem(item)}
              keyExtractor={(item: any, index: number) => `${index}`}
            />
          </>
        }
        {!isEmpty(assignedToEveryOne) && <>
          <View style={{ flex: 1, paddingVertical: 10, flexDirection: 'row', alignItems: 'center' }}>
            <View style={{ marginTop: 10 }}>
              <Text style={styles.inventoryCollection}>New Orders</Text>
            </View>
          </View>
          <FlatList
            data={assignedToEveryOne}
            extraData={assignedToEveryOne}
            renderItem={(item: any) => renderItem(item)}
            keyExtractor={(item: any, index: number) => `${index}`}
          />
        </>}
      </KeyboardAwareScrollView>


      <LoadingOverlay visible={loading} />
    </View>
  )
}
export default OrderScreen
