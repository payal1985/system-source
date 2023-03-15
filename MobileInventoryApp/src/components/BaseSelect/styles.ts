import { StyleSheet } from "react-native"
import { Colors } from "/configs"
import { fontSizer,  responsiveH, responsiveW } from "../../utils/dimension"

export default StyleSheet.create({
    row: {
        flexDirection: 'row',
        alignItems: 'center'
    },
    rowBetween: {
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'space-between',
    },
    inputContainer: {
        flexDirection: 'row',
        paddingHorizontal: responsiveW(8),
        backgroundColor: Colors.white,
        width: '55%',
        borderWidth: 1,
        borderColor: Colors.gray_80
    },
    textInput: {
        paddingVertical: 3,
        marginVertical: 0,
        fontSize: 12,
        flex: 1,
        color: Colors.black
    },
    container: {
        marginVertical: responsiveH(8)
    },
    validateText: {
        fontSize: fontSizer(12),
        color: Colors.red,
        marginTop: 5
    },
    textField: {
        paddingRight: 20,
        flex: 1,
    }
})
