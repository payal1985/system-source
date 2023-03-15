import axios from "axios"
import { get, isArray, map } from "lodash"
import api from "../../api"
const API = "/api/ItemType/ProcessJsonPost"
const API_GET_CLIENT = "/api/Clients"
const API_NEW_ITEMTYPE = "/api/ItemType/SaveItemType"
const API_NEW_MANUFACTURER = "/api/Manufactory/SaveManufacturer"
const API_GET_FLOORS = "/api/InventoryFloors/GetFloors"
const API_SAVE_NEW_BUILDINGS = "/api/InventoryBuildings/SaveBuilding"
const API_GET_IMAGE_INFO = "/api/Search/GetImageInfo"
const API_GET_MANUFACTURERS = "/api/Manufactory/GetManufacturers"
const API_GET_SUBMITTED = "/api/Submissions/GetSubmission"
const API_DELETE_INVENTORY_IMAGE = "/api/InventoryImages/Delete"
const APT_GET_INVENTORYITEMINFO = "api/Inventory/GetInventoryItemInfo"
const API_UPDATE_LOCATIONS = "api/InventoryItem/UpdateLocations"
const API_SEARCH_INVENTORY_ITEM = "api/InventoryItem/SearchInventoryItems"
const API_REMOVE_ACCEPT_RULE = "User/UpdateUserAcceptRules"
const API_UPDATE_INVENTORY = "/api/Inventory/UpdateInventory"
const API_SEARCH_INVENTORY_SIMPLE = "api/Inventory/SearchSimpleInventories"
const API_GET_INVENTORY_INFO = "api/Inventory/GetInventoryInfo"
const API_SEARCH_SIMPLE_INVENTORY_ITEM = "api/InventoryItem/SearchSimpleInventoryItems"
const API_UPDATE_INVENTORY_ITEM = "api/inventoryItem/updateInventoryItem"
const API_GET_CONDITION = "api/InventoryItemCondition/GetConditions"
const API_GET_LIST_ORDER = "/api/Order/GetOrders"
const API_GET_LIST_ORDER_ITEMS = "/api/OrderItem/GetOrderItems"
const API_SET_ORDER_COMPLETED = "/api/Order/SetOrdersToCompleted"
const API_SET_ORDER_ITEM_COMPLETED = "/api/OrderItem/SetOrderItemsToCompleted"



import { isRequestError } from "../../api"
export const uploadProgressService = async (
  body: any,
  clientId: any,
  cbProgress?: (progress: number) => void,
  cbSuccess?: (data?: any) => void,
  cbError?: (err: any) => void,
) => {
  try {
    api
      .post(`${API}?clientId=${parseInt(clientId)}`, body, {
        onUploadProgress: (progressEvent) => {
          const percentCompleted = Math.floor((progressEvent.loaded / progressEvent.total) * 100)
          cbProgress?.(percentCompleted)
        },
      })
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}
export const getClientDataService = async (
  cbSuccess?: (data?: any) => void,
  cbError?: (err: any) => void,
  userId?: any,
) => {
  try {
    api
      .get(`${API_GET_CLIENT}?userId=${parseInt(userId)}`)
      .then((response) => {
        console.log("response", response)
        cbSuccess?.(response)
      })
      .catch((e) => {
        console.log("e", e)
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}
export const getFloorsDataService = async (cbSuccess?: (data?: any) => void, cbError?: (err: any) => void) => {
  try {
    api
      .get(`${API_GET_FLOORS}`)
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}
export const getBuildDataService = async (url: any, cbSuccess?: (data?: any) => void, cbError?: (err: any) => void) => {
  try {
    api
      .get(`${url}`)
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}
export const getItemTypeService = async (url: any, cbSuccess?: (data?: any) => void, cbError?: (err: any) => void) => {
  try {
    api
      .get(`${url}`)
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}
export const getItemOptionSetTypeService = async (
  url: any,
  cbSuccess?: (data?: any) => void,
  cbError?: (err: any) => void,
) => {
  try {
    api
      .get(`${url}`)
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}
export const saveNewItemTypeService = async (
  body: any,
  cbSuccess?: (data?: any) => void,
  cbError?: (err: any) => void,
) => {
  try {
    api
      .post(`${API_NEW_ITEMTYPE}`, body)
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}
export const saveNewManufacturerService = async (
  body: any,
  cbSuccess?: (data?: any) => void,
  cbError?: (err: any) => void,
) => {
  try {
    api
      .post(`${API_NEW_MANUFACTURER}`, body)
      .then((response) => {
        console.log('response', response)
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    console.log('error', error)
    return { error }
  }
}
export const saveNewBuildings = async (body: any, cbSuccess?: (data?: any) => void, cbError?: (err: any) => void) => {
  try {
    api
      .post(`${API_SAVE_NEW_BUILDINGS}`, body)
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}
export const getSearchImageInfo = async (body: any, cbSuccess?: (data?: any) => void, cbError?: (err: any) => void) => {
  try {
    api
      .put(`${API_GET_IMAGE_INFO}`, body)
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}

export const getManufactory = async () => {
  try {
    const rs = await api.get(`${API_GET_MANUFACTURERS}`)
    const data = get(rs, "data", {}) as any
    if (isRequestError(rs, data)) return { error: data ? data : get(rs, "originalError", {}) }
    if (isArray(data)) {
      map(data, (value) => {
        value.itemTypeOptionLineCode = value?.manufacturerName
        value.itemTypeOptionLineName = value?.manufacturerName
        value.itemTypeOptionLineID = value?.manufacturerId
        value.itemTypeOptionID = value?.manufacturerId
        value.itemTypeOptionId = value?.manufacturerId
      })
    }
    return {
      results: data,
    }
  } catch (error) {
    return { error }
  }
}

export const getSubmitted = async (body: any) => {
  try {
    const rs = await api.put(`${API_GET_SUBMITTED}`, body)
    const data = get(rs, "data", {}) as any
    return {
      results: data,
    }
  } catch (error) {
    return { error }
  }
}
export const getInventoryItemInfo = async (
  inventoryItemId: any,
  cbSuccess?: (data?: any) => void,
  cbError?: (err: any) => void,
  clientId?: any,
) => {
  try {
    const queryClientId = clientId ? `&clientID=${clientId}` : ""
    api
      .put(`${APT_GET_INVENTORYITEMINFO}?inventoryItemId=${inventoryItemId}${queryClientId}`)
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        console.log('eee', e)
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}
export const getStatusGroup = async (url: any, cbSuccess?: (data?: any) => void, cbError?: (err: any) => void) => {
  try {
    api
      .get(`${url}`)
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}

export const updateLocations = async (body: any) => {
  try {
    const rs = await api.put(API_UPDATE_LOCATIONS, body)
    return rs
  } catch (error) {
    return null
  }
}

export const searchInventoryItem = async (body: any) => {
  try {
    const rs = await api.put(API_SEARCH_INVENTORY_ITEM, body)
    return rs
  } catch (error) {
    return null
  }
}
export const removeUserAcceptanceRules = async (body: any) => {
  try {
    const rs = await api.post(API_REMOVE_ACCEPT_RULE, body)
    return rs
  } catch (error) {
    return null
  }
}
export const updateInventoryItem = async (body: any) => {
  try {
    const rs = await api.put(API_UPDATE_INVENTORY, body)
    return rs
  } catch (error) {
    return null
  }

}

export const searchSimpleInventory = async (body: any) => {
  try {
    const rs = await api.put(API_SEARCH_INVENTORY_SIMPLE, body)
    return rs
  } catch (error) {
    return null
  }
}

export const getInventoryInfo = async (body: any) => {
  try {
    const rs = await api.get(API_GET_INVENTORY_INFO, { params: body })
    return rs
  } catch (error) {
    return null
  }
}

export const searchSimpleInventoryItem = async (body: any) => {
  try {
    const rs = await api.put(API_SEARCH_SIMPLE_INVENTORY_ITEM, body)
    return rs
  } catch (error) {
    return null
  }

}

export const updateInventoryItemDetail = async (body: any) => {
  try {
    const rs = await api.put(API_UPDATE_INVENTORY_ITEM, body)
    return rs
  } catch (error) {
    return null
  }
}

export const getCondition = async () => {
  try {
    const rs = await api.get(API_GET_CONDITION)
    return rs
  } catch (error) {
    return null
  }
}
export const inventoryImageDelete = async (body: any) => {
  try {
    const rs = await api.post(`${API_DELETE_INVENTORY_IMAGE}`, body)
    return rs
  } catch (error) {
    return { error }
  }
}

export const getListOrder = async (
  body: any,
  cbSuccess?: (data?: any) => void,
  cbError?: (err: any) => void,
) => {
  try {
    api
      .post(`${API_GET_LIST_ORDER}`, body)
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}

export const getListOrderItems = async (
  body: any,
  cbSuccess?: (data?: any) => void,
  cbError?: (err: any) => void,
) => {
  try {
    api
      .post(`${API_GET_LIST_ORDER_ITEMS}`, body)
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}
export const updateOrderCompleted = async (
  body: any,
  cbSuccess?: (data?: any) => void,
  cbError?: (err: any) => void,
) => {
  try {
    api
      .put(`${API_SET_ORDER_COMPLETED}`, body)
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}

export const updateOrderItemCompleted = async (
  body: any,
  cbSuccess?: (data?: any) => void,
  cbError?: (err: any) => void,
) => {
  try {
    api
      .put(`${API_SET_ORDER_ITEM_COMPLETED}`, body)
      .then((response) => {
        cbSuccess?.(response)
      })
      .catch((e) => {
        cbError?.(e)
      })
  } catch (error) {
    return { error }
  }
}
