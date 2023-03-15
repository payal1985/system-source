import React from "react"
import { TouchableOpacity, Text } from "react-native"
import styles from "../styles"

interface TitleProps {
  setconditionRenderTitle: any
  custBuidingName: string
  custFloor: string
  custGPS: string
  Areanumber: string
}

const Title = (props: TitleProps) => {
  const { setconditionRenderTitle, Areanumber, custBuidingName, custFloor, custGPS } = props
  return (
    <TouchableOpacity onPress={() => setconditionRenderTitle(false)} style={{ marginTop: 20 }}>
      <Text style={styles.txtTitleOps}>{`Building : ${custBuidingName}`} </Text>
      <Text style={styles.txtTitleOps}>{`Floor : ${custFloor} `} </Text>
      <Text style={styles.txtTitleOps}>{`GPS : ${custGPS}`}</Text>
      <Text style={styles.txtTitleOps}>{`Area or Room number : ${Areanumber}`}</Text>
    </TouchableOpacity>
  )
}
export default Title
