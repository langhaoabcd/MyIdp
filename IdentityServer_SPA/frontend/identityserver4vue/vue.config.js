const CopyWebpackPlugin = require('copy-webpack-plugin')
module.exports = {
  configureWebpack: {
    plugins: [
      new CopyWebpackPlugin([
        { from: 'node_modules/oidc-client/dist/oidc-client.min.js', to: 'js' }
      ])
    ]
  },
  publicPath: './',
  pluginOptions: {
    'style-resources-loader': {
      preProcessor: 'scss',
      patterns: []
    }
  },
  css: {
    loaderOptions: {
      sass: {
        prependData: '@import "@/assets/css/common.scss";'
      }
    }
  }
}
