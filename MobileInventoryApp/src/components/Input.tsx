import React, { Component, LegacyRef } from "react"
import {
  ColorValue,
  Dimensions,
  Image,
  ImageStyle,
  ImageURISource,
  KeyboardType,
  Platform,
  StyleProp,
  StyleSheet,
  Text,
  TextInput,
  TextStyle,
  TouchableOpacity,
  View,
  ViewStyle,
} from "react-native"

const { width } = Dimensions.get("window")

interface Props<T> {
  label?: string
  inputRef?: LegacyRef<T>
  secureTextEntry?: boolean
  placeholder?: string
  value: string
  isRequire?: boolean
  multiline?: boolean
  disabled?: boolean
  style?: StyleProp<TextStyle>
  maxLength?: number
  autoCapitalize?: "none" | "characters" | "words" | "sentences"
  keyboardType?: KeyboardType
  typeValidate?: "phone" | "email"
  containerInputStyle?: StyleProp<ViewStyle>
  containerStyle?: StyleProp<ViewStyle>
  errMsg?: string
  hideRequire?: boolean
  onChangeText: (text: string) => void
  onSubmitEditing?: () => void
  hideLine?: boolean
  labelStyle?: StyleProp<TextStyle>
  rightIcon: ImageURISource
  onPressRightIcon?: () => void
  rightIconStyle?: StyleProp<ImageStyle>
  lineStyle?: StyleProp<ViewStyle>
  multilineStyle?: StyleProp<ViewStyle>
  inputStyle?: StyleProp<TextStyle>
}

interface State {
  value: string
  error: boolean | undefined
  isFocus: boolean
  errMsg: string | null
  timeout: number
}

type ThemedTextInputProps = {
  style?: any
  theme?: any
  autoFocus?: boolean
  editable?: boolean
  multiline?: boolean
  keyboardType?: KeyboardType
  underlineColorAndroid?: any
  autoCapitalize?: any
  secureTextEntry?: boolean
  placeholder?: any
  placeholderTextColor?: ColorValue
  blurOnSubmit?: any
  maxLength?: number
  onBlur?: () => void
  onFocus?: () => void
  value?: any
  onChangeText?: (value: string) => void
  onSubmitEditing?: () => void
}

export const REG_PHONE = /(84|0[3|5|7|8|9])+([0-9]{8})\b/
export const REG_EMAIL =
  /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/

const ThemedTextInput = React.forwardRef(({ style, theme, ...props }: ThemedTextInputProps, ref: any) => (
  <TextInput ref={ref} style={style} keyboardAppearance="light" {...props} />
))

export function validatePhoneNumber(phone?: string) {
  if (!phone) return false
  return REG_PHONE.test(phone)
}

export function validateEmail(email?: string) {
  if (!email) return false
  return REG_EMAIL.test(email)
}

export default class Input extends Component<Props<any>, State> {
  inputRef = null
  constructor(props: any) {
    super(props)
    this.state = {
      value: this.props.value,
      error: false,
      isFocus: false,
      errMsg: null,
      timeout: 0,
    }
  }

  getErrorMessage() {
    const { typeValidate } = this.props
    if (typeValidate == "phone") return "Phone validate"
    return "Email validate"
  }

  _onBlur = () => {
    const { value } = this.state
    const { typeValidate } = this.props

    if (!value || value.trim() == "") {
      this.setState({ error: true, isFocus: false })
    } else if (!validatePhoneNumber(value) && typeValidate == "phone") {
      this.setState({
        error: true,
        isFocus: false,
        errMsg: this.getErrorMessage(),
      })
    } else if (!validateEmail(value) && typeValidate == "email") {
      this.setState({
        error: true,
        isFocus: false,
        errMsg: this.getErrorMessage(),
      })
    } else this.setState({ error: false, isFocus: false })
  }
  componentWillReceiveProps(newProps: any) {
    const { isRequire, value, typeValidate, errMsg } = newProps
    const { isFocus } = this.state
    if (isRequire && value && !typeValidate) this.setState({ ...this.state, error: false, value: value })
    if (isRequire && (!value || value.trim() == "") && isFocus) {
      this.setState({ ...this.state, error: true, value: value })
    }
    if ((typeValidate && value && isFocus) || errMsg) {
      if (
        (!validatePhoneNumber(value) && typeValidate == "phone") ||
        (!validateEmail(value) && typeValidate == "email") ||
        errMsg
      ) {
        this.setState({
          error: true,
          value: value,
          errMsg: errMsg || this.getErrorMessage(),
        })
      } else this.setState({ error: false, value: value, errMsg: null })
    }
  }
  onFocus = () => {
    const { value } = this.state
    const { isRequire, typeValidate, errMsg } = this.props
    if (!typeValidate) this.setState({ isFocus: true, error: !value && isRequire })
    if (
      (typeValidate && typeValidate == "phone" && !validatePhoneNumber(value)) ||
      (typeValidate == "email" && !validateEmail(value) && !value.trim()) ||
      errMsg
    ) {
      this.setState({
        error: true,
        isFocus: true,
        errMsg: errMsg || this.getErrorMessage(),
      })
    }
    this.inputRef.focus()
  }

  focus = () => {
    this.inputRef.focus()
  }

  getMessageErrorByPlaceHolder = () => {
    return ""
  }

  handleChangeText = (text: string) => {
    const { onChangeText } = this.props
    if (this.state.timeout) clearTimeout(this.state.timeout)
    onChangeText(text)
  }

  shouldComponentUpdate(nextProps: any) {
    if (this.state.error || this.props.disabled != nextProps.disabled) return true
    return nextProps.value != this.props.value
  }

  render() {
    const {
      label,
      // inputRef,
      secureTextEntry,
      placeholder,
      value,
      isRequire,
      multiline,
      disabled,
      style,
      maxLength = 100,
      autoCapitalize = "none",
      keyboardType = "default",
      typeValidate,
      containerInputStyle,
      hideRequire = false,
      onChangeText,
      onSubmitEditing,
      containerStyle,
      hideLine = false,
      labelStyle,
      rightIcon,
      onPressRightIcon,
      rightIconStyle,
      lineStyle,
      inputStyle,
      multilineStyle,
      ...props
    } = this.props
    const { errMsg, error, isFocus } = this.state
    return (
      <View style={[styles.container, containerStyle]}>
        {label && <Text style={[styles.label, labelStyle]}>{label}</Text>}

        <View style={[styles.containerInput, disabled ? styles.disable : {}, containerInputStyle]}>
          <ThemedTextInput
            {...props}
            style={[styles.input, multiline ? styles.multiline : {}, inputStyle, multilineStyle]}
            ref={(input) => (this.inputRef = input)}
            autoFocus={isFocus}
            editable={!disabled}
            multiline={multiline}
            keyboardType={keyboardType}
            underlineColorAndroid="transparent"
            autoCapitalize={autoCapitalize}
            secureTextEntry={secureTextEntry}
            placeholder={placeholder}
            placeholderTextColor="#707070"
            blurOnSubmit={false}
            maxLength={maxLength}
            onBlur={() => (isRequire ? this._onBlur() : {})}
            onFocus={() => this.setState({ isFocus: true })}
            value={value}
            onChangeText={this.handleChangeText}
            onSubmitEditing={onSubmitEditing}
          />
          {rightIcon && (
            <TouchableOpacity
              onPress={onPressRightIcon}
              children={<Image source={rightIcon} style={[styles.iconRight, rightIconStyle]} />}
            />
          )}
        </View>
        {!hideLine && <View style={[styles.line, lineStyle]} />}
        {error && <Text style={styles.errorText} children={errMsg || this.getMessageErrorByPlaceHolder()} />}
      </View>
    )
  }
}

const styles = StyleSheet.create({
  line: {
    height: Platform.OS == "ios" ? 0.5 : 0.75,
    backgroundColor: "#BBBBBB",
    marginTop: Platform.OS == "ios" ? 4 : -5,
  },
  iconRight: {
    width: 15,
    height: 15,
    resizeMode: "contain",
  },
  container: {
    marginTop: 12,
  },
  unit: {
    color: "#8D8C8C",
    fontSize: 14,
  },
  containerInput: {
    flexDirection: "row",
    borderColor: "#707070",
    borderRadius: 5,
    alignItems: "center",
    paddingHorizontal: 0,
  },
  input: {
    fontSize: 16,
    paddingTop: Platform.OS == "ios" ? 8 : 4,
    flex: 1,
    paddingLeft: 8,
  },
  label: {
    fontSize: 16,
  },
  multiline: {
    minHeight: 82,
    maxHeight: 168,
    textAlignVertical: "top",
    borderWidth: 0.5,
    borderColor: "#BBBBBB",
    borderRadius: 5,
    marginTop: 5,
  },
  require: {
    fontSize: 14,
  },
  errorText: {
    fontSize: 13,
    color: "#FF4D4F",
    marginTop: 4,
    marginLeft: 6,
  },
  leftIcon: {
    width: 29,
    height: 20,
    resizeMode: "contain",
  },
  centerIcon: {
    position: "absolute",
    top: 10,
    right: width / 2 - 20,
  },
  disable: {
    backgroundColor: "#ECECEC",
  },
})
