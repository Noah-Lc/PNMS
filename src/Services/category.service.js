import config from 'config';
import { authHeader } from '../Helpers';

export const categoryService = {
    create,
    getAll,
    getByID,
    update,
    deleteByID
};

function create(name, image) {
    const data = {
        'name': name,
        'image': image
    };

    const requestOptions = {
        method: 'POST',
        headers: authHeader({ 'Content-Type': 'application/json' }),
        body: JSON.stringify(data)
    };
    return fetch(`${config.apiUrl}/api/categories`, requestOptions)
            .then(handleResponse)
            .then(category => {
                    return category;
            });
}

function update(id, name, image) {
    let data = {
        'id': id,
        'name': name
    };   

    if(image)
       data += { 'image': image }

    const requestOptions = {
        method: 'PUT',
        headers: authHeader({ 'Content-Type': 'application/json' }),
        body: JSON.stringify(data)
    };

    return fetch(`${config.apiUrl}/api/categories`, requestOptions)
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
    return fetch(`${config.apiUrl}/api/categories`, requestOptions).then(handleResponse);
}

function getByID(id) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader({ 'Content-Type': 'application/json' })
    };
    return fetch(`${config.apiUrl}/api/categories/${id}`, requestOptions);
}

function deleteByID(id) {
    const requestOptions = {
        method: 'DELETE',
        headers: authHeader({ 'Content-Type': 'application/json' })
    };
    return fetch(`${config.apiUrl}/api/categories/${id}`, requestOptions);
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