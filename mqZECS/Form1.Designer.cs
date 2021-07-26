namespace mqZECS
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_confirm = new System.Windows.Forms.Button();
            this.label_tip = new System.Windows.Forms.Label();
            this.richTextBox_message = new System.Windows.Forms.RichTextBox();
            this.button_message = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textMQServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textMQPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.textBox_Account = new System.Windows.Forms.TextBox();
            this.Account = new System.Windows.Forms.Label();
            this.textBox_SQLServer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.database = new System.Windows.Forms.Label();
            this.textDB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_DBSyncInterval = new System.Windows.Forms.TextBox();
            this.button_SendTCPMsg = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox_ConnectionType = new System.Windows.Forms.ComboBox();
            this.button_Start = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(125, 522);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 23;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // label_tip
            // 
            this.label_tip.AutoSize = true;
            this.label_tip.Location = new System.Drawing.Point(6, 658);
            this.label_tip.Name = "label_tip";
            this.label_tip.Size = new System.Drawing.Size(0, 12);
            this.label_tip.TabIndex = 22;
            // 
            // richTextBox_message
            // 
            this.richTextBox_message.Location = new System.Drawing.Point(2, 64);
            this.richTextBox_message.Name = "richTextBox_message";
            this.richTextBox_message.Size = new System.Drawing.Size(827, 452);
            this.richTextBox_message.TabIndex = 21;
            this.richTextBox_message.Text = "";
            // 
            // button_message
            // 
            this.button_message.Location = new System.Drawing.Point(6, 521);
            this.button_message.Name = "button_message";
            this.button_message.Size = new System.Drawing.Size(75, 23);
            this.button_message.TabIndex = 31;
            this.button_message.Text = "Date view";
            this.button_message.UseVisualStyleBackColor = true;
            this.button_message.Click += new System.EventHandler(this.button_message_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textMQServer
            // 
            this.textMQServer.Location = new System.Drawing.Point(79, 12);
            this.textMQServer.MaxLength = 32;
            this.textMQServer.Name = "textMQServer";
            this.textMQServer.Size = new System.Drawing.Size(118, 21);
            this.textMQServer.TabIndex = 6;
            this.textMQServer.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "MQServer";
            // 
            // textMQPort
            // 
            this.textMQPort.Location = new System.Drawing.Point(255, 12);
            this.textMQPort.MaxLength = 8;
            this.textMQPort.Name = "textMQPort";
            this.textMQPort.Size = new System.Drawing.Size(81, 21);
            this.textMQPort.TabIndex = 36;
            this.textMQPort.Text = "61616";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 35;
            this.label2.Text = "MQPort";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(342, 10);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 37;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(432, 10);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(75, 23);
            this.buttonDisconnect.TabIndex = 38;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // textBox_Account
            // 
            this.textBox_Account.Location = new System.Drawing.Point(255, 37);
            this.textBox_Account.MaxLength = 16;
            this.textBox_Account.Name = "textBox_Account";
            this.textBox_Account.Size = new System.Drawing.Size(81, 21);
            this.textBox_Account.TabIndex = 42;
            this.textBox_Account.Text = "sa";
            // 
            // Account
            // 
            this.Account.AutoSize = true;
            this.Account.Location = new System.Drawing.Point(203, 41);
            this.Account.Name = "Account";
            this.Account.Size = new System.Drawing.Size(47, 12);
            this.Account.TabIndex = 41;
            this.Account.Text = "Account";
            // 
            // textBox_SQLServer
            // 
            this.textBox_SQLServer.Location = new System.Drawing.Point(79, 37);
            this.textBox_SQLServer.MaxLength = 32;
            this.textBox_SQLServer.Name = "textBox_SQLServer";
            this.textBox_SQLServer.Size = new System.Drawing.Size(118, 21);
            this.textBox_SQLServer.TabIndex = 40;
            this.textBox_SQLServer.Text = "10.28.160.251";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 39;
            this.label4.Text = "SQL Server";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(342, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 41;
            this.label3.Text = "Password";
            // 
            // textBox_Password
            // 
            this.textBox_Password.Location = new System.Drawing.Point(402, 37);
            this.textBox_Password.MaxLength = 16;
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.PasswordChar = '*';
            this.textBox_Password.Size = new System.Drawing.Size(81, 21);
            this.textBox_Password.TabIndex = 42;
            this.textBox_Password.Text = "sasa";
            // 
            // database
            // 
            this.database.AutoSize = true;
            this.database.Location = new System.Drawing.Point(493, 41);
            this.database.Name = "database";
            this.database.Size = new System.Drawing.Size(53, 12);
            this.database.TabIndex = 41;
            this.database.Text = "Database";
            // 
            // textDB
            // 
            this.textDB.Location = new System.Drawing.Point(553, 37);
            this.textDB.MaxLength = 16;
            this.textDB.Name = "textDB";
            this.textDB.Size = new System.Drawing.Size(81, 21);
            this.textDB.TabIndex = 42;
            this.textDB.Text = "Amadeus5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(521, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 41;
            this.label5.Text = "DB Sync Interval";
            // 
            // textBox_DBSyncInterval
            // 
            this.textBox_DBSyncInterval.Location = new System.Drawing.Point(628, 11);
            this.textBox_DBSyncInterval.MaxLength = 16;
            this.textBox_DBSyncInterval.Name = "textBox_DBSyncInterval";
            this.textBox_DBSyncInterval.Size = new System.Drawing.Size(81, 21);
            this.textBox_DBSyncInterval.TabIndex = 42;
            this.textBox_DBSyncInterval.Text = "180";
            // 
            // button_SendTCPMsg
            // 
            this.button_SendTCPMsg.Location = new System.Drawing.Point(218, 522);
            this.button_SendTCPMsg.Name = "button_SendTCPMsg";
            this.button_SendTCPMsg.Size = new System.Drawing.Size(75, 23);
            this.button_SendTCPMsg.TabIndex = 43;
            this.button_SendTCPMsg.Text = "SendTcpMsg";
            this.button_SendTCPMsg.UseVisualStyleBackColor = true;
            this.button_SendTCPMsg.Click += new System.EventHandler(this.button_SendTCPMsg_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(638, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 12);
            this.label6.TabIndex = 44;
            this.label6.Text = "ConnecttionType";
            // 
            // comboBox_ConnectionType
            // 
            this.comboBox_ConnectionType.FormattingEnabled = true;
            this.comboBox_ConnectionType.Items.AddRange(new object[] {
            "ActiveMQ",
            "TCP Msg",
            "RabbitMQ"});
            this.comboBox_ConnectionType.Location = new System.Drawing.Point(740, 37);
            this.comboBox_ConnectionType.Name = "comboBox_ConnectionType";
            this.comboBox_ConnectionType.Size = new System.Drawing.Size(89, 20);
            this.comboBox_ConnectionType.TabIndex = 45;
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(300, 522);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(75, 23);
            this.button_Start.TabIndex = 46;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 554);
            this.Controls.Add(this.button_Start);
            this.Controls.Add(this.comboBox_ConnectionType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button_SendTCPMsg);
            this.Controls.Add(this.textBox_DBSyncInterval);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textDB);
            this.Controls.Add(this.database);
            this.Controls.Add(this.textBox_Password);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_Account);
            this.Controls.Add(this.Account);
            this.Controls.Add(this.textBox_SQLServer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.textMQPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textMQServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.label_tip);
            this.Controls.Add(this.richTextBox_message);
            this.Controls.Add(this.button_message);
            this.Name = "Form1";
            this.Text = "MagoACS Update";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Label label_tip;
        private System.Windows.Forms.RichTextBox richTextBox_message;
        private System.Windows.Forms.Button button_message;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textMQServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textMQPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.TextBox textBox_Account;
        private System.Windows.Forms.Label Account;
        private System.Windows.Forms.TextBox textBox_SQLServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.Label database;
        private System.Windows.Forms.TextBox textDB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_DBSyncInterval;
        private System.Windows.Forms.Button button_SendTCPMsg;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_ConnectionType;
        private System.Windows.Forms.Button button_Start;
    }
}

