Svn日志管理

背景
====

公司的项目比较多，代码管理工具用的是svn，管理者有下列需求：

1.  查看各个项目当前的开发情况，包括多少人正在开发哪些项目，查看某个员工最近在开发哪些项目，提交情况，检查代码质量等；

2.  代码提交后，需要自动发布到测试服务器；

技术
====

需要把svn的日志增量同步到数据库，同步频率控制在5分钟一次，由于sharpsvn的类库不支持dotnet
core版本，只好采用的是用vs2010开发windows服务；前端用vs code 2017
开发vue；后端用vs 2017开发dotnet core
webApi，在linux下部署，自动发布用持续集成特性的jenkins部署，后端调用jenkins的api实现自动发布。

同步Svn日志
===========

>   每次读上次同步到哪个版本，调用sharpSvn的类库，读取日志实现增量更新

前端
====

在vue-admin-template的基础上快速开发，使用了vue全家桶、ElementUI、axios、\@riophae/vue-treeselect、babel-polyfill等。

代码打包后放到linux上，用nginx负载均衡。

Api
===

>   基于Dotnet Core开发的WebApi，使用Dapper、Swagger等技术，docker
>   私有仓库，容器化部署，jenkins持续集成（获取代码，编译，推到仓库，测试服务器拉取镜像，运行容器），调用jenkins
>   api实现一键发布。

6 注意
======

项目里的数据库链接字符串，svn账号，项目的svn地址等请根据自己的情况修改

7效果图
=======

![](media/aaa28660dbc15476215fd66240908edb.png)

![](media/b9c7dfbe22e05565f2bdbbccab8b3bfb.png)

![](media/8f22b1754f967c54be4c90852456c6f4.png)

![](media/9818b8e13f4b1833ef00a1ce2c66f5c9.png)

![](media/7074b31d1e7cd66b460d1f15ac823a1a.png)

![](media/5bb5de06b4d258eba98ba8f4944b931c.png)

![](media/f09b2318b77b7d53146d86edeb951a83.png)

![](media/afd2bcad8a51fedbd38589d0a3846638.png)

![](media/61611e1a1e9622a49ce421390a1ff7cb.png)

![](media/a882d2e29d3a4c41a43b4a11a0155758.png)

![](media/51bb536bba6d6997b9ed8b075cd6ba2d.png)

![](media/40d75df9f5662a135f0a3ee237500bd4.png)

![](media/a322c26af796c6c1dde8dc5db45e8a7b.png)

![](media/d96a2a43923c99e97627e5baedb02179.png)

![](media/b6e8957782bf2125d3ea1cca676c9715.png)

![](media/e50d32f1190f09078ed992933ef2eb78.png)
