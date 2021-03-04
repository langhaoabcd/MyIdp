
namespace SmsLogin
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.label1 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAccessToken = new System.Windows.Forms.TextBox();
            this.btnIdentity = new System.Windows.Forms.Button();
            this.txtApiResource = new System.Windows.Forms.TextBox();
            this.txtIdentity = new System.Windows.Forms.TextBox();
            this.btnApiResource = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "手机号：";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(58, 81);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(534, 40);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "1.登录获取Token";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(120, 41);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(170, 23);
            this.txtPhone.TabIndex = 2;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(422, 39);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(170, 23);
            this.txtCode.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(360, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "验证码：";
            // 
            // txtAccessToken
            // 
            this.txtAccessToken.Location = new System.Drawing.Point(58, 137);
            this.txtAccessToken.Multiline = true;
            this.txtAccessToken.Name = "txtAccessToken";
            this.txtAccessToken.Size = new System.Drawing.Size(534, 95);
            this.txtAccessToken.TabIndex = 5;
            // 
            // btnIdentity
            // 
            this.btnIdentity.Location = new System.Drawing.Point(58, 438);
            this.btnIdentity.Name = "btnIdentity";
            this.btnIdentity.Size = new System.Drawing.Size(534, 41);
            this.btnIdentity.TabIndex = 6;
            this.btnIdentity.Text = "3.获取身份资源";
            this.btnIdentity.UseVisualStyleBackColor = true;
            this.btnIdentity.Click += new System.EventHandler(this.btnIdentity_Click);
            // 
            // txtApiResource
            // 
            this.txtApiResource.Location = new System.Drawing.Point(58, 298);
            this.txtApiResource.Multiline = true;
            this.txtApiResource.Name = "txtApiResource";
            this.txtApiResource.Size = new System.Drawing.Size(534, 121);
            this.txtApiResource.TabIndex = 7;
            // 
            // txtIdentity
            // 
            this.txtIdentity.Location = new System.Drawing.Point(58, 485);
            this.txtIdentity.Multiline = true;
            this.txtIdentity.Name = "txtIdentity";
            this.txtIdentity.Size = new System.Drawing.Size(534, 121);
            this.txtIdentity.TabIndex = 9;
            // 
            // btnApiResource
            // 
            this.btnApiResource.Location = new System.Drawing.Point(58, 238);
            this.btnApiResource.Name = "btnApiResource";
            this.btnApiResource.Size = new System.Drawing.Size(534, 41);
            this.btnApiResource.TabIndex = 8;
            this.btnApiResource.Text = "2.获取Api资源";
            this.btnApiResource.UseVisualStyleBackColor = true;
            this.btnApiResource.Click += new System.EventHandler(this.btnApiResource_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 645);
            this.Controls.Add(this.txtIdentity);
            this.Controls.Add(this.btnApiResource);
            this.Controls.Add(this.txtApiResource);
            this.Controls.Add(this.btnIdentity);
            this.Controls.Add(this.txtAccessToken);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "扩展使用短信登录";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAccessToken;
        private System.Windows.Forms.Button btnIdentity;
        private System.Windows.Forms.TextBox txtApiResource;
        private System.Windows.Forms.TextBox txtIdentity;
        private System.Windows.Forms.Button btnApiResource;
    }
}

