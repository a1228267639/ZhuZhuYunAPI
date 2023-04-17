using System.ComponentModel;

namespace ZhuZhuYunAPI.Models.RequestModels
{
    public class RequestLoginData
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Machine_Code { get; set; } = null!;
        [DefaultValue("22.22.22.22")]
        public string IP { get; set; } = null!;
        [DefaultValue("中国浙江杭州")]
        public string Location { get; set; } = null!;
    }
}
