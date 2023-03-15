import { StyleSheet } from "react-native"

import Colors from "/configs/Colors"
import { getScreenWidth, responsiveH , height, width} from "/utils/dimension"

const WIDTH = getScreenWidth(1)

export default StyleSheet.create({
  container: {
    height: height,
    width: width,
    backgroundColor: "rgba(0,0,0,0.0)",
  },
  wrapDialog: {
    backgroundColor: Colors.transparent,
    width: WIDTH * 0.9,
    alignItems: "center",
    borderRadius: 20,
  },
  dialog: {
    backgroundColor: Colors.opacityBlack,
    position: "absolute",
    alignItems: "center",
    justifyContent: "center",
    top: 0,
    bottom: 0,
    right: 0,
    left: 0,
    zIndex: 99,
  },
  wrapLoading: {
    backgroundColor: Colors.opacityBlack,
    padding: responsiveH(30),
    borderRadius: responsiveH(20),
  },
})
