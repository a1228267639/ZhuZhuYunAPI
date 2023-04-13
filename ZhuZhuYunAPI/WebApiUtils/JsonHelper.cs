using Newtonsoft.Json;

namespace JwtWebApiTutorial.WebApiUtils
{
    public class JsonHelper
    {
        /// <summary>
        /// 转Json回HttpResponseMessage
        /// </summary>
        /// <param name="code"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string ToJson(object result)
        {
            return JsonConvert.SerializeObject(result);
        }
    }
}
