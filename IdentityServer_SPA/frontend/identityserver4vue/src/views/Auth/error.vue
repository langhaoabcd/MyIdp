<template>
  <div>
    <h3>参数错误</h3>
    <p>
      {{ errMsg }}
    </p>
    <el-link @click="backhome" target="_blank">回到首页</el-link>
  </div>
</template>

<script>
export default {
  data() {
    return {
      errMsg: ''
    }
  },
  mounted() {
    // debugger
    let id = this.$route.query.errorId
    const obj = {
      errorId: id
    }
    this.$api.account.getError(obj).then(res => {
      console.log('error:', res)
      this.errMsg = res.data
    })
  },
  methods: {
    backhome() {
      this.$router.push({ name: 'main' })
    }
  },
  components: {}
}
</script>

<style></style>
