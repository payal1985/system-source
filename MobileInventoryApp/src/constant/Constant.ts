import { Dimensions } from "react-native"
import { fontSizer, responsiveH } from "/utils"

export const LANGUAGE = {
  EN: "EN",
}

export const ITEM_HEIGHT = responsiveH(20) + fontSizer(19)
export const URL_BASE = "http://138.229.196.129:81"
//export const URL_BASE ="https://e6cf-138-229-196-129.ngrok.io"
export const TIMEOUT_API = 30000 //30s

export const SERVER_NAME = "https://systemsource.s3.us-west-2.amazonaws.com/inventory"

const { width, height } = Dimensions.get("window")

export const CONDITION_QUANTITY_TYPE = {
  GOOD: "Good",
  FAIR: "Fair",
  POOR: "Poor",
  DAMAGED: "Damaged",
  MISSING_PARTS: "MissingParts",
}

export const IMAGE_TYPE = {
  LOCAL: "LOCAL",
  SERVER: "SERVER",
}
export const SEARCH_IMAGE_TYPE = {
  LOCAL: 3,
  SERVER: 4,
}
export const CONDITION_RENDER_TYPE = {
  ADDNEW: 1,
  EDIT: 2,
  SCANCODE: 5,
}
export const DeviceInfo = {
  width,
  height,
}
export const DEPLOYMENT_KEY = "rVdYncPIoi22zvys_q3DYmtwYQP7jqG4VKlDR"
export const DEPLOYMENT_KEY_ANDROID = "GEkk7F3GZTwFU3i3lS2GVWkYGgyWpFmHGttQL"
export const getUniqueListBy = (arr, key) => {
  return [...new Map(arr.map(item => [item[key], item])).values()]
}