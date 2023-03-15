import { useDispatch, useSelector } from "react-redux"
import { Dispatch } from "redux"

import { User } from "../../models/User"
import { AppAction } from "../../redux/index"
import { getUser } from "../../redux/user/action"
import { userSelector } from "../../redux/user/selector"

const useUser = (): [User, () => void] => {
  const user = useSelector(userSelector)
  const dispatch = useDispatch<Dispatch<AppAction>>()

  return [user, () => dispatch(getUser.request())]
}

export default useUser
