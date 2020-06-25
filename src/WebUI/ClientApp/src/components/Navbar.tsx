import React from 'react';
import { Link } from 'react-router-dom';
import AuthWidget from './AuthWidget';

export default function Navbar() {
    return (
        <nav className="navbar navbar-dark bg-primary">
            <Link to="/" className="navbar-brand">Книжная библиотека</Link>
            <div className="navbar-nav">
                <AuthWidget />
            </div>
        </nav>
    );
}
