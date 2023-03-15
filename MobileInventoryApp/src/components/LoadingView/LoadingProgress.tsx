import { View, ActivityIndicator } from "react-native"
import React from "react"
import styles from "./styles"
import { useRoute } from "@react-navigation/native"
import ProgressBar from "../ProgressBar"
import { width } from "/utils"

export interface IPropsLoading {
  isComplete?: boolean
  progress?: number
  isLoading?: boolean
}
const LoadingProgress = () => {
  const route = useRoute()
  const { progress, isComplete, isLoading = true } = route.params as IPropsLoading

  return (
    <View style={styles.container}>
      <View style={styles.loading}>
        <ActivityIndicator size={"small"} color={"#78818e"} />
      </View>
      {progress ? (
        <ProgressBar
          textStyle={styles.progressText}
          borderColor={"#52C41A"}
          progress={progress}
          isComplete={isComplete}
          styleContain={styles.styleContain}
          borderColorComplete={"rgba(51,51,51,0.1)"}
          typeProgress="bar"
          width={width - 16 * 4}
          borderRadius={4}
          height={8}
          indeterminate={isLoading}
        />
      ) : null}
    </View>
  )
}

export default LoadingProgress
