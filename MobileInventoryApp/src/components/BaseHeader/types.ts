export interface Props {
    isGoBack?: boolean
    leftIconName: string
    leftHandle?: () => void
    test?: boolean
    textCancel?: boolean
    onClosed?:() => void
    onPressButton?:() => void
    titleHeader?: string
    typeIcon?: string
    bottomExtend?: boolean
    autoOpen?: boolean
    txtQuality?: string
    hideBtn?:boolean,
}
