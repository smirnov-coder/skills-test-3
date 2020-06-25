import { useDispatch } from "react-redux";
import { Book } from "../models/book";
import { useLocalStorage } from "./useLocalStorage";

// Хук для с Books API (actionCreator'ы для работы с книгами).
export function useBooksAPI() {
    const dispatch = useDispatch();
    const [readToken, ,] = useLocalStorage("token");

    // Создаёт новую книгу на сервере.
    const createBook = (book: Book) => {
        dispatch({ type: "BOOK_ERROR", payload: "" });
        dispatch({ type: "REQUEST_CREATE_BOOK" });
        let options: RequestInit = {
            method: "post",
            body: JSON.stringify(book),
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${readToken()}`
            }
        };
        return fetch("/api/books", options)
            .then(response => {
                if (!response.ok)
                    throw new Error("Не удалось добавить новую книгу.");
            })
            .finally(() => dispatch({ type: "CREATE_BOOK_REQUESTED" }))
            .catch((error: Error) => dispatch({ type: "BOOK_ERROR", payload: error.message }));
    }

    // Получает с сервера книгу по ID.
    const getBook = (id: number) => {
        dispatch({ type: "REQUEST_GET_BOOK" });
        fetch(`/api/books/${id}`)
            .then(response => response.json())
            .then(data => dispatch({ type: "CURRENT_BOOK", payload: data }))
            .finally(() => dispatch({ type: "GET_BOOK_REQUESTED" }))
            .catch(error => console.error(error));
    }

    // Обновляет данные книги на сервере
    const updateBook = (book: Book) => {
        dispatch({ type: "BOOK_ERROR", payload: "" });
        dispatch({ type: "REQUEST_UPDATE_BOOK" });
        let options: RequestInit = {
            method: "put",
            body: JSON.stringify(book),
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${readToken()}`
            }
        };
        return fetch("/api/books", options)
            .then(response => {
                if (!response.ok)
                    throw new Error("Не удалось обновить книгу.");
            })
            .finally(() => dispatch({ type: "UPDATE_BOOK_REQUESTED" }))
            .catch((error: Error) => dispatch({ type: "BOOK_ERROR", payload: error.message }));
    }

    // Получает с сервера коллекцию всех книг.
    const getBooks = () => {
        dispatch({ type: "REQUEST_GET_BOOKS" });
        fetch(`/api/books`)
            .then(response => response.json())
            .then(data => dispatch({ type: "BOOKS", payload: data }))
            .finally(() => dispatch({ type: "GET_BOOKS_REQUESTED" }))
            .catch(error => console.error(error));
    }

    // Удаляет книги на сервере по ID книг.
    const deleteBooks = (booksIds: number[]) => {
        dispatch({ type: "BOOK_ERROR", payload: "" });
        dispatch({ type: "REQUEST_DELETE_BOOKS" });
        let options: RequestInit = {
            method: "delete",
            body: JSON.stringify(booksIds),
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${readToken()}`
            }
        };
        return fetch("/api/books", options)
            .then(response => {
                if (!response.ok)
                    throw new Error(`Не удалось удалить книгу(-и).`);
            })
            .then(() => getBooks())
            .catch((error: Error) => {
                dispatch({ type: "BOOK_ERROR", payload: error.message });
                dispatch({ type: "DELETE_BOOKS_REQUESTED" });
            });
    }

    return { createBook, getBook, updateBook, getBooks, deleteBooks };
}
