using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebScraping_Article_Codes
{
    public partial class _6_Adding_Custom_Headers : Form
    {
        public _6_Adding_Custom_Headers()
        {
            InitializeComponent();
        }

        private void _6_Adding_Custom_Headers_Load(object sender, EventArgs e)
        {
            string url = "http://logme.mobi";

            //Last Field we Enter is a Custom Header
            //User Agents can also be changed in same way            
            webBrowser1.Navigate(url, "_self", null, "Referrer: http://www.youtube.com/user/csharpdotnettech"); 
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
