namespace ScoreSharp.InfoCheckMiddleware.Modules.Test.ErrorTest
{
    public partial class TestController
    {
        [HttpPost]
        public async Task<IResult> ErrorTest() => throw new Exception("Test Exception");
    }
}
