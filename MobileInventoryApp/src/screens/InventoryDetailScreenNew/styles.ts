import { StyleSheet } from "react-native"
import Colors from "/configs/Colors"
import { fontSizer, responsiveH, responsiveW } from "../../utils/dimension"

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#f5f5f5",
  },
  subContainer: {
    flex: 8,
    padding: responsiveW(20),
    paddingTop: responsiveW(5),
  },
  choseTitleContainer: {
    flexDirection: "row",
    marginTop: 20,
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
    backgroundColor: "#FFFFFF",
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
  txtTitleOps: {
    fontSize: 20,
    marginVertical: 5,
    fontWeight: "bold",
  },

  textFilter: {
    paddingVertical: 5,
    paddingHorizontal: 15,
    borderWidth: 0.4,
    borderRadius: 15,
    marginRight: 12,
  },
  btnCustom: {
    alignItems: "center",
    backgroundColor: "#FFFFFF",
    borderRadius: 30,
    borderWidth: 1,
    borderColor: "#D6D6D6",
    flex: 1,
    textAlign: "center",
    marginHorizontal: 5,
  },
  txtCustom: {
    paddingVertical: responsiveH(5),
    fontSize: fontSizer(16),
    flex: 1,
    marginRight: responsiveW(10),
  },
  activePhoto: {
    borderWidth: 2.5,
    borderColor: Colors.red,
  },
  photo: {
    flex: 1,
    borderRadius: 10,
  },
  buttonSearchImg: {
    borderColor: "#D6D6D6",
    height: 40,
    width: 170,
    alignItems: "center",
    flexDirection: "row",
  },
  circlePressable: {
    marginLeft: 8,
    marginBottom: 10,
  },
  imgSupportFile: {
    width: 200,
    height: 200,
    borderRadius: 10,
    backgroundColor: "white",
  },
  containerViewModal: {
    flex: 1,
    backgroundColor: "#0009",
    alignItems: "center",
    justifyContent: "center",
  },
  rowGeneral: {
    flexDirection: "row",
    alignItems: "center",
  },
  containerItemValue: {
    marginTop: 20,
    flex: 1,
  },
  redRequire: {
    fontSize: 15,
    marginBottom: 10,
  },
  quantityTxt: {
    fontSize: 15,
    flex: 4,
    marginBottom: 10,
    color: "blue",
  },
  imgName: {
    fontSize: 15,
    marginBottom: 10,
  },
  btnDelete: {
    justifyContent: "center",
    alignItems: "center",
    marginLeft: 20,
    marginBottom: 10,
  },
  itemDelete: {
    position: "absolute",
    right: 5,
    top: -5,
  },
  btnShowImage: {
    width: 80,
    height: 80,
    marginTop: 10,
    marginHorizontal: 15,
  },
  rowWrap: {
    flexDirection: "row",
    flexWrap: "wrap",
    marginTop: 20,
  },
  btnCheckQuantity: {
    borderWidth: 0.4,
    marginVertical: 5,
    borderColor: "gray",
    borderRadius: 18,
    marginHorizontal: 5,
  },
  txtOptionLine: {
    padding: 10,
    paddingHorizontal: 15,
  },
  txtOptionName: {
    fontSize: 15,
    flex: 4,
    marginBottom: 10,
  },
})
export default styles
