using Centa.SvnLog.Infrastructure.Model;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Centa.SvnLog.WebApi.UnitTest
{
    /// <summary>
    /// CCHR接口测试
    /// </summary>
    public class TestCCHRController : IClassFixture<TestClientProvider>
    {
        private List<Login> _companyList;

        private readonly HttpClient _client;

        [Fact(DisplayName = "CCHR接口测试")]
        [Trait("CCHR WebApi", "CCHR WebApi")]
        public async Task TestCCHR()
        {
            foreach (var company in _companyList)
            {
                string token = await GetToken($"/api/cchr/v1.1/sz/token", company);
                Assert.True(!string.IsNullOrEmpty(token));
                var success = await ActionAsync($"/api/cchr/v1.1/sz/departments", token);
                Assert.True(success);
                success = await ActionAsync($"/api/cchr/v1.1/sz/department-adjusts", token);
                Assert.True(success);
                success = await ActionAsync($"/api/cchr/v1.1/sz/postions", token);
                Assert.True(success);
                success = await ActionAsync($"/api/cchr/v1.1/sz/employees", token);
                Assert.True(success);
                success = await ActionAsync($"/api/cchr/v1.1/sz/employee-attaches", token);
                Assert.True(success);
                success = await ActionAsync($"/api/cchr/v1.1/sz/employee-parttimes", token);
                Assert.True(success);
                success = await ActionAsync($"/api/cchr/v1.1/sz/employee-adjust-applies", token);
                Assert.True(success);
                success = await ActionAsync($"/api/cchr/v1.1/sz/employee-adjust-histories", token);
                Assert.True(success);
            }
        }


        private async Task<string> GetToken(string url, Login companySetting)
        {
            var response = await _client.PostAsJsonAsync(url,
                new Login()
                {
                    CompanyID = companySetting.CompanyID,
                    CompanySecret = companySetting.CompanySecret
                });
            var token = string.Empty;
            var result = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(result))
            {
                var obj = JObject.Parse(result);
                token = obj["data"]["token"].ToString();
            }
            return token;
        }

        private async Task<bool> ActionAsync(string url, string token)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("token", token);
            var result = await _client.GetStringAsync(url);
            return result.Contains(@"""status"":200,");
        }

        public TestCCHRController(TestClientProvider testClientProvider)
        {
            _client = testClientProvider.Client;
            _companyList = new List<Login>();
            _companyList.Add(new Login() {  CompanyID = "zhujy7", CompanySecret = "123456" });
        }

        internal class Login
        {
            public string CompanyID { get; set; }
            public string CompanySecret { get; set; }
        }
    }
}
