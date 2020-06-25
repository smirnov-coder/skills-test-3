import React, { useEffect, SyntheticEvent, useState } from 'react';
import { useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import { useBooksAPI } from '../hooks/useBooksAPI';
import { RootState } from '../store';

// Компонент для вывода списка книг (в виде таблицы).
export default function BooksTablePage() {
    // Разметка компонента зависит от того, аутентифицирован ли пользователь или нет.
    const isAuthenticated = useSelector((state: RootState) => state.auth.isAuthenticated);

    // Компонент использует redux-модуль books.
    const books = useSelector((state: RootState) => state.books);
    const { getBooks, deleteBooks } = useBooksAPI();

    // Состояние компонента:
    // - deletedBooksIds: массив идентификаторов удаляемыйх книг;
    // - message: сообщение о результате операции.
    const [deletedBooksIds, setDeletedBooksIds] = useState<number[]>([]);
    const [message, setMessage] = useState<string | null>(null);

    // При первом рендере запросим с сервера все книги.
    useEffect(() => {
        getBooks();
    }, []);

    // Подпишимся на изменение информации об ошибке.
    useEffect(() => {
        setMessage(books.error);
    }, [books.error]);

    // Обработчик клика по checkbox.
    const handleChange = (e: SyntheticEvent) => {
        let checkbox = e.target as HTMLInputElement;
        let booksIds: number[];
        if (checkbox.checked)
            booksIds = deletedBooksIds.concat(Number(checkbox.value));
        else
            booksIds = deletedBooksIds.filter(bookId => bookId !== Number(checkbox.value));
        setDeletedBooksIds(booksIds);
    }

    // Обработчик нажатия на кнопку удаления выбранных книг.
    const handleDelete = (e: SyntheticEvent) => {
        e.preventDefault();
        e.stopPropagation();
        if (deletedBooksIds.length > 0) {
            deleteBooks(deletedBooksIds)
                // После успешного удаления выбранных книг вывести сообщение.
                .then(() => setMessage("Книга(-и) успешно удалена(-ы)."));
        }
    }

    const alertClass = books.error ? "alert-danger" : "alert-success";

    return (
        <div className="p-4">
            <h1 className="">Список книг</h1>
            {!message ? null : <div className={`alert ${alertClass}`} role="alert">{message}</div>}
            <table className="table table-hover">
                {!isAuthenticated ? null :
                    <caption className="mt-3">
                        <Link to="/book/create" className="btn btn-success">Добавить</Link>
                        <button className="btn btn-danger ml-1" onClick={handleDelete} disabled={books.isLoading}>
                            Удалить выбранные
                        </button>
                    </caption>
                }
                <thead>
                    <tr>
                        {!isAuthenticated ? null : <th scope="col"></th>}
                        <th scope="col">ID</th>
                        <th scope="col">Название</th>
                        <th scope="col">Автор</th>
                        <th scope="col">Жанр</th>
                        <th scope="col">Год</th>
                        {!isAuthenticated ? null : <th scope="col">Действия</th>}
                    </tr>
                </thead>
                <tbody>
                    {books.items.map(book => (
                        <tr key={book.id}>
                            {!isAuthenticated ? null :
                                <td>
                                    <div className="custom-control custom-checkbox">
                                        <input type="checkbox" className="custom-control-input"
                                            id={`checkbox-${book.id}`} value={book.id} onChange={handleChange} />
                                        <label className="custom-control-label" htmlFor={`checkbox-${book.id}`}></label>
                                    </div>
                                </td>
                            }
                            <th scope="row">{book.id}</th>
                            <td>{book.title}</td>
                            <td>{book.author}</td>
                            <td>{book.genre}</td>
                            <td>{book.year}</td>
                            {!isAuthenticated ? null : <td><Link to={`/book/edit/${book.id}`}>Редактировать</Link></td>}
                        </tr>))
                    }
                </tbody>
            </table>
        </div>
    );
}
