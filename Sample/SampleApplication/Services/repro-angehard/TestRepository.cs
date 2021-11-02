using System;
using BoilerplateFree;

namespace BoilerPlateFreeTesting
{
    [AutoGenerateConstructor]
    public partial class TestRepository
    {
        public string getTestData()
        {
            return "some-test-data";
        }
    }
}