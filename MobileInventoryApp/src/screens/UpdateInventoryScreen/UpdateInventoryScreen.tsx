import React from "react"
import {
  FlatList,
  Image,
  Text,
  TextInput,
  TouchableOpacity,
  View,
  ScrollView,
  Alert,
  ActivityIndicator,
} from "react-native"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import IconCheck from "react-native-vector-icons/Feather"
import MaterialCommunityIcons from "react-native-vector-icons/MaterialCommunityIcons"
import Ionicons from "react-native-vector-icons/Ionicons"
import Icon from "react-native-vector-icons/Ionicons"
import IconFeather from "react-native-vector-icons/Feather"
import AntDesign from "react-native-vector-icons/AntDesign"

import { BottomButtonNext, Header, BaseBottomSheet, BaseBottomSheetShowImage } from "../../components"
import styles from "./styles"
import ModalBox from "react-native-modalbox"
import { ModalFullScreen } from "/components/ModalFullScreen"
import { Colors } from "/configs"
import useDetailListInventory from "./useDetailListInventory"
import { SERVER_NAME } from "../../constant/Constant"
import { isEmpty } from "lodash"

const UpdateInventoryScreen = () => {
  const {
    buildingDataState,
    areaNumber,
    floorDataState,
    gPSValue,
    inventoryItemList,
    modalBox,
    dataModalType,
    openModalType,
    canAddData,
    titleModalType,
    typeModalType,
    detailShow,
    isSearchList,
    itemTypeState,
    listSearch,
    idClient,
    textSearch,
    infoDetail,
    infoImageModal,
    modalShowImage,
    modalShowImageOut,
    listChoose,
    isLoadMore,
    refFlatList,
    refItemTypeData,
    refBuildingData,
    refFloorData,
    conditionDataState,
    refConditionData,
    showImageZoom,
    handleOpenScan,
    handleOpenModal,
    getAndShowLocation,
    goBack,
    setAreaNumber,
    setGPSValue,
    setDetailShow,
    ConditionDisableBtnDone,
    handleOnPress,
    onSelectSharedModal,
    setOpenModalType,
    searchList,
    closeModal,
    searchInventory,
    chooseItemToAdd,
    setTextSearch,
    removeAll,
    setInfoDetail,
    setNoDoneBtn,
    setModalShowImage,
    setModalShowImageOut,
    chooseFunc,
    handleLoadMore,
    setCurrentPage,
    funcGetInventoryInfo,
    setModalBox,
    setIsSearchList,
    setBuildingDataState,
    setFloorDataState,
    setItemTypeState,
    setConditionDataState,
  } = useDetailListInventory()
  let onEndReachedCalledDuringMomentum = true

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
  }

  const renderItemSearch = ({ item, index }: any) => {
    return (
      <TouchableOpacity
        key={index}
        onPress={() => chooseItemToAdd(item?.inventoryId, item?.totalQuantity)}
        style={[styles.containerItemRow, { justifyContent: !isEmpty(listChoose) ? "space-between" : "flex-start" }]}
      >
        <TouchableOpacity
          onPress={() =>
            showImageZoom({ uri: `${SERVER_NAME}/${idClient}/${item?.mainImage}`, name: item?.mainImage }, true)
          }
        >
          <Image source={{ uri: `${SERVER_NAME}/${idClient}/${item?.mainImage}` }} style={styles.img} />
        </TouchableOpacity>
        <View style={{ flex: 1 }}>
          <Text style={[styles.txtListSearch, { color: Colors.blue }]}>
            {`ID: ${item?.inventoryId || ''} - ${item?.itemTypeName || ""} - Quantity: ${item?.totalQuantity || ""}`}
          </Text>
          <Text style={styles.txtListSearch}>{`Tag: ${item?.tag || ""}`}</Text>
          <TouchableOpacity
            onPress={() => {
              funcGetInventoryInfo(item?.inventoryId)
            }}
          >
            <Text numberOfLines={3} style={styles.txtListSearch}>{`Description: ${item?.description || ""}`}</Text>
          </TouchableOpacity>
        </View>
      </TouchableOpacity>
    )
  }

  const renderChoseTitle = () => {
    return (
      <FlatList
        ListHeaderComponent={
          <View style={{ marginBottom: 16 }}>
            <View style={styles.choseTitleContainer}>
              <View style={styles.subChoseTitleContainer}>
                <Text style={styles.txtTitleChose}>Text Search</Text>
                <View style={[styles.btnDefault, { backgroundColor: "#FFFF" }]}>
                  <TextInput style={styles.txtSize16} onChangeText={(text) => setTextSearch(text)} value={textSearch} />
                </View>
              </View>
            </View>

            <View style={styles.choseTitleContainer}>
              <View style={styles.subChoseTitleContainer}>
                <Text style={styles.txtTitleChose}>ItemTypes</Text>
                <TouchableOpacity
                  style={[styles.btnDefault, { backgroundColor: "#FFFF" }]}
                  onPress={() => handleOpenModal("itemType")}
                >
                  <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
                    {itemTypeState?.itemTypeName}
                  </Text>
                  <Icon name={"list"} size={20} />
                </TouchableOpacity>
              </View>
            </View>


            <View style={styles.subChoseTitleContainer}>
              <Text style={styles.txtTitleChose}>Condition</Text>
              <TouchableOpacity
                style={[styles.btnDefault, { backgroundColor: "#FFFF" }]}
                onPress={() => handleOpenModal("Condition")}
              >
                <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
                  {conditionDataState?.itemTypeName}
                </Text>
                <Icon name={"list"} size={20} />
              </TouchableOpacity>
            </View>



            <View style={styles.subChoseTitleContainer}>
              <Text style={styles.txtTitleChose}>Building</Text>
              <TouchableOpacity
                style={[styles.btnDefault, { backgroundColor: "#FFFF" }]}
                onPress={() => handleOpenModal("Building")}
              >
                <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
                  {buildingDataState?.buildingName}
                </Text>
                <Icon name={"list"} size={20} />
              </TouchableOpacity>
            </View>


            <View style={styles.subChoseTitleContainer}>
              <Text style={styles.txtTitleChose}>Floor</Text>
              <TouchableOpacity
                style={[styles.btnDefault, { backgroundColor: "#FFFF" }]}
                onPress={() => handleOpenModal("Floors")}
              >
                <Text style={styles.txtSize16} maxFontSizeMultiplier={1}>
                  {floorDataState?.floorName}
                </Text>
                <Icon name={"list"} size={20} />
              </TouchableOpacity>
            </View>


            <View style={styles.choseTitleContainer}>
              <View style={styles.subChoseTitleContainer}>
                <Text style={styles.txtTitleChose}>Area or Room number</Text>
                <View style={[styles.btnDefault, { backgroundColor: "#FFFF" }]}>
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



          </View>
        }
        ref={refFlatList}
        showsVerticalScrollIndicator={false}
        data={listSearch}
        renderItem={renderItemSearch}
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
        keyExtractor={(it, index) => `${index}`}
        style={{ marginBottom: 100 }}
      />
    )
  }
  const keysInfo = infoDetail ? Object.keys(infoDetail) : null
  const valuesInfo: any = infoDetail ? Object.values(infoDetail) : null
  const indexMainPhoto = keysInfo?.findIndex((it: any) => it === "mainPhoto" || it === "mainImage")
  return (
    <>
      <View style={styles.container}>
        <Header isGoBack actionGoBack={goBack} labels={"Update Inventory"} />
        <KeyboardAwareScrollView
          bounces={false}
          showsVerticalScrollIndicator={false}
          extraScrollHeight={150}
          enableOnAndroid={true}
          style={styles.subContainer}
        >
          {!isEmpty(inventoryItemList) && (
            <TouchableOpacity
              style={styles.removeBtn}
              onPress={() => {
                Alert.alert("Warning", "Are you sure ?", [
                  { text: "OK", onPress: removeAll },
                  { text: "Cancel", style: "cancel" },
                ])
              }}
            >
              <Text style={styles.txtBtn}>Remove All</Text>
            </TouchableOpacity>
          )}
          {renderButtonScan()}
          <View style={styles.button}>
            <View style={{ flex: 2 }} />
            <TouchableOpacity
              onPress={() => {
                if (!isEmpty(listSearch)) {
                  searchList()
                } else {
                  setModalBox(true)
                  setIsSearchList(true)
                  refItemTypeData.current = { itemTypeName: "ALL", itemTypeID: 0 }
                  setBuildingDataState(refBuildingData.current)
                  setFloorDataState(refFloorData.current)
                  setItemTypeState(refItemTypeData.current)
                  setConditionDataState(refConditionData.current)
                }
              }}
              style={styles.btnContainer}
            >
              <Ionicons name={"search-sharp"} size={25} color={"#fff"} />
              <Text style={styles.txtButton}>Search Inventory</Text>
            </TouchableOpacity>
            <View style={{ flex: 2 }} />
          </View>
        </KeyboardAwareScrollView>
      </View>

      <ModalBox
        isOpen={modalBox}
        style={styles.mainModal}
        entry={"top"}
        position={"bottom"}
        swipeToClose={false}
        onClosed={closeModal}
      >
        <View style={{ flex: 1 }}>
          <View style={styles.containerRow}>
            <TouchableOpacity
              onPress={() => {
                refFlatList.current &&
                  !isEmpty(listSearch) &&
                  refFlatList.current?.scrollToOffset({ y: 0, animated: true })
              }}
              style={{ marginTop: 16, opacity: 0 }}
            >
              <AntDesign name={"totop"} size={20} />
            </TouchableOpacity>
            <Text style={styles.titleModalBox}>{isSearchList ? "Search Inventory Item" : "Pick Location"}</Text>
            <TouchableOpacity onPress={closeModal} style={{ marginTop: 16 }}>
              <IconFeather name={"x"} size={20} />
            </TouchableOpacity>
          </View>
          {renderChoseTitle()}
        </View>
        <View style={styles.containerBtnNext}>
          <BottomButtonNext
            labels={isSearchList ? "Search" : "Done"}
            disabled={isSearchList ? false : ConditionDisableBtnDone()}
            onPressButton={
              isSearchList
                ? () => {
                  searchInventory(1, false)
                  setCurrentPage(1)
                  refFlatList.current &&
                    !isEmpty(listSearch) &&
                    refFlatList.current?.scrollToOffset({ y: 0, animated: true })
                }
                : handleOnPress
            }
          />
        </View>
        {openModalType && (
          <BaseBottomSheet
            open={modalBox}
            options={dataModalType}
            flex={0.6}
            addMore={canAddData}
            title={titleModalType}
            type={typeModalType}
            onSelect={(item) => onSelectSharedModal(item)}
            onClosed={() => setOpenModalType(false)}
            onOpened={() => setOpenModalType(true)}
          />
        )}
      </ModalBox>

      <ModalFullScreen
        backdrop={true}
        isOpen={detailShow}
        onClosed={() => {
          setDetailShow(false)
          setNoDoneBtn(false)
          setModalShowImage(false)
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
                <TouchableOpacity onPress={() => setDetailShow(false)}>
                  <IconFeather name={"x"} size={20} />
                </TouchableOpacity>
              </View>
              {indexMainPhoto && indexMainPhoto !== -1 && (
                <TouchableOpacity
                  style={{ width: 50, height: 50 }}
                  onPress={() => {
                    showImageZoom({
                      name: valuesInfo?.[indexMainPhoto]?.toString(),
                      uri: `${SERVER_NAME}/${idClient}/${valuesInfo?.[indexMainPhoto]?.toString()}`,
                    })
                  }}
                >
                  <Image
                    source={{ uri: `${SERVER_NAME}/${idClient}/${valuesInfo?.[indexMainPhoto]?.toString()}` }}
                    style={styles.img}
                  />
                </TouchableOpacity>
              )}
              <View style={{ paddingBottom: 32 }}>
                {keysInfo?.map((it: string, index: number) => {
                  if (valuesInfo?.[index]?.toString() === "hide_txt_value") return null
                  if (it === "itemTypeOptions" || it == "mainImage" || it == "mainPhoto") return null
                  return (
                    <View style={styles.rowDetail}>
                      <Text style={styles.singleItem}>{`${it?.toString()}: `}</Text>
                      <Text style={{ flex: 1 }}>{valuesInfo?.[index]?.toString() || ""}</Text>
                    </View>
                  )
                })}
              </View>
            </ScrollView>
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
        }
      />
      {modalShowImageOut && (
        <BaseBottomSheetShowImage
          open={modalShowImageOut}
          options={infoImageModal}
          flex={0.9}
          onClosed={() => setModalShowImageOut(false)}
          onOpened={() => setModalShowImageOut(true)}
          value={true}
        />
      )}
    </>
  )
}

export default UpdateInventoryScreen
