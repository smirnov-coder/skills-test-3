import React, { useState, SyntheticEvent, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useSelector } from "react-redux";
import { RootState } from '../store';
import { useBooksAPI } from "../hooks/useBooksAPI";
import { Book } from '../models/book';

export interface BookPageProps {
    mode: "create" | "edit";
}

// Компонент страницы для создания/редактирования книги. Поведение компонента определяется режимом,
// передаваемым в props.
export default function BookPage(props: BookPageProps) {
    // Режим компонента.
    const mode = props.mode;

    // Параметр маршрута 'id' для редактирования книги.
    const { id } = useParams();

    // Компонент использует redux-модуль books.
    const books = useSelector((state: RootState) => state.books);
    const { createBook, getBook, updateBook } = useBooksAPI();

    // Пустые данные книги (для очистки полей ввода).
    const transientBook: Book = {
        id: 0,
        title: "",
        author: "",
        genre: "",
        year: new Date().getFullYear()
    };

    // Состояние компонента
    // - bookData: данные книги, создаваемой или редактируемой;
    // - message: сообщение о результате операции.
    const [bookData, setBookData] = useState(books.current);
    const [message, setMessage] = useState<string | null>(null);

    // Подпишимся на обновление redux-модуля books.
    useEffect(() => {
        setBookData(books.current);
    }, [books.current]);

    useEffect(() => {
        setMessage(books.error);
    }, [books.error]);

    // При первом рендере для режима редактирования однократно запросим данные книги с сервера. Иначе очистим все
    // поля ввода.
    useEffect(() => {
        if (mode === "edit" && id)
            getBook(id);
        else
            setBookData(transientBook);
    }, []);

    // Общий обработчик изменения значения текстового поля.
    const handleChange = (e: SyntheticEvent, fieldName: string) => {
        setBookData({
            ...bookData,
            [fieldName]: (e.target as HTMLInputElement).value
        });
    }

    // Обработчик сабмита формы.
    const handleSubmit = (e: SyntheticEvent) => {
        e.preventDefault();
        e.stopPropagation();

        if (mode === "create") {
            createBook(bookData)
                .then(() => {
                    // После успешного создания книги очистим поля ввода.
                    setBookData(transientBook);
                    // И выведем сообщение.
                    setMessage("Новая книга успешно добавлена.");
                });
            
        } else {
            updateBook(bookData)
                .then(() => {
                    // После успешного обновления книги просто выведем сообщение.
                    setMessage("Данные книги успешно обновлены.");
                });
        }
    }

    // Заголовок на странице зависит от режима.
    const header = mode === "create" ? "Добавить книгу" : `Редактировать книгу ID: ${id}`

    // Настроить цвет сообщения о результате операции.
    const alertClass = books.error ? "alert-danger" : "alert-success";

    return (
        <div className="container">
            <h1>{header}</h1>
            {!message ? null : <div className={`alert ${alertClass}`} role="alert">{message}</div>}
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="title">Название</label>
                    <input type="text" className="form-control" id="title" value={bookData.title}
                        onChange={e => handleChange(e, "title")} required maxLength={200} />
                </div>
                <div className="form-group">
                    <label htmlFor="author">Автор</label>
                    <input type="text" className="form-control" id="author" value={bookData.author}
                        onChange={e => handleChange(e, "author")} required maxLength={100} />
                </div>
                <div className="form-group">
                    <label htmlFor="Жанр">Жанр</label>
                    <input type="text" className="form-control" id="Жанр" value={bookData.genre}
                        onChange={e => handleChange(e, "genre")} required maxLength={100} />
                </div>
                <div className="form-group">
                    <label htmlFor="Год">Год</label>
                    <input type="number" className="form-control" id="Год" value={bookData.year}
                        onChange={e => handleChange(e, "year")} required min={0} />
                </div>
                <button className="btn btn-primary" disabled={books.isLoading}>Сохранить</button>
            </form>
        </div>
    );
}
