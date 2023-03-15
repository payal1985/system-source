import { StyleSheet } from "react-native"
import {  getScreenWidth, responsiveW } from "/utils/dimension"

export const WIDTH = getScreenWidth(1)

export default StyleSheet.create({
  img: {
    width: responsiveW(32),
    height: responsiveW(28),
    tintColor: "#333",
  },
  label: { 
    fontSize: 12, 
    marginTop: 5, 
  },
  labelFocus: {
    color: 'green'
  }
})
