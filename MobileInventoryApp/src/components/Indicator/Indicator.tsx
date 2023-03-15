import React from "react"
import { ActivityIndicator, View } from "react-native"
import Modal from "react-native-modalbox"

import Colors from "../../configs/Colors"
import styles from "./styles"
import { Props } from "./types"

const Indicator = (props: Props) => {
  const { ...rest } = props

  return (
    <Modal
      {...rest}
      coverScreen={true}
      style={styles.container}
      backdropPressToClose={false}
      swipeToClose={false}
      backButtonClose={false}
    >
      <View style={[styles.dialog, props.containerStyle]}>
        <View style={styles.wrapDialog}>
          <View style={[styles.wrapLoading, props.wrapLoading]}>
            <ActivityIndicator size={props.size || "small"} color={props.color || Colors.white} />
          </View>
        </View>
      </View>
    </Modal>
  )
}

export default Indicator
