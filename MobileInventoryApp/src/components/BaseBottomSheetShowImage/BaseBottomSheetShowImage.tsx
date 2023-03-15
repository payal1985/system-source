import React from "react"
import { Image, TouchableOpacity, Text, View } from "react-native"
import Icon from "react-native-vector-icons/Feather"
import { fontSizer, responsiveW } from "../../utils/dimension"
import ModalBox from "react-native-modalbox"
import { BaseBottomSheetShowImageProps } from "./types"
import CustomCheckbox from "../CustomCheckbox"
import styles from "./styles"

const BaseBottomSheetShowImage = ({
  open,
  onClosed,
  flex = 0.5,
  options,
  isShowChooseMainPhoto,
  onValueChange,
  value,
}: BaseBottomSheetShowImageProps) => {
  return (
    <ModalBox
      isOpen={open}
      entry={"bottom"}
      position={"bottom"}
      swipeToClose={false}
      onClosed={onClosed}
      style={{
        backgroundColor: "#fff",
        flex: flex,
        borderTopLeftRadius: 30,
        borderTopRightRadius: 30,
      }}
    >
      <View style={{ flex: 1 }}>
        <View
          style={{ borderBottomWidth: 1, borderBottomColor: "#efefef", padding: responsiveW(20), flexDirection: "row" }}
        >
          <View style={{ flex: 1 }}></View>
          <View style={{ flex: 8, justifyContent: "center", alignItems: "center" }}>
            <Text style={{ fontSize: fontSizer(16) }}>
              <Text style={{ fontSize: fontSizer(16) }}>{options.name}</Text>
            </Text>
          </View>

          <TouchableOpacity onPress={onClosed} style={{ flex: 1, justifyContent: "center", alignItems: "center" }}>
            <Icon name={"x"} size={20} />
          </TouchableOpacity>
        </View>

        {isShowChooseMainPhoto && (
          <CustomCheckbox
            style={styles.wrapCheckbox}
            iOSCheckBoxStyles={styles.iOSCheckBoxStyles}
            onValueChange={onValueChange}
            value={value}

          >
            <Text>Set main photo</Text>
          </CustomCheckbox>
        )}

        <View style={{ flex: 8, paddingHorizontal: responsiveW(20), marginVertical: 20 }}>
          <Image resizeMode={"contain"} style={{ width: "100%", height: "100%" }} source={{ uri: options.uri }}></Image>
        </View>
      </View>
    </ModalBox>
  )
}

export default BaseBottomSheetShowImage
