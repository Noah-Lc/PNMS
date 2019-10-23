export function authHeader(headers) {
    // return authorization header with jwt token
    let user = JSON.parse(localStorage.getItem('user'));

    if (user && user.access_token) {
        console.log( user);
        return { 'Authorization': 'Bearer ' + user.access_token, headers };
    } else {
        return {};
    }
}