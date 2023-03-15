import { useNavigation } from "@react-navigation/native"
import React from "react"
import { FlatList, Text, TouchableOpacity, View } from "react-native"
import { KeyboardAwareScrollView } from "react-native-keyboard-aware-scroll-view"
import IconImage from "react-native-vector-icons/FontAwesome5"
import AsyncStorage from "@react-native-community/async-storage"
import Icon from "react-native-vector-icons/AntDesign"
import styles from './styles'
import { getScreenWidth } from "../../utils/dimension"

type ItemProps = {
  item?: any
}

const CompletedScreen = () => {
  const { goBack } = useNavigation()
  var initialElements = [
    {
      id: "0",
      data: "Nov 1",
      text: "Inventory Collection",
      version: "Version 1",
      save: "Save at: 10 days ago",
      submit: "2 day ago",
    },
    {
      id: "1",
      data: "Nov 2",
      text: "Inventory Collection",
      version: "Version 2",
      save: "Save at: 7 days ago",
      submit: "2 day ago",
    },
    {
      id: "2",
      data: "Nov 3",
      text: "Inventory Collection",
      version: "Version 7",
      save: "Save at: 1 days ago",
      submit: "2 day ago",
    },
  ]

  const Item = ({ item }: ItemProps) => (
    <View>
      <Text style={{ fontSize: 16, fontWeight: "bold", marginVertical: 5, marginHorizontal: 10 }}>{item.data}</Text>
      <TouchableOpacity onPress={() => clearAll()} style={styles.item}>
        <View style={{ flex: 0.5, marginHorizontal: 15, marginTop: 5 }}>
          <IconImage name={"laptop-code"} size={20} />
        </View>
        <View style={{ flex: 5 }}>
          <Text style={styles.title}>{item.text}</Text>
          <Text style={[styles.subTitle, { marginTop: 5 }]}>{item.version}</Text>
          <Text style={[styles.subTitle, { marginTop: 5 }]}>{item.save}</Text>
          <Text style={[styles.subTitle, { marginTop: 5 }]}>{item.submit}</Text>
        </View>
        <View style={{ flex: 1, alignItems: "flex-end", marginRight: 10 }}></View>
      </TouchableOpacity>
    </View>
  )
  const clearAll = async () => {
    try {
      await AsyncStorage.clear()
    } catch (e) {
      // clear error
    }
  }
  const renderItem = ({ item }: ItemProps) => <Item item={item} />
  return (
    <View style={styles.container}>
      <View style={{ width: getScreenWidth(1), height: 50 }}></View>
      <KeyboardAwareScrollView
        bounces={false}
        showsVerticalScrollIndicator={false}
        extraScrollHeight={150}
        enableOnAndroid={true}
        style={{ flex: 8 }}>
        <View style={styles.content}>
          <View style={styles.viewLeft}>
            <TouchableOpacity onPress={() => goBack()} style={styles.btnLeft}>
              <Icon name={"left"} size={25} color={"blue"} />
            </TouchableOpacity>
            <View style={{ flex: 8 }}></View>
            <View style={{ flex: 2, alignItems: "flex-end" }}></View>
          </View>
          <Text style={styles.txtCompleted}>Completed</Text>
        </View>

        <FlatList data={initialElements} renderItem={renderItem} keyExtractor={(item) => item.id} />
      </KeyboardAwareScrollView>
    </View>
  )
}

export default CompletedScreen
