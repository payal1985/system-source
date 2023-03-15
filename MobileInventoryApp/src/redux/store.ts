import { applyMiddleware, createStore } from "redux"
import { persistReducer, persistStore } from "redux-persist"

import middleWares, { setupMiddleware } from "./middlewarer"
import { persistOptions } from "./options"
import reducers from "./reducers"

const persistedReducer = persistReducer(persistOptions, reducers)

const store = createStore(persistedReducer, applyMiddleware(...middleWares))
const persistor = persistStore(store)

setupMiddleware()

export { store, persistor }
