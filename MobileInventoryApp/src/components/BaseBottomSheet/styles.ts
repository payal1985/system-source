import { StyleSheet } from "react-native"
import { getScreenHeight, responsiveW, getScreenWidth } from "../../utils/dimension"

export default StyleSheet.create({
  ctModal: {
    backgroundColor: "#fff",
    flex: 0.3,
    borderTopLeftRadius: 20,
    borderTopRightRadius: 20,
  },
  ctInput: {
    borderBottomWidth: 1,
    marginHorizontal: 25,
    flexDirection: "row",
  },
  modalFullScreen: {
    alignSelf: "center",
    justifyContent: "center",
    backgroundColor: "transparent",
  },
  containerAddNew: {
    backgroundColor: "white",
    marginHorizontal: responsiveW(20),
    padding: responsiveW(16),
    overflow: "hidden",
  },
  rowBtn: {
    flexDirection: "row",
    justifyContent: "center",
    marginTop: responsiveW(16),
  },
})
