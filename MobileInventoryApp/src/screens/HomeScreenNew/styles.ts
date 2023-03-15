import { StyleSheet } from "react-native"
import { fontSizer } from "/utils/dimension"
import { getScreenWidth } from "../../utils/dimension"

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
  mainModal: {
    backgroundColor: "#fff",
    borderTopLeftRadius: 30,
    borderTopRightRadius: 30,
    flex: 0.4,
  },
  border: {
    borderTopWidth: 3,
    borderTopColor: "#6e6877",
    flexDirection: "row",
    flex: 1,
    marginTop: 5,
  },
  item: {
    backgroundColor: "#fff",
    paddingTop: 10,
    paddingBottom: 15,
    flexDirection: "row",
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
  subTitle: {
    fontSize: 12,
  },
  touchLogout: {
    flex: 2,
    justifyContent: "center",
    alignItems: "flex-end",
  },
  btnSelect: {
    flexDirection: "row",
    width: "50%",
    alignItems: "center",
  },
  content: {
    flex: 1,
    justifyContent: "center",
  },
  viewLeft: {
    flex: 1,
    padding: 10,
    flexDirection: "row",
  },
  btnLeft: {
    flex: 2,
    justifyContent: "center",
  },
  logoOut: {
    flex: 8,
    justifyContent: "center",
    alignItems: "center",
  },
  inventoryCollection: {
    marginLeft: 10,
    fontSize: 26,
    color: "#000",
    fontWeight: "bold",
    margin: 5,
  },
  containerViewImg: {
    flex: 0.5,
    marginHorizontal: 15,
    marginTop: 5,
  },
  contentItem: {
    flex: 5,
  },
  logOut: {
    fontSize: 18,
    color: "blue",
  },
  height: {
    width: getScreenWidth(1),
    height: 50,
  },
  touchShowModal: {
    flex: 1,
    paddingVertical: 10,
    marginTop: 20,
  },
  touchContent: {
    flexDirection: "row",
    alignItems: "center",
  },
  txtStatus: {
    fontSize: 18,
    color: "#000",
    marginLeft: 10,
  },
  imgDownArrow: {
    height: 16,
    width: 16,
    marginTop: 2,
    marginLeft: 5,
  },
  containerView: {
    flex: 0.1,
    flexDirection: "row",
    marginTop: 0,
  },
  viewAction: {
    flex: 1,
    marginTop: 10,
    marginHorizontal: 15,
  },
  txtClient: {
    fontWeight: "bold",
    fontSize: 18,
  },
  txtDate: {
    fontSize: 14,
    marginTop: 10,
  },
  containerStatus: {
    flex: 1,
    marginTop: 10,
    marginHorizontal: 15,
  },
  rowContainer: {
    flexDirection: "row",
    marginTop: 20,
  },
})
