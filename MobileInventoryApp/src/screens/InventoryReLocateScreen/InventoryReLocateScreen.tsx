import React, { AnchorHTMLAttributes } from "react"
import { FlatList, Image, Text, TextInput, TouchableOpacity, View, ScrollView, Alert, ActivityIndicator } from "react-native"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import IconCheck from "react-native-vector-icons/Feather"
import MaterialCommunityIcons from "react-native-vector-icons/MaterialCommunityIcons"
import Ionicons from "react-native-vector-icons/Ionicons"
import Icon from "react-native-vector-icons/Ionicons"
import IconFeather from "react-native-vector-icons/Feather"

import { BottomButtonNext, Header, BaseBottomSheet, BaseBottomSheetShowImage } from "../../components"
import CustomCheckbox from "../../components/CustomCheckbox"
import styles from "./styles"
import ModalBox from "react-native-modalbox"
import { ModalFullScreen } from "/components/ModalFullScreen"
import { Colors } from "/configs"
import useDetailListInventory from "./useDetalListInventory"
import { SERVER_NAME } from "../../constant/Constant"
import { isEmpty, valuesIn } from "lodash"
import AntDesign from "react-native-vector-icons/AntDesign"
import moment from "moment"


type RenderItemProps = {
  item?: any
  index?: any
}

const InventoryReLocateScreen = () => {
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
    valueToolTip,
    checkBox,
    textSearch,
    infoDetail,
    noDoneBtn,
    infoImageModal,
    modalShowImage,
    autoShow,
    modalShowImageOut,
    listChoose,
    refScrollView,
    isLoadMore,
    refBuildingData,
    refFloorData,
    refItemTypeData,
    refConditionData,
    conditionDataState,
    showImageZoom,
    renderTxtLocations,
    deleteRow,
    handleOpenScan,
    handleOpenModal,
    getAndShowLocation,
    goBack,
    setAreaNumber,
    setGPSValue,
    setDetailShow,
    conditionDisable,
    nextFunc,
    ConditionDisableBtnDone,
    handleOnPress,
    onSelectSharedModal,
    setOpenModalType,
    setValueInventory,
    addItemType,
    searchList,
    closeModal,
    searchInventory,
    chooseItemToAdd,
    setValueToolTip,
    addAllItem,
    setCheckBox,
    setTextSearch,
    removeAll,
    setInfoDetail,
    setNoDoneBtn,
    setModalShowImage,
    setAutoShow,
    setModalShowImageOut,
    chooseFunc,
    setListChoose,
    selectAllFunc,
    funcGetDetailInventoryItem,
    getValueByCode,
    renderValueArr,
    handleLoadMore,
    setCurrentPage,
    setBuildingDataState,
    setFloorDataState,
    setIsSearchList,
    setItemTypeState,
    setModalBox,
    setConditionDataState,
    renderExpiredDate
  } = useDetailListInventory()
  let onEndReachedCalledDuringMomentum = true

  
  const renderItem = ({ item, index }: RenderItemProps) => {
    const itemDescription = item?.itemTypeOptions?.filter((it: any) => it?.itemTypeOptionCode === "Description")
    const valueDescription = itemDescription[0]?.itemTypeOptionReturnValue?.[0]?.returnValue
    const itemTypeOptionsCache = item?.itemTypeOptions
    const formatBody = {
      "Item Type Name": item?.itemTypeName,
      "Warranty": renderExpiredDate(item),
      mainPhoto: item?.mainPhoto || item?.mainImage,
      Condition: item?.condition,
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
    return (
      <TouchableOpacity
        onPress={() => {
          setInfoDetail(formatBody)
          setDetailShow(true)
        }}
        key={index}
        style={styles.containerItem}
      >
        <TouchableOpacity
          onPress={() =>
            showImageZoom({ uri: `${SERVER_NAME}/${idClient}/${item?.mainPhoto || item?.mainImage}`, name: item?.mainPhoto || item?.mainImage }, true)
          }
        >
          <Image source={{ uri: `${SERVER_NAME}/${idClient}/${item?.mainPhoto || item?.mainImage}` }} style={styles.img} />
        </TouchableOpacity>
        <View style={{ flex: 1 }}>
          <Text style={styles.title}>{`${item?.inventoryItemId} ${item?.itemName || item?.itemTypeName} -- ${item?.condition
            }\n${renderTxtLocations(item)}`}</Text>
          <View>
            <Text numberOfLines={3} style={styles.title}>{`Description: ${valueDescription || ""}`}</Text>
          </View>
        </View>
        <TouchableOpacity onPress={() => deleteRow(index)} style={{ flex: 0 }}>
          <IconCheck name={"trash-2"} size={20} />
        </TouchableOpacity>
      </TouchableOpacity>
    )
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
  }

  const renderItemSearch = ({ item, index }: any) => {
    const isInListChoose = listChoose?.findIndex((it: any) => it === item?.inventoryItemId) !== -1
    return (
      <>
        <View
          key={index}
          style={[styles.containerItemRow, { justifyContent: !isEmpty(listChoose) ? "space-between" : "flex-start" }]}
        >
          <TouchableOpacity
            onPress={() =>
              showImageZoom({ uri: `${SERVER_NAME}/${idClient}/${item?.mainImage}`, name: item?.mainImage }, true)
            }
          >
            <Image source={{ uri: `${SERVER_NAME}/${idClient}/${item?.mainImage}` }} style={styles.img} />
          </TouchableOpacity>
          <TouchableOpacity
            style={{ flex: 1 }}
            onPress={() => chooseItemToAdd(item?.inventoryItemId)}
          >
            <Text style={[styles.txtListSearch, { color: Colors.blue }]}>
              {`${item?.inventoryItemId || ""} -- ${item?.condition || ""} -- ${item?.itemType || ""}`}
            </Text>
            <Text
              style={styles.txtListSearch}
            >{`${item?.building || ""} - Floor: ${item?.floor || ""} - Room: ${item?.room || ""}`}</Text>
            <TouchableOpacity
              onPress={() => {
                funcGetDetailInventoryItem(item?.inventoryItemId)
              }}
            >
              <Text numberOfLines={3} style={styles.txtListSearch}>{`Description: ${item?.description || ""}`}</Text>
            </TouchableOpacity>
          </TouchableOpacity>
          <CustomCheckbox
            iOSCheckBoxStyles={{ marginTop: 0 }}
            tintColors={{ true: Colors.easternBlue, false: Colors.gray_80 }}
            value={isInListChoose}
            onValueChange={(value) => chooseFunc(value, item?.inventoryItemId)}
            style={{ zIndex: 99 }}
          />
        </View>
      </>
    )
  }
  const renderHeader = () => {
    return !isEmpty(listSearch) ? (
      <View style={styles.containerHeader}>
        <Text style={styles.txtHeader}>Select All</Text>
        <CustomCheckbox
          iOSCheckBoxStyles={{ marginTop: 0 }}
          tintColors={{ true: Colors.easternBlue, false: Colors.gray_80 }}
          value={listChoose?.length === listSearch?.length}
          onValueChange={(value) => {
            setCheckBox(value)
            selectAllFunc(value)
          }}
        />
        <TouchableOpacity
          style={[styles.doneBtn, { backgroundColor: isEmpty(listChoose) ? Colors.grayBG : Colors.redHeader }]}
          disabled={isEmpty(listChoose)}
          onPress={addAllItem}
        >
          <Text style={styles.txtBtn}>Select</Text>
        </TouchableOpacity>
      </View>
    ) : null
  }
  const renderChoseTitle = () => {
    return (
      <>
        {isSearchList && (
          <View style={styles.choseTitleContainer}>
            <View style={styles.subChoseTitleContainer}>
              <Text style={styles.txtTitleChose}>Text Search</Text>
              <View style={[styles.btnDefault, { backgroundColor: "#FFFF" }]}>
                <TextInput
                  style={styles.txtSize16}
                  maxFontSizeMultiplier={1}
                  onChangeText={(text) => setTextSearch(text)}
                >
                  {textSearch}
                </TextInput>
              </View>
            </View>
          </View>
        )}
        {isSearchList && (
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
        )}



        <View style={styles.choseTitleContainer}>
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
        </View>

        <View style={styles.choseTitleContainer}>
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
        </View>

        <View style={styles.choseTitleContainer}>
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


        {!isSearchList && (
          <View style={styles.choseTitleContainer}>
            <View style={styles.subChoseTitleContainer}>
              <Text style={styles.txtTitleChose}>GPS</Text>
              <View style={[styles.btnDefault, { backgroundColor: "#FFFF" }]}>
                <TextInput
                  style={styles.txtSize16}
                  numberOfLines={2}
                  maxFontSizeMultiplier={1}
                  onChangeText={(text) => setGPSValue(text)}
                >
                  {gPSValue}
                </TextInput>
                <TouchableOpacity onPress={getAndShowLocation}>
                  <Icon name={"location"} size={20} />
                </TouchableOpacity>
              </View>
            </View>
          </View>
        )}


        {isSearchList && (
          <FlatList
            ListHeaderComponent={renderHeader}
            contentContainerStyle={{ marginVertical: 16 }}
            data={listSearch}
            style={{ paddingBottom: 80 }}
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
          />
        )}
      </>
    )
  }
  const keysInfo = infoDetail ? Object.keys(infoDetail) : null
  const valuesInfo: any = infoDetail ? Object.values(infoDetail) : null
  const indexMainPhoto = keysInfo?.findIndex((it: any) => it === "mainPhoto" || it === "mainImage")
  return (
    <>
      <View style={styles.container}>
        <Header isGoBack actionGoBack={goBack} labels={"Re-Locate Inventory"} />
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
          <FlatList
            data={inventoryItemList}
            renderItem={renderItem}
            keyExtractor={(item, index) => `${item}${index}`}
          />
          {renderButtonScan()}
          <View style={styles.button}>
            <View style={{ flex: 2 }} />
            <TouchableOpacity
              onPress={() => {
                if (!isEmpty(listSearch)) {
                  searchList()
                  setCheckBox(false)
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
        <BottomButtonNext labels="Move to Location" disabled={conditionDisable()} onPressButton={() => nextFunc()} />
      </View>

      <ModalBox
        isOpen={modalBox}
        style={styles.mainModal}
        entry={"top"}
        position={"bottom"}
        swipeToClose={false}
        onClosed={closeModal}
      >
        <View style={styles.containerRow}>
          <TouchableOpacity
            onPress={() => {
              refScrollView.current &&
                !isEmpty(listSearch) &&
                refScrollView.current?.scrollTo()
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
        <KeyboardAwareScrollView showsVerticalScrollIndicator={false} ref={(ref: any) => refScrollView.current = ref}>
          {renderChoseTitle()}
        </KeyboardAwareScrollView>
        <View style={styles.containerBtnNext}>
          <BottomButtonNext
            labels={isSearchList ? "Search" : "Done"}
            disabled={isSearchList ? false : ConditionDisableBtnDone()}
            onPressButton={isSearchList ? () => {
              searchInventory(1, false)
              setCurrentPage(1)
            } : handleOnPress}
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
                  style={{ width: 50, height: 50, marginBottom: 16 }}
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
      <ModalFullScreen
        isOpen={autoShow}
        containerStyle={{ paddingVertical: 16 }}
        onClosed={() => setAutoShow(false)}
        useNativeDriver={true}
        backdropPressToClose={true}
        style={styles.modalFullScreen}
        element={
          <View style={styles.containerViewModal}>
            <Text>Moved Successfully</Text>
          </View>
        }
      />
    </>
  )
}

export default InventoryReLocateScreen
