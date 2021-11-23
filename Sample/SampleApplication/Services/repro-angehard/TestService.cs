using System;
using BoilerplateFree;
using Microsoft.Extensions.Logging;

namespace BoilerPlateFreeTesting
{
    [AutoGenerateConstructor]
    public partial class TestService
    {
        private readonly TestRepository _testRepository;

        public void runService()
        {
            var testData = _testRepository.getTestData();
            Console.WriteLine($"giving out testdata: {testData}");
        }
    }
}