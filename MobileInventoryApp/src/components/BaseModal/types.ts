import { StyleProp, TextStyle, ViewStyle } from "react-native"

export interface Props {
  label?: string
  value?: string
  onChangeText?: (text: string) => void
  showValidateText?: boolean
  validateText?: string
  showSuccess?: boolean
  disable?: boolean
  secureTextEntry?: boolean
  onBlur?: () => void

  textStyle?: StyleProp<TextStyle>
  textLabelStyle?: StyleProp<TextStyle>
  inputWrapperStyle?: StyleProp<ViewStyle>
  containerStyle?: StyleProp<ViewStyle>
  keyboardType?:
    | "default"
    | "email-address"
    | "numeric"
    | "phone-pad"
    | "number-pad"
    | "decimal-pad"
    | "visible-password"
    | "ascii-capable"
    | "numbers-and-punctuation"
    | "url"
    | "name-phone-pad"
    | "twitter"
    | "web-search"
    | undefined
  icInfoRight?: boolean
  onIcInfoRightPress?: () => void
  icInfoLeft?: boolean
  onIcInfoLeftPress?: () => void

  placeHolder?: string
  ref?: any
  onTouchStart?: any
  textInputCustom?: Element
  maxLength?: number
}

export type BaseModalProps = {
  open?: boolean
  onClosed: () => void
  flex?: number
}
