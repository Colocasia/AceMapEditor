# Changelog

所有重要的项目变更都会记录在此文件中。

格式基于 [Keep a Changelog](https://keepachangelog.com/zh-CN/1.0.0/)，
版本号遵循 [语义化版本](https://semver.org/lang/zh-CN/)。

## [Unreleased]

### 重大变更 - 架构完全重构
- **Core库作为.NET Standard 2.1类库**
  - Core不再是Unity Package，改为标准.NET项目
  - 编译为`AceMapEditor.Core.dll`，真正引擎无关
  - 移除所有Unity特定文件（.asmdef等）

- **Unity Package重命名和重构**
  - `com.colocasia.acemapeditor` → `com.acemapeditor.unity`
  - 结构：`Packages/com.acemapeditor.unity/`
  - Unity Package引用预编译的Core.dll（位于Plugins/）

- **DLL分发模式**
  - Core编译为DLL，Unity和Godot共享同一个DLL
  - 添加自动化构建脚本：`Build/build-core.sh`
  - DLL自动复制到Unity Package和Releases目录

### 新增
- Core项目文件：`Core/AceMapEditor.Core.csproj`
- 构建脚本：`Build/build-core.sh`
- Core README：详细的Core库说明文档
- Releases目录：存放编译后的DLL

### 目录结构变更
```
旧结构:
├── AceMapEditor.Core/
├── AceMapEditor.Unity/
└── package.json (根目录)

新结构:
├── Core/                       # .NET Standard 2.1项目
├── Packages/
│   └── com.acemapeditor.unity/ # Unity Package
├── Build/                      # 构建脚本
└── Releases/                   # 发布DLL
```

### 更新
- 完全重写README.md，反映新的DLL架构
- 更新所有文档的目录结构说明
- Unity安装地址变更为：`?path=/Packages/com.acemapeditor.unity`

### 计划中
- 地形编辑系统实现
- 单位放置系统
- 触发器编辑器
- 区域系统

## [0.1.0] - 2025-01-18

### 新增
- 项目初始化
- UPM Package配置
- 基础文档结构
  - 需求文档
  - 项目文档
  - 代码规范
- 目录结构规划
- 多层纹理融合系统设计

### 技术决策
- 采用UPM Package方式组织代码
- 使用URP渲染管线
- 自定义Mesh地形系统（不使用Unity Terrain）
- 分层架构设计（5层：UI、表现层、业务逻辑、引擎抽象、Unity实现）
- 高性能C#编程（struct、指针、NativeArray）

---

## 版本说明

- **[Unreleased]**: 未发布的开发中功能
- **[0.1.0]**: 项目初始化版本
