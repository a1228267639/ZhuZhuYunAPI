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
            PanoUser panoUser = new PanoUser();
            if (UserInfo != null)
            {
                List<PanoUser>? panoUsers = panoUserContext.PanoUser.ToList().FindAll(a => a.UserID == requestActivateData.UserID);
                if (panoUsers != null && panoUsers.Any())
                {
                    panoUser = LoginController.DateSort(panoUsers);
                    if (LoginController.DataCompare(panoUser.End_Date, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))
                    {
                        return ApiResponse.BadRequest("账号激活时间未到");
                    }
                    else
                    {
                        List<string>? machine_List = JsonConvert.DeserializeObject<List<string>>(UserInfo.Machine_Codes);
                        if (machine_List != null)
                        {
                            panoUser = new PanoUser();
                            panoUser.IP = requestActivateData.IP;
                            panoUser.UserID = requestActivateData.UserID;
                            panoUser.Location = requestActivateData.Location;
                            panoUser.Reg_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            switch (requestActivateData.Reg_Type)//1是天 2月 3是年 4是永久
                            {
                                case 1:
                                    panoUser.End_Date = DateTime.Now.AddDays(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                    UserInfo.User_Type = 2;
                                    break;
                                case 2:
                                    panoUser.End_Date = DateTime.Now.AddMonths(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                    UserInfo.User_Type = 2;
                                    break;
                                case 3:
                                    panoUser.End_Date = DateTime.Now.AddYears(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                    UserInfo.User_Type = 2;
                                    break;
                                case 4:
                                    panoUser.End_Date = DateTime.Now.AddYears(AppConfigHelper.GetRegTime()).ToString("yyyy-MM-dd HH:mm:ss");
                                    UserInfo.User_Type = 3;
                                    UserInfo.BindMachine_Count = 2;
                                    break;
                                default:
                                    panoUser.End_Date = DateTime.Now.AddDays(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                    UserInfo.User_Type = 2;
                                    break;
                            }
                            panoUser.Reg_Info = requestActivateData.Reg_Info;
                            panoUser.Reg_Day = requestActivateData.Reg_Day;

                            panoUserContext.PanoUser.Add(panoUser);  //添加一个
                            UserInfo.Machine_Codes = JsonConvert.SerializeObject(machine_List);

                            List<string>? money_List = JsonConvert.DeserializeObject<List<string>>(UserInfo.Reg_Money);
                            if (money_List == null)
                            {
                                money_List = new List<string>();
                            }
                            money_List.Add(requestActivateData.Reg_Money);
                            UserInfo.Reg_Money = JsonConvert.SerializeObject(money_List);
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
                    List<string>? machine_List = JsonConvert.DeserializeObject<List<string>>(UserInfo.Machine_Codes);
                    if (machine_List != null)
                    {
                        panoUser = new PanoUser();
                        panoUser.IP = requestActivateData.IP;
                        panoUser.UserID = requestActivateData.UserID;
                        panoUser.Location = requestActivateData.Location;
                        panoUser.Reg_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        switch (requestActivateData.Reg_Type)//1是天 2月 3是年 4是永久
                        {
                            case 1:
                                panoUser.End_Date = DateTime.Now.AddDays(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                UserInfo.User_Type = 2;
                                break;
                            case 2:
                                panoUser.End_Date = DateTime.Now.AddMonths(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                UserInfo.User_Type = 2;
                                break;
                            case 3:
                                panoUser.End_Date = DateTime.Now.AddYears(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                UserInfo.User_Type = 2;
                                break;
                            case 4:
                                panoUser.End_Date = DateTime.Now.AddYears(AppConfigHelper.GetRegTime()).ToString("yyyy-MM-dd HH:mm:ss");
                                UserInfo.User_Type = 3;
                                UserInfo.BindMachine_Count = 2;
                                break;
                            default:
                                panoUser.End_Date = DateTime.Now.AddDays(requestActivateData.Reg_Day).ToString("yyyy-MM-dd HH:mm:ss");
                                UserInfo.User_Type = 2;
                                break;
                        }
                        panoUser.Reg_Info = requestActivateData.Reg_Info;
                        panoUser.Reg_Day = requestActivateData.Reg_Day;

                        panoUserContext.PanoUser.Add(panoUser);  //添加一个
                        UserInfo.Machine_Codes = JsonConvert.SerializeObject(machine_List);

                        List<string>? money_List = JsonConvert.DeserializeObject<List<string>>(UserInfo.Reg_Money.ToString());
                        if (money_List == null)
                        {
                            money_List = new List<string>();
                        }
                        money_List.Add(requestActivateData.Reg_Money);
                        UserInfo.Reg_Money = JsonConvert.SerializeObject(money_List);
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
    }
}
