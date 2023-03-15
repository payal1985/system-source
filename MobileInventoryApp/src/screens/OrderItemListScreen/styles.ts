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
    fontSize: 18,
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
})
