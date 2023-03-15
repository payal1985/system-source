import React, { useCallback, useEffect, useMemo, useState } from "react"
import { ScrollView, Text, TouchableOpacity, View } from "react-native"
import { Menu, MenuOption, MenuOptions, MenuTrigger, renderers } from "react-native-popup-menu"
import Icon from "react-native-vector-icons/FontAwesome5"

import Colors from "/configs/Colors"

import styles from "./styles"
import { Props, SelectOption } from "./types"

const Select = (props: Props) => {
  const [select, setSelect] = useState<string>("")

  useEffect(() => {
    setSelect(props?.label!)
  }, [props])

  const onSelect = useCallback(
    (value: SelectOption) => {
      setSelect(value.name || props?.label! || "")
      if (props.onSelect) {
        props.onSelect(value)
      }
    },
    [props],
  )

  const renderMenuTrigger = useMemo(() => {
    const customStyles = {
      triggerOuterWrapper: styles.triggerOuterWrapper,
      TriggerTouchableComponent: TouchableOpacity,
      triggerWrapper: [styles.triggerWrapper, props.triggerStyle],
    }

    return (
      <MenuTrigger customStyles={customStyles} disabled={props.disabled}>
        <View style={{ flexDirection: "row" }}>
          <Text numberOfLines={1} style={[styles.label, props.labelStyle]}>
            {select}
          </Text>
          {!props.hideRight && (
            <Icon color={props.colorRight || Colors.white} name={props.iconRight || "chevron-down"} size={16} />
          )}
        </View>
      </MenuTrigger>
    )
  }, [
    props.triggerStyle,
    props.disabled,
    props.leftIcon,
    props.hideLeft,
    props.colorLeft,
    props.labelStyle,
    props.hideRight,
    props.colorRight,
    props.iconRight,
    select,
  ])

  const renderItemMenu = useCallback(() => {
    return (
      <ScrollView showsVerticalScrollIndicator={false}>
        {props.options &&
          props.options.map((option, index) => (
            <MenuOption value={option.name} key={`${index}`} onSelect={() => onSelect(option)}>
              <View style={styles.labelContainer}>
                <Text style={[styles.label, props.textMenuStyle]}>{option.name || option.accountNo}</Text>
                {props.showSub && <Text style={styles.sub}>{option.sub}</Text>}
                {props.showSub1 && <Text style={styles.sub1}>{option.sub1}</Text>}
              </View>
            </MenuOption>
          ))}
        {props.action && (
          <MenuOption value={props.labelAction} key={props.labelAction} onSelect={props.onSelectAction}>
            <Text style={styles.options}>{props.labelAction}</Text>
          </MenuOption>
        )}
      </ScrollView>
    )
  }, [onSelect, props.colorLeft, props.hideImageLeft, props.options])

  const renderMenuOption = useMemo(() => {
    const customStyles = {
      optionsContainer: [styles.optionsContainer, props.menuStyle],
      optionWrapper: styles.optionWrapper,
    }

    return <MenuOptions customStyles={customStyles}>{renderItemMenu()}</MenuOptions>
  }, [props.menuStyle, renderItemMenu])

  return (
    <View style={[styles.container, props.containerStyle]}>
      <Menu
        renderer={renderers.Popover}
        rendererProps={{ preferredPlacement: "bottom", placement: "bottom" }}
        onOpen={props.onOpen}
      >
        {renderMenuTrigger}
        {renderMenuOption}
      </Menu>
    </View>
  )
}

export default Select
