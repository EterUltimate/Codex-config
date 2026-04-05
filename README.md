# Codex 配置工具

一个用于管理 Codex API 配置的 Windows Forms 应用程序。
<img width="887" height="976" alt="image" src="https://github.com/user-attachments/assets/5e7199da-a153-4de4-af6a-b44d2f9783e0" />


## 功能特性

- ✅ **自动搜索配置文件** - 自动查找系统中的 Codex 配置文件（支持 .toml 和 .json 格式）
- ✅ **自适应 UI 布局** - 界面组件根据内容自动调整大小，文字不会被遮挡
- ✅ **主题切换** - 支持暗色/亮色主题和 Material 风格
- ✅ **字体和间距自定义** - 可自定义界面字体和控件间距
- ✅ **API 测试** - 测试 API 可达性和获取模型列表
- ✅ **配置管理** - 轻松保存和管理 Codex 配置

## 系统要求

- Windows 10/11
- .NET 10.0 Runtime（或使用独立发布版本）

## 使用方法

### 方式一：使用独立 EXE 文件（推荐）

1. 从 `publish` 文件夹中复制 `WinFormsApp1.exe`
2. 双击运行即可，无需安装 .NET Runtime

### 方式二：使用 .NET 运行

```bash
dotnet run
```

## 编译和发布

### 开发环境运行

```bash
dotnet run
```

### 发布独立 EXE

```bash
# PowerShell
.\scripts\publish-exe.ps1

# 或直接运行
dotnet publish WinFormsApp1.csproj -c Release -r win-x64 /p:PublishSingleFile=true /p:PublishTrimmed=false /p:SelfContained=true -o publish
```

发布的 exe 文件位于 `publish` 文件夹中。

## 配置文件位置

程序会自动搜索以下位置的配置文件：

1. `C:\Users\<用户名>\.codex\config.toml` （优先）
2. `C:\Users\<用户名>\.codex\config.json`
3. `C:\Users\<用户名>\.skills\config.json`
4. 其他常见 Codex 安装目录

## 技术栈

- .NET 10.0
- Windows Forms
- C# 12

## 许可证

MIT License
