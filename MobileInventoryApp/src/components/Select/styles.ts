import { StyleSheet } from "react-native"

import { Colors } from "/configs"
import { getScreenHeight, getScreenWidth, responsiveH, responsiveW } from "/utils/dimension"

import { fontSizer } from "../../utils/dimension"

export const MAX_HEIGHT = getScreenHeight(0.3)
const WIDTH = getScreenWidth(1)

export default StyleSheet.create({
  container: {},
  optionsContainer: {
    width: WIDTH * 0.3,
    height: MAX_HEIGHT,
  },
  optionWrapper: {
    paddingHorizontal: responsiveH(10),
    paddingVertical: 0,
  },
  triggerOuterWrapper: {
    alignSelf: "baseline",
  },
  triggerWrapper: {
    flexDirection: "row",
    alignItems: "center",
    backgroundColor: Colors.opacityBlack,
    width: WIDTH * 0.25,
  },
  label: {
    fontSize: fontSizer(14),
    fontWeight: "300",
    color: Colors.textColor,
    flex: 1,
  },
  sub: {
    fontSize: fontSizer(12),
    fontWeight: "300",
    color: Colors.textColor,
    flex: 1,
  },
  sub1: {
    fontSize: fontSizer(12),
    fontWeight: "500",
    color: Colors.textColor,
    flex: 1,
  },
  options: {
    textAlign: "left",
    color: "#ED1C24",
    fontSize: fontSizer(12),
    paddingVertical: responsiveH(6),
  },
  labelContainer: {
    // flexDirection: "row",
    borderBottomColor: Colors.gray_C4,
    borderBottomWidth: 1,
    paddingVertical: responsiveH(10),
  },
})
