import { StyleSheet, Dimensions } from "react-native"

import Colors from "/configs/Colors"
import { fontSizer, responsiveW } from "/utils/dimension"
import { responsiveH } from "../../utils/dimension"

const { width } = Dimensions.get("window")

export default StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#f5f5f5",
  },
  subContainer: {
    flex: 8,
    backgroundColor: "#f1f0f6",
  },
  subTitle: {
    flex: 1,
    paddingVertical: 20,
    alignItems: "center",
    justifyContent: "center",
    backgroundColor: "#f1f0f6",
  },
  txtTitle: {
    fontSize: 20,
    color: "#000",
    fontWeight: "bold",
  },
  button: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    flexDirection: "row",
    backgroundColor: "#fff",
    paddingVertical: 5,
  },
  item: {
    backgroundColor: "#fff",
    paddingVertical: 10,
    flexDirection: "row",
    alignItems: "center",
    marginBottom: 2,
  },
  title: {
    fontSize: 16,
    flex: 4,
  },
  btnContainer: {
    alignItems: "center",
    justifyContent: "space-between",
    flexDirection: "row",
    backgroundColor: Colors.button,
    paddingHorizontal: responsiveW(20),
    flex: 5,
    margin: 5,
    borderRadius: 30,
  },
  txtButton: {
    paddingVertical: responsiveH(10),
    fontSize: fontSizer(16),
    flex: 1,
    marginLeft: responsiveW(10),
    color: "#fff",
  },
  containerItem: {
    backgroundColor: "#fff",
    paddingVertical: 10,
    flexDirection: "row",
    alignItems: "center",
    marginBottom: 2,
    paddingHorizontal: 16,
  },
  note: {
    fontWeight: "bold",
    fontSize: 18,
    marginBottom: 12,
  },
  mainModal: {
    backgroundColor: "#fff",
    borderTopLeftRadius: 30,
    borderTopRightRadius: 30,
    flex: 0.8,
    paddingHorizontal: 16,
  },
  choseTitleContainer: {
    flexDirection: "row",
  },
  subChoseTitleContainer: {
    marginTop: 20,
    flex: 1,
    flexDirection: "row",
    alignItems: "center",
  },
  txtTitleChose: {
    fontSize: 15,
    flex: 4,
  },
  btnDefault: {
    alignItems: "center",
    justifyContent: "space-between",
    flexDirection: "row",
    backgroundColor: Colors.lightGray,
    paddingHorizontal: responsiveW(18),
    borderRadius: 10,
    borderWidth: 1,
    borderColor: "#D6D6D6",
    flex: 3,
  },
  txtSize16: {
    paddingVertical: responsiveH(15),
    fontSize: fontSizer(16),
    flex: 1,
    marginRight: responsiveW(10),
  },
  containerBtnNext: {
    flex: 1,
    justifyContent: "flex-end",
    alignItems: "center",
    position: "absolute",
    bottom: 0,
  },
  titleModalBox: {
    fontSize: 16,
    marginTop: 16,
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
  contentModalBoxRow: {
    borderBottomWidth: 1,
    borderColor: Colors.gray_C4,
    flexDirection: "row",
  },
  textInput: {
    height: 40,
    flex: 1,
  },
  txtListSearch: {
    fontSize: 16,
  },
  containerSeparator: {
    height: 2,
    width,
    backgroundColor: Colors.borderColor,
    marginVertical: 12,
  },
  containerRow: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
  },
  containerItemRow: {
    flexDirection: "row",
    alignItems: "center",
  },
  img: {
    width: 64,
    height: 64,
    marginRight: 12,
    borderRadius: 4,
    backgroundColor: "pink",
  },
  containerHeader: {
    marginVertical: 8,
    flexDirection: "row",
    alignItems: "center",
  },
  txtHeader: {
    fontSize: 16,
    fontWeight: "bold",
    marginRight: 8,
  },
  doneBtn: {
    backgroundColor: Colors.redHeader,
    paddingHorizontal: 12,
    paddingVertical: 4,
    borderRadius: 12,
  },
  txtBtn: {
    color: Colors.white,
    fontSize: 16,
  },
  removeBtn: {
    backgroundColor: Colors.redHeader,
    paddingHorizontal: 12,
    paddingVertical: 4,
    borderRadius: 12,
    alignSelf: "flex-end",
    marginBottom: 16,
    marginRight: 16,
    alignItems: "center",
  },
  rowDetail: {
    flexDirection: "row",
    alignItems: "center",
    marginTop: 16,
  },
  singleItem: {
    marginRight: 16,
    fontWeight: "bold",
  },
  modalFullScreenWhite: {
    backgroundColor: "#fff",
    borderTopLeftRadius: 30,
    borderTopRightRadius: 30,
    flex: 0.9,
  },
  containerModal: {
    padding: 16,
  },
  containerViewModal: {
    backgroundColor: "#fff",
    alignItems: "center",
    marginHorizontal: 32,
    padding: 16,
    borderRadius: 12,
  },
})
