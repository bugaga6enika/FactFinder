using FluentAssertions;
using System.Globalization;
using TestClass = FactFinder.StringFormatValidator;

namespace FactFinder.UnitTests.StringFormatValidator
{
    public class IsFormatTests
    {
        public IsFormatTests()
        {
            var culture = new CultureInfo("de-DE");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Given_NullOrEmpty_stringToCheck_Should_Throw_ArgumentException(string str)
        {
            // Act
            var act = () => TestClass.IsFormat(str, "date");

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Given_NullOrEmpty_format_Should_Throw_ArgumentException(string f)
        {
            // Act
            var act = () => TestClass.IsFormat("0", f);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Given_Invalid_format_Should_Throw_Exception_With_Message_Format_Not_Allowed()
        {
            // Act
            var act = () => TestClass.IsFormat("0", "bool");

            // Assert
            act.Should().Throw<FormatNotAllowedException>().WithMessage("Format not allowed.");
        }

        [Theory]
        [InlineData("2", "number")]
        [InlineData("-45", "number")]
        [InlineData("0x7ffffff", "number")]
        [InlineData("256.56", "number")]
        [InlineData("-342342.432432", "number")]
        [InlineData("2", "Number")]
        [InlineData("-45", "NUMBER")]
        [InlineData("0x7ffffff", "nuMBer")]
        [InlineData("CC66FF", "nuMBer")]
        [InlineData("1111100000110001", "NuMber")]
        [InlineData("256.56", "NuMbEr")]
        [InlineData("-342342.432432", "numBEr")]
        public void Given_Valid_Number_As_stringToCheck_Returns_True(string str, string f)
        {
            // Arrange and Act
            var result = TestClass.IsFormat(str, f);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("2wewe", "number")]
        [InlineData("-452ew", "number")]
        [InlineData("4sdfsdf", "number")]
        [InlineData("-342342.4r2432", "number")]
        [InlineData("-342342,4r2432", "number")]
        [InlineData("2wewe", "Number")]
        [InlineData("-452ew", "NUMBER")]
        [InlineData("0x7ffffffz", "nuMBer")]
        [InlineData("4sdfsdf", "NuMbEr")]
        [InlineData("-342342.4r2432", "numBEr")]
        [InlineData("-342342,4r2432", "numBEr")]
        public void Given_Invalid_Number_As_stringToCheck_Returns_False(string str, string f)
        {
            // Arrange and Act
            var result = TestClass.IsFormat(str, f);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("12/05/2012", "date")]
        [InlineData("30.10.2017", "date")]
        [InlineData("9/1/1999", "date")]
        [InlineData("10.2.2045", "date")]
        [InlineData("Feb 16, 1978", "date")]
        [InlineData("12/5/2012", "dATE")]
        [InlineData("30.10.2017", "datE")]
        [InlineData("28/09/1999", "daTE")]
        [InlineData("10.02.2045", "DATE")]
        [InlineData("Feb 16, 1978", "DATE")]
        public void Given_Valid_Date_As_stringToCheck_Returns_True(string str, string f)
        {
            // Arrange and Act
            var result = TestClass.IsFormat(str, f);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("5/28/2012", "date")]
        [InlineData("05.18.2017", "date")]
        [InlineData("09/17/1999", "date")]
        [InlineData("1.27.2045", "date")]
        [InlineData("30 Feb, 1978", "date")]
        [InlineData("1/51/2012", "dATE")]
        [InlineData("3.14.2017", "datE")]
        [InlineData("02/19/1999", "daTE")]
        [InlineData("08.22.2045", "DATE")]
        [InlineData(" 30 Feb, 1978", "DATE")]
        public void Given_Invalid_Date_As_stringToCheck_Returns_False(string str, string f)
        {
            // Arrange and Act
            var result = TestClass.IsFormat(str, f);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("14:25:45.5554524", "timespan")]
        [InlineData("2:10", "timespan")]
        [InlineData("17", "timespan")]
        [InlineData("2:45:22", "timespan")]
        [InlineData("-02:15:59", "timespan")]
        [InlineData("14:25:45.5554524", "timeSpan")]
        [InlineData("2:10", "TIMESpan")]
        [InlineData("17", "tiMeSPAn")]
        [InlineData("2:45:22", "tiMESPan")]
        [InlineData("-02:15:59", "TiMeSpAn")]
        public void Given_Valid_TimeSpan_As_stringToCheck_Returns_True(string str, string f)
        {
            // Arrange and Act
            var result = TestClass.IsFormat(str, f);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("25:45:23.878787878", "timespan")]
        [InlineData("invalid time span", "timespan")]
        [InlineData("17.", "timespan")]
        [InlineData("2 pm", "timespan")]
        [InlineData("25:45:23.878787878", "timeSpan")]
        [InlineData("invalid time span", "TIMESpan")]
        [InlineData("17.", "tiMeSPAn")]
        [InlineData("2 pm", "tiMESPan")]
        public void Given_Invalid_TimeSpan_As_stringToCheck_Returns_False(string str, string f)
        {
            // Arrange and Act
            var result = TestClass.IsFormat(str, f);

            // Assert
            result.Should().BeFalse();
        }
    }
}
