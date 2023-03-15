import axios from "axios"
import { TIMEOUT_API, URL_BASE } from "/constant/Constant"
import Storage from "/helper/Storage"
import { logout } from "/redux/logout/actions"
import { store } from "/redux/store"

axios.defaults.headers.common.Accept = "*/*"
const fetch = axios.create({
  baseURL: URL_BASE,
  timeout: TIMEOUT_API,
  headers: {
    // "Access-Control-Allow-Origin": "*",
    "Content-Type": "application/json; charset=utf-8",
  },
})

/**
 * listen status response
 */
fetch.interceptors.response.use(
  async (response) => {
    console.log('response', response)
    const router = response?.config?.url
    if (router === "/User/Login") {
      return response
    }
    const status = response?.status
    //check filter route login
    if (status === 401) {
      const refreshTk = await Storage.get("@RefreshToken")
      const user: any = await Storage.get("@User")
      if (refreshTk && refreshTk !== "") {
        return handleRefreshToken(refreshTk, JSON.parse(user)).then(async (rs) => {
          const { tokenString, refreshToken } = rs.data
          setTokenHeader(tokenString)
          await Promise.all([Storage.set("@RefreshToken", refreshToken), Storage.set("@TokenUser", tokenString)])
          const config = response.config
          return fetch(config)
        })
      }
      return response
    }
    return response
  },
  async (error) => {
    console.log('errorResponse', error?.response)
    const status = error?.response?.status
    if (status === 401) {
      const isInValidToken = error?.response?.data === "InvalidToken"
      const isAuthorized = error?.response?.data === "Anauthorized"
      if (isAuthorized) {
        await Promise.all([Storage.remove("@RefreshToken"), Storage.remove("@TokenUser"), Storage.remove("@User")])
        store.dispatch(logout.success({} as any))
        return Promise.reject(error)
      } else {
        const refreshTk = await Storage.get("@RefreshToken")
        const user: any = await Storage.get("@User")
        //refresh token
        if (isInValidToken && refreshTk && refreshTk !== "") {
          return handleRefreshToken(refreshTk, JSON.parse(user))
            .then(async (rs) => {
              const { tokenString, refreshToken } = rs.data
              setTokenHeader(tokenString)
              error.response.config.headers["Authorization"] = "Bearer " + tokenString
              await Promise.all([Storage.set("@RefreshToken", refreshToken), Storage.set("@TokenUser", tokenString)])
              return fetch(error.response.config)
            })
            .catch(async (e) => {
              await Promise.all([
                Storage.remove("@RefreshToken"),
                Storage.remove("@TokenUser"),
                Storage.remove("@User"),
              ])
              store.dispatch(logout.success({} as any))
              return Promise.reject(error)
            })
        } else {
          await Promise.all([Storage.remove("@RefreshToken"), Storage.remove("@TokenUser"), Storage.remove("@User")])
          store.dispatch(logout.success({} as any))
          return Promise.reject(error)
        }
      }
    } else return Promise.reject(error)
  },
)

export function handleRefreshToken(refreshToken: string, user: any) {
  fetch.defaults.headers.common["refreshToken"] = `${refreshToken}`
  fetch.defaults.headers.common["userId"] = parseInt(user?.userId)
  return fetch.post("/User/RefreshToken/refresh-token")
}

export async function setIpAddressHeader(idAddress: string) {
  fetch.defaults.headers.common["X-Forwarded-For"] = `${idAddress}`
}

export async function setTokenHeader(token: string) {
  fetch.defaults.headers.common["Authorization"] = `Bearer ${token}`
}

export function isRequestError(response: any, data: any) {
  if (response.status !== 200 || !data || data.error) return true
  return false
}
export default fetch
