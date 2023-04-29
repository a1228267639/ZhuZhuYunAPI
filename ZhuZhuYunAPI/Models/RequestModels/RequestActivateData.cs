using System.ComponentModel;

namespace ZhuZhuYunAPI.Models.RequestModels
{
    public class RequestActivateData
    {
        public int UserID { get; set; }
        [DefaultValue("168")]
        public string Reg_Money { get; set; } = null!;
        [DefaultValue("1")]
        public int Reg_Type { get; set; } = 1;//1是天 2月 3是年 4是永久
        [DefaultValue("1")]
        public int Reg_Day { get; set; } = 1;
        [DefaultValue("测试")]
        public string Reg_Info { get; set; } = null!;
        [DefaultValue(null)]
        public IFormFile Reg_Vocher { get; set; } = null!;
    }
}
