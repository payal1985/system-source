import { get } from "lodash"
import api, { isRequestError } from "../../api"
const API_LOGOUT = "/User/Logout"

export const logoutService = async (token: string) => {
  try {
    const response = await api.post(API_LOGOUT, { token })
    const data = get(response, "data", {}) as any
    if (isRequestError(response, data)) return { error: data ? data : get(response, "originalError", {}) }
    return {
      result: data,
    }
  } catch (error) {
    return { error }
  }
}
