import { Book } from "../models/book";

// Состояние модуль auth.
export interface AuthState {
    // Флаг, показывающий аутентифицирован ли пользователь.
    isAuthenticated: boolean;

    // Имя пользователя.
    userName: string;

    // Флаг, показывающий выполняется ли взаимодействие с сервером.
    isLoading: boolean;

    // Ошибка.
    error: any;
}

// Состояние модуля books.
export interface BooksState {
    // Коллекция книг.
    items: Book[];

    // Данные текущей редактируемой книги.
    current: Book;

    // Флаг, показывающий выполняется ли взаимодействие с сервером.
    isLoading: boolean;

    // Ошибка.
    error: any;
}

export interface AppState {
    auth: AuthState;
    books: BooksState;
}

export const INITIAL_STATE: AppState = {
    auth: {
        isAuthenticated: false,
        userName: "",
        isLoading: false,
        error: null
    },
    books: {
        items: [],
        current: {
            id: 0,
            title: "",
            author: "",
            genre: "",
            year: new Date().getFullYear()
        },
        isLoading: false,
        error: null
    }
}
