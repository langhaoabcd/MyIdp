using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public DiscoveryDocumentResponse disco { get; set; }

        private async void button1_Click(object sender, EventArgs e)
        {
            var username = textBox1.Text;
            var pwd = textBox2.Text;

            //发现文档
            using var client = new HttpClient();
            disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000/");
            if (disco.IsError)
            {
                MessageBox.Show(disco.Error);
                return;
            }

            //request token
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "winform client",
                ClientSecret = "winform secret",                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
                UserName = username,
                Password = pwd,
                Scope = "test1Api openid profile",
            });
            textBox3.Text = tokenResponse.AccessToken;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var accessToken = textBox3.Text;

            // call api
            using var apiClient = new HttpClient();
            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.GetAsync("http://localhost:5001/api/identity");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(response.StatusCode.ToString());
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                textBox4.Text = content;
            }
            //textBox4.Text;
        }

        private async void button3_ClickAsync(object sender, EventArgs e)
        {
            var accessToken = textBox3.Text;

            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.GetAsync(disco.UserInfoEndpoint);//获取identity Resource
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(response.StatusCode.ToString());
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                textBox5.Text = content;
            }
            //textBox5.Text;
        }
    }
}
