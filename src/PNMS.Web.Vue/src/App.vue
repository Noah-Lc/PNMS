<template>
  <main-layout>
    <router-view />
  </main-layout>
</template>

<script>
  import MainLayout from './layouts/Main.vue'

export default {
  components: {
    MainLayout
  }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
