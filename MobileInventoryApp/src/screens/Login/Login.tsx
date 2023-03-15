import React, { useCallback, useEffect, useRef, useState } from "react"
import {
  Alert,
  Image,
  Keyboard,
  Platform,
  StatusBar,
  Switch,
  Text,
  TextInput,
  TouchableOpacity,
  View,
  FlatList,
} from "react-native"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import Icon from "react-native-vector-icons/FontAwesome5"
import useLanguage from "/utils/hook/useLanguage"
import MultiLanguage from "/utils/MultiLanguage"
import { setTokenHeader } from "/api"
import { IC_LEFT_ARROW } from "../../assets/images"
import { fontSizer, responsiveW } from "../../utils/dimension"
import { isEmpty, isNil } from "lodash"
import { validateEmail } from "/components/Input"
import LoadingOverlay from "/components/LoadingView/LoadingOverlay"
import { useDispatch, useSelector } from "react-redux"
import { login } from "/redux/login/actions"
import { AppReducerType } from "/redux/reducers"
import { ReduxStateType } from "/redux/types"
import styles from "./styles"
import Storage from "/helper/Storage"
import { ModalFullScreen } from "/components/ModalFullScreen"
import AsyncStorage from "@react-native-community/async-storage"
import IconMaterial from 'react-native-vector-icons/MaterialCommunityIcons'
import Clipboard from "@react-native-community/clipboard"
import { useFocusEffect } from "@react-navigation/native"
import { getClientDataService } from "/redux/progress/service"

const Login = () => {
  const [isEnabled, setIsEnable] = useState<boolean>(false)
  const [language] = useLanguage()
  const [codeName, setCodename] = useState<string>("")
  const [password, setPassword] = useState<string>("")
  const [showPassword, setShowPassword] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(false)
  const dispatch = useDispatch()
  const [showEye, setShowEye] = useState<boolean>(false)
  const userLogin = useSelector((state: any) => state?.[AppReducerType.TOKEN])
  const [jsonError, setJsonError] = useState([])
  const [isShowModal, setIsShowModal] = useState(false)
  const refCount = useRef(0)

  const toggleSwitch = () => setIsEnable((previousState) => !previousState)

  const onTouch = () => {
    setPassword("")
    setShowEye(true)
  }

  const getJsonError = async () => {
    try {
      const jsonError: any = await AsyncStorage.getItem('JSON_ERROR')
      if (!isEmpty(jsonError)) {
        const parseJson = await JSON.parse(jsonError)
        setJsonError(parseJson)
      }
    } catch (error) { }
  }
  const getUserLogin = async () => {
    try {
      const userLogin: any = await Storage.get("@UserLogin")
      const parseUserLogin = JSON.parse(userLogin)
      const loginName = parseUserLogin?.loginName
      if (loginName) {
        setCodename(loginName)
      }
    } catch (error) {
      console.log("error", error)
    }
  }

  useEffect(() => {
    getUserLogin()
  }, [])

  useFocusEffect(useCallback(() => {
    getJsonError()
  }, []))

  const getAndSetClient = async () => {
    getClientDataService(async (rs) => {
      if (rs?.status === 200 && rs?.data?.length === 1) {
        await AsyncStorage.setItem("CLIENT_ID", JSON.stringify(rs?.data?.[0]))
      } else {
        await AsyncStorage.removeItem("CLIENT_ID")
      }
    }, async (e) => {
      await AsyncStorage.removeItem("CLIENT_ID")
    }, userLogin?.userId)
  }

  useEffect(() => {
    if (userLogin?.state === ReduxStateType.LOADED) {
      setLoading(false)
      if (userLogin?.error) {
        setTimeout(() => {
          Alert.alert("Error", `Username or password is incorrect, please try again! ${userLogin?.status || 0}`)
        }, 300)
      } else {
        if (userLogin?.tokenString?.trim() !== "") {
          setTokenHeader(userLogin?.tokenString)
        }
      }
      if (!isEmpty(userLogin)) {
        getAndSetClient()
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

  const removeAll = async () => {
    setJsonError([])
    await AsyncStorage.removeItem('JSON_ERROR')
  }

  const renderHeader = () => {
    return <View style={[styles.viewBtn, { padding: 16 }]}>
      <TouchableOpacity onPress={removeAll}>
        <Text style={styles.txt}>Clear all</Text>
      </TouchableOpacity>
      <Text style={styles.txt}>Json Error</Text>
      <TouchableOpacity onPress={() => setIsShowModal(false)}>
        <IconMaterial name="close" size={24} color='black' />
      </TouchableOpacity>
    </View>
  }

  const deleteFunc = (index: any) => {
    setJsonError((preState) => {
      preState?.splice(index, 1)
      AsyncStorage.setItem('JSON_ERROR', JSON.stringify(preState))
      return [...preState]
    })
  }
  const renderItem = ({ item, index }: any) => {
    console.log('index', index)
    return <TouchableOpacity key={`${index}`} onPress={() => {
      Clipboard.setString(JSON.stringify(item))
      Alert.alert('Copy success')
    }} style={[styles.viewBtn, { paddingHorizontal: 16 }]}>
      <Text>{item?.deviceDate}</Text>
      <TouchableOpacity onPress={() => deleteFunc(index)} style={styles.icon}>
        <IconMaterial name="delete-empty" size={32} color="black" />
      </TouchableOpacity>
    </TouchableOpacity>
  }
  const logoPress = () => {
    refCount.current++
    if (refCount.current === 5) {
      setIsShowModal(true)
      refCount.current = 0
    }
    Keyboard.dismiss()
  }


  return (
    <View style={styles.container}>
      <StatusBar barStyle="dark-content" translucent backgroundColor="rgba(0,0,0,0)" />
      <KeyboardAwareScrollView
        bounces={false}
        showsVerticalScrollIndicator={false}
        contentContainerStyle={{ justifyContent: "center", flex: 1 }}
        keyboardShouldPersistTaps="always"
        enableOnAndroid={true}
        extraScrollHeight={100}
      >
        <TouchableOpacity onPress={logoPress} activeOpacity={1}>
          <View style={styles.topView}>
            <View style={styles.logoView}>
              <Image
                source={require("../../assets/images/logo_login.png")}
                style={{ height: 100, width: 300, marginBottom: 20 }}
                resizeMode="cover"
              />
            </View>
            <View style={styles.mainView}>
              <TextInput
                value={codeName}
                onChangeText={(text) => setCodename(text)}
                style={styles.txtInput}
                placeholder={MultiLanguage("UserName", language.textStatic)}
                placeholderTextColor="#A3A3A3"
              />
            </View>
            <View style={styles.viewPass}>
              <TextInput
                value={password}
                secureTextEntry={!showPassword}
                onChangeText={(text) => setPassword(text)}
                style={styles.txtPass}
                placeholder={MultiLanguage("Password", language.textStatic)}
                placeholderTextColor="#A3A3A3"
                onTouchStart={() => (showEye ? undefined : onTouch())}
              />

              {showEye ? (
                <TouchableOpacity style={{}} onPress={() => setShowPassword(!showPassword)}>
                  <Icon name={showPassword ? "eye" : "eye-slash"} color="#A3A3A3" size={fontSizer(20)} solid={false} />
                </TouchableOpacity>
              ) : null}
            </View>

            <View style={styles.viewBtn}>
              <Switch
                style={[
                  Platform.OS === "ios" && { transform: [{ scaleX: 0.7 }, { scaleY: 0.7 }] },
                  { marginRight: responsiveW(5) },
                ]}
                trackColor={{ false: "#767577", true: "red" }}
                thumbColor={isEnabled ? "#FFF" : "#f4f3f4"}
                ios_backgroundColor="#808080"
                onValueChange={toggleSwitch}
                value={isEnabled}
              />
              <Text style={styles.txtCheck}>{MultiLanguage("Sign In", language.textStatic)}</Text>
              <TouchableOpacity
                style={styles.btnLogin}
                onPress={handleLogin}
                children={<Image style={styles.imgLogin} source={IC_LEFT_ARROW} />}
              />
            </View>
            <TouchableOpacity
              children={
                <Text style={styles.btnSavePass} children={MultiLanguage("Forgot Password ?", language.textStatic)} />
              }
            />
          </View>
        </TouchableOpacity>
      </KeyboardAwareScrollView>
      <LoadingOverlay visible={loading} />
      <ModalFullScreen
        backdrop={true}
        isOpen={isShowModal}
        onClosed={() => {
          setIsShowModal(false)
        }}
        useNativeDriver={true}
        backdropPressToClose={true}
        entry={"bottom"}
        position={"bottom"}
        style={styles.modalFullScreenWhite}
        swipeToClose={false}
        element={
          <FlatList ListHeaderComponent={renderHeader} keyExtractor={(it, index) => `${it}${index}`} data={jsonError} ListEmptyComponent={() => <Text style={[styles.txt, { flex: 1, textAlign: 'center' }]}>Nothing is error</Text>} renderItem={renderItem} />
        }
      />
    </View>
  )
}
export default Login
