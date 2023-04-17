using CodeLab.Share.ViewModels.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApiUtils;
using ZhuZhuYunAPI.Models;
using ZhuZhuYunAPI.Models.RequestModels;
using ZhuZhuYunAPI.Models.ResponseModels;
using Newtonsoft.Json;

namespace ZhuZhuYunAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private PanoUserContext panoUserContext;
        public LoginController(IWebHostEnvironment webHostEnvironment, PanoUserContext panoUserContext)
        {
            this._webHostEnvironment = webHostEnvironment;
            this.panoUserContext = panoUserContext;
        }

        //添加一个数据，传入一个不带ID的
        [HttpPost("Login")]
        public ApiResponse PostLogin(RequestLoginData requestLogin)
        {
            LoginData? loginData = panoUserContext.LoginData.FirstOrDefault(data => data.UserName == requestLogin.UserName);
            if (loginData == null)
            {
                return ApiResponse.BadRequest("登录失败,账号错误");
            }
            else
            {
                if (loginData.Password == requestLogin.Password)
                {
                    string token = GenerateToken(requestLogin);
                    ResponseLoginData ResponseLoginData = new ResponseLoginData();
                    ResponseLoginData.Token = token;
                    ResponseLoginData.UserName = requestLogin.UserName;
                    ResponseLoginData.Password = MD5Encrypt64(requestLogin.Password);
                    UserInfo? userInfo = panoUserContext.UserInfo.FirstOrDefault(data => data.UserID == loginData.Id);
                    if (userInfo == null)//没有用户信息
                    {
                        userInfo = new UserInfo();
                        PanoTempUser? PanoTempUser = panoUserContext.PanoTempUser.FirstOrDefault(panaTempUser => panaTempUser.Machine_Code == requestLogin.Machine_Code);
                        if (PanoTempUser == null)
                        {
                            userInfo.User_Type = 1;
                            PanoTempUser = new PanoTempUser();
                            PanoTempUser.UserID = loginData.Id;
                            PanoTempUser.IP = requestLogin.IP;
                            PanoTempUser.Machine_Code = requestLogin.Machine_Code;
                            PanoTempUser.Location = requestLogin.Location;
                            PanoTempUser.Reg_Day = AppConfigHelper.GetTempRegTime();
                            PanoTempUser.Reg_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            PanoTempUser.End_Date = DateTime.Now.AddDays(AppConfigHelper.GetTempRegTime()).ToString("yyyy-MM-dd HH:mm:ss");
                            panoUserContext.PanoTempUser.Add(PanoTempUser);  //添加一个
                            panoUserContext.SaveChanges();
                        }
                        else
                        {
                            if (DataCompare(PanoTempUser.End_Date, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                            {
                                userInfo.User_Type = 1;//免费测试
                            }
                            else
                            {
                                userInfo.User_Type = -1;//到期
                            }
                        }
                        userInfo.UserID = loginData.Id;
                        userInfo.BindMachine_Count = 1;
                        //userInfo.Reg_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //userInfo.Reg_Day = AppConfigHelper.GetTempRegTime();
                        //userInfo.End_Date = DateTime.Now.AddDays(AppConfigHelper.GetTempRegTime()).ToString("yyyy-MM-dd HH:mm:ss");
                        List<string> machine_List = new List<string>();
                        machine_List.Add(requestLogin.Machine_Code);
                        string machine_Json = JsonConvert.SerializeObject(machine_List);
                        userInfo.Machine_Codes = machine_Json;
                        userInfo.Reg_Money = "";
                        panoUserContext.UserInfo.Add(userInfo);
                        panoUserContext.SaveChanges();
                        ResponseLoginData.User_Info = userInfo;
                        GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                        return ApiResponse.Ok(ResponseLoginData);
                    }
                    else
                    {
                        List<string>? machine_List = JsonConvert.DeserializeObject<List<string>>(userInfo.Machine_Codes);
                        // 1是测试  2是付费  3是永久激活 -1到期 
                        switch (userInfo.User_Type)
                        {
                            case 1:
                                {
                                    if (machine_List != null)
                                    {
                                        if (machine_List[0] == requestLogin.Machine_Code) //如果绑定的是对应自己的机器码
                                        {
                                            PanoTempUser? PanoTempUser = panoUserContext.PanoTempUser.FirstOrDefault(panaTempUser => panaTempUser.Machine_Code == requestLogin.Machine_Code);
                                            if (PanoTempUser == null)
                                            {
                                                PanoTempUser = new PanoTempUser();
                                                PanoTempUser.UserID = loginData.Id;
                                                PanoTempUser.IP = requestLogin.IP;
                                                PanoTempUser.Machine_Code = requestLogin.Machine_Code;
                                                PanoTempUser.Location = requestLogin.Location;
                                                PanoTempUser.Reg_Day = AppConfigHelper.GetTempRegTime();
                                                PanoTempUser.Reg_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                PanoTempUser.End_Date = DateTime.Now.AddDays(AppConfigHelper.GetTempRegTime()).ToString("yyyy-MM-dd HH:mm:ss");
                                                panoUserContext.PanoTempUser.Add(PanoTempUser);  //添加一个
                                                panoUserContext.SaveChanges();
                                                ResponseLoginData.User_Info = userInfo;
                                                GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                                return ApiResponse.Ok(ResponseLoginData);
                                            }
                                            else
                                            {
                                                if (DataCompare(PanoTempUser.End_Date, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                                                {
                                                    userInfo.User_Type = 1; //免费测试
                                                }
                                                else
                                                {
                                                    userInfo.User_Type = -1;//到期
                                                }
                                                panoUserContext.UserInfo.Update(userInfo);
                                                panoUserContext.SaveChanges();
                                                ResponseLoginData.User_Info = userInfo;
                                                GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                                return ApiResponse.Ok(ResponseLoginData);
                                            }
                                        }
                                        else //如果登录的机器码和 用户信息不一致   
                                        {
                                            PanoTempUser? PanoTempUser = panoUserContext.PanoTempUser.FirstOrDefault(panaTempUser => panaTempUser.Machine_Code == requestLogin.Machine_Code);
                                            if (PanoTempUser == null)//检查登录的机器有没有使用过 没有使用的话就 新增一条测试 数据
                                            {
                                                PanoTempUser = new PanoTempUser();
                                                PanoTempUser.UserID = loginData.Id;
                                                PanoTempUser.IP = requestLogin.IP;
                                                PanoTempUser.Machine_Code = requestLogin.Machine_Code;
                                                PanoTempUser.Location = requestLogin.Location;
                                                PanoTempUser.Reg_Day = AppConfigHelper.GetTempRegTime();
                                                PanoTempUser.Reg_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                PanoTempUser.End_Date = DateTime.Now.AddDays(AppConfigHelper.GetTempRegTime()).ToString("yyyy-MM-dd HH:mm:ss");
                                                panoUserContext.PanoTempUser.Add(PanoTempUser);  //添加一个
                                                panoUserContext.SaveChanges();

                                                machine_List.Clear();
                                                machine_List.Add(requestLogin.Machine_Code);
                                                string machine_Json = JsonConvert.SerializeObject(machine_List);
                                                userInfo.Machine_Codes = machine_Json;
                                                panoUserContext.UserInfo.Update(userInfo);//更新用户的绑定数据
                                                panoUserContext.SaveChanges();
                                                ResponseLoginData.User_Info = userInfo;
                                                GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                                return ApiResponse.Ok(ResponseLoginData);
                                            }
                                            else//如果使用 检查有没有到期
                                            {
                                                if (DataCompare(PanoTempUser.End_Date, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                                                {
                                                    userInfo.User_Type = 1; //免费测试
                                                }
                                                else
                                                {
                                                    userInfo.User_Type = -1;//到期
                                                }
                                                machine_List.Clear();
                                                machine_List.Add(requestLogin.Machine_Code);
                                                string machine_Json = JsonConvert.SerializeObject(machine_List);
                                                userInfo.Machine_Codes = machine_Json;
                                                panoUserContext.UserInfo.Update(userInfo);//更新用户的绑定数据
                                                panoUserContext.SaveChanges();

                                                ResponseLoginData.User_Info = userInfo;
                                                GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                                return ApiResponse.Ok(ResponseLoginData);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        PanoTempUser? PanoTempUser = panoUserContext.PanoTempUser.FirstOrDefault(panaTempUser => panaTempUser.Machine_Code == requestLogin.Machine_Code);
                                        if (PanoTempUser == null)
                                        {
                                            PanoTempUser = new PanoTempUser();
                                            PanoTempUser.UserID = loginData.Id;
                                            PanoTempUser.IP = requestLogin.IP;
                                            PanoTempUser.Machine_Code = requestLogin.Machine_Code;
                                            PanoTempUser.Location = requestLogin.Location;
                                            PanoTempUser.Reg_Day = AppConfigHelper.GetTempRegTime();
                                            PanoTempUser.Reg_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            PanoTempUser.End_Date = DateTime.Now.AddDays(AppConfigHelper.GetTempRegTime()).ToString("yyyy-MM-dd HH:mm:ss");
                                            panoUserContext.PanoTempUser.Add(PanoTempUser);  //添加一个
                                            panoUserContext.SaveChanges();

                                            machine_List = new List<string>();
                                            machine_List.Add(requestLogin.Machine_Code);
                                            string machine_Json = JsonConvert.SerializeObject(machine_List);
                                            userInfo.Machine_Codes = machine_Json;
                                            panoUserContext.UserInfo.Update(userInfo);//更新用户的绑定数据
                                            panoUserContext.SaveChanges();
                                            ResponseLoginData.User_Info = userInfo;
                                            GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                            return ApiResponse.Ok(ResponseLoginData);
                                        }
                                        else
                                        {
                                            if (DataCompare(PanoTempUser.End_Date, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                                            {
                                                userInfo.User_Type = 1; //免费测试
                                            }
                                            else
                                            {
                                                userInfo.User_Type = -1;//到期
                                            }
                                            machine_List = new List<string>();
                                            machine_List.Add(requestLogin.Machine_Code);
                                            string machine_Json = JsonConvert.SerializeObject(machine_List);
                                            userInfo.Machine_Codes = machine_Json;
                                            panoUserContext.UserInfo.Update(userInfo);//更新用户的绑定数据
                                            panoUserContext.SaveChanges();

                                            ResponseLoginData.User_Info = userInfo;
                                            GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                            return ApiResponse.Ok(ResponseLoginData);
                                        }
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (machine_List != null)
                                    {
                                        if (machine_List.Contains(requestLogin.Machine_Code)) //如果绑定的是对应自己的机器码
                                        {
                                            PanoUser? mPanoUser = panoUserContext.PanoUser.FirstOrDefault(panaUser => panaUser.UserID == userInfo.UserID);
                                            if (mPanoUser == null)
                                            {
                                                userInfo.User_Type = -1;// 设备没有绑定 做到期处理
                                                ResponseLoginData.User_Info = userInfo;
                                                GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                                return ApiResponse.Ok(ResponseLoginData, "没有激活记录,数据错误");
                                            }
                                            else
                                            {
                                                if (DataCompare(mPanoUser.End_Date, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                                                {
                                                    userInfo.User_Type = 2; //付费
                                                }
                                                else
                                                {
                                                    userInfo.User_Type = -1;//到期
                                                }
                                                panoUserContext.UserInfo.Update(userInfo);
                                                panoUserContext.SaveChanges();

                                                ResponseLoginData.User_Info = userInfo;
                                                GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                                return ApiResponse.Ok(ResponseLoginData);
                                            }
                                        }
                                        else
                                        {
                                            userInfo.User_Type = -1;// 设备没有绑定 做到期处理
                                            ResponseLoginData.User_Info = userInfo;
                                            GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                            return ApiResponse.Ok(ResponseLoginData,"设备未绑定");
                                        }
                                    }
                                    else
                                    {
                                        userInfo.User_Type = -1;// 设备没有绑定 做到期处理
                                        ResponseLoginData.User_Info = userInfo;
                                        GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                        return ApiResponse.Ok(ResponseLoginData,"用户没有绑定设备");
                                    }
                                }
                                break;
                            case 3:
                                {
                                    if (machine_List != null)
                                    {
                                        if (machine_List.Contains(requestLogin.Machine_Code)) //如果绑定的是对应自己的机器码
                                        {
                                            PanoUser? mPanoUser = panoUserContext.PanoUser.FirstOrDefault(panaUser => panaUser.UserID == userInfo.UserID);
                                            if (mPanoUser == null)
                                            {
                                                userInfo.User_Type = -1;// 设备没有绑定 做到期处理
                                                ResponseLoginData.User_Info = userInfo;
                                                GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                                return ApiResponse.Ok(ResponseLoginData, "没有激活记录,数据错误");
                                            }
                                            else
                                            {
                                                // 设备没有绑定 做到期处理
                                                ResponseLoginData.User_Info = userInfo;
                                                GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                                return ApiResponse.Ok(ResponseLoginData);
                                            }
                                        }
                                        else
                                        {
                                            userInfo.User_Type = -1;// 设备没有绑定 做到期处理
                                            ResponseLoginData.User_Info = userInfo;
                                            GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                            return ApiResponse.Ok(ResponseLoginData, "设备未绑定");
                                        }
                                    }
                                    else
                                    {
                                        userInfo.User_Type = -1;// 设备没有绑定 做到期处理
                                        ResponseLoginData.User_Info = userInfo;
                                        GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                        return ApiResponse.Ok(ResponseLoginData, "用户没有绑定设备");
                                    }
                                }
                                break;
                            case -1:
                                {
                                    if (machine_List != null)
                                    {
                                        if (machine_List[0] == requestLogin.Machine_Code) //如果绑定的是对应自己的机器码
                                        {
                                            PanoTempUser? PanoTempUser = panoUserContext.PanoTempUser.FirstOrDefault(panaTempUser => panaTempUser.Machine_Code == requestLogin.Machine_Code);
                                            if (PanoTempUser == null)
                                            {
                                                PanoTempUser = new PanoTempUser();
                                                PanoTempUser.UserID = loginData.Id;
                                                PanoTempUser.IP = requestLogin.IP;
                                                PanoTempUser.Machine_Code = requestLogin.Machine_Code;
                                                PanoTempUser.Location = requestLogin.Location;
                                                PanoTempUser.Reg_Day = AppConfigHelper.GetTempRegTime();
                                                PanoTempUser.Reg_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                PanoTempUser.End_Date = DateTime.Now.AddDays(AppConfigHelper.GetTempRegTime()).ToString("yyyy-MM-dd HH:mm:ss");
                                                panoUserContext.PanoTempUser.Add(PanoTempUser);  //添加一个
                                                panoUserContext.SaveChanges();
                                                userInfo.User_Type = 1;
                                                userInfo.User_Type = 1;
                                                panoUserContext.UserInfo.Update(userInfo);
                                                panoUserContext.SaveChanges();
                                                ResponseLoginData.User_Info = userInfo;
                                                GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                                return ApiResponse.Ok(ResponseLoginData);
                                            }
                                            else
                                            {
                                                if (DataCompare(PanoTempUser.End_Date, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                                                {
                                                    userInfo.User_Type = 1; //免费测试
                                                }
                                                else
                                                {
                                                    userInfo.User_Type = -1;//到期
                                                }
                                                panoUserContext.UserInfo.Update(userInfo);
                                                panoUserContext.SaveChanges();
                                                ResponseLoginData.User_Info = userInfo;
                                                GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                                return ApiResponse.Ok(ResponseLoginData);
                                            }
                                        }
                                        else
                                        {
                                            PanoTempUser? PanoTempUser = panoUserContext.PanoTempUser.FirstOrDefault(panaTempUser => panaTempUser.Machine_Code == requestLogin.Machine_Code);
                                            if (PanoTempUser == null)
                                            {
                                                PanoTempUser = new PanoTempUser();
                                                PanoTempUser.UserID = loginData.Id;
                                                PanoTempUser.IP = requestLogin.IP;
                                                PanoTempUser.Machine_Code = requestLogin.Machine_Code;
                                                PanoTempUser.Location = requestLogin.Location;
                                                PanoTempUser.Reg_Day = AppConfigHelper.GetTempRegTime();
                                                PanoTempUser.Reg_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                                PanoTempUser.End_Date = DateTime.Now.AddDays(AppConfigHelper.GetTempRegTime()).ToString("yyyy-MM-dd HH:mm:ss");
                                                panoUserContext.PanoTempUser.Add(PanoTempUser);  //添加一个
                                                panoUserContext.SaveChanges();
                                                userInfo.User_Type = 1;
                                                userInfo.User_Type = 1;
                                                machine_List.Clear();
                                                machine_List.Add(requestLogin.Machine_Code);
                                                string machine_Json = JsonConvert.SerializeObject(machine_List);
                                                userInfo.Machine_Codes = machine_Json;
                                                panoUserContext.UserInfo.Update(userInfo);//更新用户的绑定数据
                                                panoUserContext.SaveChanges();
                                                ResponseLoginData.User_Info = userInfo;
                                                GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                                return ApiResponse.Ok(ResponseLoginData);
                                            }
                                            else
                                            {
                                                if (DataCompare(PanoTempUser.End_Date, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                                                {
                                                    userInfo.User_Type = 1; //免费测试
                                                }
                                                else
                                                {
                                                    userInfo.User_Type = -1;//到期
                                                }
                                                machine_List.Clear();
                                                machine_List.Add(requestLogin.Machine_Code);
                                                string machine_Json = JsonConvert.SerializeObject(machine_List);
                                                userInfo.Machine_Codes = machine_Json;
                                                panoUserContext.UserInfo.Update(userInfo);//更新用户的绑定数据
                                                panoUserContext.SaveChanges();
                                                ResponseLoginData.User_Info = userInfo;
                                                GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                                return ApiResponse.Ok(ResponseLoginData);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        userInfo.User_Type = -1;// 设备没有绑定 做到期处理
                                        ResponseLoginData.User_Info = userInfo;
                                        GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                        return ApiResponse.Ok(ResponseLoginData);
                                    }
                                }
                                break;
                            default:
                                userInfo.User_Type = -1;// 设备没有绑定 做到期处理
                                ResponseLoginData.User_Info = userInfo;
                                GenerateLoginRecord(requestLogin, panoUserContext, loginData.Id);
                                return ApiResponse.Ok(ResponseLoginData);
                                break;
                        }
                    }
                }
                else
                {
                    return ApiResponse.BadRequest("登录失败,密码错误");
                }
            }
        }

        public static void GenerateLoginRecord(RequestLoginData requestLogin, PanoUserContext panoUserContext, int UserID)
        {
            PanoLoginRecord panoLoginRecord = new PanoLoginRecord();
            panoLoginRecord.IP = requestLogin.IP;
            panoLoginRecord.Location_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            panoLoginRecord.Machine_Code = requestLogin.Machine_Code;
            panoLoginRecord.Location = requestLogin.Location;
            panoLoginRecord.UserID = UserID;
            panoUserContext.PanoLoginRecord.Add(panoLoginRecord);  //添加一个
            panoUserContext.SaveChanges();
        }

        public static void GeneratetRegisterRecord(RequestRegisterData requestRegisterData, PanoUserContext panoUserContext, int UserID)
        {
            PanoRegisterRecord panoRegisterRecord = new PanoRegisterRecord();
            panoRegisterRecord.IP = requestRegisterData.IP;
            panoRegisterRecord.Location_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            panoRegisterRecord.Machine_Code = requestRegisterData.Machine_Code;
            panoRegisterRecord.Location = requestRegisterData.Location;
            panoRegisterRecord.UserID = UserID;
            panoUserContext.PanoRegisterRecord.Add(panoRegisterRecord);  //添加一个
            panoUserContext.SaveChanges();
        }
        //添加一个数据，传入一个不带ID的
        [HttpPost("Registe")]
        public ApiResponse PostRegister(RequestRegisterData requestRegisterData)
        {
            LoginData? loginData = panoUserContext.LoginData.FirstOrDefault(data => data.UserName == requestRegisterData.UserName);
            if (loginData != null)
            {
                return ApiResponse.BadRequest("注册失败,账号已存在");
            }
            else
            {
                loginData = new LoginData();
                loginData.UserName = requestRegisterData.UserName;
                loginData.Password = requestRegisterData.Password;
                loginData.Info = requestRegisterData.Location;
                var login = panoUserContext.LoginData.Add(loginData);  //添加一个
                panoUserContext.SaveChanges();
                GeneratetRegisterRecord(requestRegisterData, panoUserContext, login.Entity.Id);
                return ApiResponse.Ok("注册成功");
            }
            return ApiResponse.BadRequest("注册失败");
        }
        private string GenerateToken(RequestLoginData user)
        {
            JwtSecurityTokenHandler jwtTokenHendler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfigHelper.GetToKey()));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.Now.AddHours(AppConfigHelper.GetExpires()),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = AppConfigHelper.GetIssuer(),
                Audience = AppConfigHelper.GetAudience()
            };

            var token = jwtTokenHendler.CreateToken(tokenDescriptor);
            return jwtTokenHendler.WriteToken(token);
        }

        public static string MD5Encrypt64(string password)
        {
            string cl = password;
            //string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }

        public static bool DataCompare(string data1, string data2)
        {
            DateTime dateTime1, dateTime2;
            if (DateTime.TryParse(data1, out dateTime1))
            {
                if (DateTime.TryParse(data2, out dateTime2))
                {
                    //dateTime1>dateTime2,返回1，dateTime1=dateTime2，返回0,  dateTime1<dateTime2返回-1
                    if (DateTime.Compare(dateTime1, dateTime2) == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public static PanoUser DateSort(List<PanoUser> panoUsers)
        {
            PanoUser[] panos =panoUsers.ToArray();
            Array.Sort(panos, (a, b) =>b.End_Date.CompareTo(a.End_Date));
            return panos[0];
        }

        private bool Log(Exception ex)
        {
            Debug.Print(ex.Message);
            return false;
        }
    }
}