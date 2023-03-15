import React from "react"
import Modal from "react-native-modalbox"

import styles from "./styles"
import { Props } from "./types"
import { log } from "/utils"

const ModalFullScreen = (props: Props) => {
  return (
    <Modal
      ref={props.modalRef}
      backButtonClose={true}
      style={[styles.modal, props.containerStyle]}
      position={"center"}
      backdropPressToClose
      {...props}
      coverScreen={props.coverScreen !== undefined ? props.coverScreen : true}
    >
      {props.element}
    </Modal>
  )
}

export default ModalFullScreen
