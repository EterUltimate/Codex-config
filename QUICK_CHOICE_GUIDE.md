# 📦 快速选择指南

## 我应该下载哪个版本？

### 🤔 问自己这个问题：

**您的电脑上已经安装了 .NET 10.0 吗？**

---

### ✅ 如果答案是 "不确定" 或 "没有"

👉 **下载完整独立版** (`WinFormsApp1-Standalone.exe`)

- 文件大小: ~111 MB
- 优点: 下载后直接运行，无需安装任何东西
- 适合: 95% 的用户

---

### ✅ 如果答案是 "是的，已安装"

👉 **下载轻量版** (`WinFormsApp1-Lightweight.exe`)

- 文件大小: ~0.75 MB (快 150 倍!)
- 优点: 下载超快，体积小巧
- 适合: 开发者、技术人员

---

## 💡 如何检查是否已安装 .NET 10.0？

打开命令提示符或 PowerShell，运行：

```powershell
dotnet --list-runtimes
```

如果看到类似这样的输出，说明已安装：
```
Microsoft.WindowsDesktop.App 10.0.x [C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App]
```

如果没有看到 `10.0.x` 版本，则需要：
1. 访问 https://dotnet.microsoft.com/download/dotnet/10.0
2. 下载并安装 ".NET Desktop Runtime 10.0"

---

## 🎯 简单总结

| 用户类型 | 推荐版本 | 原因 |
|---------|---------|------|
| 普通用户 | 完整独立版 | 最简单，开箱即用 |
| 第一次使用 | 完整独立版 | 无需配置 |
| 开发者 | 轻量版 | 体积小，已有环境 |
| 带宽有限 | 轻量版 | 下载快 |
| 企业部署 | 完整独立版 | 便于分发 |

---

## ⚡ 一键下载链接（发布后）

- **完整独立版**: https://github.com/EterUltimate/Codex-config/releases/latest/download/WinFormsApp1-Standalone.exe
- **轻量版**: https://github.com/EterUltimate/Codex-config/releases/latest/download/WinFormsApp1-Lightweight.exe

---

**还不确定？** → 选择完整独立版，永远不会错！✅
