import { ColorValue, KeyboardType, NativeSyntheticEvent, StyleProp, TextInputChangeEventData, TextStyle, ViewStyle } from "react-native"

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


export type BaseTextInPutProps = {
  label?: string
  value?: string
  textColor?: ColorValue | undefined
  baseColor?: ColorValue | undefined
  keyboardType?: KeyboardType
  onChangeText?: (value: string) => void
  secureTextEntry?: boolean
  width?: string
  onFocus?: () => void
  maxLength?: number
  placeholder?: any
  onBlur?: (event: NativeSyntheticEvent<TextInputChangeEventData>) => void
  validateText?: string
  disable?: boolean
  colorText?: string
  disableText?: boolean
  canRenderRight?: boolean
  nameIcon: string
  colorIcon: string
  iconSize: number
  fontSize?: number
}
