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
    public partial class _3_Navigate_to_OLX_s_1st_Add_s_Page : Form
    {
        List<string> hist = new List<string>();

        public _3_Navigate_to_OLX_s_1st_Add_s_Page()
        {
            InitializeComponent();
        }

        private void _3_Navigate_to_OLX_s_1st_Add_s_Page_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://www.olx.com/cars-cat-378");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //Getting AddsBlock HtmlElement 
            HtmlElement he = webBrowser1.Document.GetElementById("the-list");

            if (he != null)
            {                
                //Getting Collection of all the Anchor Tags in AddsBlock 
                HtmlElementCollection hec = he.GetElementsByTagName("a");
                
                //getting Link
                string link = hec[0].GetAttribute("href");

                MessageBox.Show(link);

                //Navigating to Link
                webBrowser1.Navigate(link);
            }
        }
    }
}
