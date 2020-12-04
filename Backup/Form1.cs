using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Specialized;
using System.Diagnostics;

namespace WebScraping_Article_Codes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _1WebBrowser_Download_Event w = new _1WebBrowser_Download_Event();
            w.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _2_Web_Browser_Single_Event w = new _2_Web_Browser_Single_Event();
            w.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _3_Navigate_to_OLX_s_1st_Add_s_Page w = new _3_Navigate_to_OLX_s_1st_Add_s_Page();
            w.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _4_Navigate_to_All_the_Adds w = new _4_Navigate_to_All_the_Adds();
            w.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _5_Yahoo_Signin_Form w = new _5_Yahoo_Signin_Form();
            w.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _6_Adding_Custom_Headers w = new _6_Adding_Custom_Headers();
            w.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _7_Saving_All_Images w = new _7_Saving_All_Images();
            w.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _8_Setting_Proxy_for_IE w = new _8_Setting_Proxy_for_IE();
            w.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //using System.Text.RegularExpression

            string txt = "abc 123 xyz";

            Regex reg = new Regex(@"[0-9]+");

            Match m = reg.Match(txt);

            MessageBox.Show(m.Value);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string txt = "This is 2nd Example";

            Regex reg = new Regex(@"[A-Z0-9a-z]+");

            //Alternative Regex Can be

            //reg = new Regex("[^ ]+");

            //See Article for more details

            MatchCollection mc = reg.Matches(txt);

            foreach (Match m in mc)
                MessageBox.Show(m.Value);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string txt = "Now a days my cell numer is 123-45678. my old was 987-65432. 2nd was good";

            Regex reg = new Regex(@"\d\d\d-\d\d\d\d\d");

            MatchCollection mc = reg.Matches(txt);

            foreach (Match m in mc)
                MessageBox.Show(m.Value);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                //blockig Mode
                //if data is more, Form may become unresponsive
                //See article for details view
                //using System.Net

                WebClient wc = new WebClient();

                textBox1.Text = (wc.DownloadString("http://www.yahoo.com"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                //downloading file and saving to local directory
                //using Blocking Mode DownloadFile

                WebClient wc = new WebClient();

                wc.DownloadFile("http://www.dotnetperls.com/one.png", "one.png");

                MessageBox.Show("Image Saved at" + Directory.GetCurrentDirectory() + "\\one.png");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                //Unblocking mode, or Asychronous mode

                WebClient wc = new WebClient();

                //this Event will be fired once Dowloading is Complete
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);

                wc.DownloadStringAsync(new Uri("http://www.yahoo.com"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                //Accessing the Downloaded String
                string html = e.Result;
                this.Text = "Complete";
                //Code to Use Downloaded String       
                textBox1.Text = html;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                WebClient wc = new WebClient();

                StreamReader sr = new StreamReader(wc.OpenRead("http://www.yahoo.com"));
                //Here You Can Perform IO 

                //Operations like, Read, ReadLine   

                //ReadBlock, ReadToEnd etc 

                //Supported by StreamReader Class

                //Like we are just readling 5 lines

                for (int i = 0; i < 5; i++)
                {
                    textBox1.Text += sr.ReadLine() + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                string uriString = "http://www.google.com/search";

                //Create a new WebClient instance.  

                WebClient wc = new WebClient();

                //using System.Collections.Specialized;
                //Create a new NameValueCollection instance to hold the QueryString parameters and values.

                NameValueCollection myQSC = new NameValueCollection();

                //Add Parameters to the Collection      

                myQSC.Add("q", "CSharpdotNetTech");

                // Attach QueryString to the WebClient. 
                wc.QueryString = myQSC;

                //Download the search results Web page into 'searchresult.htm' 

                wc.DownloadFile(uriString, "searchresult.html");

                Process.Start("searchresult.html");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                String uriString = "http://logme.mobi";

                // Create a new WebClient instance.  

                WebClient myWebClient = new WebClient();

                myWebClient.Proxy = new WebProxy("77.243.10.210:3128");

                textBox1.Text = myWebClient.DownloadString(uriString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
            else
                MessageBox.Show("Wait For Completion");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 5; i++)
            {
                backgroundWorker1.ReportProgress(i * 20, i.ToString());
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            button18.Text = e.ProgressPercentage.ToString() + " - " + (string)e.UserState;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button18.Text = "Complete - BackgroundWorker";
        }

        private void button19_Click(object sender, EventArgs e)
        {
            _9_Facebook_Login_using_HTTPWebRequest w = new _9_Facebook_Login_using_HTTPWebRequest();
            w.ShowDialog();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            _10_Facebook_Pic_Upload_HttpWebRequest w = new _10_Facebook_Pic_Upload_HttpWebRequest();
            w.ShowDialog();
        }
    }
}