using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WebScraping_Article_Codes
{
    public partial class _8_Setting_Proxy_for_IE : Form
    {
        public _8_Setting_Proxy_for_IE()
        {
            InitializeComponent();
        }

        private void _8_Setting_Proxy_for_IE_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Changing Registry value for Proxy
            //u need to add
            //using Microsoft.Win32;
            //to use Registry class

            RegistryKey registry = Registry.CurrentUser.OpenSubKey(
              "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

            //registry.SetValue("ProxyEnable", 1);

            registry.SetValue("ProxyServer", textBox1.Text);

            registry.Flush();

            registry.Close();
        }
    }
}
