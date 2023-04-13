namespace ZhuZhuYunAPI.Models.RequestModels
{
    public class RequestActivateData
    {
        public int UserID { get; set; }
        //public string Machine_Code { get; set; } = null!;
        public string IP { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Reg_Money { get; set; } = null!;
        public int Reg_Type { get; set; } = 1;//1是天 2月 3是年 4是永久
        public int Reg_Day { get; set; } = 1;
        public string Reg_Info { get; set; } = null!;
    }
}
