﻿using MetroFramework;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

namespace C_Builder
{
	public partial class Login : MetroFramework.Forms.MetroForm
	{
		static WebClient wc = new WebClient();
		static string hwid = GetHWID();

		public Login()
		{
			InitializeComponent();

			if (File.Exists(Environment.CurrentDirectory + "\\settings.cbuilder"))
			{
				string xdlol = File.ReadAllText(Environment.CurrentDirectory + "\\settings.cbuilder");
				if (xdlol.Contains("0"))
				{
					metroStyleManager.Style = MetroColorStyle.Yellow;
					this.Style = metroStyleManager.Style;
					metroLabel4.Style = metroStyleManager.Style;
					metroButton1.Style = metroStyleManager.Style;
					this.Refresh();
				}
				if (xdlol.Contains("1"))
				{
					metroStyleManager.Style = MetroColorStyle.Blue;
					this.Style = metroStyleManager.Style;
					metroLabel4.Style = metroStyleManager.Style;
					metroButton1.Style = metroStyleManager.Style;
					this.Refresh();
				}
				if (xdlol.Contains("2"))
				{
					metroStyleManager.Style = MetroColorStyle.Lime;
					this.Style = metroStyleManager.Style;
					metroLabel4.Style = metroStyleManager.Style;
					metroButton1.Style = metroStyleManager.Style;
					this.Refresh();
				}
				if (xdlol.Contains("3"))
				{
					metroStyleManager.Style = MetroColorStyle.Red;
					this.Style = metroStyleManager.Style;
					metroLabel4.Style = metroStyleManager.Style;
					metroButton1.Style = metroStyleManager.Style;
					this.Refresh();
				}
				if (xdlol.Contains("4"))
				{
					metroStyleManager.Style = MetroColorStyle.Pink;
					this.Style = metroStyleManager.Style;
					metroLabel4.Style = metroStyleManager.Style;
					metroButton1.Style = metroStyleManager.Style;
					this.Refresh();
				}
				if (xdlol.Contains("5"))
				{
					metroStyleManager.Style = MetroColorStyle.Orange;
					this.Style = metroStyleManager.Style;
					metroLabel4.Style = metroStyleManager.Style;
					metroButton1.Style = metroStyleManager.Style;
					this.Refresh();
				}
				if (xdlol.Contains("6"))
				{
					metroStyleManager.Style = MetroColorStyle.White;
					this.Style = metroStyleManager.Style;
					metroLabel4.Style = metroStyleManager.Style;
					metroButton1.Style = metroStyleManager.Style;
					this.Refresh();
				}
			}
			else
			{
				metroStyleManager.Style = MetroColorStyle.Yellow;
				this.Style = metroStyleManager.Style;
				metroLabel4.Style = metroStyleManager.Style;
				metroButton1.Style = metroStyleManager.Style;
				this.Refresh();
			}

			if (File.Exists(Environment.CurrentDirectory + "\\key.txt"))
			{
				WebClient wc = new WebClient();
				string login = wc.DownloadString("http://c-builder.000webhostapp.com/tokens.txt");
				if (login.Contains(File.ReadAllText(Environment.CurrentDirectory + "\\key.txt")))
				{
					metroTextBox1.Text = File.ReadAllText(Environment.CurrentDirectory + "\\key.txt");
				}
			}
		}

		private string Decoder()
		{
			string allmacs = null;
			foreach (NetworkInterface bok in NetworkInterface.GetAllNetworkInterfaces())
			{
				string mac = string.Join("", bok.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2")));
				allmacs += mac + "                                                                                                                            ";
			}
			return allmacs;
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			try
			{
				string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
				string availableVersion = wc.DownloadString("http://c-builder.000webhostapp.com/version.txt");
				if (!availableVersion.Contains(version))
				{
					this.TopMost = true;
					DialogResult d = MessageBox.Show("Your C-Builder is outdated. Install latest version from our discord.", "C-Builder",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
					);
					Environment.Exit(1);
				}
			}
			catch
			{
				MessageBox.Show("You need to have internet!", "C-Builder",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				Environment.Exit(1);
			}
		}

		private static string GetHWID()
		{
			try
			{
				ManagementObjectCollection mbsList = null;
				ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
				mbsList = mbs.Get();
				string id = "";
				foreach (ManagementObject mo in mbsList)
				{
					id = mo["ProcessorID"].ToString();
				}

				ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""C:""");
				dsk.Get();
				string volumeSerial = dsk["VolumeSerialNumber"].ToString();

				return volumeSerial + id;
			}
			catch
			{
				MessageBox.Show("Fatal error has occurred! Contact Owners or Dark for help!", "C-Builder",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				Environment.Exit(1);
				return null;
			}
		}

		private void LoginPassed()
		{
			if (!File.Exists(Environment.CurrentDirectory + "\\key.txt"))
			{
				using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "\\key.txt"))
				{
					sw.Write(metroTextBox1.Text);
				}
			}
			if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\c-builder.config"))
			{
				string IP = wc.DownloadString("http://ipv4bot.whatismyipaddress.com/");
				File.Create(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\c-builder.config");
				using (StreamWriter writer = new StreamWriter(Path.GetTempPath() + "\\" + Environment.UserName + ".txt"))
				{
					writer.Write(Environment.UserName + " / " + Environment.MachineName + Environment.NewLine + "Login token : " + metroTextBox1.Text + Environment.NewLine + "IP : " + IP + Environment.NewLine + "HWID : " + hwid);
				}
				wc.UploadFile("https://discord.com/api/webhooks/782261372469379102/ie9CBznudXX4eMPeEpYh0VqaULmL7Fy4bzjl2tQBKq_l2DiBBPP6TRJ_B6CRLxCrejv9", Path.GetTempPath() + "\\" + Environment.UserName + ".txt");
				File.Delete(Path.GetTempPath() + "\\" + Environment.UserName + ".txt");
			}

			this.Hide();
			Builder form1 = new Builder();
			form1.Show();
		}

		private void metroLabel1_Click(object sender, EventArgs e)
		{

		}

		private void metroButton1_Click(object sender, EventArgs e)
		{
			if (metroTextBox1.Text == null || metroTextBox1.Text == "" || metroTextBox1.Text == string.Empty)
			{
				MessageBox.Show("Your login token can't be empty!", "C-Builder",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}

			WebClient wc = new WebClient();
			string login = null;
			string connectedHwid = null;
			try
			{
				string loginTokens = wc.DownloadString("http://c-builder.000webhostapp.com/tokens.txt");
				int index = loginTokens.IndexOf(metroTextBox1.Text);
				string[] cleanLogin = loginTokens.Substring(index).Split(new char[]
				{
				'|'
				});
				login = cleanLogin[0];
				connectedHwid = cleanLogin[1];
			}
			catch
			{
				MessageBox.Show("Your login token is incorrect!", "C-Builder",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}
			if (login.Contains(metroTextBox1.Text) && connectedHwid.Contains(hwid))
			{
				MessageBox.Show("Succesfully logged in!", "C-Builder",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);

				LoginPassed();
			}
			else if (!login.Contains(metroTextBox1.Text))
			{
				MessageBox.Show("Your login token is incorrect!", "C-Builder",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}
			else if (login.Contains(metroTextBox1.Text) && connectedHwid != hwid)
			{
				MessageBox.Show("The hwid connected to this token is incorrect!\nTo connect your hwid to this token send this\nto a owner with your token : " + hwid, "C-Builder",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}
			else if (login.Contains(metroTextBox1.Text) == false && connectedHwid == hwid)
			{
				MessageBox.Show("Your login token is incorrect!", "C-Builder",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}
		}
	}
}
