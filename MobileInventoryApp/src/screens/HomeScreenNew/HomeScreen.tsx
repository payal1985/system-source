import React, { useMemo } from "react"
import { FlatList, Image, Text, TouchableOpacity, View } from "react-native"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import ModalBox from "react-native-modalbox"
import IconImage from "react-native-vector-icons/FontAwesome5"
import FontAwesome from "react-native-vector-icons/FontAwesome"
import Icon from "react-native-vector-icons/AntDesign"

import LoadingOverlay from "/components/LoadingView/LoadingOverlay"
import { BaseButton } from "/components"
import styles from "./styles"
import moment from "moment"
import HomeScreenIndex from "./index"
type RenderItemProps = {
  item?: any
}
export const HomeScreenNew = () => {
  const {
    status,
    dataItemType,
    exampleState,
    loading,
    modalErr,
    openModalStatus,
    renderStatus,
    setmodalErr,
    openModal,
    handleLogout,
    goBack,
    handleNewClient,
    setOpenModalStatus,
    changeStatus,
    handleDelete,
    handleFillout,
  } = HomeScreenIndex()
  const renderItem = ({ item }: RenderItemProps): any => {
    if (status == item?.status) {
      return (
        <TouchableOpacity
          key={item.id}
          onPress={() => openModal(item)}
          style={styles.item}
          disabled={item?.status == "Submitted" ? true : false}
        >
          <View style={styles.containerViewImg}>
            <IconImage name={"laptop-code"} size={20} />
          </View>
          <View style={styles.contentItem}>
            <Text style={styles.title}>{item?.client ? item?.client : "Client"}</Text>
            <Text style={[styles.subTitle, { marginTop: 5 }]}>{`${moment(item?.deviceDate).format(
              "DD/MM/YYYY hh:mm a z",
            )}`}</Text>
            <Text style={[styles.subTitle, { marginTop: 5 }]}>{item?.status}</Text>
          </View>
        </TouchableOpacity>
      )
    }
  }

  const renderLogout = useMemo(() => {
    return (
      <TouchableOpacity onPress={handleLogout} style={styles.touchLogout}>
        <Text style={styles.logOut}>Logout</Text>
      </TouchableOpacity>
    )
  }, [])
  return (
    <View style={styles.container}>
      <View style={styles.height}></View>
      <KeyboardAwareScrollView
        bounces={false}
        showsVerticalScrollIndicator={false}
        extraScrollHeight={150}
        enableOnAndroid={true}
        style={{ flex: 8 }}
      >
        <View style={styles.content}>
          <View style={styles.viewLeft}>
            <TouchableOpacity onPress={() => goBack()} style={styles.btnLeft}>
              <Icon name={"left"} size={25} color={"blue"} />
            </TouchableOpacity>
            <View style={styles.logoOut}></View>
            {renderLogout}
          </View>
          <View style={{ justifyContent: "center", alignItems: "center", marginVertical: 15 }}>
            <BaseButton
              backgroundColor={"#BE3030"}
              label={"Create New Inventory"}
              onPress={() => handleNewClient()}
              fontSize={14}
              width={"70%"}
            />
          </View>

          <Text style={styles.inventoryCollection}>Inventory Collection\</Text>
        </View>
        <View style={styles.touchShowModal}>
          <TouchableOpacity style={styles.touchContent} onPress={() => setOpenModalStatus(true)}>
            <Text style={styles.txtStatus}>{renderStatus()}</Text>
            <Image source={require("../../assets/images/down-arrow.png")} style={styles.imgDownArrow} />
          </TouchableOpacity>
        </View>

        <FlatList
          data={exampleState}
          extraData={exampleState}
          renderItem={(item: any) => renderItem(item)}
          keyExtractor={(item: any, index: number) => `${index}`}
        />
      </KeyboardAwareScrollView>
      <ModalBox
        transparent={true}
        isOpen={modalErr}
        entry={"bottom"}
        position={"bottom"}
        swipeToClose={true}
        onClosed={() => setmodalErr(false)}
        style={styles.mainModal}
      >
        <View style={{ flex: 1 }}>
          <View style={styles.containerView}>
            <View style={{ flex: 3 }} />
            <View style={styles.border}></View>
            <View style={{ flex: 3 }} />
          </View>
          <View style={styles.viewAction}>
            <Text style={styles.txtClient}>{dataItemType?.client}</Text>
            <Text style={styles.txtDate}>{`${moment(dataItemType?.deviceDate).format("DD/MM/YYYY hh:mm a z")}`}</Text>
            <View style={{ marginTop: 40 }}>
              <TouchableOpacity style={{ flexDirection: "row" }} onPress={() => handleFillout()}>
                <Icon name={"edit"} size={25} color={"gray"} />
                <Text style={styles.txtProgress}> Edit</Text>
              </TouchableOpacity>
            </View>
            <View style={{ marginTop: 20 }}>
              <TouchableOpacity style={{ flexDirection: "row" }} onPress={() => handleDelete()}>
                <Icon name={"delete"} size={25} color={"gray"} />
                <Text style={styles.txtProgress}>Delete</Text>
              </TouchableOpacity>
            </View>
          </View>
        </View>
      </ModalBox>
      <ModalBox
        transparent={true}
        isOpen={openModalStatus}
        entry={"bottom"}
        position={"bottom"}
        swipeToClose={true}
        onClosed={() => setOpenModalStatus(false)}
        style={styles.mainModal}
      >
        <View style={{ flex: 1 }}>
          <View style={styles.containerView}>
            <View style={{ flex: 3 }} />
            <View style={styles.border}></View>
            <View style={{ flex: 3 }} />
          </View>
          <View style={styles.containerStatus}>
            <Text style={styles.txtClient}>Select status</Text>

            <View style={styles.rowContainer}>
              <TouchableOpacity style={styles.btnSelect} onPress={() => changeStatus("InProgress")}>
                <FontAwesome name={status == "InProgress" ? "check-circle-o" : "circle-o"} size={25} color={"gray"} />
                <Text style={styles.txtProgress}> In Progress</Text>
              </TouchableOpacity>
              <TouchableOpacity style={styles.btnSelect} onPress={() => changeStatus("Submitted")}>
                <FontAwesome name={status == "Submitted" ? "check-circle-o" : "circle-o"} size={25} color={"gray"} />
                <Text style={styles.txtProgress}> Submitted</Text>
              </TouchableOpacity>
            </View>
            <View style={styles.rowContainer}>
              <TouchableOpacity style={styles.btnSelect} onPress={() => changeStatus("InvalidData")}>
                <FontAwesome name={status == "InvalidData" ? "check-circle-o" : "circle-o"} size={25} color={"gray"} />
                <Text style={styles.txtProgress}> Submission Fail</Text>
              </TouchableOpacity>
              <TouchableOpacity style={styles.btnSelect} onPress={() => changeStatus("ExistedSubmission")}>
                <FontAwesome
                  name={status == "ExistedSubmission" ? "check-circle-o" : "circle-o"}
                  size={25}
                  color={"gray"}
                />
                <Text style={styles.txtProgress}> Existed submission</Text>
              </TouchableOpacity>
            </View>
          </View>
        </View>
      </ModalBox>
      <LoadingOverlay visible={loading} />
    </View>
  )
}
