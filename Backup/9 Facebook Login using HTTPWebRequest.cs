using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace WebScraping_Article_Codes
{
    public partial class _9_Facebook_Login_using_HTTPWebRequest : Form
    {
        //For this example you shal study related protion of the article
        //which explains why we are making following HTTPWebRequest packtes
        //why the different fields are included in the request

        string postData;
        string username;
        string password;

        CookieCollection cookies = new CookieCollection();

        string useragent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0";


        public _9_Facebook_Login_using_HTTPWebRequest()
        {
            InitializeComponent();
        }

        private void _9_Gmail_Login_using_HTTPWebRequest_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                cookies = new CookieCollection();
                postData = "";
                username = textBox1.Text;
                password = textBox2.Text;
                backgroundWorker1.RunWorkerAsync();
                button1.Enabled = false;
            }
        }
        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                backgroundWorker1.ReportProgress(0, "Getting Login Page");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.facebook.com/");
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
                request.UserAgent = useragent;
                request.KeepAlive = false;
                request.Timeout = 45000;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                cookies.Add(response.Cookies);

                StreamReader sr = new StreamReader(response.GetResponseStream());

                //Here were getting the Login Form HTML
                //then collection name and values of all the input fields of the form
                //for details, see article, use of Live HTTP Headers
                string html = sr.ReadToEnd();
                html = html.Substring(html.IndexOf("login_form"));
                html = html.Remove(html.IndexOf("/form"));
                Regex reg = new Regex(@"name=""[^""]+"" value=""[^""]+""");
                MatchCollection mc = reg.Matches(html);
                List<string> values = new List<string>();
                for (int k = 0; k < mc.Count; k++)
                {
                    if (k != 0)
                        postData += "&";
                    if (k == 1)
                    {
                        postData += "email=" + username + "&pass=" + password + "&";
                    }
                    string m = mc[k].Value.Replace("\"", "");
                    m = m.Replace("name=", "");
                    m = m.Replace(" value=", "=");
                    postData += m;
                    //MessageBox.Show(m);
                }
                //This is the data which is posted to login form Action url
                //See article related section, for how we see postdata using Live HTTP Headers
                MessageBox.Show(postData);

                //Facebook returns around 9 Cookies, in case of successful login
                //so this check is to see either it was successful login attempt or not
                backgroundWorker1.ReportProgress(0, "Posting Login Info");
                string getUrl = "https://www.facebook.com/login.php?login_attempt=1";
                HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(getUrl);
                getRequest.CookieContainer = new CookieContainer();
                getRequest.CookieContainer.Add(cookies); //recover cookies First request
                getRequest.Method = WebRequestMethods.Http.Post;
                getRequest.UserAgent = useragent;
                getRequest.AllowWriteStreamBuffering = true;
                getRequest.ProtocolVersion = HttpVersion.Version11;
                getRequest.AllowAutoRedirect = false;
                getRequest.ContentType = "application/x-www-form-urlencoded";
                getRequest.Referer = "https://www.facebook.com";
                getRequest.KeepAlive = false;
                getRequest.Timeout = 45000;

                byte[] byteArray = Encoding.ASCII.GetBytes(postData);
                getRequest.ContentLength = byteArray.Length;
                Stream newStream = getRequest.GetRequestStream(); //open connection
                newStream.Write(byteArray, 0, byteArray.Length); // Send the data.
                newStream.Close();

                HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse();
                cookies.Add(getResponse.Cookies);                

                if (getResponse.Cookies.Count > 6)
                {
                    backgroundWorker1.ReportProgress(0, "Getting Main Page");
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.facebook.com/");
                    req.CookieContainer = new CookieContainer();
                    req.CookieContainer.Add(cookies);
                    req.UserAgent = useragent;
                    req.KeepAlive = false;
                    req.Timeout = 45000;

                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                    StreamReader srs = new StreamReader(resp.GetResponseStream());
                    StreamWriter sw = new StreamWriter("main.html");
                    sw.WriteLine(srs.ReadToEnd());
                    sw.Close();
                    srs.Close();

                    //MessageBox.Show(getResponse.Cookies.Count.ToString());
                    getResponse.Close();

                    Process.Start("main.html");
                }
                else
                {
                    MessageBox.Show("Failed to Login, Either Security Enabled or Wrong Password");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = (string)e.UserState;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
