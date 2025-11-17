namespace ScoreSharp.PaperMiddleware.Modules.Test
{
    public partial class TestController
    {
        [HttpPost]
        public async Task<IResult> ErrorTest() => throw new Exception("Test Exception");
    }
}
