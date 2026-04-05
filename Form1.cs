using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".skills", "config.json");
        private readonly string uiSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CodexConfig", "ui-settings.json");

        public Form1()
        {
            InitializeComponent();

            try
            {
                var saved = LoadUiSettings();
                if (saved != null)
                {
                    if (cmbFont != null && !string.IsNullOrEmpty(saved.FontName) && cmbFont.Items.Contains(saved.FontName))
                        cmbFont.SelectedItem = saved.FontName;

                    if (cmbSpacing != null && !string.IsNullOrEmpty(saved.SpacingMode) && cmbSpacing.Items.Contains(saved.SpacingMode))
                        cmbSpacing.SelectedItem = saved.SpacingMode;

                    if (chkDarkTheme != null)
                        chkDarkTheme.Checked = saved.DarkTheme;

                    if (chkMaterialStyle != null)
                        chkMaterialStyle.Checked = saved.MaterialStyle;

                    var fontName = cmbFont?.SelectedItem as string ?? this.Font.FontFamily.Name;
                    ApplyFontToAllControls(this, new Font(fontName, saved.FontSize > 0 ? saved.FontSize : 10F));
                    var spacingMode = cmbSpacing?.SelectedItem as string ?? "默认";
                    int spacing = spacingMode == "紧凑" ? 4 : spacingMode == "宽松" ? 16 : 8;
                    ApplySpacing(spacing);
                    ApplyTheme(saved.DarkTheme);
                    ApplyMaterialStyle(saved.MaterialStyle);
                }
                else
                {
                    if (cmbFont != null && cmbFont.Items.Contains("Microsoft YaHei"))
                        cmbFont.SelectedItem = "Microsoft YaHei";
                    var fontName = cmbFont?.SelectedItem as string ?? this.Font.FontFamily.Name;
                    ApplyFontToAllControls(this, new Font(fontName, 10F));
                    if (cmbSpacing != null && cmbSpacing.Items.Contains("默认"))
                        cmbSpacing.SelectedItem = "默认";
                    var spacingMode = cmbSpacing?.SelectedItem as string ?? "默认";
                    int spacing = spacingMode == "紧凑" ? 4 : spacingMode == "宽松" ? 16 : 8;
                    ApplySpacing(spacing);
                }
            }
            catch { }

            // initialize config path - try auto search first
            AutoSearchCodexConfig();

            // save on close
            this.FormClosing += (s, e) => SaveUiSettings();

            // wire change events to persist immediately
            if (cmbFont != null) cmbFont.SelectedIndexChanged += CmbFont_SelectedIndexChanged;
            if (cmbSpacing != null) cmbSpacing.SelectedIndexChanged += CmbSpacing_SelectedIndexChanged;
            if (chkDarkTheme != null) chkDarkTheme.CheckedChanged += ChkDarkTheme_CheckedChanged;
            if (chkMaterialStyle != null) chkMaterialStyle.CheckedChanged += ChkMaterialStyle_CheckedChanged;
        }

        private class UiSettings
        {
            public string? FontName { get; set; }
            public float FontSize { get; set; }
            public string? SpacingMode { get; set; }
            public bool DarkTheme { get; set; }
            public bool MaterialStyle { get; set; }
        }

        private UiSettings? LoadUiSettings()
        {
            try
            {
                if (File.Exists(uiSettingsPath))
                {
                    var txt = File.ReadAllText(uiSettingsPath);
                    return JsonSerializer.Deserialize<UiSettings>(txt);
                }
            }
            catch { }
            return null;
        }

        private void SaveUiSettings()
        {
            try
            {
                var s = new UiSettings
                {
                    FontName = cmbFont?.SelectedItem as string,
                    FontSize = this.Font.Size,
                    SpacingMode = cmbSpacing?.SelectedItem as string,
                    DarkTheme = chkDarkTheme?.Checked ?? false,
                    MaterialStyle = chkMaterialStyle?.Checked ?? false
                };

                var dir = Path.GetDirectoryName(uiSettingsPath);
                if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var txt = JsonSerializer.Serialize(s, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(uiSettingsPath, txt);
            }
            catch { }
        }

        private void CmbFont_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbFont?.SelectedItem is string name)
            {
                try { var newFont = new Font(name, this.Font.Size); ApplyFontToAllControls(this, newFont); } catch { }
                SaveUiSettings();
            }
        }

        private void CmbSpacing_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbSpacing?.SelectedItem is string mode)
            {
                int spacing = mode == "紧凑" ? 4 : mode == "宽松" ? 16 : 8;
                ApplySpacing(spacing);
                SaveUiSettings();
            }
        }

        private void ChkDarkTheme_CheckedChanged(object? sender, EventArgs e)
        {
            ApplyTheme(chkDarkTheme?.Checked ?? false);
            SaveUiSettings();
        }

        private void ChkMaterialStyle_CheckedChanged(object? sender, EventArgs e)
        {
            ApplyMaterialStyle(chkMaterialStyle?.Checked ?? false);
            SaveUiSettings();
        }

        /// <summary>
        /// 自动搜索 Codex 配置文件
        /// </summary>
        private void AutoSearchCodexConfig()
        {
            try
            {
                lblStatus.Text = "正在搜索 Codex 配置文件...";
                
                // 定义可能的搜索路径（支持 .json 和 .toml）
                var searchPaths = new List<string>();
                
                // 1. 用户主目录下的 .codex 目录
                var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var codexDir = Path.Combine(userProfile, ".codex");
                if (Directory.Exists(codexDir))
                {
                    // 优先查找 config.toml
                    var tomlConfig = Path.Combine(codexDir, "config.toml");
                    if (File.Exists(tomlConfig))
                    {
                        searchPaths.Add(tomlConfig);
                    }
                    
                    // 也查找 config.json
                    var jsonConfig = Path.Combine(codexDir, "config.json");
                    if (File.Exists(jsonConfig))
                    {
                        searchPaths.Add(jsonConfig);
                    }
                }
                
                // 2. 用户主目录下的 .skills 目录
                var skillsDir = Path.Combine(userProfile, ".skills");
                if (Directory.Exists(skillsDir))
                {
                    var skillsConfig = Path.Combine(skillsDir, "config.json");
                    if (File.Exists(skillsConfig))
                    {
                        searchPaths.Add(skillsConfig);
                    }
                }
                
                // 3. 常见的 Codex 安装目录
                var commonDirs = new[]
                {
                    Path.Combine(userProfile, "codex"),
                    Path.Combine(userProfile, "Codex"),
                    Path.Combine(userProfile, ".codex"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "codex"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Codex"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "codex"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Codex"),
                };
                
                foreach (var dir in commonDirs)
                {
                    if (Directory.Exists(dir))
                    {
                        // 在目录中查找 config.toml（优先）
                        var tomlFile = Path.Combine(dir, "config.toml");
                        if (File.Exists(tomlFile))
                        {
                            searchPaths.Add(tomlFile);
                        }
                        
                        // 在目录中查找 config.json
                        var jsonFile = Path.Combine(dir, "config.json");
                        if (File.Exists(jsonFile))
                        {
                            searchPaths.Add(jsonFile);
                        }
                        
                        // 在子目录中查找 .skills/config.json
                        var subSkillsDir = Path.Combine(dir, ".skills");
                        if (Directory.Exists(subSkillsDir))
                        {
                            var skillsConfig = Path.Combine(subSkillsDir, "config.json");
                            if (File.Exists(skillsConfig))
                            {
                                searchPaths.Add(skillsConfig);
                            }
                        }
                    }
                }
                
                // 4. 在当前工作目录及其父目录中查找
                var currentDir = Directory.GetCurrentDirectory();
                for (int i = 0; i < 5; i++) // 最多向上查找5层
                {
                    // 查找 .codex/config.toml
                    var codexSubDir = Path.Combine(currentDir, ".codex");
                    if (Directory.Exists(codexSubDir))
                    {
                        var tomlConfig = Path.Combine(codexSubDir, "config.toml");
                        if (File.Exists(tomlConfig))
                        {
                            searchPaths.Add(tomlConfig);
                        }
                    }
                    
                    // 查找 .skills/config.json
                    var skillsSubDir = Path.Combine(currentDir, ".skills");
                    if (Directory.Exists(skillsSubDir))
                    {
                        var skillsConfig = Path.Combine(skillsSubDir, "config.json");
                        if (File.Exists(skillsConfig))
                        {
                            searchPaths.Add(skillsConfig);
                        }
                    }
                    
                    var parentDir = Directory.GetParent(currentDir);
                    if (parentDir == null) break;
                    currentDir = parentDir.FullName;
                }
                
                // 去重并验证文件存在性，优先使用 .toml 文件
                var foundConfigs = searchPaths
                    .Where(p => !string.IsNullOrEmpty(p) && File.Exists(p))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(p => p.EndsWith(".toml", StringComparison.OrdinalIgnoreCase) ? 0 : 1) // TOML 优先
                    .ToList();
                
                if (foundConfigs.Count > 0)
                {
                    // 优先使用第一个找到的配置（TOML 优先）
                    configPath = foundConfigs[0];
                    txtConfigPath.Text = configPath;
                    
                    var fileType = configPath.EndsWith(".toml") ? "TOML" : "JSON";
                    lblStatus.Text = $"找到 {foundConfigs.Count} 个配置文件，已选择 ({fileType}): {configPath}";
                    
                    if (foundConfigs.Count > 1)
                    {
                        lblStatus.Text += $" (共找到 {foundConfigs.Count} 个)";
                    }
                }
                else
                {
                    // 未找到，使用默认路径（优先 .toml）
                    configPath = Path.Combine(codexDir, "config.toml");
                    txtConfigPath.Text = configPath;
                    lblStatus.Text = "未找到现有配置文件，将创建新文件 (config.toml)";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"搜索失败: {ex.Message}";
                // 出错时使用默认路径
                var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                configPath = Path.Combine(userProfile, ".codex", "config.toml");
                txtConfigPath.Text = configPath;
            }
        }

        /// <summary>
        /// 手动触发自动搜索
        /// </summary>
        private void BtnAutoSearch_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "将搜索系统中的 Codex 配置文件。是否继续？",
                "自动搜索",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                AutoSearchCodexConfig();
            }
        }

        private void ApplyFontToAllControls(Control parent, Font font)
        {
            try { parent.Font = font; } catch { }
            foreach (Control c in parent.Controls)
            {
                try { c.Font = font; } catch { }
                if (c.HasChildren) ApplyFontToAllControls(c, font);
            }
        }

        private void ApplySpacing(int margin)
        {
            void SetMargins(Control parent)
            {
                foreach (Control c in parent.Controls)
                {
                    c.Margin = new Padding(margin);
                    if (c.HasChildren) SetMargins(c);
                }
            }
            SetMargins(this);
        }

        private void ApplyTheme(bool dark)
        {
            Color bg = dark ? Color.FromArgb(34, 34, 34) : Color.FromArgb(250, 250, 252);
            Color fg = dark ? Color.FromArgb(230, 230, 230) : Color.FromArgb(33, 33, 33);
            this.BackColor = bg; this.ForeColor = fg;
            void SetTheme(Control parent)
            {
                foreach (Control c in parent.Controls)
                {
                    if (c is Label || c is CheckBox || c is LinkLabel) { c.ForeColor = fg; c.BackColor = Color.Transparent; }
                    else if (c is TextBox || c is ListBox) { c.BackColor = dark ? Color.FromArgb(48, 48, 48) : Color.White; c.ForeColor = fg; }
                    else if (c is Button b)
                    {
                        if (!(chkMaterialStyle?.Checked ?? false)) { b.BackColor = dark ? Color.FromArgb(48, 103, 255) : Color.FromArgb(41, 121, 255); b.ForeColor = Color.White; }
                    }
                    if (c.HasChildren) SetTheme(c);
                }
            }
            SetTheme(this);
        }

        private void ApplyMaterialStyle(bool on)
        {
            foreach (Control c in this.Controls)
            {
                if (c is Button b)
                {
                    if (on) { b.FlatStyle = FlatStyle.Flat; b.BackColor = Color.FromArgb(41, 121, 255); b.ForeColor = Color.White; b.FlatAppearance.BorderSize = 0; b.Cursor = Cursors.Hand; b.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 90, 200); }
                    else { b.FlatStyle = FlatStyle.Standard; b.BackColor = SystemColors.Control; b.ForeColor = SystemColors.ControlText; }
                }
            }
        }

        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler();
            try { handler.UseProxy = chkUseSystemProxy?.Checked ?? true; } catch { handler.UseProxy = true; }
            if (chkIgnoreCertErrors?.Checked ?? false) handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            return new HttpClient(handler, disposeHandler: true);
        }

        private async void BtnTestApi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApiUrl.Text)) { MessageBox.Show("请填写 API URL", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            btnTestApi.Enabled = false; lblApiStatus.Text = "正在测试 API 可达性...";
            try { using var httpClient = CreateHttpClient(); httpClient.Timeout = TimeSpan.FromSeconds(8); var apiUrl = txtApiUrl.Text.TrimEnd('/'); using var resp = await httpClient.GetAsync(apiUrl); if (resp.IsSuccessStatusCode) { lblApiStatus.ForeColor = Color.Green; lblApiStatus.Text = $"API 可达 (HTTP {(int)resp.StatusCode})"; } else { lblApiStatus.ForeColor = Color.OrangeRed; lblApiStatus.Text = $"API 返回 {(int)resp.StatusCode} {resp.ReasonPhrase}"; } }
            catch (TaskCanceledException) { lblApiStatus.ForeColor = Color.OrangeRed; lblApiStatus.Text = "请求超时，请检查网络或代理设置"; }
            catch (HttpRequestException ex) { lblApiStatus.ForeColor = Color.OrangeRed; lblApiStatus.Text = $"HTTP 错误: {ex.Message}"; }
            catch (Exception ex) { lblApiStatus.ForeColor = Color.OrangeRed; lblApiStatus.Text = $"测试失败: {ex.Message}"; }
            finally { btnTestApi.Enabled = true; }
        }

        private void BtnBrowseConfig_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog(); sfd.Title = "选择或创建配置文件"; sfd.Filter = "JSON 文件 (*.json)|*.json|所有文件 (*.*)|*.*"; sfd.FileName = Path.GetFileName(configPath); sfd.InitialDirectory = Path.GetDirectoryName(configPath) ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); if (sfd.ShowDialog() == DialogResult.OK) txtConfigPath.Text = sfd.FileName;
        }

        private async void BtnFetchModels_Click(object sender, EventArgs e)
        {
            if (!ValidateApiInputs()) return;
            btnFetchModels.Enabled = false; progressBar.Visible = true; lblStatus.Text = "正在获取模型列表..."; lstModels.Items.Clear();
            try
            {
                using var httpClient = CreateHttpClient(); httpClient.DefaultRequestHeaders.Clear(); httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {txtApiKey.Text}"); httpClient.Timeout = TimeSpan.FromSeconds(15);
                string apiUrl = txtApiUrl.Text.TrimEnd('/'); string modelsEndpoint = $"{apiUrl}/models"; using var resp = await httpClient.GetAsync(modelsEndpoint); if (!resp.IsSuccessStatusCode) { lblStatus.Text = $"获取失败: HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}"; return; }
                var response = await resp.Content.ReadAsStringAsync(); using var jsonDoc = JsonDocument.Parse(response); var root = jsonDoc.RootElement;
                if (root.ValueKind == JsonValueKind.Object)
                {
                    if (root.TryGetProperty("data", out var dataArray))
                    {
                        foreach (var it in dataArray.EnumerateArray())
                        {
                            if (it.TryGetProperty("id", out var idElement))
                                lstModels.Items.Add(idElement.GetString());
                        }
                    }
                    else if (root.TryGetProperty("models", out var modelsArray))
                    {
                        foreach (var it in modelsArray.EnumerateArray())
                        {
                            lstModels.Items.Add(it.GetString());
                        }
                    }
                }
                if (lstModels.Items.Count == 0) lblStatus.Text = "未找到模型"; else lblStatus.Text = $"找到 {lstModels.Items.Count} 个模型";
            }
            catch (TaskCanceledException) { MessageBox.Show("请求超时: 请检查网络或 API URL 是否正确", "超时", MessageBoxButtons.OK, MessageBoxIcon.Warning); lblStatus.Text = "获取超时"; }
            catch (HttpRequestException ex) { MessageBox.Show($"网络请求失败: {ex.Message}", "网络错误", MessageBoxButtons.OK, MessageBoxIcon.Error); lblStatus.Text = "网络错误"; }
            catch (JsonException ex) { MessageBox.Show($"解析响应失败: {ex.Message}", "解析错误", MessageBoxButtons.OK, MessageBoxIcon.Error); lblStatus.Text = "解析错误"; }
            catch (Exception ex) { MessageBox.Show($"获取模型列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); lblStatus.Text = "获取失败"; }
            finally { btnFetchModels.Enabled = true; progressBar.Visible = false; }
        }

        private bool ValidateApiInputs()
        {
            if (string.IsNullOrWhiteSpace(txtApiUrl.Text))
            {
                MessageBox.Show("API URL 不能为空", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!Uri.IsWellFormedUriString(txtApiUrl.Text, UriKind.Absolute))
            {
                MessageBox.Show("API URL 格式不正确", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApiKey.Text))
            {
                MessageBox.Show("API Key 不能为空", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Update configPath from UI
            if (!string.IsNullOrWhiteSpace(txtConfigPath.Text))
            {
                configPath = txtConfigPath.Text;
            }

            return true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (lstModels.SelectedItems.Count == 0)
            {
                MessageBox.Show("请至少选择一个模型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // use UI config path
            if (!string.IsNullOrWhiteSpace(txtConfigPath.Text))
            {
                configPath = txtConfigPath.Text;
            }

            try
            {
                var dir = Path.GetDirectoryName(configPath);
                if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                JsonObject root;
                if (!File.Exists(configPath))
                {
                    // create a skeleton compatible with QuantumNous/skills newapi format
                    root = new JsonObject
                    {
                        ["version"] = "1.0",
                        ["model_providers"] = new JsonObject(),
                        ["models"] = new JsonArray()
                    };
                }
                else
                {
                    string configContent = File.ReadAllText(configPath);
                    root = JsonNode.Parse(configContent) as JsonObject ?? new JsonObject();
                }

                var providerName = txtProviderName.Text.Trim();
                if (string.IsNullOrEmpty(providerName))
                {
                    MessageBox.Show("提供商名称不能为空", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 构建新的提供商配置 (符合 skills/newapi 的预期字段)
                var newProvider = new JsonObject
                {
                    ["name"] = providerName,
                    ["base_url"] = txtApiUrl.Text.TrimEnd('/'),
                    ["env_key"] = $"{providerName.ToUpper()}_API_KEY",
                    ["type"] = "newapi",
                    ["wire_api"] = "chat"
                };

                // 更新 model_providers
                if (root.TryGetPropertyValue("model_providers", out var mpNode) && mpNode is JsonObject mpObj)
                {
                    mpObj[providerName] = newProvider;
                }
                else
                {
                    var mp = new JsonObject { [providerName] = newProvider };
                    root["model_providers"] = mp;
                }

                // 将选定的模型设置为可用模型
                var selectedModels = lstModels.SelectedItems.Cast<object>().Select(m => m.ToString()).ToList();
                if (selectedModels.Any())
                {
                    JsonArray modelsArray;
                    if (root.TryGetPropertyValue("models", out var modelsNode) && modelsNode is JsonArray existingArray)
                    {
                        modelsArray = existingArray;
                    }
                    else
                    {
                        modelsArray = new JsonArray();
                        root["models"] = modelsArray;
                    }

                    // Add unique
                    var existing = new HashSet<string>(modelsArray.Select(n => n?.ToString()), StringComparer.OrdinalIgnoreCase);
                    foreach (var model in selectedModels)
                    {
                        if (!existing.Contains(model))
                        {
                            modelsArray.Add(model);
                        }
                    }
                }

                // 保存配置
                var options = new JsonSerializerOptions { WriteIndented = true };
                string updatedJson = root.ToJsonString(options);
                File.WriteAllText(configPath, updatedJson);

                // 提示设置环境变量
                string envKeyName = $"{providerName.ToUpper()}_API_KEY";
                string message = $"配置已保存!\n\n请设置环境变量:\n{envKeyName}={txtApiKey.Text}\n\n点击确定复制 API Key 到剪贴板?";
                DialogResult copyResult = MessageBox.Show(message, "成功", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (copyResult == DialogResult.OK)
                {
                    Clipboard.SetText(txtApiKey.Text);
                    MessageBox.Show("API Key 已复制到剪贴板", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
