using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;

namespace WebScraping_Article_Codes
{
    public partial class _7_Saving_All_Images : Form
    {
        public _7_Saving_All_Images()
        {
            InitializeComponent();
        }

        private void _7_Saving_All_Images_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://www.olx.com");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //to use following DOM classes, u need to add reference to Microsoft.mshtml
            //using mshtml;

            IHTMLDocument2 doc = (IHTMLDocument2)webBrowser1.Document.DomDocument;

            IHTMLControlRange imgRange = (IHTMLControlRange)((HTMLBody)doc.body).createControlRange();

            foreach (IHTMLImgElement img in doc.images)
            {

                imgRange.add((IHTMLControlElement)img);

                imgRange.execCommand("Copy", false, null);

                try
                {
                    using (Bitmap bmp = (Bitmap)Clipboard.GetDataObject().GetData(DataFormats.Bitmap))
                        bmp.Save(img.nameProp + ".jpg");
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
