import { StyleSheet } from "react-native"

import { Colors } from "/configs"

import { fontSizer, getScreenHeight, getScreenWidth, responsiveH, responsiveW } from "../../utils/dimension"

const WIDTH = getScreenWidth(1)

const HEIGHT = getScreenHeight(1)

export const ITEM_HEIGHT = responsiveH(20) + fontSizer(19)

export default StyleSheet.create({
  containerStyle: {
    width: WIDTH * 0.28,
    backgroundColor: "#ED1C24",
    marginTop: 2,
  },
  triggerStyle: {
    backgroundColor: Colors.transparent,
    paddingHorizontal: responsiveW(5),
    paddingVertical: responsiveW(4),
    minWidth: WIDTH * 0.28,
  },
  menuStyle: {
    backgroundColor: "#F1F1F1",
    width: WIDTH * 0.28,
  },
  labelStyle: {
    color: "#FFFFFF",
    textAlign: "left",
    fontSize: 14,
    fontWeight: "bold",
    marginLeft: (WIDTH * 0.28) / 6,
  },
})
