import { useRoute, useNavigation } from "@react-navigation/native"
import React, { useCallback, useEffect, useMemo, useRef, useState } from "react"
import { View, Text, TouchableOpacity, Dimensions, ActivityIndicator } from "react-native"
import { TabView, SceneMap } from "react-native-tab-view"
import { Header } from ".."
import styles from "./styles"
import Tab from "./Tab"
import fetch from "/api"
import url from "/api/url"
import { BaseAlert, BaseButton } from "/components"
import { IMAGE_TYPE, SEARCH_IMAGE_TYPE } from "/constant/Constant"
import Routes from "/navigators/Routes"
import { getDetailInventoryClient } from "/utils/sqlite/tableInventoryClient"
import Storage from "/helper/Storage"
import LoadingOverlay from "/components/LoadingView/LoadingOverlay"
import { getSearchImageInfo } from "/redux/progress/service"
import useGlobalData from "/utils/hook/useGlobalData"
import { isEmpty } from "lodash"
import _ from "lodash"

type PARAMS =
  | {
    itemTypeId?: string | number
    clientId?: string | number
    idClient?: string | number
  }
  | undefined

type Navigate = {
  key: string
  name: string
  params: any
  merge: boolean
}

const SearchImageScreen = () => {
  const localData = useRef()
  const { navigate, dispatch: dispatchNavigation } = useNavigation()
  const navigation = useNavigation()

  const route = useRoute()
  const params: PARAMS = route?.params || {}
  const { itemTypeId, clientId, idClient } = params
  const [loading, setLoading] = useState<boolean>(false)
  const [globalData] = useGlobalData()
  const [clientGroupId, setclientGroupId] = useState(globalData.item[0]?.InventoryClientGroupID)

  const [activeTab, setActiveTab] = useState<number>(0)
  const [routes] = useState([
    { key: IMAGE_TYPE.LOCAL, title: "Local" },
    { key: IMAGE_TYPE.SERVER, title: "Server" },
  ])
  const [localImages, setLocalImages] = useState<any[]>([])
  const [activeImage, setActiveImage] = useState<string>()
  const [type, setType] = useState<string>(IMAGE_TYPE.LOCAL)
  const [serverData, setServerData] = useState<any[]>([])
  const [indexChoseImage, setindexChoseImage] = useState<any>(null)
  const [indexChoseImageLocal, setindexChoseImageLocal] = useState<any>(null)
  const [isLoadMore, setIsLoadMore] = useState<boolean>(false)
  const refItemTypeId = useRef<any>(null)

  const onSelectImage = useCallback(
    (img: any, index: any) => () => {
      setActiveImage(img)
      setindexChoseImage(index)
      setindexChoseImageLocal(index)
    },
    [],
  )

  const renderTabBar = useCallback(
    ({ jumpTo }) => {
      const onChangeTab = (scene: string) => () => {
        setType(scene)
        jumpTo(scene)
      }
      return (
        <View style={styles.wrapTitle}>
          {routes.map((item, index) => (
            <TouchableOpacity style={[styles.btnTitle]} key={item.key} onPress={onChangeTab(item.key)}>
              <Text style={[styles.title, activeTab === index && styles.activeTab]}>{item.title}</Text>
            </TouchableOpacity>
          ))}
        </View>
      )
    },
    [routes, activeTab],
  )

  const getLocalData = useCallback(async (dataRawLocal) => {
    try {
      const user = await Storage.get("@User")
      const objUser = JSON.parse(`${user}`)
      let data = [dataRawLocal]
      if (data) {
        const dataTemp = data.filter((item: any) => (item?.dataItemType || []).some((item: any) => item?.mainImage))
        const itemDataTemp = dataTemp.map((item: any) => {
          return { ...item, dataItemType: item.dataItemType.filter((elemet: any) => elemet.itemTypeId == itemTypeId) }
        })
        localData.current = itemDataTemp
        const dataTemp2: any[] = []
        itemDataTemp.map((item: any) => {
          (item?.dataItemType || []).map((item: any) => {
            !!item?.mainImageFull && dataTemp2.push(item)
          })
        })
        let getDataParentIdNull = dataTemp2.filter((element: any) => {
          return element.parentRowID === null
        })
        setLocalImages(getDataParentIdNull)
      }
      // })
    } catch (e) {
      // read error
    }
  }, [])
  const onPressDone = useCallback(() => {
    setLoading(true)
    let imageInfo
    if (type === IMAGE_TYPE.LOCAL) {
      imageInfo = localImages.find((item, index) => index === indexChoseImageLocal)
      console.log('imageInfo', { imageInfo })
      handleRawDataImageInfo(imageInfo)
    } else if (type === IMAGE_TYPE.SERVER) {
      imageInfo = serverData.find((item, index) => index === indexChoseImage)
      handleGetImageInfo(imageInfo)
    }
  }, [indexChoseImageLocal, type, serverData, indexChoseImage])
  const handleGetImageInfo = (imageInfo: any) => {
    let body = {
      clientId: clientId,
      inventoryId: imageInfo.inventoryId,
      searchType: 1,
      clientGroupId: clientGroupId,
    }
    getSearchImageInfo(
      body,
      (res) => {
        console.log("res handleGetImageInfo", JSON.stringify(res))
        if (res.status == 200) {
          let imageInfoRaw = _.cloneDeep(res?.data[0])
          handleRawDataImageInfo({ ...imageInfoRaw, inventoryId: imageInfo.inventoryId }, imageInfo.inventoryId)
          setLoading(false)
        } else {
          setLoading(false)
          BaseAlert(res, "Get ImageInfo")
        }
      },
      (e) => {
        setLoading(false)
        BaseAlert(e, "Get ImageInfo")
      },
    )
  }
  const handleRawDataImageInfo = (imageInfo: any, inventoryIdRaw?: any) => {
    let rawDataItemType = imageInfo.itemTypeOptions
    rawDataItemType.find((data: any, i: any) => {
      if (
        data.itemTypeOptionReturnValue === null &&
        data.itemTypeOptionReturnValue === "" &&
        data.itemTypeOptionCode != "TotalCount" &&
        data.itemTypeOptionCode != "Custom-Modular-AV"
      ) {
        rawDataItemType[i].itemTypeOptionReturnValue = [{ returnValue: "" }]
      }
      if (
        data.itemTypeOptionReturnValue === null &&
        data.itemTypeOptionReturnValue === "" &&
        data.itemTypeOptionCode == "Custom-Modular-AV"
      ) {
        rawDataItemType[i].itemTypeOptionReturnValue = []
      }
      if (
        type == IMAGE_TYPE.SERVER &&
        data.itemTypeOptionReturnValue !== null &&
        data.itemTypeOptionReturnValue.length > 0 &&
        data.itemTypeOptionCode == "Custom-Modular-AV"
      ) {
        let arrChek = []
        let arrPick = data.itemTypeOptionReturnValue
        let arrOptionLines = data.itemTypeOptionLines
        for (let k = 0; k < arrOptionLines.length; k++) {
          for (let z = 0; z < arrPick.length; z++) {
            if (
              arrPick[z].returnValue?.trimStart().trimEnd() ==
              arrOptionLines[k].itemTypeOptionLineName?.trimStart().trimEnd()
            ) {
              let returnValue = { returnValue: arrOptionLines[k] }
              arrChek.push(returnValue)
            }
          }
        }
        rawDataItemType[i].itemTypeOptionReturnValue = arrChek
      }
      if (data.itemTypeOptionCode == "TotalCount" && type === IMAGE_TYPE.LOCAL) {
        let arrUrl: any = []
        rawDataItemType[i].itemTypeOptionReturnValue?.map((_it: any) => {
          _it?.conditionData?.map((itChild: any) => {
            arrUrl?.push(...itChild?.url)
          })
          _it.type = _it?.conditionID
          _it.representativePhotos = [{ representativePhotoTotalCount: _it?.txtQuantity, url: arrUrl }]
          return _it
        })
      }
    })
    let inventoryId = type === IMAGE_TYPE.SERVER ? inventoryIdRaw : null
    let parentRowID = type === IMAGE_TYPE.SERVER ? inventoryIdRaw : imageInfo?.inventoryRowID
    const navigateSearchImage: Navigate = {
      key: "searchImageScreen",
      name: Routes.INVENTORY_DETAIL_SCREEN,
      params: {
        type,
        conditionRender: type === IMAGE_TYPE.SERVER ? SEARCH_IMAGE_TYPE.SERVER : SEARCH_IMAGE_TYPE.LOCAL,
        isEdit: false,
        imageInfo,
        idClient,
        parentRowID: parentRowID,
        inventoryId,
      },
      merge: true,
    }
    navigate(navigateSearchImage)
    setLoading(false)
  }
  const renderScene = useMemo(
    () =>
      SceneMap({
        [IMAGE_TYPE.LOCAL]: () => (
          <Tab
            type={IMAGE_TYPE.LOCAL}
            data={localImages}
            indexChoseImage={indexChoseImage}
            indexChoseImageLocal={indexChoseImageLocal}
            activeImage={activeImage}
            onSelectImage={onSelectImage}
          />
        ),
        [IMAGE_TYPE.SERVER]: () => {
          const [serverDataCache, setServerDataCache] = useState([])
          const [currentPageCache, setCurrentPageCache] = useState(1)
          const [activeImageCache, setActiveImageCache] = useState<string>("")
          const [indexChooseImageCache, setIndexChooseImageCache] = useState<number>()
          const getServerData = useCallback(
            async (currentPage = 1) => {
              try {
                let params = {
                  itemTypeId,
                  clientId,
                  currentPage,
                  itemsPerPage: 15,
                }
                const { status, data } = await fetch.put(url.SEARCH_IMAGES, params)
                if (status === 200 && !!data?.totalItem && !isEmpty(data?.inventories)) {
                  setServerDataCache((preData) => {
                    return currentPage === 1 ? data?.inventories : preData.concat(data?.inventories)
                  })
                }
                setIsLoadMore(false)
              } catch (error) {
                setIsLoadMore(false)
                console.log({ error })
              }
            },
            [itemTypeId, clientId, currentPageCache],
          )

          useEffect(() => {
            getServerData()
          }, [])

          const onLoadMoreCache = useCallback(() => {
            setIsLoadMore(true)
            setCurrentPageCache((prePage) => prePage + 1)
          }, [currentPageCache])

          const onRefresh = useCallback(() => {
            setCurrentPageCache(1)
          }, [currentPageCache])

          useEffect(() => {
            getServerData(currentPageCache)
          }, [currentPageCache])

          const onSelectImageCache = useCallback(
            (img: any, index: number) => () => {
              setActiveImageCache(img)
              setIndexChooseImageCache(index)
            },
            [],
          )

          useEffect(() => {
            setServerData(serverDataCache)
          }, [serverDataCache])

          useEffect(() => {
            if (activeImageCache) {
              setActiveImage(activeImageCache)
            }
          }, [activeImageCache])

          useEffect(() => {
            if (indexChooseImageCache !== undefined || indexChooseImageCache !== null) {
              setindexChoseImage(indexChooseImageCache)
            }
          }, [indexChooseImageCache])
          return (
            <Tab
              type={IMAGE_TYPE.SERVER}
              data={serverDataCache}
              activeImage={activeImageCache}
              indexChoseImage={indexChooseImageCache}
              indexChoseImageLocal={indexChoseImageLocal}
              onSelectImage={onSelectImageCache}
              onRefresh={onRefresh}
              onLoadMore={onLoadMoreCache}
              clientId={clientId}
            />
          )
        },
      }),
    [localImages, itemTypeId, clientId, indexChoseImageLocal],
  )
  const getMyStringValue = () => {
    idClient &&
      getDetailInventoryClient(idClient, (data) => {
        getLocalData(data)
      })
  }

  useEffect(() => {
    const unsubscribe = navigation.addListener("focus", () => {
      if (itemTypeId === refItemTypeId.current) return
      refItemTypeId.current = itemTypeId
      setType(IMAGE_TYPE.LOCAL)
      setServerData([])
      getMyStringValue()
      setActiveTab(0)
      setActiveImage("")
      setindexChoseImage("")
    })
    return unsubscribe
  }, [navigation, params])

  return (
    <View style={styles.container}>
      <Header isGoBack labels={"Search image"} />
      <LoadingOverlay visible={loading} />
      <TabView
        swipeEnabled={false}
        navigationState={{ index: activeTab, routes }}
        renderScene={renderScene}
        onIndexChange={setActiveTab}
        renderTabBar={renderTabBar}
        initialLayout={{ width: Dimensions.get("window").width }}
      />
      {/* {isLoadMore && <ActivityIndicator size={32} color={"#000"} style={{ marginBottom: 16 }} />} */}
      <BaseButton
        label={"Done"}
        style={[styles.btnDone, !activeImage && { backgroundColor: "#666" }]}
        onPress={onPressDone}
        disabled={!activeImage}
        fontSize={16}
      />
    </View>
  )
}

export default SearchImageScreen
