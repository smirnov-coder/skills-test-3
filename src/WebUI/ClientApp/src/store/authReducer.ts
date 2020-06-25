import { INITIAL_STATE, AuthState } from "./state";

// Редьюсер для модуля auth. Размер небольшой, так что нет смысла декомпозировать его.
export default function (state: AuthState = INITIAL_STATE.auth, action: any): AuthState {
    switch (action.type) {
        case "REQUEST_LOGIN":
            return {
                ...state,
                isLoading: true
            }

        case "LOGIN_REQUESTED":
            return {
                ...state,
                isLoading: false
            }

        case "AUTH_STATE":
            return {
                ...state,
                isAuthenticated: action.payload.isAuthenticated,
                userName: action.payload.userName
            }

        case "AUTH_ERROR":
            return {
                ...state,
                error: action.payload
            }

        default:
            return state;
    }
}
