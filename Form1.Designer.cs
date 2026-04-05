using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private IContainer components = null;

        private TextBox txtApiUrl;
        private TextBox txtApiKey;
        private TextBox txtProviderName;
        private TextBox txtConfigPath;
        private Button btnBrowseConfig;
        private Button btnAutoSearch;
        private CheckBox chkUseSystemProxy;
        private CheckBox chkIgnoreCertErrors;
        private Button btnFetchModels;
        private Button btnTestApi;
        private ListBox lstModels;
        private Button btnSave;
        private Label lblStatus;
        private Label lblApiStatus;
        private ProgressBar progressBar;
        private ToolTip toolTip;
        private ComboBox cmbFont;
        private ComboBox cmbSpacing;
        private CheckBox chkDarkTheme;
        private CheckBox chkMaterialStyle;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            this.toolTip = new ToolTip(this.components);
            this.Text = "Codex 配置工具";
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(800, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimumSize = new Size(700, 600);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.BackColor = Color.FromArgb(250, 250, 252);

            // 主容器 - 使用垂直 FlowLayoutPanel
            var mainPanel = new FlowLayoutPanel() 
            { 
                Dock = DockStyle.Fill, 
                FlowDirection = FlowDirection.TopDown, 
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(15)
            };

            // ===== 第1区域：标题 =====
            var headerLabel = new Label() 
            { 
                Text = "Codex · 新 API 快速配置", 
                AutoSize = true, 
                Font = new Font("Segoe UI", 16F, FontStyle.Bold), 
                ForeColor = Color.FromArgb(33,33,33), 
                Margin = new Padding(0, 0, 0, 15),
                MinimumSize = new Size(700, 40)
            };
            mainPanel.Controls.Add(headerLabel);

            // ===== 第2区域：主题设置栏 =====
            var themePanel = new GroupBox() 
            { 
                Text = "界面设置", 
                Dock = DockStyle.Top, 
                AutoSize = true,
                MinimumSize = new Size(700, 0),
                Margin = new Padding(0, 0, 0, 15),
                Padding = new Padding(15)
            };
            var themeFlow = new FlowLayoutPanel() 
            { 
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(5)
            };
            
            cmbFont = new ComboBox() 
            { 
                Width = 150, 
                DropDownStyle = ComboBoxStyle.DropDownList, 
                Margin = new Padding(5) 
            };
            cmbFont.Items.AddRange(new object[] { "Segoe UI", "Microsoft YaHei", "Consolas" });
            cmbFont.SelectedIndex = 0;
            cmbFont.SelectedIndexChanged += CmbFont_SelectedIndexChanged;

            cmbSpacing = new ComboBox() 
            { 
                Width = 100, 
                DropDownStyle = ComboBoxStyle.DropDownList, 
                Margin = new Padding(5) 
            };
            cmbSpacing.Items.AddRange(new object[] { "紧凑", "默认", "宽松" });
            cmbSpacing.SelectedIndex = 1;
            cmbSpacing.SelectedIndexChanged += CmbSpacing_SelectedIndexChanged;

            chkDarkTheme = new CheckBox() 
            { 
                Text = "暗色主题", 
                AutoSize = true, 
                Margin = new Padding(10, 5, 5, 5) 
            };
            chkDarkTheme.CheckedChanged += ChkDarkTheme_CheckedChanged;

            chkMaterialStyle = new CheckBox() 
            { 
                Text = "Material 风格", 
                AutoSize = true, 
                Margin = new Padding(10, 5, 5, 5) 
            };
            chkMaterialStyle.CheckedChanged += ChkMaterialStyle_CheckedChanged;

            themeFlow.Controls.AddRange(new Control[] { 
                new Label() { Text = "字体:", AutoSize = true, Margin = new Padding(5, 10, 5, 5) },
                cmbFont,
                new Label() { Text = "间距:", AutoSize = true, Margin = new Padding(20, 10, 5, 5) },
                cmbSpacing,
                chkDarkTheme,
                chkMaterialStyle
            });
            themePanel.Controls.Add(themeFlow);
            mainPanel.Controls.Add(themePanel);

            // ===== 第3区域：API 配置 =====
            var apiGroup = new GroupBox() 
            { 
                Text = "API 配置", 
                Dock = DockStyle.Top,
                AutoSize = true,
                MinimumSize = new Size(700, 0),
                Margin = new Padding(0, 0, 0, 15),
                Padding = new Padding(15)
            };
            var apiTable = new TableLayoutPanel() 
            { 
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                AutoSize = true
            };
            apiTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            apiTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 4; i++)
                apiTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            txtApiUrl = new TextBox() 
            { 
                Text = "https://api.newapi.com/v1", 
                BorderStyle = BorderStyle.FixedSingle, 
                PlaceholderText = "例如: https://api.newapi.com/v1",
                Margin = new Padding(5),
                MinimumSize = new Size(400, 25)
            };
            txtApiKey = new TextBox() 
            { 
                PasswordChar = '*', 
                BorderStyle = BorderStyle.FixedSingle, 
                PlaceholderText = "在此输入 API Key",
                Margin = new Padding(5),
                MinimumSize = new Size(400, 25)
            };
            txtProviderName = new TextBox() 
            { 
                Text = "newapi", 
                BorderStyle = BorderStyle.FixedSingle, 
                PlaceholderText = "提供商名称",
                Margin = new Padding(5),
                Width = 200
            };
            txtConfigPath = new TextBox() 
            { 
                BorderStyle = BorderStyle.FixedSingle, 
                PlaceholderText = "配置文件路径 (.json)",
                Margin = new Padding(5),
                MinimumSize = new Size(400, 25)
            };
            btnBrowseConfig = new Button() 
            { 
                Text = "浏览...", 
                Size = new Size(80, 28), 
                Margin = new Padding(5, 5, 0, 5)
            };
            btnBrowseConfig.Click += BtnBrowseConfig_Click;

            btnAutoSearch = new Button() 
            { 
                Text = "自动搜索", 
                Size = new Size(90, 28), 
                Margin = new Padding(5, 5, 0, 5)
            };
            btnAutoSearch.Click += BtnAutoSearch_Click;

            var configPathPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0)
            };
            configPathPanel.Controls.Add(txtConfigPath);
            configPathPanel.Controls.Add(btnAutoSearch);
            configPathPanel.Controls.Add(btnBrowseConfig);

            apiTable.Controls.Add(new Label() { Text = "API URL:", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(5, 8, 5, 5) }, 0, 0);
            apiTable.Controls.Add(txtApiUrl, 1, 0);
            apiTable.Controls.Add(new Label() { Text = "API Key:", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(5, 8, 5, 5) }, 0, 1);
            apiTable.Controls.Add(txtApiKey, 1, 1);
            apiTable.Controls.Add(new Label() { Text = "提供商:", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(5, 8, 5, 5) }, 0, 2);
            apiTable.Controls.Add(txtProviderName, 1, 2);
            apiTable.Controls.Add(new Label() { Text = "配置文件:", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(5, 8, 5, 5) }, 0, 3);
            apiTable.Controls.Add(configPathPanel, 1, 3);

            apiGroup.Controls.Add(apiTable);
            mainPanel.Controls.Add(apiGroup);

            // ===== 第4区域：选项设置 =====
            var optionsGroup = new GroupBox() 
            { 
                Text = "网络选项", 
                Dock = DockStyle.Top,
                AutoSize = true,
                MinimumSize = new Size(700, 0),
                Margin = new Padding(0, 0, 0, 15),
                Padding = new Padding(15)
            };
            var optionsFlow = new FlowLayoutPanel() 
            { 
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(5)
            };
            
            chkUseSystemProxy = new CheckBox() 
            { 
                Text = "使用系统代理", 
                AutoSize = true, 
                Margin = new Padding(5, 10, 20, 5)
            };
            chkIgnoreCertErrors = new CheckBox() 
            { 
                Text = "忽略 HTTPS 证书错误 (不推荐)", 
                AutoSize = true, 
                Margin = new Padding(5, 10, 5, 5)
            };
            
            optionsFlow.Controls.AddRange(new Control[] { chkUseSystemProxy, chkIgnoreCertErrors });
            optionsGroup.Controls.Add(optionsFlow);
            mainPanel.Controls.Add(optionsGroup);

            // ===== 第5区域：操作按钮 =====
            var actionGroup = new GroupBox() 
            { 
                Text = "操作", 
                Dock = DockStyle.Top,
                AutoSize = true,
                MinimumSize = new Size(700, 0),
                Margin = new Padding(0, 0, 0, 15),
                Padding = new Padding(15)
            };
            var actionFlow = new FlowLayoutPanel() 
            { 
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(5)
            };

            btnFetchModels = new Button() 
            { 
                Text = "获取模型列表", 
                Size = new Size(130, 40), 
                Margin = new Padding(5, 8, 10, 5)
            };
            btnFetchModels.Click += BtnFetchModels_Click;

            btnTestApi = new Button() 
            { 
                Text = "测试 API 可达性", 
                Size = new Size(140, 40), 
                Margin = new Padding(5, 8, 10, 5)
            };
            btnTestApi.Click += BtnTestApi_Click;

            btnSave = new Button() 
            { 
                Text = "保存配置", 
                Size = new Size(120, 40), 
                Margin = new Padding(5, 8, 5, 5)
            };
            btnSave.Click += BtnSave_Click;

            actionFlow.Controls.AddRange(new Control[] { btnFetchModels, btnTestApi, btnSave });
            actionGroup.Controls.Add(actionFlow);
            mainPanel.Controls.Add(actionGroup);

            // ===== 第6区域：状态显示 =====
            var statusGroup = new GroupBox() 
            { 
                Text = "状态信息", 
                Dock = DockStyle.Top,
                AutoSize = true,
                MinimumSize = new Size(700, 0),
                Margin = new Padding(0, 0, 0, 15),
                Padding = new Padding(15)
            };
            var statusFlow = new FlowLayoutPanel() 
            { 
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(5)
            };

            lblStatus = new Label() 
            { 
                AutoSize = true, 
                ForeColor = Color.DimGray, 
                Margin = new Padding(5, 10, 25, 5),
                MinimumSize = new Size(250, 20)
            };
            lblApiStatus = new Label() 
            { 
                AutoSize = true, 
                ForeColor = Color.DimGray, 
                Margin = new Padding(5, 10, 5, 5),
                MinimumSize = new Size(250, 20)
            };

            statusFlow.Controls.AddRange(new Control[] { lblStatus, lblApiStatus });
            statusGroup.Controls.Add(statusFlow);
            mainPanel.Controls.Add(statusGroup);

            // ===== 第7区域：进度条 =====
            progressBar = new ProgressBar() 
            { 
                Size = new Size(700, 12), 
                Visible = false, 
                Style = ProgressBarStyle.Continuous, 
                Margin = new Padding(0, 0, 0, 10)
            };
            mainPanel.Controls.Add(progressBar);

            // ===== 第8区域：模型列表 =====
            var modelsGroup = new GroupBox() 
            { 
                Text = "可用模型列表 (多选)", 
                Dock = DockStyle.Fill,
                MinimumSize = new Size(700, 200),
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(15)
            };
            lstModels = new ListBox() 
            { 
                Dock = DockStyle.Fill,
                SelectionMode = SelectionMode.MultiSimple, 
                BorderStyle = BorderStyle.FixedSingle, 
                Margin = new Padding(0),
                Font = new Font("Consolas", 9F)
            };
            modelsGroup.Controls.Add(lstModels);
            mainPanel.Controls.Add(modelsGroup);

            // Modern button style
            foreach (var btn in new Button[] { btnFetchModels, btnTestApi, btnSave, btnBrowseConfig, btnAutoSearch })
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.BackColor = Color.FromArgb(33, 150, 243);
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderSize = 0;
                btn.Cursor = Cursors.Hand;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(25, 118, 210);
            }

            // Tooltips
            this.toolTip.SetToolTip(txtApiUrl, "API 基础 URL (例如 https://api.newapi.com/v1)");
            this.toolTip.SetToolTip(txtApiKey, "用于身份验证的 API Key");
            this.toolTip.SetToolTip(txtProviderName, "在配置中使用的提供商标识");
            this.toolTip.SetToolTip(txtConfigPath, "配置文件 (.json) 的保存路径");

            this.Controls.Add(mainPanel);
        }

        #endregion
    }
}
