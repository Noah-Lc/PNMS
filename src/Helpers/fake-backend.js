export function configureFakeBackend() {
    let users = [
        { id: 2, username: 'admin', password: 'admin', fullname: 'admin' },
        { id: 1, username: 'test', password: 'test', fullname: 'Test' }
    ];
    let categories = [
        { id: 1, name: 'test01', image: 'test01' }, 
        { id: 2, name: 'test02', image: 'test02' }, 
        { id: 3, name: 'test03', image: 'test03' }
    ];
    let items = [
        { id: 1, title: 'test01', categoryID: 1, text: 'test01', date:'2013-10-21', slug: '\\test01' }, 
        { id: 2, title: 'test02', categoryID: 2, text: 'test02', date:'2013-12-15', slug: '\\test02' }, 
        { id: 3, title: 'test03', categoryID: 3, text: 'test03', date:'2014-05-06', slug: '\\test03' }
    ];

    let realFetch = window.fetch;
    window.fetch = function (url, opts) {
        return new Promise((resolve, reject) => {
            // wrap in timeout to simulate server api call
            setTimeout(() => {
                // authenticate
                if (url.endsWith('/api/authentification') && opts.method === 'POST') {
                    // get parameters from post request
                    console.log(opts.body);
                    let params = JSON.parse(opts.body);

                    // find if any user matches login credentials
                    let filteredUsers = users.filter(user => {
                        return user.username === params.username && user.password === params.password;
                    });

                    if (filteredUsers.length) {
                        // if login details are valid return user details and fake jwt token
                        let user = filteredUsers[0];
                        let responseJson = {
                            id: user.id,
                            username: user.username,
                            fullname: user.fullname,
                            token: 'fake-jwt-token'
                        };
                        resolve({ ok: true, text: () => Promise.resolve(JSON.stringify(responseJson)) });
                    } else {
                        // else return error
                        reject('Username or password is incorrect');
                    }

                    return;
                }

                // Create new item
                if (url.endsWith('/api/items') && opts.method === 'POST') {
                    // get parameters from post request
                    let params = JSON.parse(opts.body);


                    if (params.id) {
                        items = items.filter(item => params.id != item.id);

                        resolve({ ok: true, text: () => Promise.resolve(JSON.stringify(items)) });
                    } else {
                        // else return error
                        reject('Username or password is incorrect');
                    }

                    return;
                }

                // delete item
                if (url.endsWith('/api/items') && opts.method === 'DELETE') {
                    // get parameters from post request
                    let params = JSON.parse(opts.body);

                    if (params) {
                        // if params are valid return a fake item details
                        let responseJson = {
                            id: 4,
                            title: params.title,
                            text: params.text,
                            date: params.date,
                            link: '\\' + params.link
                        };
                        items.push(responseJson);
                        resolve({ ok: true, text: () => Promise.resolve(JSON.stringify(responseJson)) });
                    } else {
                        // else return error
                        reject('Username or password is incorrect');
                    }

                    return;
                }

                // get categories
                if (url.endsWith('/api/categories') && opts.method === 'GET') {

                    if (opts.headers && opts.headers.Authorization === 'Bearer fake-jwt-token') {
                        resolve({ ok: true, text: () => Promise.resolve(JSON.stringify(categories)) });
                    } else {
                        // return 401 not authorised if token is null or invalid
                        reject('Unauthorised');
                    }
                    return;
                }

                // get items
                if (url.endsWith('/api/items') && opts.method === 'GET') {

                    if (opts.headers && opts.headers.Authorization === 'Bearer fake-jwt-token') {
                        resolve({ ok: true, text: () => Promise.resolve(JSON.stringify(items)) });
                    } else {
                        // return 401 not authorised if token is null or invalid
                        reject('Unauthorised');
                    }
                    return;
                }

                // pass through any requests not handled above
                realFetch(url, opts).then(response => resolve(response));

            }, 500);
        });
    }
}