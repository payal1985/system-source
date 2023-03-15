import Routes from "./Routes"
import { IPropsLoading } from "/components/LoadingView/LoadingProgress"

export const showLoadingProgress = (navigation: any, props: IPropsLoading, isUpdateParams = false) => {
  const { isComplete, progress } = props
  if (isUpdateParams) {
    navigation.navigate({
      name: Routes.LOADING_PROGRESS,
      params: { isComplete, progress },
      merge: true,
    })
  } else {
    navigation.navigate(Routes.LOADING_PROGRESS, {
      isComplete,
      progress,
    })
  }
}

export const hideLoadingProgress = (navigation: any) => {
  const { index, routes } = navigation.dangerouslyGetState()
  const currentRoute = routes[index].name
  if (currentRoute === Routes.LOADING_PROGRESS) {
    navigation.pop()
  }
}
