using System.Data;
using ScoreSharp.API.Modules.Test.Models;
using ScoreSharp.Common.Adapters.MW3.Models;

namespace ScoreSharp.API;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("測試相關API")]
public class TestController : ControllerBase
{
    private readonly IScoreSharpDapperContext _dapperContext;
    private readonly IMapper _mapper;
    private readonly ScoreSharpContext _scoreSharpContext;
    private readonly IFusionCache _fusionCache;

    private readonly IMW3ProcAdapter _mw3Adapter;
    private readonly IMW3MSAPIAdapter _mw3APIAdapter;
    private readonly IMW3APAPIAdapter _mw3APAPIAdapter;

    public TestController(
        IScoreSharpDapperContext dapperContext,
        IMapper mapper,
        ScoreSharpContext scoreSharpContext,
        IFusionCache fusionCache,
        IMW3ProcAdapter mw3Adapter,
        IMW3MSAPIAdapter mw3APIAdapter,
        IMW3APAPIAdapter mw3APAPIAdapter
    )
    {
        _dapperContext = dapperContext;
        _mapper = mapper;
        _scoreSharpContext = scoreSharpContext;
        _fusionCache = fusionCache;
        _mw3Adapter = mw3Adapter;
        _mw3APIAdapter = mw3APIAdapter;
        _mw3APAPIAdapter = mw3APAPIAdapter;
    }

    /// <summary>
    /// 清除Cache資料
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IResult> DeleteCache()
    {
        await _fusionCache.RemoveAsync(RedisKeyConst.GetAddressInfoOptions);
        await _fusionCache.RemoveByTagAsync(SecurityConstants.PolicyRedisTag.RoleAction);
        await _fusionCache.RemoveByTagAsync(SecurityConstants.PolicyRedisTag.Action);

        return Results.Ok("成功清除Cache");
    }

    /// <summary>
    /// 產出修改申請書編號用的Request
    /// </summary>
    /// <param name="applyNo">申請書編號</param>
    /// <returns></returns>
    [HttpGet("{applyNo}")]
    public async Task<IResult> GetApplicationInfoReqData([FromRoute] string applyNo)
    {
        Reviewer_ApplyCreditCardInfoMain? main;
        Reviewer_ApplyCreditCardInfoHandle? handle;

        using (var conn = _dapperContext.CreateScoreSharpConnection())
        {
            SqlMapper.GridReader results = await conn.QueryMultipleAsync(
                sql: "Usp_GetApplyCreditCardInfoByApplyNo",
                param: new { ApplyNo = applyNo },
                commandType: CommandType.StoredProcedure
            );
            main = results.Read<Reviewer_ApplyCreditCardInfoMain>().ToList().SingleOrDefault();
            handle = results.Read<Reviewer_ApplyCreditCardInfoHandle>().ToList().Where(x => x.UserType == UserType.正卡人).SingleOrDefault();
        }

        if (main is null)
            return Results.NotFound();

        // 回傳資料
        GetApplicationInfoReqDataResponse response = _mapper.Map<GetApplicationInfoReqDataResponse>(main);
        _mapper.Map(handle, response);

        return Results.Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> QueryOCSI929([FromBody] string id)
    {
        return Ok(await _mw3Adapter.QueryOCSI929(id));
    }

    [HttpPost]
    public async Task<IActionResult> QuerySearchCusData([FromBody] string id)
    {
        return Ok(await _mw3Adapter.QuerySearchCusData(id));
    }

    [HttpPost]
    public async Task<IActionResult> QueryTravelCardCustomer([FromBody] string id)
    {
        return Ok(await _mw3Adapter.QueryTravelCardCustomer(id));
    }

    [HttpPost]
    public async Task<IActionResult> QueryIBM7020([FromBody] string id)
    {
        return Ok(await _mw3Adapter.QueryIBM7020(id));
    }

    [HttpPost]
    public async Task<IActionResult> QueryConcernDetail([FromBody] string id)
    {
        return Ok(await _mw3APIAdapter.QueryConcernDetail(id));
    }

    [HttpPost]
    public async Task<IActionResult> QueryNameCheck([FromBody] string name)
    {
        return Ok(await _mw3APAPIAdapter.QueryNameCheck(name, "ScoreSharp.API", Ulid.NewUlid().ToString()));
    }

    [HttpPost]
    public async Task<IActionResult> SyncKYC([FromBody] SyncKycMW3Info request)
    {
        return Ok(await _mw3APAPIAdapter.SyncKYC(request));
    }

    /// <summary>
    /// 更改建議核准KYC
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SuggestKYC([FromBody] SuggestKycMW3Info request)
    {
        return Ok(await _mw3APAPIAdapter.SuggestKYC(request));
    }

    [HttpPost]
    public async Task<IActionResult> QueryCustKYCRiskLevel([FromBody] QueryCustKYCRiskLevelRequestInfo request)
    {
        return Ok(await _mw3APAPIAdapter.QueryCustKYCRiskLevel(request));
    }

    [HttpPost]
    public IActionResult ConvertBase64ToExcel([FromBody] ConvertBase64ToExcelRequest request)
    {
        try
        {
            // 驗證輸入
            if (string.IsNullOrEmpty(request.Base64String))
            {
                return BadRequest("Base64 字串不能為空");
            }

            // 將 Base64 字串轉換成 byte[]
            byte[] fileBytes = Convert.FromBase64String(request.Base64String);

            // 設定檔案名稱（可從參數傳入或使用預設）
            string fileName = string.IsNullOrEmpty(request.FileName) ? "output.xlsx" : request.FileName;

            // 確保副檔名是 .xlsx
            if (!fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                fileName += ".xlsx";
            }

            // 回傳 Excel 檔案
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (FormatException)
        {
            return BadRequest("無效的 Base64 字串格式");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"發生錯誤: {ex.Message}");
        }
    }
}
