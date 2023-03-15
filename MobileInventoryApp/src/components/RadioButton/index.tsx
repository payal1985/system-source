import React from 'react'
import { StyleSheet, TouchableOpacity, View } from 'react-native'
import Colors from '../../configs/Colors'
export const RadioButton = ({ value, onValueChange }: any) => {
    const onPressFunc = () => {
        onValueChange && onValueChange(!value)
    }
    return <TouchableOpacity style={styles.container} onPress={onPressFunc}>
        {value && <View style={styles.circle} />}
    </TouchableOpacity>
}
const styles = StyleSheet.create({
    container: {
        width: 20,
        height: 20,
        borderRadius: 10,
        borderWidth: 1,
        borderColor: Colors.black,
        marginLeft: 8,
        alignItems: 'center',
        justifyContent: 'center'
    },
    circle: {
        width: 10,
        height: 10,
        borderRadius: 5,
        backgroundColor: Colors.black
    }
})