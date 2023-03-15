import { all } from "redux-saga/effects"

import getAccountSagas from "./account/saga"
import getAccountActiveSagas from "./accountActive/saga"
import getLanguageSagas from "./language/saga"
import getGlobalDataSagas from "./globalData/saga"
import { tokenSagas } from "./login/sagas"
import userSagas from "./user/saga"
import { logoutSagas } from "./logout/sagas"
import { manufacturerSaga } from "./manufacturer/saga"

export default function* rootSaga() {
  yield all([
    userSagas(),
    tokenSagas(),
    getAccountSagas(),
    getAccountActiveSagas(),
    getLanguageSagas(),
    getGlobalDataSagas(),
    logoutSagas(),
    manufacturerSaga(),
  ])
}
