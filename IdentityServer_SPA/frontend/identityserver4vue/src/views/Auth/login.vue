<template>
  <div class="homePage">
    <h1 style="text-align:center">Sigin</h1>
    <div class="loginForm">
      <el-tabs v-model="activeName" @tab-click="handleClick">
        <el-tab-pane label="账户密码登录" name="first">
          <el-form
          ref="loginForm"
          label-position="top"
          :model="loginForm"
          label-width="200px"
        >
            <el-form-item label="用户名:">
              <el-input v-model="loginForm.Username"></el-input>
            </el-form-item>
            <el-form-item label="密码:">
              <el-input type="Password" v-model="loginForm.Password"></el-input>
            </el-form-item>
            <el-form-item>
              <el-checkbox v-model="loginForm.RememberLogin">记住我</el-checkbox>
            </el-form-item>
            <div class="button">
              <el-button style="width:100%" type="primary" @click="login(false)">登录</el-button>
            </div>
          </el-form>
        </el-tab-pane>
        <el-tab-pane label="手机号登录" name="second">
          <el-form
          ref="loginForm2"
          label-position="top"
          :model="loginForm"
          label-width="200px"
        >
            <el-form-item label="手机号:">
              <el-input v-model="loginForm.Phone"></el-input>
            </el-form-item>
            <el-form-item label="验证码:">
              <el-input v-model="loginForm.Code"></el-input>
            </el-form-item>
            <div class="button">
              <el-button style="width:100%" type="primary" @click="login(false)">登录</el-button>
            </div>
        </el-form>
        </el-tab-pane>
      </el-tabs>
      <div style="display: flex;
    justify-content: space-evenly;">
        <el-link style="color:blueviolet" @click="thirdLogin()">GitHub登录</el-link>
        <!-- <el-link style="color:blueviolet" @click="thirdIdsLogin()">IDS4登录</el-link> -->
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: '',
  data() {
    return {
      activeName: 'first',
      loginForm: {
        Username: 'bob',
        Password: 'b1',
        ReturnUrl: '',
        RememberLogin: false,
        Cancle: false,
        Phone: '15966662345',
        Code: '123456',
      }
    }
  },
  created() {},
  mounted() {
    const url = this.$route.query.ReturnUrl
    this.loginForm.ReturnUrl = url
  },
  methods: {
    handleClick() {},
    login(cancle) {
      this.loginForm.Cancle = cancle
      if(this.activeName === 'first'){
        this.$api.account.login(this.loginForm).then(res => {
          window.location.href = res.data
        })
      } else {
        this.$api.account.loginsms(this.loginForm).then(res => {
          window.location.href = res.data
        })
      }
    },
    thirdLogin() {
      const baseUrl = Vue.prototype.$config.serveUrl + '/api'
      location.href = baseUrl + "/account/ExternalLogin?scheme=GitHub&returnUrl="+escape(this.loginForm.ReturnUrl) ;
    },
    thirdIdsLogin() {

    }
  }
}
</script>

<style lang="scss">
.loginForm {
  padding: 20px;
  border-radius: $baseRadius;
  background-image: linear-gradient(#d27a8a, #f89c91);
  opacity: 0.7;
  label {
    float: left !important;
    font-weight: bold;
    font-size: 16px;
  }
  h1 {
    color: $deepColor;
  }
  width: 400px;
}
.homePage {
  height: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background-image: url('../../../public/Images/欢迎界面2.jpg');
  .button {
    display: flex;
    align-items: center;
    justify-content: center;
  }
}
</style>
