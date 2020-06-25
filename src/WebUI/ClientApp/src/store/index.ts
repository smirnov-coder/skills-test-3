import { createStore } from "redux";
import rootReducer from "./rootReducer";
import { devToolsEnhancer } from "redux-devtools-extension";

export type RootState = ReturnType<typeof rootReducer>;

export default createStore(rootReducer, devToolsEnhancer({}));
