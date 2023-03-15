import { all, put, takeLatest } from "redux-saga/effects"

import { accountActiveAc } from "./action"

export function* getAccountActiveSaga(action: ReturnType<typeof accountActiveAc.request>) {
  yield put(accountActiveAc.success(action.payload))
}

function* watchGetList() {
  yield takeLatest(accountActiveAc.request, getAccountActiveSaga)
}

export default function* getAccountActiveSagas() {
  yield all([watchGetList()])
}
