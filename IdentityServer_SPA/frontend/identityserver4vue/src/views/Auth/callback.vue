<template>
  <div>登陆成功，正在跳转。。。。。。</div>
</template>

<script>
export default {
  data() {
    return {}
  },
  created() {
    this.$authServices
      .callback()
      .then(res => {
        this.$router.push({ name: 'main' })
      })
      .catch(err => {
        console.log(err)
      })
  },
  mounted() {},
  methods: {},
  components: {}
}
</script>

<style></style>
