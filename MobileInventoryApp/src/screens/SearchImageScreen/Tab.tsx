import React, { useCallback, useEffect, useState } from "react"
import { FlatList, Image, Text, TouchableOpacity, View } from "react-native"
import styles from "./styles"
import CustomCheckbox from "/components/CustomCheckbox"
import FImage from "/components/FImage"
import { Colors } from "/configs"
import { SERVER_NAME, IMAGE_TYPE, URL_BASE } from "/constant/Constant"

type TabProps = {
  data?: any[]
  type?: string
  activeImage?: string
  onSelectImage?: (value: string, index: number) => () => void
  onRefresh?: () => void
  onLoadMore?: () => void
  indexChoseImage: any
  indexChoseImageLocal: any
  clientId?: any
}

let onEndReachedCalledDuringMomentum = true

const Tab = ({
  data,
  type,
  activeImage,
  onSelectImage,
  onRefresh,
  onLoadMore,
  indexChoseImage,
  indexChoseImageLocal,
  clientId,
}: TabProps) => {
  const dataListImage = data

  const renderItem = useCallback(
    ({ item, index }: any) => {
      const uri = type === IMAGE_TYPE.LOCAL ? item.mainImageFull.uri : `${SERVER_NAME}/${clientId}/${item?.mainImage}`
      const selectedImage: string = type === IMAGE_TYPE.LOCAL ? uri : item?.mainImage
      const selectedIndex: number = type === IMAGE_TYPE.LOCAL ? indexChoseImageLocal : indexChoseImage
      return (
        <TouchableOpacity key={index} onPressIn={onSelectImage && onSelectImage(selectedImage, index)}>
          <Text style={styles.inventoryID}>{item?.inventoryId}</Text>
          <FImage source={{ uri }} style={styles.wrapImg} />
          {selectedIndex === index && <CustomCheckbox style={styles.checkBox} value={true} disabled />}
        </TouchableOpacity>
      )
    },
    [indexChoseImage],
  )
  return (
    <FlatList
      keyExtractor={(item, index) => `${item?.mainImage}${index}`}
      data={dataListImage}
      renderItem={renderItem}
      numColumns={3}
      onRefresh={onRefresh}
      refreshing={false}
      onEndReachedThreshold={0.1}
      onEndReached={() => {
        if (!onEndReachedCalledDuringMomentum) {
          onLoadMore && onLoadMore()
          onEndReachedCalledDuringMomentum = true
        }
      }}
      onMomentumScrollBegin={() => {
        onEndReachedCalledDuringMomentum = false
      }}
      contentContainerStyle={{ paddingHorizontal: 16 }}
    />
  )
}
export default Tab
