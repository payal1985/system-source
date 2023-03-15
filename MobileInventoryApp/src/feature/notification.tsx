import React, { ReactElement } from "react"
import { View } from "react-native"

export interface Props {
  children: ReactElement
}

const Notification = (props: Props) => {
  return <View style={{ flex: 1 }}> </View>
}

export default Notification
