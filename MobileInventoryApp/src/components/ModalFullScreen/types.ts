import { ReactElement } from "react"
import { StyleProp } from "react-native"
import { ModalProps } from "react-native-modalbox"

export interface Props extends ModalProps {
  element?: Element
  containerStyle?: StyleProp<object>
  children?: ReactElement
  coverScreen?: boolean
  modalRef?: any
}

export interface State {}
