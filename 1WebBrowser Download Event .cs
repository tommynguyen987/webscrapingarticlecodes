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
    public partial class _1WebBrowser_Download_Event : Form
    {
        public _1WebBrowser_Download_Event()
        {
            InitializeComponent();
        }

        private void _1WebBrowser_Download_Event_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://www.olx.com");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //this Document completed Event Fires Multiple times
            //Why
            //actualy this event fires for each iframe
            this.Text += " a";
        }
    }
}
