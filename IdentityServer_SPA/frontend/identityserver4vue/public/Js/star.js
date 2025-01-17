var context
var arr = new Array()
var starCount = 1000
var rains = new Array()
var rainCount = 20
//初始化画布及context
function init() {
  //获取canvas
  // var stars = document.getElementById('stars')
  var stars = document.createElement('canvas')
  stars.height = window.innerHeight
  stars.width = window.innerWidth
  stars.setAttribute(
    'style',
    'position: fixed;left: 0;top: 0;pointer-events: none;z-index:2;height:100%'
  )
  stars.setAttribute('id', 'canvas_star')
  document.getElementsByTagName('body')[0].appendChild(stars)
  //获取context
  context = stars.getContext('2d')
}
//创建一个星星对象
var Star = function() {
  this.x = window.innerWidth * Math.random() //横坐标
  this.y = 5000 * Math.random() //纵坐标
  this.text = '。' //文本
  this.color = 'white' //颜色
  //产生随机颜色
  this.getColor = function() {
    var _r = Math.random()
    if (_r < 0.5) {
      this.color = '#333'
    } else {
      this.color = 'white'
    }
  }
  //初始化
  this.init = function() {
    this.getColor()
  }
  //绘制
  this.draw = function() {
    context.fillStyle = this.color
    context.fillText(this.text, this.x, this.y)
  }
}
//页面加载的时候

var starinit = function() {
  init()
  //画星星
  for (var i = 0; i < starCount; i++) {
    var star = new Star()
    star.init()
    star.draw()
    arr.push(star)
  }
  // 画流星
  // for (var c = 0; c < rainCount; c++) {
  //   var rain = new MeteorRain()
  //   rain.init()
  //   rain.draw()
  //   rains.push(rain)
  // }
  playStars() //绘制闪动的星星
  // playRains() //绘制流星
}
//画月亮
function drawMoon() {
  var moon = new Image()
  moon.src = './images/moon.jpg'
  context.drawImage(moon, -5, -10)
}

//星星闪起来
function playStars() {
  for (var n = 0; n < starCount; n++) {
    arr[n].getColor()
    arr[n].draw()
  }
  setTimeout(playStars, randomDistance(100, 500))
}
function randomDistance(max, min) {
  var distance = Math.floor(Math.random() * (max - min + 1) + min)
  return distance
}
function playRains() {
  var stars = document.getElementById('stars')
  // js随机生成流星
  for (var j = 0; j < 50; j++) {
    var newStar = document.createElement('div')
    newStar.className = 'star'
    newStar.style.top = randomDistance(300, -100) + 'px'
    newStar.style.left = randomDistance(2000, 200) + 'px'
    stars.appendChild(newStar)
  }
  var star = document.getElementsByClassName('star')

  // 给流星添加动画延时
  for (var i = 0, len = star.length; i < len; i++) {
    star[i].style.animationDelay = i % 6 == 0 ? '0s' : i * 0.8 + 's'
  }
}
//绘制流星
// function playRains() {
//   for (var n = 0; n < rainCount; n++) {
//     var rain = rains[n]
//     rain.move() //移动
//     if (rain.y > window.innerHeight) {
//       //超出界限后重来
//       context.clearRect(rain.x, rain.y - rain.height, rain.width, rain.height)
//       rains[n] = new MeteorRain()
//       rains[n].init()
//     }
//   }
//   setTimeout(playRains, 2)
// }
/*流星雨开始*/
var MeteorRain = function() {
  this.x = -1
  this.y = -1
  this.length = -1 //长度
  this.angle = 30 //倾斜角度
  this.width = -1 //宽度
  this.height = -1 //高度
  this.speed = 1 //速度
  this.offset_x = -1 //横轴移动偏移量
  this.offset_y = -1 //纵轴移动偏移量
  this.alpha = 0 //透明度
  this.color1 = '' //流星的色彩
  this.color2 = '' //流星的色彩
  /****************初始化函数********************/
  this.init = function() //初始化
  {
    this.getPos()
    this.alpha = 0.3 //透明度
    this.getRandomColor()
    //最小长度，最大长度
    var x = Math.random() * 30
    this.length = Math.ceil(x)
    // x = Math.random()*10+30;
    this.angle = 30 //流星倾斜角
    x = Math.random() + 0.5
    this.speed = Math.ceil(x) //流星的速度
    var cos = Math.cos((this.angle * 3.14) / 180)
    var sin = Math.sin((this.angle * 3.14) / 180)
    this.width = this.length * cos //流星所占宽度
    this.height = this.length * sin //流星所占高度
    this.offset_x = this.speed * cos
    this.offset_y = this.speed * sin
  }
  /**************获取随机颜色函数*****************/
  this.getRandomColor = function() {
    var r = Math.ceil(250 * Math.random()) //Math.ceil()对一个数进行上舍入。
    //中段颜色
    this.color1 = 'rgba(' + r + ',' + r + ',' + r + ',1)'
    //结束颜色
    this.color2 = '#1d2236'
  }
  /***************重新计算流星坐标的函数******************/
  this.countPos = function() //
  {
    //往左下移动,x减少，y增加
    this.x = this.x - this.offset_x
    this.y = this.y + this.offset_y
  }
  /*****************获取随机坐标的函数*****************/
  this.getPos = function() //
  {
    //横坐标200--1200
    this.x = Math.random() * window.innerWidth //窗口高度
    //纵坐标小于600
    this.y = Math.random() * window.innerHeight //窗口宽度
  }
  /****绘制流星***************************/
  this.draw = function() //绘制一个流星的函数
  {
    context.save()
    context.beginPath()
    context.lineWidth = 1 //宽度
    context.globalAlpha = this.alpha //设置透明度
    //创建横向渐变颜色,起点坐标至终点坐标
    var line = context.createLinearGradient(
      this.x,
      this.y,
      this.x + this.width,
      this.y - this.height
    )
    //分段设置颜色
    line.addColorStop(0, 'rgba(255,255,255,0.5)')
    line.addColorStop(0.4, this.color1)
    line.addColorStop(0.8, this.color2)
    context.strokeStyle = line
    //起点
    context.moveTo(this.x, this.y)
    //终点
    context.lineTo(this.x + this.width, this.y - this.height)
    context.closePath()
    context.stroke()
    context.restore()
  }
  this.move = function() {
    //清空流星像素
    var x = this.x + this.width - this.offset_x
    var y = this.y - this.height
    context.clearRect(x - 3, y - 3, this.offset_x + 5, this.offset_y + 5)
    // context.strokeStyle="red";
    // context.strokeRect(x,y-1,this.offset_x+1,this.offset_y+1);
    //重新计算位置，往左下移动
    this.countPos()
    //透明度增加
    this.alpha -= 0.002
    //重绘
    this.draw()
  }
}

/*流星雨结束*/
export default starinit
