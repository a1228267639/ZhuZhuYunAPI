using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Text.Json;
using ZhuZhuYunAPI.Models;
using Microsoft.EntityFrameworkCore;
using CodeLab.Share.ViewModels.Response;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using ZhuZhuYunAPI.Models.ResponseModels;

namespace ZhuZhuYunAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PanoController : ControllerBase
    {
        private PanoUserContext panoUserContext;
        public PanoController(PanoUserContext panoUserContext)
        {
            this.panoUserContext = panoUserContext;
        }

        //查找表中所有数据
        [HttpGet("PanoUserRecord/getall"), Authorize()]
        public ApiResponse GetAll()
        {
            List<PanoUserRecord> PanoUserTable = panoUserContext.PanoUser.ToList();  //查出所有     
            return ApiResponse.Ok(PanoUserTable);
        }
        //查找表中所有数据
        [HttpGet("PanoUserRecord/getActivate"), Authorize()]
        public ApiResponse GetActivate(int User_ID)
        {
            PanoUserRecord? PanoUser = panoUserContext.PanoUser.FirstOrDefault(a => a.UserID == User_ID);   
            if (PanoUser == null)
            {
                return ApiResponse.BadRequest("机器未激活");
            }
            return ApiResponse.Ok(PanoUser);
        }
        //分页查询
        [HttpGet("PanoUserRecord/getallt"), Authorize()]
        public ApiResponse GetList(int pageIndex, int pageSize)
        {
            List<PanoUserRecord> panoUserTabaCountList = panoUserContext.PanoUser.ToList();  //查出所有
            if (panoUserTabaCountList != null)
            {
                if (panoUserTabaCountList.Count > 0)
                {
                    //总页数
                    //int total = panoUserTabaCountList.Count % pageSize == 0 ? panoUserTabaCountList.Count / pageSize : (panoUserTabaCountList.Count / pageSize) + 1;
                    ResponsePanoUser responsePano = new ResponsePanoUser();

                    List<PanoUserRecord> pacth = panoUserTabaCountList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    responsePano.PanoUsers = pacth;
                    responsePano.TotalCount = panoUserTabaCountList.Count;
                    return ApiResponse.Ok(responsePano);
                }
                else
                {
                    return ApiResponse.BadRequest("数据为空");
                }
            }
            else
            {
                return ApiResponse.BadRequest("数据为空");
            }
        }

        //分页查询
        [HttpGet("PanoUserRecord/getMachine"), Authorize()]
        public ApiResponse GetMachine(int pageIndex, int pageSize, int type, string value)
        {
            List<PanoUserRecord> panoUserTabaCountList = null;
            switch (type)
            {
                case 1:
                    panoUserTabaCountList = panoUserContext.PanoUser.ToList().FindAll((item) => { return item.UserID.ToString().Contains(value); });
                    break;
                case 2:
                    panoUserTabaCountList = panoUserContext.PanoUser.ToList().FindAll((item) => { return item.Reg_Day.ToString().Contains(value); });
                    break;
                case 3:
                    //panoUserTabaCountList = panoUserContext.PanoUser.ToList().FindAll((item) => { return item.Location.Contains(value); });
                    break;
                case 4:
                   // panoUserTabaCountList = panoUserContext.PanoUserRecord.ToList().FindAll((item) => { return item.Reg_Money.Contains(value); });
                    break;
                case 5:
                    panoUserTabaCountList = panoUserContext.PanoUser.ToList().FindAll((item) => { return item.Reg_Info.Contains(value); });
                    break;
            }
            if (panoUserTabaCountList != null)
            {
                if (panoUserTabaCountList.Count > 0)
                {
                    //总页数
                    //int total = panoUserTabaCountList.Count % pageSize == 0 ? panoUserTabaCountList.Count / pageSize : (panoUserTabaCountList.Count / pageSize) + 1;
                    ResponsePanoUser responsePano = new ResponsePanoUser();

                    List<PanoUserRecord> pacth = panoUserTabaCountList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    responsePano.PanoUsers = pacth;
                    responsePano.TotalCount = panoUserTabaCountList.Count;
                    return ApiResponse.Ok(responsePano);
                }
                else
                {
                    return ApiResponse.BadRequest("数据为空");
                }
            }
            else
            {
                return ApiResponse.BadRequest("数据为空");
            }
        }



        //添加一个数据，传入一个不带ID的
        [HttpPost("PanoUserRecord/postone"), Authorize()]
        public ApiResponse PostOne(PanoUserRecord panoUser)
        {
            var PanoUser = panoUserContext.PanoUser.FirstOrDefault(a => a.UserID == panoUser.UserID);
            if (PanoUser != null)
            {
                return ApiResponse.BadRequest("添加失败，机器码重复");
            }
            panoUserContext.PanoUser.Add(panoUser);  //添加一个
            panoUserContext.SaveChanges();
            return ApiResponse.Ok("添加成功");
        }

        //修改数据,传入对象，找到对应id的数据实现更新
        [HttpPost("PanoUserRecord/Update"), Authorize()]
        public ApiResponse Modify(PanoUserRecord panoUser)
        {
            var PanoUser = panoUserContext.PanoUser.FirstOrDefault(a => a.UserID == panoUser.UserID);
            if (PanoUser == null)
            {
                return ApiResponse.BadRequest("修改失败，未找到数据");
            }
            //修改数据
            // PanoUserTable.Id = PanoUserRecord.Id;
            //PanoUserRecord.Machine_Code = panoUser.Machine_Code;
            //PanoUserRecord.Reg_Code = panoUser.Reg_Code;
            PanoUser.Reg_Day = panoUser.Reg_Day;
            //PanoUserRecord.Reg_Date = panoUser.Reg_Money;
            PanoUser.End_Date = panoUser.End_Date;
            //PanoUserRecord.Reg_Money = panoUser.Reg_Money;
            PanoUser.Reg_Info = panoUser.Reg_Info;
            panoUserContext.PanoUser.Update(PanoUser);
            panoUserContext.SaveChanges();
            return ApiResponse.Ok(PanoUser);
        }

        //移除一个对象，根据id移除
        [HttpPost("PanoUserRecord/Removeone"), Authorize()]
        public ApiResponse Remove(int User_ID)
        {
            PanoUserRecord? PanoUser = panoUserContext.PanoUser.FirstOrDefault(a => a.UserID == User_ID);
            //删除数据
            if (PanoUser == null)
            {
                return ApiResponse.NotFound();
            }
            panoUserContext.PanoUser.Remove(PanoUser);
            panoUserContext.SaveChanges();
            return ApiResponse.Ok("机器码 ：" + PanoUser.UserID + "已删除");
        }


    }
}
