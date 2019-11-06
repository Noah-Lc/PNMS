import config from 'config';

export const userService = {
    login,
    logout,
    register
};

function register(username, password, fullname) {
    const data = {
        'username': username,
        'password': password,
        'fullname': fullname
    };
    
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    };

    return fetch(`${config.apiUrl}/api/user`, requestOptions)
        .then(res => {
            return res;
        });
}

function login(username, password) {
    const data = {
        'username': username,
        'password': password
    };   

    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    };

    return fetch(`${config.apiUrl}/api/authentification`, requestOptions)
        .then(handleResponse)
        .then(user => {
            // login successful if there's a jwt token in the response
            if (user.token) {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('user', JSON.stringify(user));
            }
            return user;
        });
}

function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
}

function handleResponse(response) {
    return response.text().then(text => {
        const data = text && JSON.parse(text);
        if (!response.ok) {
            if (response.status === 401) {
                // auto logout if 401 response returned from api
                logout();
                location.reload(true);
            }
            // throwing the error
            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }
        return data;
    });
}