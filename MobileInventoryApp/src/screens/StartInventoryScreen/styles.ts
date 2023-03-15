import { StyleSheet } from "react-native"
import { fontSizer, responsiveW } from "/utils/dimension"
import {responsiveH } from "../../utils/dimension"

export default StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#f5f5f5",
  },
  colorRequired: {
    color: "red",
  },
  mainContainer: {
    flex: 8,
    padding: responsiveW(20),
    paddingTop: responsiveW(5),
  },
  txtContainer: {
    flex: 1,
    fontSize: 20,
    alignItems: "center",
    textAlign: "center",
    marginVertical: 20,
  },
  txtOptions: {
    marginTop: 20,
    flex: 1,
    flexDirection: "row",
    alignItems: "center",
  },
  subTxtOptiops: {
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
  txtValue: {
    paddingVertical: responsiveH(15),
    fontSize: fontSizer(16),
    flex: 1,
    marginRight: responsiveW(10),
  },
  content: {
    flexDirection: "row", 
    marginTop: 20 
  }
})
