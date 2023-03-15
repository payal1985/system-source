import { StyleProp, ViewStyle } from "react-native"
import { ModalProps } from "react-native-modalbox";

export interface Props extends ModalProps{
  size?: "small" | "large"
  color?: string
  containerStyle?: StyleProp<ViewStyle>
  wrapLoading?: {}
}
