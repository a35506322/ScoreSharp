using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreSharp.Common.Enums;

/// <summary>
/// 卡片階段
/// </summary>
public enum CardStep
{
    [EnumIsActive(true)]
    月收入確認 = 1,

    [EnumIsActive(true)]
    人工徵審 = 2,
}
