using CodeLab.Share.ViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApiUtils;
using ZhuZhuYunAPI.Models;
using ZhuZhuYunAPI.Models.RequestModels;

namespace ZhuZhuYunAPI.Controllers
{
    public class ActivateController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private PanoUserContext panoUserContext;
        public ActivateController(IWebHostEnvironment webHostEnvironment, PanoUserContext panoUserContext)
        {
            this._webHostEnvironment = webHostEnvironment;
            this.panoUserContext = panoUserContext;
        }

        // [HttpPost("ActivateUser"), Authorize()]
        [HttpPost("ActivateUser")]
        public ApiResponse ActivateUser(RequestActivateData requestActivateData)
        {
            var UserInfo = panoUserContext.UserInfo.FirstOrDefault(a => a.UserID == requestActivateData.UserID);
            PanoUserRecord panoUser;
            if (UserInfo != null)
            {
                List<PanoUserRecord>? panoUsers = panoUserContext.PanoUser.ToList().FindAll(a => a.UserID == requestActivateData.UserID);
                if (panoUsers != null && panoUsers.Any())// 如果有激活记录  
                {
                    panoUser = LoginController.DateSort(panoUsers);
                    if (panoUser.Reg_Day == AppConfigHelper.GetRegTime())
                    {
                        return ApiResponse.BadRequest("账号已经永久激活");
                    }
                    if (LoginController.DataCompare(panoUser.End_Date, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                    {
                        return ApiResponse.BadRequest("账号激活时间未到");
                    }
                    else
                    {
                        List<string>? machine_List = JsonConvert.DeserializeObject<List<string>>(UserInfo.Machine_Codes);
                        if (machine_List != null)
                        {
                            panoUser = new PanoUserRecord();
                            panoUser.UserID = requestActivateData.UserID;
                            panoUser.Reg_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            switch (requestActivateData.Reg_Type)//1是天 2月 3是年 4是永久
                            {
                                case 1:
                                    panoUser.End_Date = DateTime.Now.AddDays(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                    panoUser.Reg_Day = requestActivateData.Reg_Day;
                                    UserInfo.User_Type = 2;
                                    break;
                                case 2:
                                    panoUser.End_Date = DateTime.Now.AddMonths(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                    panoUser.Reg_Day = requestActivateData.Reg_Day;
                                    UserInfo.User_Type = 2;
                                    break;
                                case 3:
                                    panoUser.End_Date = DateTime.Now.AddYears(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                    panoUser.Reg_Day = requestActivateData.Reg_Day;
                                    UserInfo.User_Type = 2;
                                    break;
                                case 4:
                                    panoUser.End_Date = DateTime.Now.AddYears(AppConfigHelper.GetRegTime()).ToString("yyyy-MM-dd HH:mm:ss");
                                    panoUser.Reg_Day = AppConfigHelper.GetRegTime();
                                    UserInfo.User_Type = 3;
                                    UserInfo.BindMachine_Count = 2;
                                    break;
                                default:
                                    panoUser.End_Date = DateTime.Now.AddDays(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                    panoUser.Reg_Day = requestActivateData.Reg_Day;
                                    UserInfo.User_Type = 2;
                                    break;
                            }
                            panoUser.Reg_Info = requestActivateData.Reg_Info;
                            panoUser.Reg_Vocher = UpLoadIMG(requestActivateData.Reg_Vocher);
                            panoUser.Reg_Money = requestActivateData.Reg_Money;
                            panoUserContext.PanoUser.Add(panoUser);  //添加一条付费激活数据

                            panoUserContext.UserInfo.Update(UserInfo);
                            panoUserContext.SaveChanges();
                            return ApiResponse.Ok("激活成功");
                        }
                        else
                        {
                            return ApiResponse.BadRequest("激活失败,没有绑定机器码");
                        }
                    }
                }
                else //没有激活记录的
                {
                    List<string>? machine_List = JsonConvert.DeserializeObject<List<string>>(UserInfo.Machine_Codes);
                    if (machine_List != null)
                    {
                        panoUser = new PanoUserRecord();
                        panoUser.UserID = requestActivateData.UserID;
                        panoUser.Reg_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        switch (requestActivateData.Reg_Type)//1是天 2月 3是年 4是永久
                        {
                            case 1:
                                panoUser.End_Date = DateTime.Now.AddDays(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                panoUser.Reg_Day = requestActivateData.Reg_Day;
                                UserInfo.User_Type = 2;
                                break;
                            case 2:
                                panoUser.End_Date = DateTime.Now.AddMonths(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                panoUser.Reg_Day = requestActivateData.Reg_Day;
                                UserInfo.User_Type = 2;
                                break;
                            case 3:
                                panoUser.End_Date = DateTime.Now.AddYears(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                panoUser.Reg_Day = requestActivateData.Reg_Day;
                                UserInfo.User_Type = 2;
                                break;
                            case 4:
                                panoUser.End_Date = DateTime.Now.AddYears(AppConfigHelper.GetRegTime()).ToString("yyyy-MM-dd HH:mm:ss");
                                panoUser.Reg_Day = AppConfigHelper.GetRegTime();
                                UserInfo.User_Type = 3;
                                UserInfo.BindMachine_Count = 2;
                                break;
                            default:
                                panoUser.End_Date = DateTime.Now.AddDays(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                panoUser.Reg_Day = requestActivateData.Reg_Day;
                                UserInfo.User_Type = 2;
                                break;
                        }
                        panoUser.Reg_Info = requestActivateData.Reg_Info;
                        panoUser.Reg_Vocher = UpLoadIMG(requestActivateData.Reg_Vocher);
                        panoUser.Reg_Money = requestActivateData.Reg_Money;
                        panoUserContext.PanoUser.Add(panoUser);  //添加一个

                        panoUserContext.UserInfo.Update(UserInfo);
                        panoUserContext.SaveChanges();
                        return ApiResponse.Ok("激活成功");
                    }
                    else
                    {
                        return ApiResponse.BadRequest("激活失败,没有绑定机器码");
                    }
                }
            }
            else
            {
                return ApiResponse.BadRequest("激活失败,用户不存在");
            }
        }

        public string UpLoadIMG(IFormFile file)
        {
            if (file == null)
            {
                return "";
            }
            //返回的文件地址
            string filenames = "";
            var now = DateTime.Now;
            //文件存储路径
            var filePath = string.Format("/Uploads/{0}/{1}/{2}/", now.ToString("yyyy"), now.ToString("MM"), now.ToString("dd"));
            //获取当前web目录
            var webRootPath = _webHostEnvironment.WebRootPath;
            if (!Directory.Exists(webRootPath + filePath))
            {
                Directory.CreateDirectory(webRootPath + filePath);
            }
            try
            {
                if (file != null)
                {
                    #region  图片文件的条件判断
                    //文件后缀
                    var fileExtension = Path.GetExtension(file.FileName);

                    //判断后缀是否是图片
                    const string fileFilt = ".gif|.jpg|.jpeg|.png";
                    if (fileExtension == null)
                    {
                        return "";
                    }
                    if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
                    {
                        return "";
                    }
                    //判断文件大小    
                    long length = file.Length;
                    if (length > 1024 * 1024 * 50) //50M
                    {
                        return "";
                    }
                    #endregion

                    var strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff"); //取得时间字符串
                    var strRan = Convert.ToString(new Random().Next(100, 999)); //生成三位随机数
                    var saveName = strDateTime + "_" + strRan + fileExtension;

                    //插入图片数据                 
                    using (FileStream fs = System.IO.File.Create(webRootPath + filePath + saveName))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    filenames = (filePath + saveName);
                }
                return filenames;
            }
            catch (Exception ex)
            {
                //这边增加日志，记录错误的原因
                //ex.ToString();
                return "";
            }
        }
    }
}
