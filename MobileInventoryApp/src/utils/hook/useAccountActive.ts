import { useDispatch, useSelector } from "react-redux"
import { Dispatch } from "redux"

import { AppAction } from "../../redux/index"
import { Account } from "/models/Account"
import { accountActiveAc } from "/redux/accountActive/action"
import { accountActiveSelector } from "/redux/accountActive/selector"

const useAccountActive = (): [Account, (params: any) => void] => {
  const accountActive = useSelector(accountActiveSelector)
  
  const dispatch = useDispatch<Dispatch<AppAction>>()

  return [accountActive, (data: any) => dispatch(accountActiveAc.request(data))]
}

export default useAccountActive
