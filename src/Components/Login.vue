<template>
    <div class="page login-page">
        <div class="container">
            <div class="form-outer text-center d-flex align-items-center">
                <div class="form-inner">
                    <div class="logo text-uppercase">
                        <span>PNMS </span>
                        <strong class="text-primary">Dashboard</strong>
                    </div>
                    <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud.</p>
                    <div v-if="alert.message" class="my-2" :class="`alert ${alert.type}`">{{alert.message}}</div>
                    <form @submit.prevent="handleSubmit" class="text-left form-validate">
                        <div class="form-group-material">
                            <input id="login-username" v-model="username" placeholder="Username" type="text" name="loginUsername" :class="{ 'is-invalid': submitted && !validateUsername(username) }" class="input-material">
                        </div>
                        <div class="form-group-material">
                            <input id="login-password" v-model="password" placeholder="Password" type="password" name="loginPassword" class="input-material"  :class="{ 'is-invalid': submitted && !validatePassword(password) }">
                        </div>
                        <div class="form-group text-center">
                            <button id="login" type="submit" :disabled="loggingIn" class="btn btn-primary">
                            Login <span v-show="loggingIn" class="loader"></span>
                            </button>
                            <!-- This should be submit button but I replaced it with <a> for demo purposes-->
                        </div>
                    </form><small>Do not have an account? </small><a href="/register" class="signup">Signup</a>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    data () {
        return {
            username: '',
            password: '',
            submitted: false
        }
    },
    computed: {
        loggingIn () {
            return this.$store.state.authentication.status.loggingIn;
        },
        alert () {
            return this.$store.state.alert
        }
    },
    created () {
        // reset login status
        this.$store.dispatch('authentication/logout');
    },
    methods: {
        handleSubmit (e) {
            this.submitted = true;
            const { username, password } = this;
            const { dispatch } = this.$store;
            if (username && password) {
                dispatch('authentication/login', { username, password });
            }
        },
        validateUsername(usernname){
          var re = /^[0-9a-zA-Z_.-]+$/;
          return re.test(usernname);
        },
        validatePassword(){
          const { password } = this;
          return password.length >= 4;
        }
    },
    watch:{
        $route (to, from){
            // clear alert on location change
            this.$store.dispatch('alert/clear');
        }
    } 
};
</script>