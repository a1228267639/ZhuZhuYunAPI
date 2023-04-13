namespace ZhuZhuYunAPI.Models.ResponseModels
{
    public class ResponseLoginData
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Token { get; set; } = null!;
        public UserInfo User_Info { get; set; } = null!;
    }
}
