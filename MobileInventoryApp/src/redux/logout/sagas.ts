import { all, put, takeLatest } from "redux-saga/effects"

import { logout } from "./actions"
import { logoutService } from "./service"
import { setTokenHeader } from "/api"
import Storage from "/helper/Storage"

export function* logoutSaga(action: ReturnType<typeof logout.request>) {
  const { cbError, cbSuccess, refreshToken } = action.payload
  try {
    //@ts-ignore
    const { result, error } = yield logoutService(refreshToken)
    setTokenHeader("")
    yield Promise.all([Storage.remove("@RefreshToken"), Storage.remove("@TokenUser"), Storage.remove("@User")])
    yield put(logout.success({} as any))
    if (error) {
      cbError?.(error)
      return
    }
    if (result) {
      cbSuccess?.()
    }
  } catch (error) {
    cbError?.(error)
  }
}

function* watchLogout() {
  yield takeLatest(logout.request, logoutSaga)
}

export function* logoutSagas() {
  yield all([watchLogout()])
}
