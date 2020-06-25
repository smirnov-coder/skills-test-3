import React, { SyntheticEvent, useState } from "react";
import { useSelector } from "react-redux";
import { RootState } from "../store";
import { useAuthAPI } from "../hooks/useAuthAPI";
import { useLocation, useHistory } from "react-router-dom";

// Компонент страницы входа на сайт.
export default function LoginPage() {
    // Состояние компонента: учётные данные пользователя.
    const [credentials, setCredentials] = useState({
        userName: "",
        password: ""
    });

    // Используем роутер для перехода на другие страницы после успешного входа.
    const location = useLocation();
    const history = useHistory();

    // Компонент использует redux-модуль auth.
    const auth = useSelector((state: RootState) => state.auth);
    const { login } = useAuthAPI();

    // Обработчик сабмита формы.
    const handleSubmit = (e: SyntheticEvent) => {
        e.preventDefault();
        e.stopPropagation();

        login(credentials)
            .then(() => {
                // После успешного входа производим редирект на returnUrl (если предоставлен) или на главную старницу.
                let returnUrl = "/";
                if (location.state && (location.state as any).returnUrl)
                    returnUrl = (location.state as any).returnUrl;
                history.push(returnUrl);
            })
            .catch(error => { /* "Проглотить" ошибку. */});
    }

    // Общий обработчик изменения значения поля ввода.
    const handleChange = (e: SyntheticEvent, fieldName: string) => {
        setCredentials({
            ...credentials,
            [fieldName]: (e.target as HTMLInputElement).value
        });
    }

    const alertClass = auth.error ? "alert-danger" : "alert-success";

    return (
        <div className="container">
            <h1>Вход на сайт</h1>
            {!auth.error ? null : <div className={`alert ${alertClass}`} role="alert">{auth.error} </div>}
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="userName">Имя пользователя</label>
                    <input type="text" className="form-control" id="userName" value={credentials.userName}
                        onChange={e => handleChange(e, "userName")} required maxLength={50} />
                </div>
                <div className="form-group">
                    <label htmlFor="password">Пароль</label>
                    <input type="password" className="form-control" id="password" value={credentials.password}
                        onChange={e => handleChange(e, "password")} required maxLength={20} />
                </div>
                <button className="btn btn-primary" disabled={auth.isLoading}>Войти</button>
                <div className="text-danger mt-3">
                    Учётные данные для тестов:
                    <ul>
                        <li>Имя пользователя: admin</li>
                        <li>Пароль: Admin-123</li>
                    </ul>
                </div>
            </form>
        </div>
    );
}
