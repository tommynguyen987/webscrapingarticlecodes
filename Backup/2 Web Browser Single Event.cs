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
    public partial class _2_Web_Browser_Single_Event : Form
    {
        List<string> hist = new List<string>();

        public _2_Web_Browser_Single_Event()
        {
            InitializeComponent();
        }

        private void _2_Web_Browser_Single_Event_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://www.olx.com");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //this Document completed Event Fires Multiple times
            //How to avoid it
            //Keep record of already fired event for same url
            //maintain the history
            //if event fired for one url, then add it in history and 
            //next time simply return if event fires for same url
            
            if(!hist.Contains(webBrowser1.Url.ToString()))
                return;
         
            hist.Add(webBrowser1.Url.ToString());

            this.Text+=" a";
        }
    }
}
