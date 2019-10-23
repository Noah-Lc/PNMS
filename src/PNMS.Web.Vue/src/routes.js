import Dashboard from './components/Dashboard.vue';
import Register from './components/Register.vue';
import Login from './components/Login.vue';
import NotFound from './pages/404.vue';

const routes = [
    { path: '/', name: 'Dashboard' ,component: Dashboard },
    { path: '/register', name: 'Register', component: Register },
    { path: '/login', name: 'Login' ,component: Login },
    { path: '/404', name: 'NotFound', component: NotFound },  
    { path: '*', redirect: '/404' }, 
];

export default routes;
