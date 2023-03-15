import { StyleSheet, Platform } from "react-native"
import { fontSizer, responsiveH, responsiveW } from "/utils/dimension"
import Colors from "/configs/Colors"

export default StyleSheet.create({
  container: {
    flex: 1,
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
  previewImage: {
    width: "100%",
    height: "100%",
  },

  containInformation: {
    marginVertical: 5,
    borderRadius: 15,
    paddingVertical: 10,
    paddingLeft: 15,
    flexDirection: "row",
    alignItems: "center",
  },

  wrapperInfor: {
    backgroundColor: "white",
    borderRadius: 10,
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    shadowOpacity: Platform.OS === "android" ? 0.6 : 0.2,
    shadowRadius: 10,
    elevation: 2,
    shadowOffset: {
      width: 0,
      height: 0,
    },
  },
  guide_take_camera: {
    alignSelf: "center",
    height: 25,
    width: 25,
  },
  containGuideToTakeCamera: {
    flex: 1,
    alignItems: "center",
    justifyContent: "center",
  },

  img_background: {
    flex: 1,
    width: undefined,
    height: undefined,
  },
  ic_image_delete: {
    height: 22,
    width: 22,
  },
  containInfor: {},
  txtCamera: {
    paddingVertical: responsiveH(10),
    fontSize: fontSizer(16),
    flex: 0.8,
    backgroundColor:"#fff",
    paddingLeft:responsiveW(20),
    color: "#fff",
  },
  btnSearch: {
    alignItems: "center",
    justifyContent: "space-between",
    flexDirection: "row",
    paddingHorizontal: responsiveW(20),
    flex: 5,
    margin: 2,
    borderRadius: 10,
  },
  viewSearch: {
    flex: 0.8,
    backgroundColor: "#fff",
  },
  viewItemName: {
    flexDirection: "row",
    flexWrap: "wrap",
    marginTop: 5,
    alignItems: "flex-start",
    marginHorizontal: 20,
  },
  btnItemName: {
    borderWidth: 0.4,
    marginVertical: 5,
    borderColor: "gray",
    borderRadius: 18,
    marginHorizontal: 5,
  },
  txtItemName: {
    padding: 10,
    paddingHorizontal: 15,
  },
  viewTakePicture: {
    width: 56,
    backgroundColor: Colors.antGray,
    height: 56,
    borderRadius: 30,
    borderWidth: 2,
    borderColor: "#fff",
  },
})
