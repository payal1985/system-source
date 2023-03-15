import { ReactNode } from "react"
import { StyleProp, TextProps as Props, TextStyle } from "react-native"

import Typography from "../../configs/Typography"

export default interface TextProps extends Props {
  /**
   * An optional style override
   */
  style: StyleProp<TextStyle>

  /**
   * The color to apply
   */
  color?: string

  /**
   * The typography
   */
  typography: keyof typeof Typography

  children?: ReactNode

  multiLang?: boolean
  text?: string
}
