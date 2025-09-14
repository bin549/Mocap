using SFB;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FolderUtils : MonoBehaviour {
    public static string CheckDirectory(string path) {
        return Directory.Exists(path) ? GetDirectoryPath(path) : null;
    }

    public static DirectoryInfo SafeCreateDirectory(string path) {
        return Directory.Exists(path) ? null : Directory.CreateDirectory(path);
    }

    public static string SelectFolder() {
        var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", Application.dataPath, true);
        string path = "";
        foreach (var p in paths) {
            path += p;
        }
        return path;
    }

    public static string[] GetFilterdFiles(string directory, string[] extensions) {
        if (string.IsNullOrEmpty(directory)) {
            directory = Environment.CurrentDirectory;
        }
        
        Debug.Log($"FolderUtils.GetFilterdFiles - 查找目录: {directory}");
        Debug.Log($"FolderUtils.GetFilterdFiles - 文件扩展名: {string.Join(", ", extensions)}");
        
        // 确保目录存在
        if (!Directory.Exists(directory)) {
            Debug.LogWarning($"Directory does not exist: {directory}");
            return new string[0];
        }
        
        DirectoryInfo mydir = new DirectoryInfo(directory);
        FileInfo[] f = mydir.GetFiles();
        Debug.Log($"目录中找到 {f.Length} 个文件");
        
        List<FileInfo> f2 = new List<FileInfo>();
        List<string> f3 = new List<string>();
        
        foreach (FileInfo file in f) {
            Debug.Log($"检查文件: {file.Name} (扩展名: {file.Extension})");
            foreach (var extension in extensions) {
                if (file.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase)) {
                    Debug.Log($"匹配到文件: {file.Name}");
                    f2.Add(file);
                    break; // 避免重复添加同一个文件
                }
            }
        }
        
        Debug.Log($"找到 {f2.Count} 个匹配的文件");
        
        foreach (FileInfo file in f2) {
            // 使用完整路径，确保跨平台兼容性
            f3.Add(file.FullName);
        }
        
        return f3.ToArray();
    }

    private static string GetDirectoryPath(string directory) {
        var directoryPath = Path.GetFullPath(directory);
        if (!directoryPath.EndsWith(Path.DirectorySeparatorChar.ToString())) {
            directoryPath += Path.DirectorySeparatorChar;
        }
        if (Path.GetPathRoot(directoryPath) == directoryPath) {
            return directory;
        }
        return Path.GetDirectoryName(directoryPath) + Path.DirectorySeparatorChar;
    }
}
