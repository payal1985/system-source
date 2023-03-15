import { StyleSheet } from "react-native"
import Colors from "/configs/Colors"
import { fontSizer, responsiveW } from "/utils/dimension"
import { responsiveH } from "../../utils/dimension"

export default StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#f1f0f6",
  },
  txtProgress: {
    fontSize: fontSizer(16),
    color: "#000",
    marginHorizontal: 15,
    marginVertical: 5,
    fontWeight: "400",
  },
  txtStatus: {
    fontSize: fontSizer(14),
    color: "#fff",
  },
  scrollView: {
    flex: 8,
    padding: responsiveW(20),
    paddingTop: responsiveW(5),
  },
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
  item: {
    backgroundColor: "#fff",
    paddingTop: 10,
    paddingBottom: 15,
    flexDirection: "row",
    // alignItems: "center",
    marginVertical: 10,
    marginHorizontal: 10,
    borderRadius: 5,
    flex: 1,
    shadowColor: "#00000070",
    shadowOffset: {
      width: 0,
      height: 3,
    },
    shadowOpacity: 0.2,
    shadowRadius: 4,
  },
  title: {
    fontSize: 18,
  },
  btnCamera: {
    alignItems: "center",
    justifyContent: "space-between",
    flexDirection: "row",
    backgroundColor: "#019de7",
    paddingHorizontal: responsiveW(20),
    flex: 5,
    margin: 5,
    borderRadius: 10,
  },
  txtSize16: {
    paddingVertical: responsiveH(10),
    fontSize: fontSizer(16),
    flex: 1,
    marginLeft: responsiveW(10),
    color: "#fff",
  },
  btnSearch: {
    alignItems: "center",
    justifyContent: "space-between",
    flexDirection: "row",
    backgroundColor: "#D3D3D3",
    paddingHorizontal: responsiveW(20),
    margin: 5,
    borderRadius: 10,
  },
  subTitle: {
    fontSize: 12,
  },
  txtSize14: {
    fontSize: fontSizer(14),
    color: "#fff",
  },
  viewNoti: {
    backgroundColor: "#000",
    borderRadius: 50,
    borderWidth: 1,
    borderColor: "gray",
    padding: 5,
    justifyContent: "center",
    width: 25,
    height: 25,
    alignItems: "center",
  },
  content: {
    flex: 1,
    justifyContent: "center",
  },
  txtCompleted: {
    marginLeft: 10, fontSize: 26, color: "#000", fontWeight: "bold", margin: 5 
  },
  viewLeft:{
    flex: 1, 
    padding: 10, 
    flexDirection: "row"
  },
  btnLeft: {
    flex: 2, 
    justifyContent: "center" 
  }
})
