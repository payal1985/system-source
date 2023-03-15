import { TextStyle, ViewStyle } from "react-native"
import { height, width } from "/utils"
const SIZE_LOADING = 70

interface ILoadingViewOverlay {
  background: ViewStyle
  activityIndicator: ViewStyle
  container: ViewStyle
  textContainer: ViewStyle
  textContent: TextStyle
}
export const stylesOverlay: ILoadingViewOverlay = {
  activityIndicator: {
    flex: 1,
  },
  background: {
    alignItems: "center",
    bottom: 0,
    justifyContent: "center",
    left: 0,
    position: "absolute",
    right: 0,
    top: 0,
  },
  container: {
    backgroundColor: "transparent",
    bottom: 0,
    flex: 1,
    left: 0,
    position: "absolute",
    right: 0,
    top: 0,
  },
  textContainer: {
    alignItems: "center",
    bottom: 0,
    flex: 1,
    justifyContent: "center",
    left: 0,
    position: "absolute",
    right: 0,
    top: 0,
  },
  textContent: {
    fontSize: 16,
    fontWeight: "bold",
    height: 50,
    top: 80,
  },
}

interface ILoadingView {
  container: ViewStyle
  progressText?: TextStyle
  loading?: ViewStyle
  styleContain?: ViewStyle
}
const styles: ILoadingView = {
  container: {
    flex: 1,
    flexDirection: "column",
    justifyContent: "space-evenly",
    alignItems: "center",
    backgroundColor: "rgba(0,0,0,0.6)",
  },
  progressText: {
    color: "white",
    fontSize: 12,
  },
  styleContain: {
    flex: 0,
    position: "absolute",
    top: height / 2 + 16,
    width: width / 2,
    marginTop: 16,
  },
  loading: {
    width: SIZE_LOADING,
    borderRadius: 4,
    height: SIZE_LOADING,
    alignItems: "center",
    justifyContent: "center",
  },
}

export default styles
