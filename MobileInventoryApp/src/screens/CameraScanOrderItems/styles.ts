import { StyleSheet } from "react-native"
import Colors from "/configs/Colors"
import { responsiveW } from "/utils/dimension"
export default StyleSheet.create({
  container: {
    flex: 1,
  },
  button: {
    width: 56,
    backgroundColor: Colors.antGray,
    height: 56,
    borderRadius: 30,
    borderWidth: 2,
    borderColor: "#fff",
  },
  capture: {
    height: 60,
    width: 60,
    borderRadius: 30,
    backgroundColor: "#fff",
    alignItems: "center",
    justifyContent: "center",
    margin: 20,
    marginRight: 20,
    marginTop: 10,
  },

  btnSearch: {
    alignItems: "center",
    justifyContent: "space-between",
    flexDirection: "row",
    paddingHorizontal: responsiveW(20),
    flex: 5,
    margin: 5,
    borderRadius: 10,
  },
})
