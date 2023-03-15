export interface Props {
    isGoBack?: boolean
    leftIconName?: string
    leftHandle?: () => void
    label?: string
    isSaved?: boolean
    handleSave?: () => void
    labels?: string
    isExtraHeight?: boolean
    conditionRender? : number
    actionGoBack?: any
}
