import { accountSelector } from "./../../redux/account/selector"
import { useDispatch, useSelector } from "react-redux"
import { Dispatch } from "redux"

import { AppAction } from "../../redux/index"
import { accountAc } from "/redux/account/action"
import { Account } from "/models/Account"

const useAccount = (): [Account, (params: any) => void] => {
  const account = useSelector(accountSelector)
  const dispatch = useDispatch<Dispatch<AppAction>>()

  return [account, (data: any) => dispatch(accountAc.request(data))]
}

export default useAccount
