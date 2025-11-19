// UnityEditorUIAdapter.cs
// Unity编辑器UI适配器实现
// Author: AceMapEditor Team
// Date: 2025-01-19

using UnityEditor;
using UnityEngine;
using AceMapEditor.Core.Interfaces;

namespace AceMapEditor.Unity.Editor.Adapters
{
    /// <summary>
    /// Unity编辑器UI适配器
    /// </summary>
    public class UnityEditorUIAdapter : IEditorUIAdapter
    {
        public string? ShowOpenFileDialog(string title, string extension)
        {
            string path = EditorUtility.OpenFilePanel(title, "", extension);
            return string.IsNullOrEmpty(path) ? null : path;
        }

        public string? ShowSaveFileDialog(string title, string defaultName, string extension)
        {
            string path = EditorUtility.SaveFilePanel(title, "", defaultName, extension);
            return string.IsNullOrEmpty(path) ? null : path;
        }

        public void ShowMessage(string title, string message, MessageType type = MessageType.Info)
        {
            switch (type)
            {
                case MessageType.Info:
                    EditorUtility.DisplayDialog(title, message, "OK");
                    break;

                case MessageType.Warning:
                    Debug.LogWarning($"{title}: {message}");
                    EditorUtility.DisplayDialog(title, message, "OK");
                    break;

                case MessageType.Error:
                    Debug.LogError($"{title}: {message}");
                    EditorUtility.DisplayDialog(title, message, "OK");
                    break;
            }
        }

        public bool ShowConfirmDialog(string title, string message, string okButton = "OK", string cancelButton = "Cancel")
        {
            return EditorUtility.DisplayDialog(title, message, okButton, cancelButton);
        }

        public void LogInfo(string message)
        {
            Debug.Log($"[AceMapEditor] {message}");
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning($"[AceMapEditor] {message}");
        }

        public void LogError(string message)
        {
            Debug.LogError($"[AceMapEditor] {message}");
        }

        public void MarkDirty()
        {
            // Unity编辑器会自动处理重绘
            EditorUtility.SetDirty(Selection.activeObject);
        }
    }
}
