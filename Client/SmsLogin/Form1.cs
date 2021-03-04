using IdentityModel.Client;
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

namespace SmsLogin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public DiscoveryDocumentResponse disco { get; set; }
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var phone = txtPhone.Text;
            var code = txtCode.Text;

            //发现文档
            using var client = new HttpClient();
            disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000/");
            if (disco.IsError)
            {
                MessageBox.Show(disco.Error);
                return;
            }

            //request token
            var tokenResponse = await client.RequestTokenAsync(new TokenRequest
            {
                GrantType = "phone_number",
                Address = disco.TokenEndpoint,
                ClientId = "phone_number_auth_client",
                ClientSecret = "phone_number_aut_secret",
                Parameters = new Dictionary<string, string>
                {
                    {"phone",phone },
                    {"code",code }
                },
                //Scope = "test1Api openid profile",
            });
            txtAccessToken.Text = tokenResponse.AccessToken;
        }

        private async void btnApiResource_Click(object sender, EventArgs e)
        {
            var accessToken = txtAccessToken.Text;

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
                txtApiResource.Text = content;
            }
        }

        private async void btnIdentity_Click(object sender, EventArgs e)
        {
            var accessToken = txtAccessToken.Text;

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
                txtIdentity.Text = content;
            }
        }
    }
}
