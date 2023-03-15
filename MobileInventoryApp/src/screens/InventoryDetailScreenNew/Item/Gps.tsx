import React from "react"
import { View, Text, TextInput } from "react-native"
import styles from "../styles"

interface GpsProps {
  item: any
  checkCompareOnBlur: any
  custGPS: any
  setcustGPS: any
}

const Gps = (props: GpsProps) => {
  const { checkCompareOnBlur, custGPS, item, setcustGPS } = props
  return (
    <View style={{ flex: 1 }}>
      <View style={styles.containerItemValue}>
        <Text style={styles.txtOptionName}>
          {item.itemTypeOptionName}
          {item.isRequired ? <Text style={{ color: "red" }}> *</Text> : null}
        </Text>
        <View style={styles.btnDefault}>
          <TextInput
            numberOfLines={3}
            multiline
            onBlur={() => checkCompareOnBlur()}
            style={styles.txtSize16}
            value={custGPS}
            onChangeText={(txt) => setcustGPS(txt)}
          />
        </View>
      </View>
    </View>
  )
}
export default Gps
