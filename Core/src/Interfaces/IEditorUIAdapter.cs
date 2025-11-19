// AceMapEditor/Core/src/Interfaces/IEditorUIAdapter.cs
// 编辑器UI适配器接口
// Author: AceMapEditor Team
// Date: 2025-01-19

using System;

namespace AceMapEditor.Core.Interfaces
{
    /// <summary>
    /// 编辑器UI适配器接口
    /// 抽象不同引擎的UI系统
    /// </summary>
    public interface IEditorUIAdapter
    {
        /// <summary>
        /// 显示文件打开对话框
        /// </summary>
        /// <param name="title">对话框标题</param>
        /// <param name="extension">文件扩展名（如"acemap"）</param>
        /// <returns>选择的文件路径，如果取消则返回null</returns>
        string? ShowOpenFileDialog(string title, string extension);

        /// <summary>
        /// 显示文件保存对话框
        /// </summary>
        /// <param name="title">对话框标题</param>
        /// <param name="defaultName">默认文件名</param>
        /// <param name="extension">文件扩展名</param>
        /// <returns>保存的文件路径，如果取消则返回null</returns>
        string? ShowSaveFileDialog(string title, string defaultName, string extension);

        /// <summary>
        /// 显示消息对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">消息内容</param>
        /// <param name="type">消息类型</param>
        void ShowMessage(string title, string message, MessageType type = MessageType.Info);

        /// <summary>
        /// 显示确认对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">消息内容</param>
        /// <param name="okButton">确定按钮文本</param>
        /// <param name="cancelButton">取消按钮文本</param>
        /// <returns>是否点击确定</returns>
        bool ShowConfirmDialog(string title, string message, string okButton = "OK", string cancelButton = "Cancel");

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志消息</param>
        void LogInfo(string message);

        /// <summary>
        /// 记录警告
        /// </summary>
        /// <param name="message">警告消息</param>
        void LogWarning(string message);

        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="message">错误消息</param>
        void LogError(string message);

        /// <summary>
        /// 标记编辑器需要重绘
        /// </summary>
        void MarkDirty();
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>信息</summary>
        Info,
        /// <summary>警告</summary>
        Warning,
        /// <summary>错误</summary>
        Error
    }
}
