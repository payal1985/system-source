import React from "react"
import { StyleSheet, Text as RNText } from "react-native"

import Typography from "../../configs/Typography"
import styles from "./styles"
import TextProps from "./TextProps"

const Text = (props: TextProps) => {
  const { multiLang, children, style: styleOverride, color, typography: typographyKey, ...rest } = props

  const style = StyleSheet.flatten([styles.text, { color, fontFamily: Typography[typographyKey] }, styleOverride])

  if ((!children || children === "") && !multiLang) {
    return null
  }

  return (
    <RNText style={style} {...rest}>
      {children}
    </RNText>
  )
}

Text.defaultProps = {
  typography: "normal",
  style: {},
}

export default Text
