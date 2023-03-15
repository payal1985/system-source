import { useFocusEffect, useNavigation, useRoute } from "@react-navigation/native"
import React, { useCallback, useEffect, useState } from "react"
import { FlatList, Text, TouchableOpacity, View, Alert } from "react-native"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import IconCheck from "react-native-vector-icons/Feather"
import MaterialCommunityIcons from "react-native-vector-icons/MaterialCommunityIcons"
import Routes from "/navigators/Routes"
import { BottomButtonNext, Header, BaseAlert } from "../../components"
import { get } from "lodash"
import styles from "./styles"
import { getListOrderItems, updateOrderCompleted, updateOrderItemCompleted } from "/redux/progress/service"
// import { NativeModules, NativeEventEmitter } from 'react-native'; //bear
import Storage from "/helper/Storage";
import useGlobalData from "/utils/hook/useGlobalData"

// const { UHFModule, UHFManager, VC, UHFEventListener } = NativeModules; //bear

// const EventEmitter = new NativeEventEmitter(UHFEventListener); // bear

type RenderItemProps = {
  item?: any
  index?: any
}

const OrderItemListScreen = () => {
  const { navigate, goBack } = useNavigation()
  const route = useRoute()
  const orderID = get(route, "params.orderID")
  const goBackFromScan = get(route, "params.goBackFromScan")
  const dataOrderItemsFromScan = get(route, "params.dataOrderItems")
  const [dataOrderItems, setdataOrderItems] = useState<any>([])
  const [globalData] = useGlobalData()

  // useEffect(() => {
  //   getListOrderItemsData()
  // }, [])
  useFocusEffect(
    useCallback(() => {
      if (goBackFromScan) {
        setdataOrderItems(dataOrderItemsFromScan)
      } else {
        getListOrderItemsData()
      }
    }, []),
  )
  // useEffect(() => { //bear begin
  //Result of the scanbarcode.  It returns the barcode content
  //   EventEmitter.addListener('onBarcode', (data) => {
  //     console.log('RN Data:');
  //     console.log(data);
  //     if (data?.includes("https://ssi.com")) {
  //       let parts = data.split('=', 2);
  //       let inventoryItemId = parts[1]
  //       // let inventoryItemId = 167
  //       // chooseItemToAdd(inventoryItemId)
  //     }
  //   });

  //   return () => {
  //     // this.listenerOnPause.remove();
  //     UHFManager.removeUHF();
  //   }
  // }, [])
  // useEffect(() => {
  //   initUHF()
  // }, [])
 //Start to connect and listen to the device
  // const initUHF = () => {
  //   UHFManager.initUHF();
  //   connectEvent()
  // };

  ////Exit the scan device
  // const removeUHF = () => {
  //   UHFManager.removeUHF();
  // };

  // //after got the list of devices from the get DeviceEvent and show the list of device for choosing
  // //the device to connect.  Note: result is in row index starting Zero
 // const connectEvent = () => {
  //   UHFManager.connectEvent(0);
  //   // VC.openViewControllerInMainStory();
  // };
 // //Find all the devices
  // const getDeviceEvent = async () => {
  //   try {

  //     const data = await UHFManager.getDeviceEvent("getDeviceEvent");
  //     console.log(`Data Device: ${data}`);
  //   } catch (e) {
  //     console.error(e);
  //   }

  // };
  // //Listen to the connected device and EventEmitter.addListener('onBarcode'..) will get barcode content
  // const getBarcodeEvent = async () => { //bear
  //   try {

  //     const data = await UHFManager.getDeviceEvent("getDeviceEvent");
  //     console.log(`Status listner: ${data}`);
  //   } catch (e) {
  //     console.error(e);
  //   }
  // };

  // const renderButtonScanHandHeld = () => { //bear
  //   return (
  //     <View style={styles.button}>
  //       <View style={{ flex: 2 }} />
  //       <TouchableOpacity onPress={() => getBarcodeEvent()} style={styles.btnContainer}>
  //         {/*<TouchableOpacity onPress={() => getBarcodeEventTest()} style={styles.btnContainer}>*/}
  //         <MaterialCommunityIcons name={"barcode-scan"} size={25} color={"#fff"} />
  //         <Text style={styles.txtButton}>Scan BarCode TSL</Text>
  //       </TouchableOpacity>
  //       <View style={{ flex: 2 }} />
  //     </View>
  //   )
  // }
  const getListOrderItemsData = async () => {
    let body = {
      "clientId": globalData?.item[0].clientId,
      "orderId": orderID
    }
    getListOrderItems(
      body,
      (res) => {
        if (res.status == 200) {
          let data = res.data
          setdataOrderItems([...data])
        } else {
          Alert.alert('Error when trying to find the order items')
        }
      },
      (e) => {
        Alert.alert('Error when trying to find the order items')
      },
    )
  }
  const handleDoneUpdateOrder = async () => {
    let body = {
      "clientId": globalData?.item[0].clientId,
      "orderId": orderID,
      "orderItemIds": []
    }
    updateOrderCompleted(
      body,
      (res) => {
        if (res.status == 200) {
        dataOrderItems?.map((it: any) => {
          it.matchScan = true
          return it
        })
        setdataOrderItems([...dataOrderItems])
        } else {
          Alert.alert('Success')
        }
      },
      (e) => {
        BaseAlert(e, "Error get data order")
      },
    )
  }
  const goBackFunc = () => {
    goBack()
  }
  const handleOpenScan = () => {
    navigate(Routes.CAMERA_SCAN_ORDER_ITEMS, { dataOrderItems: dataOrderItems })
  }
  const renderButtonScan = () => {
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
  // const renderButtonScanHandHeld = () => { //bear begin
  //     return (
  //         <View style={styles.button}>
  //             <View style={{flex: 2}}/>
  //             <TouchableOpacity onPress={() => handleOpenScan()} style={styles.btnContainer}>
  //                 <MaterialCommunityIcons name={"barcode-scan"} size={25} color={"#fff"}/>
  //                 <Text style={styles.txtButton}>Scan HandHeld</Text>
  //             </TouchableOpacity>
  //             <View style={{flex: 2}}/>
  //         </View>
  //     )
  //     // }
  // } //bear end
  const handleDoneOrderItem = async (inventoryItemId: any) => {
    let body = {
      "clientId": globalData?.item[0].clientId,
      "inventoryItemId": inventoryItemId
    }
    updateOrderItemCompleted(
      body,
      (res) => {
        console.log("res",res)
        if (res.status == 200) {
          let arrRaw = dataOrderItems
          for (let i = 0; i < arrRaw.length; i++) {
            if (arrRaw[i].inventoryItemID == inventoryItemId) {
              arrRaw[i].matchScan = true
            }
          }
          setdataOrderItems([...arrRaw])
          BaseAlert("Success", "Successfully relocate")

        } else {
          BaseAlert(res, "Error get data order")
        }
      },
      (e) => {
        BaseAlert(e, "Error get data order")
      },
    )
  }
  const renderItem = ({ item, index }: RenderItemProps) => {
    return (
      <View
        key={index}
        style={{
          backgroundColor: item.matchScan ? 'green' : "#fff",
          paddingVertical: 15,
          flexDirection: "row",
          alignItems: "center",
          marginBottom: 2,
        }}
      >
        <TouchableOpacity style={{ flex: 6, marginLeft: 20 }}>
          <Text style={styles.title}>{`Order Item ID ${item.orderItemID || item?.itemTypeName}`}</Text>
        </TouchableOpacity>

        <TouchableOpacity
          onPress={() => handleDoneOrderItem(item.inventoryItemID)}
          style={{ flex: 2, alignItems: "flex-end", marginRight: 20 }}
        >

          <IconCheck name={"check-circle"} size={20} />
        </TouchableOpacity>
      </View>
    )
  }
  return (
    <View style={styles.container}>
      <Header isGoBack actionGoBack={goBackFunc} labels={"List Order Item"} />

      <KeyboardAwareScrollView
        bounces={false}
        showsVerticalScrollIndicator={false}
        extraScrollHeight={150}
        enableOnAndroid={true}
        style={styles.subContainer}
      >
        <View style={styles.subTitle}>
          <Text style={styles.txtTitle}>List Order Item</Text>
        </View>
        <FlatList data={dataOrderItems} renderItem={renderItem} keyExtractor={(item, index) => `${item}${index}`} />

        <View style={styles.button}>
          <View style={{ flex: 2 }} />

          <View style={{ flex: 2 }} />
        </View>
        {renderButtonScan()}
      </KeyboardAwareScrollView>
      <BottomButtonNext labels="Done" onPressButton={() => handleDoneUpdateOrder()} />
    </View>
  )
}

export default OrderItemListScreen
