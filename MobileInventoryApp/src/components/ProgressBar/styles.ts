import { TextStyle, ViewStyle } from "react-native"

interface IItemView {
  container: ViewStyle
  progressText: TextStyle
}
const styles: IItemView = {
  container: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },
  progressText: {
    color: "black",
    fontSize: 14,
  },
}
export default styles
