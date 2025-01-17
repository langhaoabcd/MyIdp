module.exports = {
  root: true,
  env: {
    node: true
  },
  extends: ['plugin:vue/essential', 'eslint:recommended', '@vue/prettier'],
  parserOptions: {
    parser: 'babel-eslint'
  },
  rules: {
    quotes: [1, 'single'],
    'no-debugger': 0,
    'vue/no-parsing-error': ['off'],
    // 'prettier/prettier': [
    //   'error',
    //   {
    //     singleQuote: true,
    //     semi: false
    //   }
    // ],
    'no-unused-vars': 'off',
    'no-console': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
    'no-debugger': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
    'prettier/prettier': 'off'
  }
}
