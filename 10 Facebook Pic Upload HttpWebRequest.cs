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
    public partial class _10_Facebook_Pic_Upload_HttpWebRequest : Form
    {
        string postData;
        string username;
        string password;

        CookieCollection cookies = new CookieCollection();

        string useragent = "Mozilla/2.0 (Windows NT 6.1; WOW64; rv:19.0) Gecko/20100101 Firefox/6.0";


        public _10_Facebook_Pic_Upload_HttpWebRequest()
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
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string file = openFileDialog1.FileName;
                    cookies = new CookieCollection();
                    postData = "";
                    username = textBox1.Text;
                    password = textBox2.Text;
                    backgroundWorker1.RunWorkerAsync(file);
                    button1.Enabled = false;
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string file = (string)e.Argument;
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
                
                //Facebook returns around 9 Cookies, in case of successful login
                //so this check is to see either it was successful login attempt or not
                if (getResponse.Cookies.Count > 6)
                {
                    //Getting Albums Page
                    //To get link to an album, so that picture can be uploaded to that
                    HttpWebRequest Arequest = (HttpWebRequest)WebRequest.Create("http://www.facebook.com/media/albums/");
                    Arequest.CookieContainer = new CookieContainer();
                    Arequest.CookieContainer.Add(cookies);
                    Arequest.UserAgent = useragent;
                    Arequest.KeepAlive = false;
                    Arequest.Timeout = 45000;

                    HttpWebResponse Aresponse = (HttpWebResponse)Arequest.GetResponse();

                    cookies.Add(Aresponse.Cookies);

                    StreamReader Asr = new StreamReader(Aresponse.GetResponseStream());

                    bool isFound = false;
                    string Ahtml = Asr.ReadToEnd();

                    Asr.Close();
                    Aresponse.Close();

                    //This Regex logic obtains one album link for all the albums shown on the page
                    //This is all undertanding of Webscraping and retrival or required information
                    //what all info is required, this is determined by performing the activity in 
                    //browser and then studying the browser's HTTP Headers
                    //for this purpose Live HTTP Header add on is used for Mozila Firefox
                    Regex Areg = new Regex(@"photoTextTitle[^>]+><strong>[^>]+");
                    MatchCollection Amc = Areg.Matches(Ahtml);
                    Match Am = Amc[0];
                    {
                        string href = Am.Value;
                        href = href.Substring(href.IndexOf("http"));
                        href = href.Remove(href.IndexOf("\">"));
                        string aName = Am.Value;
                        aName = aName.Substring(aName.IndexOf("<strong>") + 8);
                        aName = aName.Replace("</strong", "");

                        MessageBox.Show("Album Link:" + href);
                        isFound = true;
                        backgroundWorker1.ReportProgress(0, "Album " + aName + " Found");
                        
                        Regex Ureg = new Regex(@"user=[0-9]+""");
                        MatchCollection Umc = Ureg.Matches(Ahtml);
                        if (Umc.Count > 0)
                        {
                            string userID = Umc[0].Value.Replace("user=", "").Replace("\"", "");
                            MessageBox.Show("User ID: " + userID);
                            {
                                if (file.ToLower().EndsWith(".bmp") || file.ToLower().EndsWith(".gif") || file.ToLower().EndsWith(".jpg")
                                    || file.ToLower().EndsWith(".jpeg") || file.ToLower().EndsWith(".png") || file.ToLower().EndsWith(".tif")
                                    || file.ToLower().EndsWith(".tiff"))
                                {
                                    //The HTTP upload function, which prepares the 
                                    //post data for image uploading
                                    HTTPuploadFile(file, "Sample Upload", href, userID);                                    
                                }
                            }
                        }
                    }
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

        private bool HTTPuploadFile(string filepath, string caption, string albLink, string userID)
        {
            try
            {
                albLink = albLink.Replace("&amp;", "&");
                albLink = albLink.Replace("&amp", "&");
                

                string fbdtsg = "";
                List<string> values = new List<string>();
                backgroundWorker1.ReportProgress(0, "Requesting Upload Page");

                FileInfo fi = new FileInfo(filepath);
                string fileName = fi.Name;
                string contentType = "image/" + fi.Extension.Replace(".", "");

                //requesting album link page so that upload form html can be obtained
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(albLink);
                req.CookieContainer = new CookieContainer();
                req.CookieContainer.Add(cookies);
                req.AllowAutoRedirect = true;
                req.UserAgent = useragent;
                req.Referer = "https://www.facebook.com/";
                req.KeepAlive = true;
                req.Timeout = 30 * 1000;

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                cookies.Add(resp.Cookies);

                string html = sr.ReadToEnd();
                sr.Close();
                resp.Close();

                backgroundWorker1.ReportProgress(0, "Preparing Upload");
                
                //From File upload form, reading all input tags name and values
                Regex reg = new Regex(@"fb_dtsg"" value=""[A-Za-z0-9]+");
                Match mc = reg.Match(html);
                if (mc != null)
                {
                    string v = mc.Value.Replace(@"fb_dtsg"" value=""", "").Replace("&", "");
                    values.Add("name=\"fb_dtsg\"\r\n\r\n" + v);
                    fbdtsg = v;
                    //MessageBox.Show(v);
                }

                //userID is id
                if (mc != null)
                {
                    values.Add("name=\"id\"\r\n\r\n" + userID);
                    //MessageBox.Show(userID);
                }

                reg = new Regex(@"qn=[a-z0-9A-Z]+&");
                mc = reg.Match(html);
                if (mc != null)
                {
                    string v = mc.Value.Replace("qn=", "").Replace("&", "");
                    values.Add("name=\"qn\"\r\n\r\n" + v);
                    //MessageBox.Show(v);
                }

                values.Add("name=\"album_order\"\r\n\r\n" + "0");

                //values.Add("name=\"upload_id\"\r\n\r\n" + "z_0_y_0");

                values.Add("name=\"uploader\"\r\n\r\n" + "flash_pro");
                values.Add("name=\"response_format\"\r\n\r\n" + "json");

                reg = new Regex(@"album_fbid=[0-9]+");
                mc = reg.Match(html);
                if (mc != null)
                {
                    string v = mc.Value.Replace("album_fbid=", "").Replace("&", "");
                    values.Add("name=\"album_fbid\"\r\n\r\n" + v);
                    //MessageBox.Show(v);
                }

                values.Add("name=\"dont_publish\"\r\n\r\n" + "0");

                if (values.Count > 6)
                {
                    //preparing data to send
                    string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                    byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                    //http://upload.facebook.com/media/upload/photos/flash/?ref=album_empty&__a=1&__user=
                    HttpWebRequest wr = (HttpWebRequest)WebRequest.Create("http://upload.facebook.com/media/upload/photos/flash/?__a=1&__user=" + userID);
                    wr.ContentType = "multipart/form-data; boundary=" + boundary;
                    wr.KeepAlive = true;
                    wr.CookieContainer = new CookieContainer();
                    wr.CookieContainer.Add(cookies); //recover cookies First request
                    wr.Method = WebRequestMethods.Http.Post;
                    wr.UserAgent = useragent;
                    wr.AllowWriteStreamBuffering = true;
                    wr.ProtocolVersion = HttpVersion.Version11;
                    wr.AllowAutoRedirect = true;
                    wr.Referer = albLink;
                    //To use proxy
                    //wr.Proxy = new WebProxy("127.0.0.1:9999");

                    backgroundWorker1.ReportProgress(0, "Uploading: " + fileName);
                    //sending data
                    Stream rs = wr.GetRequestStream();
                    string formdataTemplate = "Content-Disposition: form-data; ";
                    foreach (string v in values)
                    {
                        rs.Write(boundarybytes, 0, boundarybytes.Length);
                        string formitem = formdataTemplate + v;
                        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                        rs.Write(formitembytes, 0, formitembytes.Length);
                    }
                    rs.Write(boundarybytes, 0, boundarybytes.Length);

                    //prepairing file for upload
                    string headerTemplate = "Content-Disposition: form-data; filename=\"{0}\"\r\nContent-Type: {1}\r\n\r\n";
                    string header = string.Format(headerTemplate, "mypic" + fi.Extension, contentType);
                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                    rs.Write(headerbytes, 0, headerbytes.Length);

                    //uploading file
                    FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        rs.Write(buffer, 0, bytesRead);
                    }
                    fileStream.Close();

                    //closing upload
                    byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                    rs.Write(trailer, 0, trailer.Length);
                    rs.Close();

                    HttpWebResponse wresp = (HttpWebResponse)wr.GetResponse();

                    cookies.Add(wresp.Cookies);

                    wresp.Close();

                    backgroundWorker1.ReportProgress(0, "Upload Complete " + fileName);
                    return true;
                }
            }
            catch (Exception ex)
            {
                backgroundWorker1.ReportProgress(0, ex.Message);                
                return false;
            }
            return true;
        }
    }
}
