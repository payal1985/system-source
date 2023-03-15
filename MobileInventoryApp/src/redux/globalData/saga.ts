import { all, call, put, takeLatest } from "redux-saga/effects"

import { codePushActions, globalDataActions } from "./action"

export function* getGlobalDataSaga(action: ReturnType<typeof globalDataActions.request>) {
  yield put(globalDataActions.success(action.payload))
}

export function* setCodePushDataSaga(action: ReturnType<typeof codePushActions.request>) {
  yield put(codePushActions.success(action.payload))
}

function* watchGetList() {
  yield takeLatest(globalDataActions.request, getGlobalDataSaga)
}

function* watchSetCodePush() {
  yield takeLatest(codePushActions.request, setCodePushDataSaga)
}

export default function* getGlobalDataSagas() {
  yield all([watchGetList(), watchSetCodePush()])
}
