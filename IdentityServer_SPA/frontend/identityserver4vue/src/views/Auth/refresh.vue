<template>
  <div></div>
</template>

<script>
export default {
  data() {
    return {}
  },
  created() {
    this.$authServices
      .refresh()
      .then(() => {
        console.log('刷新成功')
      })
      .catch(err => {
        console.log(err)
      })
  },
  methods: {},
  components: {}
}
</script>

<style></style>
