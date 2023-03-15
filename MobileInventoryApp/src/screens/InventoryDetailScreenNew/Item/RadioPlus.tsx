import React from "react"
import { View, Text, TouchableOpacity } from "react-native"
import styles from "../styles"
interface RadioPlusProps {
  item: any
  arrRaw: any
  handlePressCheck: any
  arrCheck: any
}

const RadioPlus = (props: RadioPlusProps) => {
  const { item, arrCheck, arrRaw, handlePressCheck } = props
  return (
    <View style={{ flex: 1 }}>
      <View style={styles.containerItemValue}>
        <Text style={styles.txtOptionName}>
          {item.itemTypeOptionName}
          {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
        </Text>
      </View>
      <View style={{ flex: 1, flexDirection: "row" }}>
        {arrRaw.map((data: any, i: number) => {
          return (
            <TouchableOpacity
              key={i}
              onPress={() => handlePressCheck(i)}
              style={{
                backgroundColor: arrCheck.includes(i) ? "gray" : "white",
                borderRadius: 20,
                marginHorizontal: 5,
              }}
            >
              <Text style={{ padding: 10, paddingHorizontal: 15 }}>{data.itemTypeOptionLineName}</Text>
            </TouchableOpacity>
          )
        })}
      </View>
    </View>
  )
}
export default RadioPlus
