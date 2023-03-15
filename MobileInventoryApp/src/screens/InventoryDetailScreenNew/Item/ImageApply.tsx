import React from "react"
import { View, Text, TouchableOpacity, FlatList, Image } from "react-native"
import styles from "../styles"
import IconImage from "react-native-vector-icons/AntDesign"
import MaterialCommunityIcons from "react-native-vector-icons/MaterialCommunityIcons"

interface ImageApplyProps {
  item: any
  arrCheckQuatity: any
  handleShowMorePicture: any
  renderQuantityTxt: any
  handleDeleteItems: any
  index: number
  handleDelete: any
  handleShowModalImage: any
  onSetMainPhoto: any
  mainPhoto: any
  localDataArray: any
  handlePressCheckQuatity: any
}

const ImageApply = (props: ImageApplyProps) => {
  const {
    item,
    arrCheckQuatity,
    handleShowMorePicture,
    renderQuantityTxt,
    handleDeleteItems,
    index,
    handleDelete,
    handleShowModalImage,
    mainPhoto,
    onSetMainPhoto,
    localDataArray,
    handlePressCheckQuatity,
  } = props
  return (
    <View style={styles.containerItemValue}>
      <View>
        <Text style={styles.redRequire}>
          {"Conditions Quantity"}
          {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
        </Text>

        {arrCheckQuatity.map((data: any, i: number) => {
          let arr = data?.dataCondition
          return (
            <View key={i} style={{ marginTop: 10 }}>
              {arr.length > 0 && (
                <TouchableOpacity onPress={() => handleShowMorePicture(data)}>
                  <Text style={styles.quantityTxt}>
                    {`${data.nameCondition} QTY`} {"-"} {renderQuantityTxt(data)}
                  </Text>
                </TouchableOpacity>
              )}
              {arr.map((dataImg: any, indexChild: number) => {
                return (
                  <View key={indexChild}>
                    <View style={{ marginLeft: 15 }}>
                      <View style={{ flexDirection: "row" }}>
                        <View>
                          <Text style={styles.imgName}>
                            {"- "}
                            {dataImg.itemName}
                          </Text>
                        </View>
                        <TouchableOpacity onPress={() => handleDeleteItems(index, data, i)} style={styles.btnDelete}>
                          <IconImage name={"delete"} size={15} />
                        </TouchableOpacity>
                      </View>
                      <FlatList
                        data={dataImg.url}
                        contentContainerStyle={{ paddingBottom: 12 }}
                        horizontal={true}
                        keyExtractor={(item, index) => `${index}`}
                        renderItem={({ item, index }) => (
                          <View>
                            <TouchableOpacity
                              style={{ marginTop: 10 }}
                              onPress={() => handleDelete(i, indexChild, index)}
                            >
                              <MaterialCommunityIcons name={"delete-circle"} size={25} style={styles.itemDelete} />
                            </TouchableOpacity>
                            <TouchableOpacity
                              style={styles.btnShowImage}
                              onPress={() => handleShowModalImage(item)}
                              onLongPress={localDataArray.parentRowID == null ? onSetMainPhoto(item) : () => {}}
                            >
                              <Image
                                source={{ uri: item.uri }}
                                style={[styles.photo, item.name === mainPhoto?.name && styles.activePhoto]}
                                resizeMode="cover"
                              />
                            </TouchableOpacity>
                          </View>
                        )}
                      />
                    </View>
                  </View>
                )
              })}
            </View>
          )
        })}
        <View style={styles.rowWrap}>
          {item.itemTypeOptionLines.map((data: any, i?: any) => {
            return (
              <TouchableOpacity
                key={i}
                onPress={() => handlePressCheckQuatity(i, data)}
                style={[styles.btnCheckQuantity, { backgroundColor: arrCheckQuatity.includes(i) ? "gray" : "white" }]}
              >
                <Text style={styles.txtOptionLine}>{data.itemTypeOptionLineCode}</Text>
              </TouchableOpacity>
            )
          })}
        </View>
      </View>
    </View>
  )
}
export default ImageApply
