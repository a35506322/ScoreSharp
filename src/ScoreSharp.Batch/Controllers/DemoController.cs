using ScoreSharp.Common.Adapters.MW3;

namespace ScoreSharp.Batch;

[ApiController]
[Route("[controller]/[action]")]
public class DemoController : ControllerBase
{
    private readonly IMW3ProcAdapter _mw3Adapter;
    private readonly IMW3MSAPIAdapter _mw3APIAdapter;

    public DemoController(IMW3ProcAdapter mw3Adapter, IMW3MSAPIAdapter mw3MSAPIAdapter)
    {
        _mw3Adapter = mw3Adapter;
        _mw3APIAdapter = mw3MSAPIAdapter;
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

    [HttpGet]
    public async Task<IActionResult> QueryOriginalCardholderData([FromQuery] string? id, [FromQuery] string? email, [FromQuery] string? mobile)
    {
        return Ok(await _mw3Adapter.QueryOriginalCardholderData(id, email, mobile));
    }
}
