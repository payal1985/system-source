import { StyleSheet, Dimensions } from "react-native"
const { width } = Dimensions.get("window")
const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "center",
    backgroundColor: "rgba(0,0,0,.3)",
    alignItems: "center",
  },
  body: {
    alignItems: "center",
    width: 0.85 * width,
    backgroundColor: "white",
    borderRadius: 12,
    paddingVertical: 12,
  },
  view_title: {
    justifyContent: "center",
  },
  img: {
    width: 80,
    height: 80,
    borderRadius: 160,
  },
  view_info: {
    width: 0.6 * width,
    marginBottom: 10,
  },
  txtTitleInfo: {
    color: "#4B5563",
    textAlign: "center",
    marginTop: 10,
  },
  text_info: {
    textAlign: "center",
    fontSize: 12.5,
    color: "#6B7280",
    marginTop: 5,
    lineHeight: 19,
  },
  view_action: {
    flexDirection: "row",
    marginBottom: 7,
  },
  containProgress: {
    width: "100%",
    height: 10,
    alignItems: "center",
    justifyContent: "center",
  },
  view_button: {
    justifyContent: "center",
    alignItems: "center",
    flex: 1,
  },
  button: {
    width: 110,
    alignItems: "center",
    backgroundColor: "#F3F4F6",
    borderRadius: 6,
    paddingVertical: 9,
  },
  text_button: {
    color: "white",
    fontSize: 12,
  },
  txtInfo: {
    color: "#6B7280",
    textAlign: "center",
    paddingHorizontal: 10,
  },
})
export default styles
