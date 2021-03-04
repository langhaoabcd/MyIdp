# IdentityServer4 for SPA UI
IdentityServer前后端分离示例，实现账号密码登录，短信登录，第三方登录


### 导图
![RUNOOB 图标](https://img2018.cnblogs.com/blog/1468246/201903/1468246-20190306172030731-1688008591.png)
![RUNOOB 图标](https://github.com/KenWang007/IdentityServer4Demo/raw/master/IdentityServer4Flow.jpg)
![RUNOOB 图标](https://github.com/KenWang007/IdentityServer4Demo/raw/master/IdentityServer4Features.jpg)

### 配置IdentityServer
- 配置数据和操作数据持久化 ok

- 使用idp保护Ocelot网关 ok
  ![RUNOOB 图标](https://user-images.githubusercontent.com/1147445/97865027-991b7200-1d1a-11eb-927e-3f5580a7f5b5.png)

- 使用自定义的用户表 ok

- 自定义授权模式    
  扩展短信登录 ok    
  第三方登录(应自动注册) ok  
 
- idp登录页前后端分离实现 ok

- 正式环境配置证书

- 生产环境必须使用https  
 https://blog.csdn.net/qianfeng_dashuju/article/details/82703180  

### 配置前后端分离vue的登录页
* 单点登录 ok   
* 单点登出  



### 配置客户端
添加oidc   
services.AddAuthentication().AddOpenIdConnect();

### 配置受保护的资源
添加jwtbearer  
services.AddAuthentication().AddJwtBearer();

### 思考
- 如何让idp生成的token在spring boot等其他语言的api中解析验证？   
  方法1.使用自省端点(introspection_endpoint)在线验证        
  方法2.使用jwks端点(jwks_uri)可本地验证(推荐)     
- 数据库中，添加客户端，和资源，授权数据,要操作哪些表？   
- 如何登入，登出不跳到中间页面，直接跳转到目标页？   
- 在登录页第三方登录中接入其他idp实现oauth    
- swagger如何集成idp?   
