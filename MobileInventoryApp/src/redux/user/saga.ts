import { all, call, put, takeLatest } from "redux-saga/effects"

import { User } from "../../models/User"
import { getUser } from "./action"

export function* getUserSaga() {
  // NOTE: call???
  // try {
  //   const user: User = yield call("")

  //   if (user) {
  //     yield put(getUser.success(user))
  //   }
  // } catch (error) {
  //   yield put(getUser.failure(error))
  // }
}

function* watchGetUser() {
  yield takeLatest(getUser.request, getUserSaga)
}

export default function* userSagas() {
  yield all([watchGetUser()])
}
