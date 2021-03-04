<template>
  <div class="header">
    <el-row type="flex">
      <el-col :span="4">
        <h3 style="text-align:center">
          <router-link to="/">身份认证管理中心</router-link></h3>
      </el-col>
      <el-col :span="2" :push="17">
        <el-menu class="el-menu-demo" mode="horizontal">
          <el-submenu index="2-4">
            <template slot="title">
              <el-avatar
                size="medium"
                src="https://cube.elemecdn.com/3/7c/3ea6beec64369c2642b92c6726f1epng.png"
              ></el-avatar>
              {{name}}</template
            >
            <!-- <el-menu-item index="2-4-1">个人信息</el-menu-item> -->
            <el-menu-item index="2-4-2" @click="logout">登出</el-menu-item>
          </el-submenu>
        </el-menu>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import authService from '../../assets/coustomJs/auth'
export default {
  data() {
    return {
      name: '',
    }
  },
  created() {
    this.getUser();
  },
  methods: {
    logout() {
      authService.logout()
    },
    async getUser(){
      var res = await authService.getUser();
      console.log('user',res);
      this.name = res.profile.name
    }
  },
  components: {}
}
</script>

<style lang="scss">
.header {
  .el-menu::after {
    display: none;
  }
}
</style>
