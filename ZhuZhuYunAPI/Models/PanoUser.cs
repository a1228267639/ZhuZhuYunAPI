using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZhuZhuYunAPI.Models
{
    public partial class PanoUser
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]//账号ID
        public int UserID { get; set; }

        [Column("reg_day")]//激活天数
        public int Reg_Day { get; set; } = 0;

        [Column("reg_date")]//激活时间
        public string Reg_Date { get; set; } = null!;

        [Column("end_date")]//到期时间
        public string End_Date { get; set; } = null!;

        [Column("ip")]//IP地址
        public string IP { get; set; } = null!;

        [Column("location")]//登录地点
        public string Location { get; set; } = null!;

        [Column("reg_info")]//激活联系人信息
        public string Reg_Info { get; set; } = null!;
    }
}
