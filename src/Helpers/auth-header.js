export function authHeader(headers) {
    // return authorization header with jwt token
    let user = JSON.parse(localStorage.getItem('user'));

    if (user && user.token) {
        return { 'Authorization': 'Bearer ' + user.token, headers };
    } else {
        return {};
    }
}