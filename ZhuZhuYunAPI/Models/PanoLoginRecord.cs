using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZhuZhuYunAPI.Models
{
    public partial class PanoRegisterRecord
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserID { get; set; }
        [Column("machine_code")]//机器码
        public string Machine_Code { get; set; } = null!;

        [Column("ip")]//IP地址
        public string IP { get; set; } = null!;

        [Column("location")]//登录地点
        public string Location { get; set; } = null!;

        [Column("location_date")]//登录时间
        public string Location_Date { get; set; } = null!;
    }
}
