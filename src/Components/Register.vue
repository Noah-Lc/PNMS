<template>
    <div class="page login-page">
      <div class="container">
        <div class="form-outer text-center d-flex align-items-center">
          <div class="form-inner">
            <div class="logo text-uppercase">
                <span>PNMS </span>
                <strong class="text-primary">Express</strong>
            </div>
            <p>Get started with your free account to manage PNMS</p>
             <div v-if="alert.message" class="my-2" :class="`alert ${alert.type}`">{{alert.message}}</div>
            <form @submit.prevent="handleSubmit" class="text-left form-validate">
              <div class="form-group-material">
                <input id="register-name" type="text" name="registerName" placeholder="Full name" v-model="fullname" class="input-material" :class="{ 'is-invalid': submitted && !validateName(fullname) }">
              </div>
              <div class="form-group-material">
                <input id="register-username" type="text" name="registerUsername" placeholder="Username" v-model="username" class="input-material" :class="{ 'is-invalid': submitted && !validateUsername(username) }">
              </div>
              <div class="form-group-material">
                <input id="register-password" type="password" name="registerPassword" placeholder="Password" v-model="password" class="input-material" :class="{ 'is-invalid': submitted && !validatePassword() }">
              </div>
              <div class="form-group-material">
                <input id="register-password" type="password" name="registerPassword" placeholder="Confirm Password" v-model="confirmPassword" class="input-material" :class="{ 'is-invalid': submitted && !validatePassword() }">
              </div>
              <div class="form-group terms-conditions text-center">
                <input id="register-agree" name="registerAgree" type="checkbox" value="1" v-model="agree" class="form-control-custom" :class="{ 'is-invalid': submitted && !agree }">
                <label for="register-agree">I agree with the terms and policy</label>
              </div>
              <div class="form-group text-center">

                <button id="register" type="submit" :disabled="registerIn" class="btn btn-primary">
                  Login <span v-show="registerIn" class="loader"></span>
                </button>

              </div>
            </form><small>Already have an account? </small><a href="/login" class="signup">Login</a>
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
            confirmPassword: '',
            fullname: '', 
            agree: false,
            submitted: false
        }
    },
    computed: {
       registerIn () {
            return this.$store.state.authentication.status.registerIn;
        },
        alert () {
            return this.$store.state.alert
        },
    },
    created () {
        // reset login status
        this.$store.dispatch('authentication/logout');
    },
    methods: {
        handleSubmit (e) {
            this.submitted = true;
            const { username, password, confirmPassword, fullname, agree } = this;
            const { dispatch } = this.$store;
            const validPassword = confirmPassword && confirmPassword === password;
            if (this.validateUsername(username) && this.validatePassword() && this.validateName(fullname) && agree) {
              this.submitted = false;
              dispatch('authentication/register', { username, password, fullname });
            }
        },
        validateEmail(email) {
          var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
          return re.test(email);
        },
        validatePassword(){
          const { password, confirmPassword } = this;
          return password.length >= 6 && confirmPassword === password
        },
        validateUsername(username){
          var re = /^[0-9a-zA-Z_.-]+$/;
          return re.test(username);
        },
        validateName(name){
          var re = /^[a-zA-Z]([-']?[a-zA-Z]+)*( [a-zA-Z]([-']?[a-zA-Z]+)*)+$/;
          return re.test(name) && name.length > 6;
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

<style>
.form-inner{
  min-width: 550px;
}
</style>