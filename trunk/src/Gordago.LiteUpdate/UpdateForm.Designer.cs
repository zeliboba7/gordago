namespace Gordago.LiteUpdate {
  partial class UpdateForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;


    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
      this._lblANewVersion = new System.Windows.Forms.Label();
      this._btnViewDetails = new System.Windows.Forms.Button();
      this._gboxLog = new System.Windows.Forms.GroupBox();
      this._txtLog = new System.Windows.Forms.RichTextBox();
      this._gboxProcess = new System.Windows.Forms.GroupBox();
      this._progressBar = new System.Windows.Forms.ProgressBar();
      this._lblProcessName = new System.Windows.Forms.Label();
      this._btnStart = new System.Windows.Forms.Button();
      this._gboxLog.SuspendLayout();
      this._gboxProcess.SuspendLayout();
      this.SuspendLayout();
      // 
      // _lblANewVersion
      // 
      this._lblANewVersion.AutoSize = true;
      this._lblANewVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this._lblANewVersion.Location = new System.Drawing.Point(13, 13);
      this._lblANewVersion.Name = "_lblANewVersion";
      this._lblANewVersion.Size = new System.Drawing.Size(200, 16);
      this._lblANewVersion.TabIndex = 0;
      this._lblANewVersion.Text = "A new version of {0} is available.";
      // 
      // _btnViewDetails
      // 
      this._btnViewDetails.Location = new System.Drawing.Point(12, 42);
      this._btnViewDetails.Name = "_btnViewDetails";
      this._btnViewDetails.Size = new System.Drawing.Size(192, 23);
      this._btnViewDetails.TabIndex = 1;
      this._btnViewDetails.Text = "View Details...";
      this._btnViewDetails.UseVisualStyleBackColor = true;
      this._btnViewDetails.Click += new System.EventHandler(this._btnViewDetails_Click);
      // 
      // _gboxLog
      // 
      this._gboxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._gboxLog.Controls.Add(this._txtLog);
      this._gboxLog.Location = new System.Drawing.Point(3, 143);
      this._gboxLog.Name = "_gboxLog";
      this._gboxLog.Size = new System.Drawing.Size(627, 174);
      this._gboxLog.TabIndex = 2;
      this._gboxLog.TabStop = false;
      this._gboxLog.Text = "Log";
      // 
      // _txtLog
      // 
      this._txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
      this._txtLog.Location = new System.Drawing.Point(3, 16);
      this._txtLog.Name = "_txtLog";
      this._txtLog.Size = new System.Drawing.Size(621, 155);
      this._txtLog.TabIndex = 0;
      this._txtLog.Text = "";
      // 
      // _gboxProcess
      // 
      this._gboxProcess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._gboxProcess.Controls.Add(this._progressBar);
      this._gboxProcess.Controls.Add(this._lblProcessName);
      this._gboxProcess.Location = new System.Drawing.Point(3, 72);
      this._gboxProcess.Name = "_gboxProcess";
      this._gboxProcess.Size = new System.Drawing.Size(627, 65);
      this._gboxProcess.TabIndex = 3;
      this._gboxProcess.TabStop = false;
      this._gboxProcess.Text = "Process";
      // 
      // _progressBar
      // 
      this._progressBar.Location = new System.Drawing.Point(9, 37);
      this._progressBar.Name = "_progressBar";
      this._progressBar.Size = new System.Drawing.Size(610, 16);
      this._progressBar.TabIndex = 1;
      // 
      // _lblProcessName
      // 
      this._lblProcessName.AutoSize = true;
      this._lblProcessName.Location = new System.Drawing.Point(6, 21);
      this._lblProcessName.Name = "_lblProcessName";
      this._lblProcessName.Size = new System.Drawing.Size(45, 13);
      this._lblProcessName.TabIndex = 0;
      this._lblProcessName.Text = "Process";
      // 
      // _btnStart
      // 
      this._btnStart.Location = new System.Drawing.Point(530, 13);
      this._btnStart.Name = "_btnStart";
      this._btnStart.Size = new System.Drawing.Size(92, 52);
      this._btnStart.TabIndex = 0;
      this._btnStart.Text = "Start";
      this._btnStart.UseVisualStyleBackColor = true;
      this._btnStart.Click += new System.EventHandler(this._btnStart_Click);
      // 
      // UpdateForm
      // 
      this.AcceptButton = this._btnStart;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(634, 321);
      this.Controls.Add(this._btnStart);
      this.Controls.Add(this._gboxProcess);
      this.Controls.Add(this._gboxLog);
      this.Controls.Add(this._btnViewDetails);
      this.Controls.Add(this._lblANewVersion);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "UpdateForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Update Manager";
      this._gboxLog.ResumeLayout(false);
      this._gboxProcess.ResumeLayout(false);
      this._gboxProcess.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label _lblANewVersion;
    private System.Windows.Forms.Button _btnViewDetails;
    private System.Windows.Forms.GroupBox _gboxLog;
    private System.Windows.Forms.GroupBox _gboxProcess;
    private System.Windows.Forms.ProgressBar _progressBar;
    private System.Windows.Forms.Label _lblProcessName;
    private System.Windows.Forms.RichTextBox _txtLog;
    private System.Windows.Forms.Button _btnStart;
  }
}