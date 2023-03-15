import React from "react"
import { Text, View } from "react-native"

import { fontSizer } from "../../utils/dimension"
import ModalBox from "react-native-modalbox"
import { BaseButton } from "/components"
import { BaseModalProps } from "./types"

const BaseModal = ({ open, onClosed, flex = 0.5 }: BaseModalProps) => {
  return (
    <View style={{ flex: 1 }}>
      <ModalBox
        isOpen={open}
        entry={"bottom"}
        position={"bottom"}
        swipeToClose={false}
        onClosed={onClosed}
        style={{
          backgroundColor: "#fff",
          borderTopLeftRadius: 30,
          borderTopRightRadius: 30,
          flex: flex,
        }}
      >
        <View style={{ flex: 3, justifyContent: "center", alignItems: "center" }}></View>
        <View style={{ flex: 0.6, justifyContent: "center", alignItems: "center" }}>
          <Text style={{ fontSize: fontSizer(17), fontWeight: "bold" }}>{"Alert"}</Text>
        </View>
        <View style={{ flex: 4, marginHorizontal: 20 }}>
          <Text style={{ fontSize: fontSizer(17), marginTop: 20 }}>{"Support"}</Text>
        </View>
        <View style={{ flex: 2, justifyContent: "center", alignItems: "center" }}>
          <BaseButton label={"OK"} onPress={onClosed} width={"90%"} />
        </View>
      </ModalBox>
    </View>
  )
}

export default BaseModal
