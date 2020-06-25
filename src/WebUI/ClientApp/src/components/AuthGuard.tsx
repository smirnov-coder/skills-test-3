import React from "react";
import { useSelector } from "react-redux";
import { Redirect, useLocation } from "react-router-dom";
import { RootState } from "../store";

// Компонент для защиты маршрутом, требующих аутентификации.
export default function AuthGuard(props: React.PropsWithChildren<{}>) {
    const isAuthenticated = useSelector((state: RootState) => state.auth.isAuthenticated);
    const location = useLocation();

    return (
        <div>
            {isAuthenticated
                ? props.children
                : <Redirect to={{ pathname: "/login", state: { returnUrl: location.pathname } }} />
            }
        </div>
    );
}
