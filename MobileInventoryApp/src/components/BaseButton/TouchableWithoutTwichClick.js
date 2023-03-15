import React, { Component } from "react"
import { TouchableOpacity } from "react-native"

export default class TouchableWithoutTwiceClick extends Component {
  constructor(props) {
    super(props)
    this.state = {
      disabled: false,
    }
  }

  componentWillUnmount() {
    if (this.timeout) {
      clearTimeout(this.timeout)
    }
  }

  onPress = async () => {
    this.setState({
      disabled: true,
    })

    this.timeout = setTimeout(() => {
      this.setState({
        disabled: false,
      })
    }, 500)

    if (this.props.onPress) await this.props.onPress()
  }

  render() {
    return (
      <TouchableOpacity {...this.props} disabled={this.props.disabled || this.state.disabled} onPress={this.onPress} />
    )
  }
}
