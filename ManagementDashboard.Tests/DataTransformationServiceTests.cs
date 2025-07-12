using System;
using ManagementDashboard.Core.Services;
using Xunit;

namespace ManagementDashboard.Tests
{
    public class DataTransformationServiceTests
    {
        [Theory]
        [InlineData("2024-06-01", "2024-06-01")]
        [InlineData("06/01/2024", "2024-06-01")]
        [InlineData("notadate", "notadate")]
        public void ToSqliteDateString_ConvertsVariousInputs(string input, string expected)
        {
            var result = DataTransformationService.ToSqliteDateString(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToSqliteDateString_ConvertsDateTime()
        {
            var dt = new DateTime(2024, 6, 1);
            var result = DataTransformationService.ToSqliteDateString(dt);
            Assert.Equal("2024-06-01", result);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        [InlineData(1, true)]
        [InlineData(0, false)]
        [InlineData("True", true)]
        [InlineData("FALSE", false)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        [InlineData("yes", true)]
        [InlineData("no", false)]
        [InlineData("t", true)]
        [InlineData("f", false)]
        [InlineData("Y", true)]
        [InlineData("N", false)]
        [InlineData("random", false)]
        [InlineData(null, false)]
        public void ToBool_ConvertsVariousInputs(object input, bool expected)
        {
            var result = DataTransformationService.ToBool(input);
            Assert.Equal(expected, result);
        }
    }
}
