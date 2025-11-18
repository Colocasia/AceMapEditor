# Changelog

所有重要的项目变更都会记录在此文件中。

格式基于 [Keep a Changelog](https://keepachangelog.com/zh-CN/1.0.0/)，
版本号遵循 [语义化版本](https://semver.org/lang/zh-CN/)。

## [Unreleased]

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
