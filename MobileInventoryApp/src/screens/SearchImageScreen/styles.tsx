import { StyleSheet } from "react-native"
import { Colors } from "/configs"
import { DeviceInfo } from "/constant/Constant"

const imageSize = (DeviceInfo.width - 64) / 3

export default StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: Colors.whiteSmoke,
  },
  wrapTitle: {
    flexDirection: "row",
    paddingHorizontal: 20,
    marginVertical: 20,
    justifyContent: "center",
  },
  btnTitle: {
    width: "40%",
    alignItems: "center",
  },
  title: {
    color: Colors.black,
    fontSize: 16,
    fontWeight: "600",
  },
  activeTab: {
    color: Colors.mainColor,
  },
  wrapImg: {
    width: imageSize,
    height: imageSize,
    marginBottom: 16,
    borderRadius: 8,
    marginRight: 16,
  },
  checkBox: {
    position: "absolute",
    right: 6,
    top: -10,
  },
  btnDone: {
    alignSelf: "center",
    bottom: 20,
    backgroundColor: Colors.mainColor,
  },
  inventoryID: {
    alignSelf: "flex-start",
    marginLeft: 4,
    color: Colors.redHeader,
    marginTop: 4,
    fontSize: 12,
  },
})
