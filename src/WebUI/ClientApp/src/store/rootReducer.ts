import { combineReducers } from "redux";
import booksReducer from "./booksReducer";
import authReducer from "./authReducer";

const rootReducer = combineReducers({
    auth: authReducer,
    books: booksReducer
});

export default rootReducer;
