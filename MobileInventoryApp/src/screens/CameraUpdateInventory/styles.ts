import { StyleSheet } from "react-native"
import Colors from "/configs/Colors"
import { fontSizer, responsiveH, responsiveW } from "/utils/dimension"

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
  txtCamera: {
    fontSize: fontSizer(16),
    flex: 0.8,
    paddingLeft: responsiveW(20),
    backgroundColor: '#fff',
    color: "#fff",
  },
  btnSearch: {
    justifyContent: "space-between",
    flexDirection: "row",
    paddingHorizontal: responsiveW(20),
    flex: 5,
    height: 150,
    borderRadius: 10,
  },
  btnItemName: {
    borderWidth: 0.4,
    marginVertical: 5,
    borderColor: "gray",
    borderRadius: 18,
    marginHorizontal: 5,
  },
  modalFullScreen: {
    alignSelf: "center",
    justifyContent: "center",
    backgroundColor: "transparent",
  },
  containerViewModal: {
    backgroundColor: '#fff',
    alignItems: 'center',
    marginHorizontal: 32,
    padding: 16,
    borderRadius: 12
  }
})
