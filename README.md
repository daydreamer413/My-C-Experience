

# Windows的超大图标模式图片展示-自定义控件

  .net中展示控件listview无法实现“直接在运行时窗体动态添加图片”、“选择一行最多展示的图片数量并自动跳整大小”、“鼠标进入时显示浅色底框”、“选中时显示深色底框”、“悬停时显示图片的详细信息包括完整名称、类型、分辨率、大小”、“导出所选中的图片并进行下一步操作”等功能，本项目流畅且美观地实现了上述功能。
<br />

 本篇README.md面向开发者
 
## 目录

- [版本参考](#版本参考)
- [使用说明](#使用说明)
  - [导入图片](#导入图片)
  - [选择展示图片的大小](#选择展示图片的大小)
  - [进入框、详情框、选中框](#进入框、详情框、选中框)
  - [导出被选中的图片](#导出被选中的图片)
- [作者](#作者)


###### 版本参考

1. Visual Studio
2. .Net Framework 4.7.2

###### 使用说明
自定义控件命名为：newListView
###### **1. 导入图片**
   通过在主窗口上自行布置按钮，如本项目示例窗口中的”添加图片“按钮，并在按钮内调用接口函数CreateDraw()实现导入功能。运行时点击该按钮，会自动根据选中图片的数量进行PictureBox的动态创建和属性绑定。
   
###### **2. 选择展示图片的大小**
   在控件属性中添加了LineCount，可输入数字2～6，表示一行展示的图片数量。数字越大，图片越小。
   
###### **3. 进入框、详情框、选中框**
   所有框的显示效果均采用双缓冲绘制，视觉上更加流畅且迅速。
   鼠标进入时，图片底框呈半透明淡蓝色
   ![image](https://github.com/daydreamer413/My-C-Experience/Study-控件/showimage.jpg)
   鼠标悬停时，显示详情框，包含图片的完整名称、类型、分辨率、大小信息
   鼠标点击时，图片底框呈半透明蓝色，表示图片被选中；再次点击时，取消底框
###### **4. 导出被选中的图片**
   通过自行调用接口函数Selected_Image()，导出被选中的图片

### 作者

Github:daydreamer413 
CSDN:白日梦想家413

### 版权说明
使用时请引用本项目链接：
商用请联系2739214278@qq.com

<!-- links -->
[your-project-path]:shaojintian/Best_README_template
[contributors-shield]: https://img.shields.io/github/contributors/shaojintian/Best_README_template.svg?style=flat-square
[contributors-url]: https://github.com/shaojintian/Best_README_template/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/shaojintian/Best_README_template.svg?style=flat-square
[forks-url]: https://github.com/shaojintian/Best_README_template/network/members
[stars-shield]: https://img.shields.io/github/stars/shaojintian/Best_README_template.svg?style=flat-square
[stars-url]: https://github.com/shaojintian/Best_README_template/stargazers
[issues-shield]: https://img.shields.io/github/issues/shaojintian/Best_README_template.svg?style=flat-square
[issues-url]: https://img.shields.io/github/issues/shaojintian/Best_README_template.svg
[license-shield]: https://img.shields.io/github/license/shaojintian/Best_README_template.svg?style=flat-square
[license-url]: https://github.com/shaojintian/Best_README_template/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=flat-square&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/shaojintian




