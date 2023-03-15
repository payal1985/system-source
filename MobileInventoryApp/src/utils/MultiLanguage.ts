import * as EN from "./MultiLanguageJson/EN.json"

// const EN = require("./MultiLanguageJson/EN.json")

type Static = {
  id: number
  languageCode: string
  languageKey: string
  languageValue: string
}

const MultiLanguage = (key: string, type: string) => {
  try {

      const word = EN.EN

      var result = Object.keys(word)
        .map((key) => [key, word[key]])
        .filter((params) => params[0].toLowerCase() === key.toLowerCase().trim())

      if (result.length === 0) {
        if (key !== "") {

        }
        return key
      } else {
        if (result[0][1] === "") {
          return result[0][0]
        } else {
          return result[0][1]
        }
      }
  } catch (error) {
    return key
  }
}

export default MultiLanguage
