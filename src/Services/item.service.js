import config from 'config';
import { authHeader } from '../Helpers';

export const itemService = {
    create,
    getAll,
    getByID,
    update,
    deleteByID
};

function create(name, text, date, link, categoryid) {
    let data = {
        'name': name,
        'text': text,
        'date': date,
        'link': link,
        'categoryid': categoryid
    };

    const requestOptions = {
        method: 'POST',
        headers: authHeader({ 'Content-Type': 'application/json' }),
        body: JSON.stringify(data)
    };

    return fetch(`${config.apiUrl}/api/items`, requestOptions)
        .then(handleResponse)
        .then(item => {
            return item;
        });
}

function update(id, name, text, date, link, categoryid) {
    const data = {
        'id': id,
        'name': name,
        'text': text,
        'date': date,
        'link': link,
        'categoryid': categoryid   
    };

    const requestOptions = {
        method: 'PUT',
        headers: authHeader({ 'Content-Type': 'application/json' }),
        body: JSON.stringify(data)
    };

    return fetch(`${config.apiUrl}/api/items`, requestOptions)
        .then(handleResponse)
        .then(item => {
                return item;
        });
}


function getAll() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader({ 'Content-Type': 'application/json' })
    };

    return fetch(`${config.apiUrl}/api/items`, requestOptions).then(handleResponse);
}

function getByID(id) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader({ 'Content-Type': 'application/json' })
    };

    return fetch(`${config.apiUrl}/api/items/${id}`, requestOptions);
}

function deleteByID(id) {
    const requestOptions = {
        method: 'DELETE',
        headers: authHeader({ 'Content-Type': 'application/json' })
    };

    return fetch(`${config.apiUrl}/api/items/${id}`, requestOptions);
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