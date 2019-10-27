import config from 'config';
import { authHeader } from '../Helpers';

export const categoryService = {
    create,
    getAll,
    getByID,
    update,
    DeleteByID
};

function create(name, ImageUrl) {

    let data = new FormData();

    data.append('name', name)
    data.append('ImageUrl', ImageUrl)

    const requestOptions = {
        method: 'POST',
        headers: authHeader({ 'Content-Type': 'multipart/form-data' }),
        body: data
    };

    return fetch(`${config.apiUrl}/api/Category`, requestOptions)
            .then(handleResponse)
            .then(category => {
                    return category;
            });
}

function update(id, name, ImageUrl) {

    let data = new FormData();
    data.append('id', id)
    data.append('name', name)
    if(ImageUrl)
    data.append('ImageUrl', ImageUrl)

    const requestOptions = {
        method: 'PUT',
        headers: authHeader({ 'Content-Type': 'multipart/form-data' }),
        body: data
    };

    return fetch(`${config.apiUrl}/api/Category`, requestOptions)
        .then(handleResponse)
        .then(category => {
            return category;
    });
}


function getAll() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader({ 'Content-Type': 'application/json' })
    };

    return fetch(`${config.apiUrl}/api/Category`, requestOptions).then(handleResponse);
}

function getByID(id) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader({ 'Content-Type': 'application/json' })
    };

    return fetch(`${config.apiUrl}/api/Category/${id}`, requestOptions);
}

function DeleteByID(id) {
    const requestOptions = {
        method: 'DELETE',
        headers: authHeader({ 'Content-Type': 'application/json' })
    };

    return fetch(`${config.apiUrl}/api/Category/${id}`, requestOptions);
}

function handleResponse(response) {
    return response.text().then(text => {
        const data = JSON.parse(text);
        if (!response.ok) {
            if (response.status === 401) {
                // auto logout if 401 response returned from api
                logout();
                location.reload(true);
            }

            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }
        return data;
    });
}