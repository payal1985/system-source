import { StyleSheet } from "react-native"
import { getScreenWidth } from "/utils/dimension"

export default StyleSheet.create({
  modal: {
    width: getScreenWidth(1) * 0.9,
    borderRadius: 5,
  },
})
