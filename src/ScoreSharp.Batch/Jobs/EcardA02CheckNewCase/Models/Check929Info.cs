using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

public class Check929Info : BaseResDto
{
    /// <summary>
    /// 929業務狀況查詢結果
    /// </summary>
    public List<Reviewer3rd_929Log> Reviewer3rd_929Logs { get; set; } = [];
    public string 是否命中 => Reviewer3rd_929Logs.Any() ? "Y" : "N";
}
