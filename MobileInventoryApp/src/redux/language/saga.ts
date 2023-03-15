import { all, call, put, takeLatest } from "redux-saga/effects"

import { languageAction } from "./action"

export function* getLanguageSaga(action: ReturnType<typeof languageAction.request>) {
  try {
    yield put(languageAction.success(action.payload))
  } catch (error: any) {
    yield put(languageAction.failure(error))
  }
}

function* watchLogin() {
  yield takeLatest(languageAction.request, getLanguageSaga)
}

export default function* getLanguageSagas() {
  yield all([watchLogin()])
}
