import { get } from "lodash"
import api, { isRequestError } from "../../api"
const API_LOGIN = "/User/Login"

export const loginService = async (body?: any) => {
  try {
    const response = await api.post(API_LOGIN, body)
    console.log('response', response)
    const data = get(response, "data", {}) as any
    if (isRequestError(response, data)) return { error: data ? data : get(response, "originalError", {}) }
    return {
      result: data,
    }
  } catch (error) {
    console.log('APILOGIn', API_LOGIN)
    return { error }
  }
}
