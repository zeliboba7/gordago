namespace WindowsApplication1 {
  partial class ProxySettingsForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this._chkEnableProxy = new System.Windows.Forms.CheckBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label1 = new System.Windows.Forms.Label();
      this._txtServer = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this._txtPort = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this._txtUser = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this._txtPassword = new System.Windows.Forms.TextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // _chkEnableProxy
      // 
      this._chkEnableProxy.AutoSize = true;
      this._chkEnableProxy.Location = new System.Drawing.Point(32, 13);
      this._chkEnableProxy.Name = "_chkEnableProxy";
      this._chkEnableProxy.Size = new System.Drawing.Size(108, 17);
      this._chkEnableProxy.TabIndex = 0;
      this._chkEnableProxy.Text = "Use Proxy Server";
      this._chkEnableProxy.UseVisualStyleBackColor = true;
      this._chkEnableProxy.CheckedChanged += new System.EventHandler(this._chkEnableProxy_CheckedChanged);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this._txtPassword);
      this.groupBox1.Controls.Add(this.label4);
      this.groupBox1.Controls.Add(this._txtUser);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this._txtPort);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this._txtServer);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Location = new System.Drawing.Point(12, 36);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(329, 127);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Proxy";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(7, 20);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(38, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Server";
      // 
      // _txtServer
      // 
      this._txtServer.Location = new System.Drawing.Point(88, 17);
      this._txtServer.Name = "_txtServer";
      this._txtServer.Size = new System.Drawing.Size(235, 20);
      this._txtServer.TabIndex = 1;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(7, 46);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(26, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Port";
      // 
      // _txtPort
      // 
      this._txtPort.Location = new System.Drawing.Point(88, 43);
      this._txtPort.Name = "_txtPort";
      this._txtPort.Size = new System.Drawing.Size(235, 20);
      this._txtPort.TabIndex = 1;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(7, 72);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(29, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "User";
      // 
      // _txtUser
      // 
      this._txtUser.Location = new System.Drawing.Point(88, 69);
      this._txtUser.Name = "_txtUser";
      this._txtUser.Size = new System.Drawing.Size(235, 20);
      this._txtUser.TabIndex = 1;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(7, 97);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(53, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "Password";
      // 
      // _txtPassword
      // 
      this._txtPassword.Location = new System.Drawing.Point(88, 94);
      this._txtPassword.Name = "_txtPassword";
      this._txtPassword.Size = new System.Drawing.Size(235, 20);
      this._txtPassword.TabIndex = 1;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(90, 186);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 2;
      this.button1.Text = "OK";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(182, 186);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 23);
      this.button2.TabIndex = 2;
      this.button2.Text = "Cancel";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // ProxySettingsForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(353, 229);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this._chkEnableProxy);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "ProxySettingsForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Proxy Settings";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox _chkEnableProxy;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox _txtPassword;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox _txtUser;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox _txtPort;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox _txtServer;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
  }
}