import { useDispatch } from "react-redux";
import { UserCredentials } from "../models/userCredentials";
import { useLocalStorage } from "./useLocalStorage";
import { AuthState } from "../store/state";

// Хук для работы с Auth API (actionCreator'ы для аутентификации).
export function useAuthAPI() {
    const dispatch = useDispatch();
    const [readToken, writeToken, clearToken] = useLocalStorage("token");

    // Осуществляет вход на сайт.
    const login = (credentials: UserCredentials) => {
        // Очистить состояние ошибки (если была).
        dispatch({ type: "AUTH_ERROR", payload: "" });

        dispatch({ type: "REQUEST_LOGIN" });
        let options: RequestInit = {
            method: "post",
            body: JSON.stringify(credentials),
            headers: { "Content-Type": "application/json" }
        };
        return fetch("/api/auth/login", options)
            .then(response => response.json())
            .then(data => {
                if ("error" in data)
                    throw new Error(data.error);
                // Если успешно вошли на сайт, то сохранить token в localStorage.
                writeToken(data.token);
                // После логина, запросить состояние аутентификации.
                return authenticate();
            })
            .finally(() => dispatch({ type: "LOGIN_REQUESTED" }))
            .catch((error: Error) => {
                dispatch({ type: "AUTH_ERROR", payload: error.message });
                return Promise.reject();
            });
    }

    // Производит аутентификацию пользователя (запрашивает состояние аутентификации).
    const authenticate = () => {
        const token = readToken();
        let options: RequestInit = {
            method: "get",
            headers: !token ? {} : { "Authorization": `Bearer ${token}` }
        };
        return fetch("/api/auth/state", options)
            .then(response => response.json())
            .then(data => dispatch({ type: "AUTH_STATE", payload: data }))
            .catch(error => console.error(error));
    }

    // Производит выход из приложения.
    const logout = () => {
        // Удалить token из localStorage.
        clearToken();
        // Очистить состояние auth модуля.
        dispatch({
            type: "AUTH_STATE",
            payload: {
                isAuthenticated: false,
                userName: "",
                error: null,
                isLoading: false
            } as AuthState
        });
        // Вернуть промис, чтобы вызывающий компонент мог выполнить доп. работу сразу после окончания логаута.
        return Promise.resolve();
    }

    return { login, logout, authenticate };
}
