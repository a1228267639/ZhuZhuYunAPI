using CodeLab.Share.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using ZhuZhuYunAPI.Models;

namespace ZhuZhuYunAPI.Controllers
{
    public class UploadController : ControllerBase
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private PanoUserContext panoUserContext;
        public UploadController(IWebHostEnvironment webHostEnvironment, PanoUserContext panoUserContext)
        {
            this._webHostEnvironment = webHostEnvironment;
            this.panoUserContext = panoUserContext;
        }

        /// <summary>
        /// 上传图片,通过Form表单提交
        /// </summary>
        /// <returns></returns>
        [Route("Upload/FormImg")]
        [HttpPost]
        public ApiResponse UploadImg(List<IFormFile> files)
        {
            if (files.Count < 1)
            {
                return ApiResponse.BadRequest("文件为空");
            }
            //返回的文件地址
            List<string> filenames = new List<string>();
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
                foreach (var item in files)
                {
                    if (item != null)
                    {
                        #region  图片文件的条件判断
                        //文件后缀
                        var fileExtension = Path.GetExtension(item.FileName);

                        //判断后缀是否是图片
                        const string fileFilt = ".gif|.jpg|.jpeg|.png";
                        if (fileExtension == null)
                        {
                            return ApiResponse.Error("上传的文件没有后缀");
                        }
                        if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
                        {
                            return ApiResponse.Error("请上传jpg、png、gif格式的图片");
                        }

                        //判断文件大小    
                        long length = item.Length;
                        if (length > 1024 * 1024 * 50) //2M
                        {
                            return ApiResponse.Error("上传的文件不能大于50M");
                        }

                        #endregion

                        var strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff"); //取得时间字符串
                        var strRan = Convert.ToString(new Random().Next(100, 999)); //生成三位随机数
                        var saveName = strDateTime + "_" + strRan + fileExtension;

                        //插入图片数据                 
                        using (FileStream fs = System.IO.File.Create(webRootPath + filePath + saveName))
                        {
                            item.CopyTo(fs);
                            fs.Flush();
                        }
                        filenames.Add(filePath + saveName);
                    }
                }
                return ApiResponse.Ok(filenames);
            }
            catch (Exception ex)
            {
                //这边增加日志，记录错误的原因
                //ex.ToString();
                return ApiResponse.BadRequest("上传失败" + ex.ToString());
            }
        }
    }
}
