SimpleUpdater.NET
=======================

##简要说明

在发布应用程序时，我们经常会需要给自己的程序加上自动升级功能。.Net Framework自带的ClickOnce中有自动升级的功能，但是往往不太好用，比如必须用ClickOnce发布，安装的文件夹一个版本一个等等，我们会想要一个比较简单、甚至绿色软件也能使用的自动升级功能，这个自动升级程序就是基于这个目的而制作的。

 

为了让使用起来更加简单快捷，我对内置的功能进行了大幅度的精简和集成，最简单的情况下只需要你只需要一行代码即可实现自动更新，如下所示：

```c#
FSLib.App.SimpleUpdater.Updater.CheckUpdateSimple("http://localhost/update.xml");
```

内含测试项目&手册，更多信息请参阅下载分发包里的手册。

##发布和支持信息

**软件发布主页** [http://www.fishlee.net/soft/simple_autoupdater/](http://www.fishlee.net/soft/simple_autoupdater/)

**使用建议** 可以在 [自动更新库知识社区] _(推荐)_ 或 [后花园论坛] 发帖询问

**更新日志** [http://www.fishlee.net/soft/simple_autoupdater/#C-124](http://www.fishlee.net/soft/simple_autoupdater/#C-124)


[自动更新库知识社区]: http://ask.fishlee.net/category-19
[后花园论坛]: http://bbs.fishlee.net/