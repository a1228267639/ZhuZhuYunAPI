using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZhuZhuYunAPI.Models
{
    public class LoginData
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]//账号
        public string UserName { get; set; } = null!;
        [Column("password")]//密码
        public string Password { get; set; } = null!;

        [Column("info")]// 修改密码辅助
        public string Info { get; set; } = null!;
    }
}
