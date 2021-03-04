<template>
  <div class="logout">正在登出。。。。。。</div>
</template>

<script>
export default {
  data() {
    return {
      logoutModel: {
        logoutId: ''
      }
    }
  },
  methods: {},
  created() {
    if (!this.logoutModel.logoutId) {
      var logoutId = this.$route.query.logoutId
      this.logoutModel.logoutId = logoutId
      this.$api.account.logout(this.logoutModel).then(res => {
        this.$router.push({ name: 'login' })
      })
    }
  },
  mounted() {},
  components: {}
}
</script>

<style></style>
