import React, { useEffect, SyntheticEvent } from "react";
import { useSelector } from "react-redux";
import { RootState } from "../store";
import { Link, useHistory } from "react-router-dom";
import { useAuthAPI } from "../hooks/useAuthAPI";

// Виджет аутентификации. Если пользователь не аутентифицирован, то показывается ссылка на страницу входа на сайт.
// Если пользователь аутентифицирован, то показывается имя пользователя и кнопка для выхода.
export default function AuthWidget() {
    const auth = useSelector((state: RootState) => state.auth);
    const { authenticate, logout } = useAuthAPI();
    const history = useHistory();

    // При первом рендере однократно запросить состояние аутентификации с сервера.
    useEffect(() => {
        authenticate();
    }, []);

    // Обработчик нажатия на кнопку выхода.
    const handleClick = (e: SyntheticEvent) => {
        e.preventDefault();
        e.stopPropagation();

        // После выхода перейти на главную страницу.
        logout().then(() => history.push("/"));
    }

    return !auth.isAuthenticated
        ? <Link to="/login" className="nav-item nav-link">Вход для администратора</Link>
        : <div className="nav-item d-flex">
            <span className="text-light mr-2 align-self-center">Вы вошли как {auth.userName}</span>
            <button type="button" className="btn btn-sm btn-danger" onClick={handleClick}>Выйти</button>
        </div>

}
