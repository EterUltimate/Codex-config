# GitHub Release 指南

## 已推送的代码

✅ 代码已成功推送到: https://github.com/EterUltimate/Codex-config

### 提交历史
- `60f246c` - Add README and update .gitignore
- `9939a26` - Initial commit: Codex配置工具 - 支持自动搜索配置文件和自适应UI布局

## 创建 Release 并上传 EXE

### 方法一：通过 GitHub 网页界面（推荐）

1. **访问 Releases 页面**
   - 打开: https://github.com/EterUltimate/Codex-config/releases
   - 点击 "Create a new release"

2. **填写 Release 信息**
   - **Tag version**: `v1.0.0`
   - **Release title**: `Codex 配置工具 v1.0.0`
   - **Description**: 
     ```
     ## 功能特性
     
     - ✅ 自动搜索 Codex 配置文件（支持 .toml 和 .json）
     - ✅ 自适应 UI 布局（文字不会被遮挡）
     - ✅ 暗色/亮色主题切换
     - ✅ Material 风格支持
     - ✅ 自定义字体和间距
     - ✅ API 测试功能
     - ✅ 模型列表获取
     - ✅ 配置保存和管理
     
     ## 系统要求
     - Windows 10/11 x64
     - 无需安装 .NET Runtime
     ```

3. **上传 EXE 文件**（建议同时上传两个版本）
   
   **版本 1: 完整独立版** (推荐)
   - 点击 "Attach binaries by dropping them here or selecting them"
   - 选择文件: `publish\WinFormsApp1.exe`
   - 重命名为: `WinFormsApp1-Standalone.exe` (可选)
   - 等待上传完成 (~111 MB)
   
   **版本 2: 轻量版**
   - 再次点击附加文件
   - 选择文件: `publish-lightweight\WinFormsApp1.exe`
   - 重命名为: `WinFormsApp1-Lightweight.exe` (可选)
   - 等待上传完成 (~0.75 MB)

4. **发布**
   - 勾选 "Set as the latest release"
   - 点击 "Publish release"

### 方法二：使用 GitHub CLI（如果已安装）

```bash
# 创建 Release
gh release create v1.0.0 `
  --title "Codex 配置工具 v1.0.0" `
  --notes "首个正式版本 - 支持自动搜索配置文件和自适应UI布局" `
  ./publish/WinFormsApp1.exe
```

## EXE 文件信息

### 完整独立版 (Standalone)
- **文件位置**: `publish\WinFormsApp1.exe`
- **文件大小**: ~111 MB
- **类型**: 独立单文件应用（包含 .NET Runtime）
- **平台**: Windows x64
- **运行要求**: Windows 10/11，无需安装额外依赖
- **推荐**: ✅ 适合大多数用户

### 轻量版 (Lightweight)
- **文件位置**: `publish-lightweight\WinFormsApp1.exe`
- **文件大小**: ~0.75 MB (比完整版小 99%)
- **类型**: 依赖框架的单文件应用
- **平台**: Windows x64
- **运行要求**: Windows 10/11 + **.NET 10.0 Desktop Runtime**
- **推荐**: 适合已安装 .NET 的用户或开发者

## 下载链接

发布后，用户可以通过以下链接下载：
```
https://github.com/EterUltimate/Codex-config/releases/latest/download/WinFormsApp1.exe
```

## 验证发布

发布完成后，访问:
- Releases 页面: https://github.com/EterUltimate/Codex-config/releases
- 最新下载: https://github.com/EterUltimate/Codex-config/releases/latest
