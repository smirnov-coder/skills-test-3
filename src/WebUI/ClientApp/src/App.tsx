import React from 'react';
import { Provider } from "react-redux";
import { BrowserRouter as Router, Switch, Route, Redirect } from 'react-router-dom';
import store from "./store";
import Navbar from './components/Navbar';
import BooksTablePage from './pages/BooksTablePage';
import BookPage from './pages/BookPage';
import LoginPage from './pages/LoginPage';
import AuthGuard from './components/AuthGuard';
import 'bootstrap/dist/css/bootstrap.css';
import './App.css';

export default function App() {
    return (
        <Provider store={store}>
            <Router>
                <Navbar />
                <Switch>
                    <Route path="/" component={BooksTablePage} exact />
                    <Route path="/book/create">
                        <AuthGuard>
                            <BookPage mode="create" />
                        </AuthGuard>
                    </Route>
                    <Route path="/book/edit/:id">
                        <AuthGuard>
                            <BookPage mode="edit" />
                        </AuthGuard>
                    </Route>
                    <Route path="/login" component={LoginPage} />

                    {/* Для простоты вместо NotFoundPage будем просто редиректить на главную страницу. */}
                    <Redirect to="/" />
                </Switch>
            </Router>
        </Provider>
    );
}
