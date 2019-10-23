import Vue from 'vue';
import Router from 'vue-router';

import HomePage from './../components/Home.vue'
import Dashboard from '../components/Dashboard.vue'
import Register from '../components/Register.vue'
import Login from '../components/Login.vue'

Vue.use(Router);

export const router = new Router({
  mode: 'history',
  routes: [
    { path: '/Dashboard', name: 'Dashboard' ,component: Dashboard },
    { path: '/register', name: 'Register', component: Register },
    { path: '/login', name: 'Login' ,component: Login },
    // otherwise redirect to home
    { path: '*', redirect: '/Dashboard' }
  ]
});

router.beforeEach((to, from, next) => {
  // redirect to login page if not logged in and trying to access a restricted page
  const publicPages = ['/login', '/register'];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem('user');

  if (authRequired && !loggedIn) {
    return next('/login');
  }

  next();
})