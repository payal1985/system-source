export interface Props {
  sellOnPress?: (data: {redeem: string, customer: any, tradeDate: string, volume: number}) => void
  contractInfo?: any
  types: string
  onError: ()=> void
}
