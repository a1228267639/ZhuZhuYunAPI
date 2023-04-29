using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZhuZhuYunAPI.Models
{
    public class UserInfo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]//账号ID
        public int UserID { get; set; }

        [Column("user_type")]//用户类型  0是免费 1是测试  2是付费  3是永久激活 -1到期 
        public int User_Type { get; set; } = 0;

        [Column("machine_codes")]// 绑定的 机器码 
        public string Machine_Codes { get; set; } = null!;

        [Column("bind_machinecount")]// 可以绑定的 机器码  数量
        public int BindMachine_Count { get; set; } = 1;
    }
}
