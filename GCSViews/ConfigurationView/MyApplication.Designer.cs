using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.Windows.Threading;
using RS232._1;
using MissionPlanner.Utilities;

namespace MissionPlanner.GCSViews.ConfigurationView
{
    
    partial class MyApplication
    {
        public comm1 com = new comm1();
        private readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();
        string g, g1, g2, g3, g4;
        bool open = false;

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

        private void checkConnection()
        {
            if (com.CheckPort() == true)
            {
                open = true;
                dispatcherTimer.Interval = TimeSpan.FromTicks(1000);
                //dispatcherTimer.Tick -= dtcker;
                dispatcherTimer.Tick += dtcker;
                dispatcherTimer.Start();
                buttonSet.Enabled = false;
                buttonClose.Enabled = true;

                //asiah
                CustomMessageBox.Show("Com port is set!");
                comboComport.Enabled = false;
                comboBaudrate.Enabled = false;
                comboParity.Enabled = false;
                comboDatabit.Enabled = false;
                comboStopbit.Enabled = false;
            }
            else
            {
                open = false;
                dispatcherTimer.Stop();
                com.ClosePort();
                com = new comm1();

                //asiah
                CustomMessageBox.Show("Serial port connection disconnected!");
                buttonSet.Enabled = true;
                buttonClose.Enabled = false;
                comboComport.Enabled = true;
                comboBaudrate.Enabled = true;
                comboParity.Enabled = true;
                comboDatabit.Enabled = true;
                comboStopbit.Enabled = true;
            }
        }

        private void getavailableports()
        {
            List<string> s = new List<string>();
            s = com.GetAvailablePorts();
            foreach (var i in s)
            {
                comboComport.Items.Add(i.ToString());
            }

            if (comboComport.Items == null)
            {
                //SendData.Text = "No Ports Available";
                //SetBtn.IsEnabled = false;
                comboComport.Enabled = false;
                comboBaudrate.Enabled = false;
                comboParity.Enabled = false;
                comboDatabit.Enabled = false;
                comboStopbit.Enabled = false;
            }

            else
            {
            }
        }

        private void InitializeBaudrate()
        {
            string[] str = new string[] { "9600", "11200", "19200", "115200" };

            foreach (string s in str)
                comboBaudrate.Items.Add(s);
            comboBaudrate.SelectedIndex = 0;
        }
            
        private void InitializeParity()
        {
            string[] str = new string[] { "Even", "Odd", "None", "Space" };

            foreach (string s in str)
                comboParity.Items.Add(s);
            comboParity.SelectedIndex = 0;
        }

        private void InitializeDatabit()
        {
            string[] str = new string[] { "4", "5", "6", "7", "8"};

            foreach (string s in str)
                comboDatabit.Items.Add(s);
            comboDatabit.SelectedIndex = 0;
        }

        private void InitializeStopbit()
        {
            string[] str = new string[] { "0", "1", "2" };

            foreach (string s in str)
                comboStopbit.Items.Add(s);
            comboStopbit.SelectedIndex = 0;
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.comboComport = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBaudrate = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboParity = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboDatabit = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboStopbit = new System.Windows.Forms.ComboBox();
            this.buttonSet = new System.Windows.Forms.Button();
            this.textboxEngineSpeed = new System.Windows.Forms.TextBox();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textboxThrottlePosition = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textboxEngineTemperature = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textboxBatteryVoltage = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textboxReceivedData = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port:";
            // 
            // comboComport
            // 
            this.comboComport.FormattingEnabled = true;
            this.comboComport.Location = new System.Drawing.Point(30, 45);
            this.comboComport.Name = "comboComport";
            this.comboComport.Size = new System.Drawing.Size(89, 21);
            this.comboComport.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(128, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Baud Rate:";
            // 
            // comboBaudrate
            // 
            this.comboBaudrate.FormattingEnabled = true;
            this.comboBaudrate.Location = new System.Drawing.Point(128, 45);
            this.comboBaudrate.Name = "comboBaudrate";
            this.comboBaudrate.Size = new System.Drawing.Size(89, 21);
            this.comboBaudrate.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(227, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Parity:";
            // 
            // comboParity
            // 
            this.comboParity.FormattingEnabled = true;
            this.comboParity.Location = new System.Drawing.Point(227, 45);
            this.comboParity.Name = "comboParity";
            this.comboParity.Size = new System.Drawing.Size(89, 21);
            this.comboParity.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(327, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Databits:";
            // 
            // comboDatabit
            // 
            this.comboDatabit.FormattingEnabled = true;
            this.comboDatabit.Location = new System.Drawing.Point(327, 45);
            this.comboDatabit.Name = "comboDatabit";
            this.comboDatabit.Size = new System.Drawing.Size(89, 21);
            this.comboDatabit.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(428, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Stopbits:";
            // 
            // comboStopbit
            // 
            this.comboStopbit.FormattingEnabled = true;
            this.comboStopbit.Location = new System.Drawing.Point(428, 45);
            this.comboStopbit.Name = "comboStopbit";
            this.comboStopbit.Size = new System.Drawing.Size(89, 21);
            this.comboStopbit.TabIndex = 9;
            // 
            // buttonSet
            // 
            this.buttonSet.Location = new System.Drawing.Point(542, 27);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new System.Drawing.Size(104, 40);
            this.buttonSet.TabIndex = 10;
            this.buttonSet.Text = "Set Port";
            this.buttonSet.UseVisualStyleBackColor = true;
            this.buttonSet.Click += new System.EventHandler(this.buttonSet_Click);
            // 
            // textboxEngineSpeed
            // 
            this.textboxEngineSpeed.Location = new System.Drawing.Point(30, 175);
            this.textboxEngineSpeed.Name = "textboxEngineSpeed";
            this.textboxEngineSpeed.Size = new System.Drawing.Size(149, 20);
            this.textboxEngineSpeed.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Engine Speed (RPM)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(185, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Throttle Position";
            // 
            // textboxThrottlePosition
            // 
            this.textboxThrottlePosition.Location = new System.Drawing.Point(185, 175);
            this.textboxThrottlePosition.Name = "textboxThrottlePosition";
            this.textboxThrottlePosition.Size = new System.Drawing.Size(149, 20);
            this.textboxThrottlePosition.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(341, 157);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Engine Temperature";
            // 
            // textboxEngineTemperature
            // 
            this.textboxEngineTemperature.Location = new System.Drawing.Point(341, 175);
            this.textboxEngineTemperature.Name = "textboxEngineTemperature";
            this.textboxEngineTemperature.Size = new System.Drawing.Size(149, 20);
            this.textboxEngineTemperature.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(497, 157);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Battery Voltage";
            // 
            // textboxBatteryVoltage
            // 
            this.textboxBatteryVoltage.Location = new System.Drawing.Point(497, 175);
            this.textboxBatteryVoltage.Name = "textboxBatteryVoltage";
            this.textboxBatteryVoltage.Size = new System.Drawing.Size(149, 20);
            this.textboxBatteryVoltage.TabIndex = 18;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(-15, -15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 19;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(-15, -15);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 20;
            // 
            // textboxReceivedData
            // 
            this.textboxReceivedData.Location = new System.Drawing.Point(30, 211);
            this.textboxReceivedData.Multiline = true;
            this.textboxReceivedData.Name = "textboxReceivedData";
            this.textboxReceivedData.Size = new System.Drawing.Size(616, 121);
            this.textboxReceivedData.TabIndex = 21;
            // 
            // buttonClose
            // 
            this.buttonClose.Enabled = false;
            this.buttonClose.Location = new System.Drawing.Point(542, 75);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(104, 40);
            this.buttonClose.TabIndex = 22;
            this.buttonClose.Text = "Close Port";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // MyApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textboxReceivedData);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textboxBatteryVoltage);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textboxEngineTemperature);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textboxThrottlePosition);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textboxEngineSpeed);
            this.Controls.Add(this.buttonSet);
            this.Controls.Add(this.comboStopbit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboDatabit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboParity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBaudrate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboComport);
            this.Controls.Add(this.label1);
            this.Name = "MyApplication";
            this.Size = new System.Drawing.Size(720, 413);
            this.Load += new System.EventHandler(this.MyApplication_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboComport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBaudrate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboParity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboDatabit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboStopbit;
        private System.Windows.Forms.Button buttonSet;
        private System.Windows.Forms.TextBox textboxEngineSpeed;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textboxThrottlePosition;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textboxEngineTemperature;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textboxBatteryVoltage;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textboxReceivedData;
        private System.Windows.Forms.Button buttonClose;
    }
}
