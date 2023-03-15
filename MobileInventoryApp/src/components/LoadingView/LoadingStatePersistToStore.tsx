import React from "react"
import { ActivityIndicator, StyleSheet, View } from "react-native"

interface LoadingStatePersistToStoreProps {}
const LoadingStatePersistToStore = (props: LoadingStatePersistToStoreProps) => {
  return (
    <View
      style={{
        ...StyleSheet.absoluteFillObject,
        alignItems: "center",
        justifyContent: "center",
        backgroundColor: "white",
      }}
    >
      <ActivityIndicator size={"large"} />
    </View>
  )
}
export default LoadingStatePersistToStore
