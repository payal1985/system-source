import { all, put, takeLatest } from "redux-saga/effects"

import { manufacturer } from "./actions"
import { getManufactory } from "../progress/service"

export function* getManufactoryFunc() {
  try {
    const { results, error } = yield getManufactory()
    if (error) {
      yield put(manufacturer.failure())
      return
    }
    yield put(manufacturer.success(results))
  } catch (error) {
    yield put(manufacturer.failure())
  }
}
function* watchManufacturer() {
  yield takeLatest(manufacturer.request, getManufactoryFunc)
}

export function* manufacturerSaga() {
  yield all([watchManufacturer()])
}
