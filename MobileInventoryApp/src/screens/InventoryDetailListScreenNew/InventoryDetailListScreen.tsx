import React from "react"
import { FlatList, Text, TouchableOpacity, View } from "react-native"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import IconCheck from "react-native-vector-icons/Feather"
import Icon from "react-native-vector-icons/Ionicons"
import { BottomButtonNext, Header } from "../../components"
import styles from "./styles"
import _ from "lodash"
import InventoryDetailListScreenIndex from "./index"

type RenderItemProps = {
  item?: any
  index?: any
}

export const InventoryDetailListScreenNew = () => {
  const { initialElements, globalData, conditionDisable, deleteRow, handleCheckItem, handlePressItem, handleOnPress } =
    InventoryDetailListScreenIndex()

  const renderItem = ({ item, index }: RenderItemProps) => {
    const isHaveParent = item?.parentRowID !== null
    const renderColor = item?.conditionRender === 3 ? "yellow" : "green"
    return (
      <View key={index} style={styles.containerItem}>
        <TouchableOpacity onPress={() => handleCheckItem(item, index)} style={styles.containerTouch}>
          <Text style={styles.title}>{`${index + 1} - ${item.itemName}`}</Text>
        </TouchableOpacity>
        {isHaveParent && <View style={[styles.haveParent, { backgroundColor: renderColor }]} />}
        <TouchableOpacity
          disabled={isHaveParent}
          onPress={() => deleteRow(index, item)}
          style={{ flex: 2, alignItems: "flex-end", marginRight: 20 }}
        >
          <IconCheck name={"trash-2"} size={20} />
        </TouchableOpacity>
      </View>
    )
  }

  return (
    <View style={styles.container}>
      <Header isGoBack labels={"Inventory Collection - ..."} />
      {console.log("data list screen", globalData)}
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
            <Text style={styles.txtButton}>Add New Row</Text>
          </TouchableOpacity>
          <View style={{ flex: 2 }} />
        </View>
      </KeyboardAwareScrollView>
      <BottomButtonNext disabled={conditionDisable()} onPressButton={() => handleOnPress()} />
    </View>
  )
}
