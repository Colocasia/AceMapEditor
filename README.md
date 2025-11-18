# Ace Map Editor

一个类似星际争霸2银河编辑器的地图编辑器。**引擎无关的核心架构**，支持Unity和未来的Godot。

## 项目结构

```
AceMapEditor/
├── Core/                          # 引擎无关核心库 (.NET Standard 2.1)
│   ├── AceMapEditor.Core.csproj
│   └── src/                       # 纯C#逻辑
│
├── Packages/
│   └── com.acemapeditor.unity/    # Unity UPM Package
│       ├── package.json
│       ├── Plugins/               # AceMapEditor.Core.dll
│       ├── Editor/                # Unity编辑器扩展
│       └── Runtime/               # Unity运行时
│
├── Build/                         # 编译脚本
│   └── build-core.sh
│
├── Releases/                      # 发布的DLL
│   └── netstandard2.1/
│       └── AceMapEditor.Core.dll
│
└── Docs/                          # 项目文档
```

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

### Unity用户
- Unity 2021.3 LTS 或更高版本
- Universal Render Pipeline (URP)
- Windows / macOS

### 开发者
- .NET SDK 6.0+ (编译Core库)

## 安装方式（Unity）

### 方式1：通过Git URL安装（推荐）

1. 打开Unity编辑器
2. 打开 `Window > Package Manager`
3. 点击 `+` 按钮，选择 `Add package from git URL...`
4. 输入：
   ```
   https://github.com/Colocasia/AceMapEditor.git?path=/Packages/com.acemapeditor.unity
   ```

### 方式2：本地Package安装

1. 克隆此仓库到本地
2. 在Unity项目中打开 `Packages/manifest.json`
3. 添加以下行：
```json
{
  "dependencies": {
    "com.acemapeditor.unity": "file:/path/to/AceMapEditor/Packages/com.acemapeditor.unity"
  }
}
```

### 方式3：复制到Packages目录

1. 克隆或下载此仓库
2. 将 `Packages/com.acemapeditor.unity/` 文件夹复制到Unity项目的 `Packages/` 目录下
3. Unity会自动识别并导入

## 快速开始

1. 安装插件后，通过 `Window > Ace Map Editor` 打开主编辑器窗口
2. 创建新地图或加载现有地图
3. 使用工具栏切换不同的编辑模式（地形/单位/触发器等）

## 架构设计

本项目采用**引擎分层架构**，核心逻辑与Unity完全解耦：

```
├── Core/                      # 引擎无关核心 (.NET Standard 2.1)
│   └── src/                   # 纯C#，无任何引擎依赖
│       ├── Interfaces/        # 引擎抽象接口
│       ├── Data/              # 数据模型
│       ├── Editor/            # 核心编辑器逻辑
│       └── Utilities/         # 工具类
│
└── Packages/
    └── com.acemapeditor.unity/  # Unity实现
        ├── Plugins/             # Core.dll
        ├── Editor/              # Unity适配器
        └── Runtime/             # Unity运行时

# 未来扩展
└── Godot/                     # Godot实现（计划中）
    └── addons/acemapeditor/
        ├── bin/               # 同样使用Core.dll
        └── editor/            # Godot适配器
```

**核心优势**：
- ✅ Core完全引擎无关（.NET Standard 2.1）
- ✅ 编译为DLL，Unity和Godot共享
- ✅ 核心逻辑只需编写一次
- ✅ 各引擎只需实现适配器层

详细文档请查看 `Docs/` 目录。

## 开发状态

当前版本：**v0.1.0-alpha**

项目处于早期开发阶段，核心功能正在实现中。

## 开发指南

### 编译Core库

```bash
# 使用编译脚本（推荐）
./Build/build-core.sh

# 或手动编译
cd Core/
dotnet build -c Release
```

编译后DLL会自动复制到：
- `Packages/com.acemapeditor.unity/Plugins/`
- `Releases/netstandard2.1/`

### 开发流程

1. 修改Core代码（纯C#）
2. 运行 `./Build/build-core.sh` 编译
3. Unity会自动重新加载DLL
4. 测试Unity适配器

## 文档

- [需求文档](Docs/需求文档.md) - 功能需求和开发阶段
- [项目文档](Docs/项目文档.md) - 技术架构和实现细节
- [代码规范](Docs/代码规范.md) - 代码规范和性能优化
- [Core README](Core/README.md) - Core库详细说明

## 许可证

[MIT License](LICENSE.md)

## 参考项目

- StarCraft II Galaxy Editor
- Warcraft III World Editor
- Unity Terrain Tools
