import React, { useMemo } from "react"
import { Image, Text, FlatList, TouchableOpacity, View, Modal } from "react-native"
import { TextInput } from "react-native-gesture-handler"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import LoadingOverlay from "/components/LoadingView/LoadingOverlay"
import {
  BaseBottomSheet,
  BaseBottomSheetCamera,
  BaseBottomSheetCameraAddMoreCondition,
  BaseBottomSheetShowImage,
  BottomButtonNext,
  Header,
} from "../../components"
import { fontSizer } from "../../utils/dimension"
import IconImage from "react-native-vector-icons/AntDesign"
import { URL_BASE, SEARCH_IMAGE_TYPE } from "/constant/Constant"
import InventoryDetailScreenIndex from "./index"
import styles from "./styles"
import {
  Title,
  ChooseItemType,
  ChooseTitle,
  ItemNotReturnValue,
  ItemReturnValue,
  DropDown,
  DropDownPlus,
  TextParagraph,
  ImageApply,
  Gps,
  TextBoxPlus,
  GpsHasValue,
  RadioPlus,
} from "./Item"

type RenderItemTypeFeildProps = {
  item: any
  index: any
}

export const InventoryDetailScreenNew = () => {
  const {
    Areanumber,
    arrCheck,
    arrCheckQuatity,
    canAddData,
    checkNV,
    conditionRenderTitle,
    conditionsType,
    custBuidingName,
    custClient,
    custFloor,
    custGPS,
    dataImageItem,
    dataItemType,
    dataSharedModal,
    disableChoseItemType,
    errorRequire,
    imageInfo,
    isInventoryClientGroupID,
    itemType,
    itemTypeNew,
    itemTypeTxt,
    loading,
    localDataArray,
    mainPhoto,
    modalNumberCode,
    modalShowImage,
    modalType,
    modalTypeCamera,
    modalTypeCameraAddMore,
    showImageModal,
    titleDropDown,
    titleSharedModal,
    typeSharedModal,
    urlSupportFile,
    valuesTextInputTextParagraph,
    itemTypeArr,
    itemTypeId,
    openSharedModal,
    onChangeTextArr,
    onSelectArea,
    onSelectItemType,
    onSelectMethod,
    onSelectSharedModal,
    onSetMainPhoto,
    onTextChangedTxtFeild,
    openModal,
    renderQuantityTxt,
    setAreanumber,
    setModalNumberCode,
    setUrlSupportFile,
    setconditionRenderTitle,
    setcustGPS,
    setmodalShowImage,
    setmodalType,
    setmodalTypeCamera,
    setmodalTypeCameraAddMore,
    setopenSharedModal,
    handleOnPress,
    addMoreTxt,
    checkImage,
    getUpdateImage,
    handleOpenModal,
    handlePressCheck,
    handlePressCheckQuatity,
    handleSaveDataLocation,
    handleShowModalImage,
    onPressSearch,
    checkCompareOnBlur,
    checkKeyBoardType,
    checkKeyType,
    changeItemType,
    showImgSupportFile,
    handleDelete,
    handleDeleteItems,
    handleOnEndEditing,
    handleSave,
    handleShowMorePicture,
    onChangeCheckBoxSetMainPhoto,
    validateRequired,
  } = InventoryDetailScreenIndex()

  const renderItemTypeFeild = ({ item, index }: RenderItemTypeFeildProps): any => {
    const isError = errorRequire?.some((__er: any) => item?.itemTypeOptionCode === __er?.itemTypeOptionCode)
    const inventorySupportFile = item?.inventorySupportFile
    const hasSupportFile = !!inventorySupportFile
    const isDecimal = item?.fieldType === "Decimal"
    if (item.valType == 1 && !item.isHide) {
      if (item?.itemTypeOptionReturnValue == null) {
        return (
          <ItemNotReturnValue
            checkCompareOnBlur={checkCompareOnBlur}
            checkKeyBoardType={checkKeyBoardType}
            checkKeyType={checkKeyType}
            hasSupportFile={hasSupportFile}
            index={index}
            inventorySupportFile={inventorySupportFile}
            isDecimal={isDecimal}
            isError={isError}
            item={item}
            onChangeTextArr={onChangeTextArr}
            showImgSupportFile={showImgSupportFile}
          />
        )
      } else {
        return (
          <ItemReturnValue
            checkCompareOnBlur={checkCompareOnBlur}
            checkKeyBoardType={checkKeyBoardType}
            checkKeyType={checkKeyType}
            hasSupportFile={hasSupportFile}
            index={index}
            inventorySupportFile={inventorySupportFile}
            isDecimal={isDecimal}
            isError={isError}
            item={item}
            onChangeTextArr={onChangeTextArr}
            showImgSupportFile={showImgSupportFile}
          />
        )
      }
    } else if (item.valType == 3 && !item.isHide) {
      //Dropdown
      return <DropDown custClient={custClient} item={item} openModal={openModal} />
    } else if (item.valType == 90 && !item.isHide) {
      //DropdownPlus
      return <DropDownPlus custClient={custClient} item={item} openModal={openModal} />
    } else if (item.valType == 2 && !item.isHide) {
      //TextParagraph
      return (
        <TextParagraph
          checkCompareOnBlur={checkCompareOnBlur}
          handleOnEndEditing={handleOnEndEditing}
          item={item}
          onTextChangedTxtFeild={onTextChangedTxtFeild}
          valuesTextInputTextParagraph={valuesTextInputTextParagraph}
        />
      )
    } else if (item.valType == 40 && !item.isHide) {
      //CheckAllApply image
      return (
        <ImageApply
          arrCheckQuatity={arrCheckQuatity}
          handleDelete={handleDelete}
          handleDeleteItems={handleDeleteItems}
          handlePressCheckQuatity={handlePressCheckQuatity}
          handleShowModalImage={handleShowModalImage}
          handleShowMorePicture={handleShowMorePicture}
          index={index}
          item={item}
          localDataArray={localDataArray}
          mainPhoto={mainPhoto}
          onSetMainPhoto={onSetMainPhoto}
          renderQuantityTxt={renderQuantityTxt}
        />
      )
    } else if (item.valType == 30 && !item.isHide) {
      //gps
      return <Gps checkCompareOnBlur={checkCompareOnBlur} custGPS={custGPS} item={item} setcustGPS={setcustGPS} />
    } else if (item.valType == 50 && !item.isHide) {
      //TextBoxPlus
      if (item?.itemTypeOptionReturnValue == null) {
        return (
          <TextBoxPlus
            addMoreTxt={addMoreTxt}
            checkCompareOnBlur={checkCompareOnBlur}
            index={index}
            item={item}
            onChangeTextArr={onChangeTextArr}
          />
        )
      } else {
        return (
          <GpsHasValue
            addMoreTxt={addMoreTxt}
            checkCompareOnBlur={checkCompareOnBlur}
            index={index}
            item={item}
            onChangeTextArr={onChangeTextArr}
          />
        )
      }
    } else if (item.valType == 80 && !item.isHide) {
      //RadioPlus
      let arrRaw = item.itemTypeOptionLines
      return <RadioPlus arrCheck={arrCheck} arrRaw={arrRaw} handlePressCheck={handlePressCheck} item={item} />
    }
  }

  const renderChoseTitle = () => {
    return (
      <ChooseTitle
        Areanumber={Areanumber}
        custBuidingName={custBuidingName}
        custFloor={custFloor}
        custGPS={custGPS}
        handleOpenModal={handleOpenModal}
        handleSaveDataLocation={handleSaveDataLocation}
        setAreanumber={setAreanumber}
        setcustGPS={setcustGPS}
      />
    )
  }
  const renderTitle = () => {
    return (
      <Title
        Areanumber={Areanumber}
        custBuidingName={custBuidingName}
        custFloor={custFloor}
        custGPS={custGPS}
        setconditionRenderTitle={setconditionRenderTitle}
      />
    )
  }

  const renderChoseItemType = () => {
    return (
      <ChooseItemType
        changeItemType={changeItemType}
        disableChoseItemType={disableChoseItemType}
        itemTypeTxt={itemTypeTxt}
      />
    )
  }

  const ListHeaderComponent = useMemo((): any => {
    const uri =
      localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL
        ? localDataArray.isEdit
          ? itemTypeArr?.mainImageFull?.uri
          : imageInfo?.mainImageFull?.uri
        : localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER
        ? localDataArray.isEdit
          ? `${URL_BASE}/${itemTypeArr?.mainPhoto}`
          : `${URL_BASE}/${imageInfo?.mainPhoto}`
        : null
    return (
      !!dataItemType?.length && (
        <View>
          {localDataArray.conditionRender == SEARCH_IMAGE_TYPE.SERVER ||
          localDataArray.conditionRender == SEARCH_IMAGE_TYPE.LOCAL ? (
            <View style={{}}>
              <Text style={{ fontSize: fontSizer(16), fontWeight: "bold", fontStyle: "italic" }}>Main Photo </Text>
              <View
                style={{
                  width: 80,
                  height: 80,
                  marginTop: 10,
                  marginHorizontal: 15,
                }}
              >
                <Image source={{ uri }} style={[styles.photo, styles.activePhoto]} resizeMode="cover" />
              </View>
            </View>
          ) : null}
        </View>
      )
    )
  }, [dataItemType, itemTypeNew])
  return (
    <View style={styles.container}>
      <Header
        isSaved={true}
        handleSave={() => (dataItemType?.length > 0 ? handleSave() : null)}
        isGoBack
        conditionRender={localDataArray.conditionRender}
        labels={"Inventory Collection"}
      />
      <LoadingOverlay visible={loading} />
      <KeyboardAwareScrollView
        bounces={false}
        showsVerticalScrollIndicator={false}
        extraScrollHeight={150}
        enableOnAndroid={true}
        style={styles.subContainer}
      >
        {!conditionRenderTitle ? renderChoseTitle() : renderTitle()}
        {renderChoseItemType()}
        <TouchableOpacity onPress={() => onPressSearch()} style={styles.buttonSearchImg}>
          <Text style={{ fontSize: fontSizer(16), fontWeight: "bold", color: "blue" }}>Search Photo </Text>
        </TouchableOpacity>
        <FlatList
          ListHeaderComponent={localDataArray.parentRowID != null ? ListHeaderComponent : null}
          data={dataItemType}
          style={{ marginBottom: 100 }}
          renderItem={(item) => renderItemTypeFeild(item)}
          extraData={[...errorRequire]}
          keyExtractor={(item, index) => `${index}`}
        />
      </KeyboardAwareScrollView>
      <BottomButtonNext disabled={validateRequired()} onPressButton={() => handleOnPress()} />

      {modalNumberCode && (
        <BaseBottomSheet
          open={modalNumberCode}
          options={checkNV}
          flex={0.6}
          addMore={true}
          title={titleDropDown}
          type={"numberNV"}
          itemTypeId={itemTypeId}
          onSelect={(item) => onSelectMethod(item)}
          onClosed={() => setModalNumberCode(false)}
          onOpened={() => setModalNumberCode(true)}
        />
      )}
      {openSharedModal && (
        <BaseBottomSheet
          open={openSharedModal}
          options={dataSharedModal}
          flex={0.6}
          addMore={canAddData}
          title={titleSharedModal}
          type={typeSharedModal}
          onSelect={(item) => onSelectSharedModal(item)}
          onClosed={() => setopenSharedModal(false)}
          onOpened={() => setopenSharedModal(true)}
        />
      )}

      {modalShowImage && (
        <BaseBottomSheetShowImage
          open={modalShowImage}
          options={showImageModal}
          flex={0.9}
          title={"Image"}
          type={"areaNumber"}
          onSelect={(item) => onSelectArea(item)}
          onClosed={() => setmodalShowImage(false)}
          onOpened={() => setmodalShowImage(true)}
          isShowChooseMainPhoto={localDataArray.parentRowID == null ? true : false}
          onValueChange={onChangeCheckBoxSetMainPhoto}
          value={mainPhoto?.name === showImageModal!.name}
        />
      )}
      {modalType && (
        <BaseBottomSheet
          open={modalType}
          options={itemType}
          flex={0.6}
          addMore={isInventoryClientGroupID}
          title={"ItemTypes"}
          type={"itemType"}
          onSelect={(item) => onSelectItemType(item)}
          onClosed={() => setmodalType(false)}
          onOpened={() => setmodalType(true)}
        />
      )}
      {modalTypeCamera && (
        <BaseBottomSheetCamera
          open={modalTypeCamera}
          options={itemType}
          flex={1}
          conditionRender={localDataArray.conditionRender}
          title={"Chose Item Type number?"}
          type={conditionsType}
          onSelect={(item) => checkImage(item)}
          onClosed={() => setmodalTypeCamera(false)}
          onOpened={() => setmodalTypeCamera(true)}
        />
      )}
      {modalTypeCameraAddMore && (
        <BaseBottomSheetCameraAddMoreCondition
          open={modalTypeCameraAddMore}
          options={dataImageItem}
          flex={1}
          title={"Chose Item Type number?"}
          type={conditionsType}
          onSelect={(item) => getUpdateImage(item)}
          onClosed={() => setmodalTypeCameraAddMore(false)}
          onOpened={() => setmodalTypeCameraAddMore(true)}
        />
      )}
      <Modal visible={!!urlSupportFile} onRequestClose={() => setUrlSupportFile("")} transparent animationType="slide">
        <TouchableOpacity activeOpacity={1} style={styles.containerViewModal} onPressOut={() => setUrlSupportFile("")}>
          <Image source={{ uri: urlSupportFile }} style={styles.imgSupportFile} />
        </TouchableOpacity>
      </Modal>
    </View>
  )
}
