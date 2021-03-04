<template>
  <div id="app">
    <el-container v-if="farmeIn" style="height:100%">
      <el-header><Header></Header></el-header>
      <el-container>
        <el-aside width="200px"><Aside></Aside></el-aside>
        <el-container>
          <el-main><router-view></router-view></el-main>
          <el-footer>Footer</el-footer>
        </el-container>
      </el-container>
    </el-container>
    <div v-else style="height:100%">
      <router-view></router-view>
    </div>
  </div>
</template>
<script>
import { get } from './utils/axios'
import Header from './components/Common/header'
import Aside from '@/components/Common/aside'
import { mapState } from 'vuex'
export default {
  // eslint-disable-next-line vue/no-unused-components
  components: { Header, Aside },
  data() {
    return {
      routeName: window.location.hash
    }
  },
  beforeCreate() {
    console.log(this.$route)
    var _this = this

    async function asyncMethod() {
      var routeName = window.location.hash
      if (
        routeName.indexOf('/login') == -1 &&
        routeName.indexOf('/callback') == -1 &&
        routeName.indexOf('/error') == -1 &&
        routeName.indexOf('/logout') == -1
      ) {
        _this.$store.commit('updateFarmeIn', true)
        var res = await _this.$authServices.getAuthState()
        _this.$store.commit('updateAuthState', res)
        if (res == false) {
          await _this.$authServices.login()
        }
      } else {
        _this.$store.commit('updateFarmeIn', false)
      }
    }
    asyncMethod()
  },
  created() {},
  mounted() {},
  computed: mapState(['farmeIn']),
  watch: {}
}
</script>
<style lang="scss">
html,
body,
#app {
  height: 100%;
  margin: 0px;
  padding: 0px;
}

.el-header {
  // box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
  border-bottom: 1px solid rgba(0, 0, 0, 0.1);
}
.el-aside {
  height: 100%;
}
</style>
