import { all, put, takeLatest } from "redux-saga/effects"

import { login } from "./actions"
import { loginService } from "./service"
import { setTokenHeader } from "/api"
import Storage from "/helper/Storage"

export function* loginSaga(action: ReturnType<typeof login.request>) {
  try {
    //@ts-ignore
    const loginName = action.payload?.loginEmail
    const loginPass = action.payload?.loginPW
    const { result, error } = yield loginService(action.payload)
    if (error) {
      //@ts-ignore
      yield put(login.failure(error))
      return
    }
    if (result && result?.tokenString) {
      setTokenHeader(result?.tokenString)
      yield Promise.all([
        Storage.set("@RefreshToken", result?.refreshToken),
        Storage.set("@TokenUser", result?.tokenString),
        Storage.set("@User", JSON.stringify(result)),
        Storage.set(
          "@UserLogin",
          JSON.stringify({
            loginName,
            loginPass,
          }),
        ),
      ])
      yield put(login.success(result))
    }
  } catch (error) {
    yield put(login.failure())
  }
}

function* watchLogin() {
  yield takeLatest(login.request, loginSaga)
}

export function* tokenSagas() {
  yield all([watchLogin()])
}
