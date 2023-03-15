import { StyleSheet } from "react-native"
import Colors from "/configs/Colors"
import { fontSizer, responsiveW } from "/utils/dimension"
import { responsiveH } from "../../utils/dimension"

export default StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#f5f5f5",
  },
  keyboardAwreScrollView: {
    flex: 8,
    paddingTop: responsiveW(5),
  },
  txtTitle: {
    flex: 1,
    fontSize: 20,
    alignItems: "center",
    textAlign: "center",
    marginVertical: 20,
  },
  txtKey: {
    fontSize: 14,
    marginBottom: 5,
  },
  txtValue: {
    fontSize: 14,
    marginBottom: 8,
  },
  optionsChose: {
    marginTop: 20,
    backgroundColor: "#fff",
    paddingHorizontal: 10,
  },
  boxEmailSub: {
    borderBottomWidth: 0.5,
    borderBottomColor: "#b4b4b4",
    marginVertical: 10,
  },
  emailSub: {
    fontSize: 16,
    marginBottom: 5,
    color: Colors.button,
  },
  none: {
    fontSize: 14,
  },
  reviewSub: {
    borderBottomWidth: 0.5,
    borderBottomColor: "#b4b4b4",
  },
  txtReviewSub: {
    fontSize: 16,
    marginVertical: 10,
    color: Colors.button,
  },
  containerChose: {
    flex: 1,
    marginTop: 10,
    marginHorizontal: 15,
  },
  txtNameScreen: {
    fontSize: 18,
    fontWeight: "700",
  },
  txtModal: {
    borderBottomWidth: 1,
    borderBottomColor: "#efefef",
    padding: responsiveW(10),
    flexDirection: "row",
  },
  scrollView: { flex: 8, padding: responsiveW(20), paddingTop: responsiveW(5) },
  img: {
    width: responsiveW(27),
    height: responsiveW(18),
  },
  viewInput: {
    marginTop: 5,
    alignItems: "center",
    justifyContent: "space-between",
    flexDirection: "row",
    backgroundColor: "#FFFFFF",
    paddingHorizontal: responsiveW(18),
    borderRadius: 10,
    borderWidth: 1,
    borderColor: "#D6D6D6",
  },
  modalStyle: {
    backgroundColor: "#fff",
    borderTopLeftRadius: 30,
    borderTopRightRadius: 30,
    flex: 0.9,
  },
  label: {
    fontSize: fontSizer(13),
    color: Colors.textGray,
    marginTop: responsiveH(5),
  },
  txtInput: {
    paddingVertical: responsiveH(15),
    fontSize: fontSizer(16),
    color: "#383838",
    flex: 1,
    marginRight: responsiveW(10),
  },
  btnCamera: {
    alignItems: "center",
    justifyContent: "space-between",
    flexDirection: "row",
    backgroundColor: "#FFFFFF",
    paddingHorizontal: responsiveW(18),
    borderRadius: 10,
    borderWidth: 1,
    borderColor: "#D6D6D6",
    flex: 3,
  },
  // txtCamera: {
  //   paddingVertical: responsiveH(15),
  //   fontSize: fontSizer(16),
  //   flex: 1,
  //   marginRight: responsiveW(10),
  // },
  boxSubmiss: {
    borderBottomWidth: 0.5,
    marginTop: 20,
    borderBottomColor: "gray",
  },
  content: {
    paddingHorizontal: 10,
    backgroundColor: "#fff",
    marginTop: 50,
  },
  boxContent: {
    flexDirection: "row",
    borderBottomWidth: 0.5,
    borderBottomColor: "#b4b4b4",
    paddingBottom: 10,
  },
  boxSwitch: {
    marginTop: 20,
    flex: 1,
    flexDirection: "row",
    alignItems: "center",
  },
  boxTxt: {
    fontSize: 15,
    flex: 4,
  },
  btnIconX: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },
  viewReviewSub: {
    flex: 8,
    justifyContent: "center",
    alignItems: "center",
  },
  barCode: {
    fontSize: 14,
    marginBottom: 8,
    color: Colors.button,
  },
  inventoryCollection: {
    fontSize: 18,
    marginTop: 20,
    fontWeight: "700",
  },
  viewBarCode: {
    borderBottomWidth: 0.5,
    marginTop: 20,
    borderBottomColor: "gray",
  },
})
