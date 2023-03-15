import { StyleSheet } from "react-native"
import { Colors } from "/configs"
import { WIDTH } from "/navigators/styles"
import { fontSizer } from "/utils/dimension"

export default StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#f1f0f6",
  },
  txtProgress: {
    fontSize: fontSizer(16),
    color: "#000",
    marginHorizontal: 15,
    marginVertical: 5,
    fontWeight: "400",
  },
  mainModal: {
    backgroundColor: "#fff",
    borderTopLeftRadius: 30,
    borderTopRightRadius: 30,
    flex: 0.4,
  },
  border: {
    borderTopWidth: 3,
    borderTopColor: "#6e6877",
    flexDirection: "row",
    flex: 1,
    marginTop: 5,
  },
  item: {
    backgroundColor: "#fff",
    paddingTop: 10,
    paddingBottom: 15,
    flexDirection: "row",

    borderRadius: 5,
    flex: 1,
    shadowColor: "#00000070",
    shadowOffset: {
      width: 0,
      height: 3,
    },
    shadowOpacity: 0.2,
    shadowRadius: 4,
  },
  title: {
    fontSize: 18,
  },
  subTitle: {
    fontSize: 12,
  },
  // txtCamera2: {
  //   fontSize: fontSizer(14),
  //   color: "#fff",
  // },
  touchLogout: {
    flex: 3,
    marginTop: 4,
    justifyContent: "center",
    alignItems: "flex-end",
  },
  btnSelect: {
    flexDirection: "row",
    width: '50%',
    alignItems: 'center'
  },
  content: {
    flex: 1,
    justifyContent: "center",
  },
  viewLeft: {
    flex: 1,
    padding: 8,
    flexDirection: "row"
  },
  btnLeft: {
    flex: 4,
  },
  logoOut: {
    flex: 6,
    justifyContent: "center",
    alignItems: "center"
  },
  inventoryCollection: {
    marginLeft: 10,
    fontSize: 26,
    color: "#000",
    fontWeight: "bold",
    margin: 5
  },
  containerRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 8,
  },
  containerItem: {
    marginTop: 8,
  },
  headerFlatList: {
    fontWeight: 'bold',
  },
  singleItem: {
    marginRight: 16,
    fontWeight: 'bold',
  },
  rowList: {
    flexDirection: 'row',
    alignItems: "center",
    flexWrap: 'wrap'
  },
  imgItem: {
    width: 64,
    height: 64,
    backgroundColor: 'pink',
    marginRight: 12,
    marginTop: 12,
    borderRadius: 12,
  },
  scrollViewHidden: {
    flex: 1,
    paddingHorizontal: 24,
  },
  rowMultiField: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginTop: 12,
  },
  txtFieldRow: {
    fontWeight: 'bold',
    flex: 1,
    textAlign: 'center'
  },
  singleItemRow: {
    flex: 1,
    marginRight: 12,
    textAlign: 'center',
  },
  containerSwipeOut: {
    marginVertical: 10,
    marginHorizontal: 10,
  },
  doneBtn: {
    marginLeft: 4
  },
  txtBtn: {
    fontSize: 18,
  },
})
