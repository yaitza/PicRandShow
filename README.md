# PicRandShow
Picture Random Show 用于随机或者固定展示图片信息，并在展示过程中不断变换图片位置；主要用于人脸识别测试。

### Single Mode
![Single](/Resource/Single.gif)

### Multiple Mode
![Multiple](/Resource/Multiple.gif)

### Random Mode
![Random](/Resource/Random.gif)

## Settings
相关模式的设置主要采用配置文件的方式，对文件PicRandShow.exe.config进行相关设置，可以达到目前支持的三种效果展示。  
![Setting](/Resource/Setting.png)

## Coding
主要采用容器Panel添加PictureBox实现的图片的展示的功能。

## ToDo
- 需要进一步了解委托，事件的相关原理以及实现方式。
- 需要了解线程的原理，以及实际实现细节。

[详见](https://yaitza.github.io/2017-08-20-CSharp-PicRandShow）。