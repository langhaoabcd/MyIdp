<template>
  <div>
    <h1>dashborad</h1>
    <div style="word-break: break-word;">
    <p>{{clamins}}</p>
    </div>
  </div>
  
</template>

<script>
import authService from '../assets/coustomJs/auth'
export default {
  components: {},
  props: {},
  data() {
    return {
      clamins: '',
    }
  },
  created() {
    this.getUser();
  },
  methods: {
    async getUser(){
      var res = await authService.getUser();
      this.clamins = JSON.stringify(res);
    }
  },
}
</script>
<style scoped></style>
