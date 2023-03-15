import React from "react"
import { Image, Keyboard, Platform, StatusBar, Switch, Text, TextInput, TouchableOpacity, View } from "react-native"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import Icon from "react-native-vector-icons/FontAwesome5"
import MultiLanguage from "/utils/MultiLanguage"
import { IC_LEFT_ARROW } from "../../assets/images"
import { fontSizer, responsiveW } from "../../utils/dimension"
import LoadingOverlay from "/components/LoadingView/LoadingOverlay"
import styles from "./styles"
import LoginIndex from "./index"

export const LoginNew = () => {
  const {
    showEye,
    showPassword,
    codeName,
    isEnabled,
    language,
    loading,
    password,
    handleLogin,
    onTouch,
    setCodename,
    setPassword,
    setShowPassword,
    toggleSwitch,
  } = LoginIndex()
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
        <TouchableOpacity onPress={() => Keyboard.dismiss()} activeOpacity={1}>
          <View style={styles.topView}>
            <View style={styles.logoView}>
              <Image
                source={require("../../assets/images/logo_login.png")}
                style={styles.logoLogin}
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
    </View>
  )
}
