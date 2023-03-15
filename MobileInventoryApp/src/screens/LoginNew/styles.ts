import { StyleSheet } from "react-native"
import Colors from "/configs/Colors"
import { fontSizer, getScreenHeight, getScreenWidth, isIPhoneX, responsiveH, responsiveW } from "../../utils/dimension"

export default StyleSheet.create({
  container: {
    width: getScreenWidth(1),
    height: getScreenHeight(1),
    backgroundColor: "#fff",
  },
  topView: {
    marginTop: responsiveH(22),
    alignItems: "center",
    paddingVertical: responsiveH(10),
    paddingHorizontal: responsiveW(34),
  },
  logoView: {
    marginTop: isIPhoneX ? responsiveH(47) : responsiveH(47),
    marginBottom: responsiveH(11),
    flexDirection: "row",
    alignItems: "center",
  },
  mainView: {
    marginTop: isIPhoneX ? responsiveH(32) : responsiveH(50),
    alignItems: "center",
    justifyContent: "space-between",
    flexDirection: "row",
    backgroundColor: "#FFFFFF",
    paddingHorizontal: responsiveW(18),
    borderRadius: 50,
    borderWidth: 1,
    borderColor: "#D6D6D6",
  },
  viewPass: {
    marginTop: responsiveH(25),
    alignItems: "center",
    justifyContent: "space-between",
    flexDirection: "row",
    backgroundColor: "#FFFFFF",
    paddingHorizontal: responsiveW(18),
    borderRadius: 50,
    borderWidth: 1,
    borderColor: "#D6D6D6",
  },
  txtInput: {
    paddingVertical: responsiveH(15),
    fontSize: fontSizer(16),
    color: "#383838",
    flex: 1,
    marginRight: responsiveW(10),
  },
  txtPass: {
    paddingVertical: responsiveH(15),
    fontSize: fontSizer(16),
    color: "#383838",
    flex: 1,
    marginRight: responsiveW(10),
  },
  viewBtn: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
    marginTop: responsiveH(25),
  },
  txtCheck: {
    color: "#383838",
    flex: 1,
    fontWeight: "400",
    fontSize: fontSizer(18),
  },
  btnLogin: {
    backgroundColor: Colors.faceBook,
    width: responsiveW(60),
    height: responsiveW(60),
    borderRadius: 30,
    justifyContent: "center",
    alignItems: "center",
  },
  imgLogin: {
    width: responsiveW(28),
    height: responsiveH(18),
    transform: [{ rotate: "180deg" }],
  },
  btnSavePass: {
    marginTop: responsiveH(27),
    fontWeight: "400",
    fontSize: fontSizer(18),
  },
  logoLogin: {
    height: 100,
    width: 300,
    marginBottom: 20,
  },
})
