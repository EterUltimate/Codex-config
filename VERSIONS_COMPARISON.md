# 版本对比说明

## 两个版本的区别

本项目提供两种发布版本，您可以根据需求选择：

### 📦 1. 完整独立版（推荐大多数用户）

**文件位置**: `publish/WinFormsApp1.exe`

**特点**:
- ✅ **包含 .NET Runtime** - 无需安装任何依赖
- ✅ **开箱即用** - 双击即可运行
- ✅ **便携性强** - 可复制到任何 Windows 电脑使用
- ❌ **文件较大** - 约 111 MB

**适用场景**:
- 分发给没有安装 .NET 的用户
- 需要便携性（U盘携带）
- 企业环境部署
- 普通终端用户

**系统要求**:
- Windows 10/11 x64
- 无需安装任何其他软件

---

### 🚀 2. 轻量版（适合开发者或已安装 .NET 的用户）

**文件位置**: `publish-lightweight/WinFormsApp1.exe`

**特点**:
- ✅ **文件极小** - 仅 0.75 MB（比完整版小 99%）
- ✅ **下载快速** - 适合网络分发
- ❌ **需要 .NET Runtime** - 用户需先安装 .NET 10.0
- ❌ **依赖外部运行时**

**适用场景**:
- 开发者自己使用
- 目标用户已安装 .NET 10.0
- 网络带宽受限的环境
- 需要快速下载的场景

**系统要求**:
- Windows 10/11 x64
- **必须安装 .NET 10.0 Desktop Runtime**

**如何安装 .NET Runtime**:
1. 访问: https://dotnet.microsoft.com/download/dotnet/10.0
2. 下载 ".NET Desktop Runtime 10.0" for Windows x64
3. 安装后重启电脑
4. 运行 WinFormsApp1.exe

---

## 文件大小对比

| 版本 | 文件大小 | 包含 Runtime | 需要额外安装 |
|------|---------|-------------|------------|
| 完整独立版 | ~111 MB | ✅ 是 | ❌ 否 |
| 轻量版 | ~0.75 MB | ❌ 否 | ✅ 是 (.NET 10.0) |

**体积差异**: 轻量版只有完整版的 **0.7%**

---

## 如何选择？

### 选择完整独立版，如果：
- ✓ 您要分发给普通用户
- ✓ 不确定用户是否安装了 .NET
- ✓ 需要最好的用户体验（一键运行）
- ✓ 文件大小不是问题

### 选择轻量版，如果：
- ✓ 您自己是开发者
- ✓ 目标用户已安装 .NET 10.0
- ✓ 网络带宽有限
- ✓ 需要最小化下载体积

---

## 重新打包命令

### 打包完整独立版
```powershell
.\scripts\publish-exe.ps1
```

### 打包轻量版
```powershell
dotnet publish WinFormsApp1.csproj -c Release -r win-x64 /p:PublishSingleFile=true /p:PublishTrimmed=false /p:SelfContained=false -o publish-lightweight
```

---

## GitHub Release 建议

创建 Release 时，建议同时上传两个版本：

1. **WinFormsApp1-Standalone.exe** (完整版)
   - 重命名自: `publish/WinFormsApp1.exe`
   
2. **WinFormsApp1-Lightweight.exe** (轻量版)
   - 重命名自: `publish-lightweight/WinFormsApp1.exe`

这样用户可以根据自己的需求选择合适的版本。
