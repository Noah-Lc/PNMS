import config from 'config';
import { authHeader } from '../Helpers';

export const categoryService = {
    create,
    getAll,
    getByID
};

function create(name, image) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, image })
    };

    return fetch(`${config.apiUrl}/api/categories`, requestOptions)
        .then(user => {
            return user;
    });
}


function getAll() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/api/categories`, requestOptions);
}

function getByID(id) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };

    return fetch(`${config.apiUrl}/api/categories/${id}`, requestOptions);
}