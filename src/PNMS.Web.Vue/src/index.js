import Vue from 'vue';

import { store } from './Store';
import { router } from './Routes';
import App from './App';

// setup fake backend
/*import { configureFakeBackend } from './Helpers';
configureFakeBackend();*/

import './../public/src/css/style.css'

new Vue({
    el: '#app',
    router,
    store,
    render: h => h(App)
});