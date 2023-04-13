namespace ZhuZhuYunAPI.Models.RequestModels
{
    public class RequestRegisterData
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Machine_Code { get; set; } = null!;
        public string IP { get; set; } = null!;
        public string Location { get; set; } = null!;
    }
}
