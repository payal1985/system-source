import { all, put, takeLatest } from "redux-saga/effects"
import { accountAc } from "./action"

export function* getAccountSaga(action: ReturnType<typeof accountAc.request>) {
  yield put(accountAc.success(action.payload))
}

function* watchGetList() {
  yield takeLatest(accountAc.request, getAccountSaga)
}

export default function* getAccountSagas() {
  yield all([watchGetList()])
}
