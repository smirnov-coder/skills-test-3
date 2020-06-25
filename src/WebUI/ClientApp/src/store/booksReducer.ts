import { INITIAL_STATE, BooksState } from "./state";

// Редьюсер для модуля books. Размер небольшой, так что нет смысла декомпозировать его.
export default function (state: BooksState = INITIAL_STATE.books, action: any): BooksState {
    switch (action.type) {
        case "REQUEST_CREATE_BOOK":
        case "REQUEST_UPDATE_BOOK":
        case "REQUEST_DELETE_BOOKS":
        case "REQUEST_GET_BOOK":
        case "REQUEST_GET_BOOKS":
            return {
                ...state,
                isLoading: true
            };

        case "CURRENT_BOOK":
            return {
                ...state,
                current: action.payload
            };

        case "CREATE_BOOK_REQUESTED":
        case "UPDATE_BOOK_REQUESTED":
        case "DELETE_BOOKS_REQUESTED":
        case "GET_BOOK_REQUESTED":
        case "GET_BOOKS_REQUESTED":
            return {
                ...state,
                isLoading: false
            };

        case "BOOKS":
            return {
                ...state,
                items: action.payload
            }

        case "BOOK_ERROR":
            return {
                ...state,
                error: action.payload
            };

        default:
            return state;
    }
}
