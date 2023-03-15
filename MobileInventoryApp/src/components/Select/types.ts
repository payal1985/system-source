import { TextStyle, ViewStyle } from "react-native"

export interface SelectOption {
  id?: number
  name?: string
  sub?: string
  sub1?: string
  code?: string
  isChangeMaturityDate?: boolean
  isPreSale?: boolean
  isLocked?:boolean
  accountNo?: string
  price?: string
  remainTime?: string
  maturityDate?:string
  minSellDate?:string
}

export interface Props {
  name?: string
  value?: string
  containerStyle?: ViewStyle
  menuStyle?: ViewStyle
  triggerStyle?: ViewStyle
  labelStyle?: TextStyle
  textMenuStyle?: TextStyle
  options: Array<SelectOption>
  onSelect: (value: SelectOption) => void
  label?: string
  disabled?: boolean
  width?: number
  leftIcon?: Element
  hideLeft?: boolean
  hideRight?: boolean
  hideImageLeft?: boolean
  iconName?: string
  colorLeft?: string
  colorRight?: string
  action?: boolean
  labelAction?: string
  onSelectAction?: () => void
  iconRight?: string
  showSub?: boolean
  showSub1?: boolean
  onOpen?: () => void
}

export interface SelectState {
  currentValue?: string
  error?: string
  valueSelect?: string
}
