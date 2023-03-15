import { StyleSheet } from "react-native"
import { fontSizer } from "/utils/dimension"

export default StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#f5f5f5",
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
  txtSize14: {
    fontSize: fontSizer(14),
    color: "#fff",
  },
})
