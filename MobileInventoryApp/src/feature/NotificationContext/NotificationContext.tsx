/* eslint-disable react-native/no-inline-styles */
import React, { ReactElement, useState } from "react"
import { Dimensions, TouchableOpacity, View } from "react-native"
import Modal from "react-native-modalbox"
import Icon from "react-native-vector-icons/FontAwesome"

import { log } from "/utils"
import { getScreenHeight, getScreenWidth } from "/utils/dimension"

import { ContextModal } from "./Context"

interface Props {
  children: ReactElement
}

const NotificationContext = (props: Props) => {
  const [popup, setPopup] = useState<boolean>(false)

  const [data, setData] = useState<any>()

  const renderItem = () => {
    return (
      <Modal
        position="center"
        style={{ height: getScreenHeight(1) * 0.8, width: getScreenWidth(1) * 0.9 }}
        onClosed={() => setPopup(false)}
        isOpen={popup}
        swipeToClose={false}
        coverScreen={true}
      >
        <TouchableOpacity
          style={{
            position: "absolute",
            zIndex: 2,
            right: -10,
            top: -10,
            backgroundColor: "red",
            width: 24,
            height: 24,
            borderRadius: 24 / 2,
            justifyContent: "center",
            alignItems: "center",
            borderWidth: 0.5,
            borderColor: "red",
          }}
          onPress={() => setPopup(false)}
        >
          <Icon name="times" size={15} color="#FFF" />
        </TouchableOpacity>
        <View
          style={{
            flex: 1,
            width: Dimensions.get("window").width * 0.9,
            height: Dimensions.get("window").height * 0.8,
          }}
        ></View>
      </Modal>
    )
  }

  return (
    <ContextModal.Provider
      value={{
        setShowPopup: (value) => setPopup(value),
        setData: (value) => setData(value),
        showPopup: popup,
      }}
    >
      {renderItem()}
      {props.children}
    </ContextModal.Provider>
  )
}

export default NotificationContext
