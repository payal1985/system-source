import { Alert } from "react-native"

const BaseAlert = (e: { message: string | undefined }, type = "get data") => {
  return Alert.alert("Error", e?.message ? `${e?.message} (${e?.status || 0})` : `Error api ${type} (${e?.status || 0})`, [
    {
      text: "OK",
      style: "cancel",
    },
  ])
}
export default BaseAlert
