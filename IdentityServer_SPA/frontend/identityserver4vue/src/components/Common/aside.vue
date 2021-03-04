<template>
  <div class="aside">
    <el-menu default-active="1-1">
      <el-submenu index="1">
        <template slot="title">
          <i class="el-icon-location"></i>
          <span>用户管理</span>
        </template>
        <el-menu-item index="1-1"><router-link to="/sysuser">用户列表</router-link></el-menu-item>
        <el-menu-item index="1-2"><router-link to="/clamins">声明列表</router-link></el-menu-item>
      </el-submenu>
    </el-menu>
  </div>
</template>

<script>
export default {
  data() {
    return {}
  },
  methods: {},
  components: {}
}
</script>
<style lang="scss">
.aside {
  height: 100%;
  .el-menu {
    height: 100%;
  }
}

a {
  text-decoration: none;
  color: black;
}
.router-link-active {
  text-decoration: none;
}
</style>
