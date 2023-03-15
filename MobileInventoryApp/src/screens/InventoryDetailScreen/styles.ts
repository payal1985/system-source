import { StyleSheet } from "react-native"
import Colors from "/configs/Colors"
import { fontSizer, responsiveW } from "/utils/dimension"
import { responsiveH } from "../../utils/dimension"


export default StyleSheet.create({
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
  button: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    flexDirection: "row",
    backgroundColor: "#fff",
    paddingVertical: 5,
  },
  containerRow: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between'
  }
})
