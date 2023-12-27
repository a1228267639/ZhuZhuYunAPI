namespace ZhuZhuYunAPI.Models.ResponseModels
{
    public class ResponsePanoUser
    {
        public int TotalCount { get; set; }
        public List<PanoUserRecord>?  PanoUsers { get; set; }
    }
}
