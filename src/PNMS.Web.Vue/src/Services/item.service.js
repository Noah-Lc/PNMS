import config from 'config';
import { authHeader } from '../Helpers';

export const itemService = {
    create,
    getAll,
    getByID
};

function create(name, text, date, link) {
    let data = new FormData();

    data.append('name', name)
    data.append('text', text)
    data.append('date', date)
    data.append('link', link)

    const requestOptions = {
        method: 'POST',
        headers: authHeader({ 'Content-Type': 'multipart/form-data' }),
        body: data
    };

    return fetch(`${config.apiUrl}/api/News`, requestOptions)
        .then(item => {
            return item;
    });
}


function getAll() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader({ 'Content-Type': 'application/json' })
    };

    return fetch(`${config.apiUrl}/api/News`, requestOptions).then(handleResponse);
}

function getByID(id) {
    const requestOptions = {
        method: 'GET',
        headers: authHeader({ 'Content-Type': 'application/json' })
    };

    return fetch(`${config.apiUrl}/api/News/${id}`, requestOptions);
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