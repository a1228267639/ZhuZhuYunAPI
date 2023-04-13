using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZhuZhuYunAPI.Models
{
    public partial class BlackRecord//黑名单
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("machine_code")]//机器码
        public string Machine_Code { get; set; } = null!;

        [Column("ip")]//IP地址
        public string IP { get; set; } = null!;

        [Column("location_date")]//登录时间
        public string Location_Date { get; set; } = null!;
    }
}
