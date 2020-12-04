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
    public partial class _5_Yahoo_Signin_Form : Form
    {
        public _5_Yahoo_Signin_Form()
        {
            InitializeComponent();
        }

        private void _5_Yahoo_Signin_Form_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://mail.yahoo.com");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Focus() function is normally not required, 
            //but sometimes sites put javascript function
            //which does allow login until input field is not focus

            HtmlElement hu = webBrowser1.Document.GetElementById("username");
            hu.Focus();

            hu.SetAttribute("Value", textBox1.Text);

            HtmlElement hp = webBrowser1.Document.GetElementById("passwd");
            hp.Focus();
            hp.SetAttribute("Value", textBox2.Text);

            HtmlElement hform = webBrowser1.Document.GetElementById("login_form");
            hform.InvokeMember("submit");

            //Alternative can be
            //HtmlElement hSubmitButton = webBrowser1.Document.GetElementById("login_form");
            //hSubmitButton.InvokeMember("click");
        }
    }
}
