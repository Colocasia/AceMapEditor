# Ace Map Editor

一个类似星际争霸2银河编辑器的Unity地图编辑器插件。

## 功能特性

- **地形编辑系统**
  - 自定义Mesh地形（非Unity Terrain）
  - 多层纹理混合（基于Splatmap）
  - 高度编辑工具（升高/降低/平滑）
  - 基于高度的智能纹理混合

- **单位系统**
  - 单位放置和管理
  - 属性编辑
  - 分组功能

- **触发器系统**
  - 事件-条件-动作编程模型
  - 变量系统（全局/局部）
  - 可扩展的触发器库

- **区域系统**
  - 矩形/圆形/多边形区域
  - 区域事件支持

- **数据编辑器**
  - ScriptableObject数据配置
  - 单位/技能/装饰物定义

## 系统要求

- Unity 2021.3 LTS 或更高版本
- Universal Render Pipeline (URP)
- Windows / macOS

## 安装方式

### 方式1：通过Git URL安装

1. 打开Unity编辑器
2. 打开 `Window > Package Manager`
3. 点击 `+` 按钮，选择 `Add package from git URL...`
4. 输入：`https://github.com/yourusername/AceMapEditor.git`

### 方式2：本地Package安装

1. 将此仓库克隆或下载到本地
2. 在Unity项目中打开 `Packages/manifest.json`
3. 添加以下行：
```json
{
  "dependencies": {
    "com.colocasia.acemapeditor": "file:/path/to/AceMapEditor"
  }
}
```

### 方式3：复制到Packages目录

1. 将整个 `AceMapEditor` 文件夹复制到Unity项目的 `Packages/` 目录下
2. Unity会自动识别并导入

## 快速开始

1. 安装插件后，通过 `Window > Ace Map Editor` 打开主编辑器窗口
2. 创建新地图或加载现有地图
3. 使用工具栏切换不同的编辑模式（地形/单位/触发器等）

## 架构设计

本插件采用分层架构设计，核心逻辑与Unity解耦，便于未来移植到其他引擎（如Godot）：

```
├── Core/           # 引擎无关的核心逻辑
├── Editor/         # Unity编辑器扩展
├── Runtime/        # 运行时代码
├── Shaders/        # URP Shader
└── Resources/      # 资源定义
```

详细文档请查看 `Docs/` 目录。

## 开发状态

当前版本：**v0.1.0-alpha**

项目处于早期开发阶段，核心功能正在实现中。

## 文档

- [需求文档](Docs/需求文档.md)
- [项目文档](Docs/项目文档.md)
- [代码规范](Docs/代码规范.md)

## 许可证

[MIT License](LICENSE.md)

## 参考项目

- StarCraft II Galaxy Editor
- Warcraft III World Editor
- Unity Terrain Tools
