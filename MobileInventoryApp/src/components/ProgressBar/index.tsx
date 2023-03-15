import React from "react"
import { TextStyle, View, ViewStyle } from "react-native"
import styles from "./styles"
import * as Progress from "react-native-progress"

type IProgressBarProps = {
  typeProgress?: ITypeProgressBar
  progress?: number | undefined
  isComplete?: boolean
  borderColorComplete?: string
  borderWidth?: number
  borderColor?: string
  textStyle?: TextStyle
  size?: number
  height?: number
  borderRadius?: number
  width?: number
  styleContain?: ViewStyle
  indeterminate?: boolean
}

type ITypeProgressBar = "circle" | "bar" | "pie" | "circleSnail"
const ProgressBar = (props: IProgressBarProps) => {
  const {
    typeProgress = "circle",
    progress = 0,
    isComplete = false,
    borderColorComplete = "#e74c3c",
    borderColor = "#F2F2F2",
    borderWidth = 1,
    size = 45,
    textStyle = styles.progressText,
    height = 5,
    width = 150,
    borderRadius = 4,
    styleContain,
    indeterminate = false,
  } = props

  return (
    <View style={[styles.container, styleContain]}>
      {typeProgress === "circle" ? (
        <Progress.Circle
          size={size}
          color={isComplete ? borderColorComplete : borderColor}
          showsText={true}
          borderColor={borderColor}
          borderWidth={borderWidth}
          progress={progress / 100}
          formatText={() => `${progress}%`}
          textStyle={textStyle}
          thickness={2}
          indeterminate={indeterminate}
        />
      ) : typeProgress === "bar" ? (
        <Progress.Bar
          color={isComplete ? borderColorComplete : borderColor}
          borderColor={borderColor}
          borderWidth={borderWidth}
          progress={progress / 100}
          height={height}
          width={width}
          useNativeDriver={true}
          borderRadius={borderRadius}
          indeterminate={indeterminate}
          indeterminateAnimationDuration={4000}
        />
      ) : null}
    </View>
  )
}
export default ProgressBar
