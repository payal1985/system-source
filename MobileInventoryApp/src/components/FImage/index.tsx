import React, { memo } from "react"
import FastImage, { FastImageProps } from "react-native-fast-image"
import { IC_DEFAULT_ERROR } from "/assets/images"

const FImage = (props: FastImageProps) => {
  return (
    <>
      <FastImage {...props} />
      <FastImage source={IC_DEFAULT_ERROR} style={[props.style, { position: "absolute", zIndex: -1 }]} />
    </>
  )
}

export default memo(FImage)
