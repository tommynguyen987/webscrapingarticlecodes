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
    public partial class _4_Navigate_to_All_the_Adds : Form
    {
        List<string> urls = new List<string>();

        public _4_Navigate_to_All_the_Adds()
        {
            InitializeComponent();
        }

        private void _4_Navigate_to_All_the_Adds_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://www.olx.com/cars-cat-378");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //at start urls=0
            //urls contains ads link
            if (urls.Count > 0)
            {
                string u = urls[0];
                urls.Remove(u);

                webBrowser1.Navigate(u);
                return;
            }

            HtmlElement he = webBrowser1.Document.GetElementById("the-list");
            HtmlElementCollection hec = he.GetElementsByTagName("a");

            foreach (HtmlElement a in hec)
            {
                string href = a.GetAttribute("href");
                if (href != "http://www.olx.com/cars-cat-378")
                {
                    //adding ad links to the urls
                    if (!urls.Contains(href))
                        urls.Add(href);
                }
            }
        }
    }
}
