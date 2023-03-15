import React, { ReactNode } from "react"
import { Keyboard, TouchableWithoutFeedback, View } from "react-native"

type Props = { children: ReactNode; onPress: () => void }

const DismissKeyboardHOC = ({ children, onPress }: Props) => {
  const _dismissSearch = () => {
    onPress ? onPress() : Keyboard.dismiss()
  }

  return (
    <TouchableWithoutFeedback accessible={false} onPress={_dismissSearch}>
      <View>{children}</View>
    </TouchableWithoutFeedback>
  )
}

export default DismissKeyboardHOC
