import Vue from 'vue';

import { store } from './_store';
import { router } from './_helpers';
import App from './App';

// setup fake backend
import { configureFakeBackend } from './_helpers';
configureFakeBackend();

import './../public/src/css/style.css'

new Vue({
    el: '#app',
    router,
    store,
    render: h => h(App)
});