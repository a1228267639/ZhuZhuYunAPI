namespace ZhuZhuYunAPI.Models.ResponseModels
{
    public class ResponsePanoLoginRecord
    {
        public int TotalCount { get; set; }
        public List<PanoLoginRecord>? PanoLoginRecords { get; set; }
    }
}
