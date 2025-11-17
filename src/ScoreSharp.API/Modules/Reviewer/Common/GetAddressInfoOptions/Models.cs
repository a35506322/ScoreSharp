using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.Common.GetAddressInfoOptions;

public class GetAddressInfoOptionsResponse
{
    /// <summary>
    /// 地址-縣市 如: 台北市
    /// </summary>
    public List<OptionsDtoTypeString> City { get; set; }

    /// <summary>
    /// 地址-區 如: 大安區
    /// </summary>
    public List<AreaDto> Area { get; set; }

    /// <summary>
    /// 地址-街道 如: 信義路
    /// </summary>
    public List<RoadDto> Road { get; set; }
}
