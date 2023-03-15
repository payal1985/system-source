import React from "react"
import { Animated, StyleSheet } from "react-native"
import { colors, horizontal } from "../theme"

const styles = StyleSheet.create({
  container: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "center",
    marginTop: 10,
  },
  pagination: {
    width: horizontal.small,
    height: horizontal.small,
    borderRadius: 25,
    marginHorizontal: horizontal.xSmall,
  },
  normalDot: {
    height: 8,
    width: 8,
    borderRadius: 4,
    backgroundColor: colors.primary,
    marginHorizontal: 4,
  },
})

type Pagination = {
  size: number
  paginationStyle?: any
  scrollX?: any
  windowWidth: number
  renderPagination?: (value: any) => void
}

const Pagination = ({ size, paginationStyle, scrollX, windowWidth, renderPagination }: Pagination) => {
  if (renderPagination) return renderPagination(scrollX)
  return (
    <Animated.View style={[styles.container, paginationStyle]}>
      {Array.from({ length: size }).map((_, index) => {
        const width = scrollX.interpolate({
          inputRange: [windowWidth * (index - 1), windowWidth * index, windowWidth * (index + 1)],
          outputRange: [8, 16, 8],
          extrapolate: "clamp",
        })
        return (
          <>
            <Animated.View key={index} style={[styles.normalDot, { width }]} />
          </>
        )
      })}
    </Animated.View>
  )
}

export default Pagination
