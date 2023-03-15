import React, { useState, useEffect } from "react"
import { View, Modal, Text, ActivityIndicator, TextStyle, ViewStyle } from "react-native"
import { stylesOverlay } from "./styles"

type ANIMATION = "none" | "slide" | "fade"
type SIZES = "small" | "large" | number | undefined
interface ILoadingOverlayProps {
  cancelable?: boolean
  color?: string
  animation?: ANIMATION
  overlayColor?: string
  size?: SIZES
  textStyle?: TextStyle
  indicatorStyle?: ViewStyle
  customIndicator?: JSX.Element
  children?: JSX.Element
  spinnerKey?: string
  visible: boolean
  textContent: string
}
type ILoadingOverlayState = {
  visible?: boolean
  textContent?: string
}
const LoadingOverlay = (props: ILoadingOverlayProps) => {
  const [loadingState, updateState] = useState<ILoadingOverlayState>({
    visible: props.visible,
    textContent: props.textContent,
  })
  const close = () => {
    updateState({ visible: false })
  }
  const {
    children,
    customIndicator,
    overlayColor,
    color,
    animation,
    spinnerKey,
    size,
    indicatorStyle,
    textStyle,
    cancelable,
  } = props

  useEffect(() => {
    updateState({ visible: props.visible, textContent: props.textContent })
  }, [props.textContent, props.visible])

  const _handleOnRequestClose = () => {
    if (cancelable) {
      close()
    }
  }

  const _renderDefaultContent = () => {
    return (
      <View style={stylesOverlay.background}>
        {customIndicator ? (
          customIndicator
        ) : (
          <ActivityIndicator
            color={color}
            size={size}
            style={[stylesOverlay.activityIndicator, { ...indicatorStyle }]}
          />
        )}
        <View style={[stylesOverlay.textContainer, { ...indicatorStyle }]}>
          <Text style={[stylesOverlay.textContent, textStyle] as TextStyle}>{loadingState.textContent}</Text>
        </View>
      </View>
    )
  }

  const _renderSpinner = () => {
    const spinner = (
      <View
        style={[stylesOverlay.container, { backgroundColor: overlayColor }]}
        key={spinnerKey ? spinnerKey : `spinner_${Date.now()}`}
      >
        {children ? children : _renderDefaultContent()}
      </View>
    )

    return (
      <Modal
        animationType={animation}
        onRequestClose={() => _handleOnRequestClose()}
        supportedOrientations={["landscape", "portrait"]}
        transparent
        visible={loadingState.visible}
      >
        {spinner}
      </Modal>
    )
  }

  return _renderSpinner()
}
LoadingOverlay.defaultProps = {
  visible: false,
  cancelable: false,
  textContent: "",
  animation: "none",
  color: "white",
  size: "large",
  overlayColor: "rgba(0, 0, 0, .25)",
}
export default LoadingOverlay
