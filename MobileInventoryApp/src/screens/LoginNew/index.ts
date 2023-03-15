import { useEffect, useState } from "react"
import { Alert } from "react-native"
import useLanguage from "/utils/hook/useLanguage"
import { setTokenHeader } from "/api"
import { isNil } from "lodash"
import { validateEmail } from "/components/Input"
import { useDispatch, useSelector } from "react-redux"
import { login } from "/redux/login/actions"
import { AppReducerType } from "/redux/reducers"
import { ReduxStateType } from "/redux/types"
const LoginIndex = () => {
  const [isEnabled, setIsEnable] = useState<boolean>(false)
  const [language] = useLanguage()
  const [codeName, setCodename] = useState<string>("")
  const [password, setPassword] = useState<string>("")
  const [showPassword, setShowPassword] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(false)
  const dispatch = useDispatch()
  const [showEye, setShowEye] = useState<boolean>(false)
  const userLogin = useSelector((state: any) => state?.[AppReducerType.TOKEN])

  const toggleSwitch = () => setIsEnable((previousState) => !previousState)

  const onTouch = () => {
    setPassword("")
    setShowEye(true)
  }

  useEffect(() => {
    if (userLogin?.state === ReduxStateType.LOADED) {
      setLoading(false)
      if (userLogin?.error) {
        setTimeout(() => {
          Alert.alert("Error", "Username or password is incorrect, please try again!")
        }, 300)
      } else {
        if (userLogin?.tokenString?.trim() !== "") {
          setTokenHeader(userLogin?.tokenString)
        }
      }
    }
  }, [userLogin])

  const handleLogin = () => {
    if (isNil(codeName) || isNil(password) || codeName?.trim() === "" || password?.trim() === "") {
      Alert.alert("Error", "Email address and password is required!")
      return
    }
    if (!validateEmail(codeName)) {
      Alert.alert("Error", "Email address is invalid!")
      return
    }
    setLoading(true)
    dispatch(
      login.request({
        loginEmail: codeName,
        loginPW: password,
      }),
    )
  }
  return {
    codeName,
    language,
    password,
    showPassword,
    loading,
    showEye,
    isEnabled,
    toggleSwitch,
    handleLogin,
    setCodename,
    setPassword,
    onTouch,
    setShowPassword,
  }
}
export default LoginIndex
