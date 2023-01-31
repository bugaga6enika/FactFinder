using FactFinder.Converters;
using FactFinder.Validators;
using FluentAssertions;
using System.Globalization;

namespace FactFinder.UnitTests.Converters.StringConverterValidatorExt
{
    public class CanBeConvertedTests
    {
        private readonly FactFinder.Converters.StringConverterValidatorExt _stringConverterValidatorExt;
        public CanBeConvertedTests()
        {
            var culture = new CultureInfo("de-DE");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            _stringConverterValidatorExt = new FactFinder.Converters.StringConverterValidatorExt(new Dictionary<Format, Func<string, bool>>
            {
                [Format.Number] = NumberValidator.CanBeParsed,
                [Format.Date] = DateValidator.CanBeParsed,
                [Format.TimeSpan] = TimeSpanValidator.CanBeParsed
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Given_NullOrEmpty_stringToCheck_Should_Throw_ArgumentException(string str)
        {
            // Act
            var act = () => _stringConverterValidatorExt.CanBeConverted(str, Format.Date);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
        

        [Theory]
        [InlineData(-2)]
        [InlineData(15)]
        public void Given_Invalid_format_Should_Throw_Exception_With_Message_Format_Not_Allowed(int format)
        {
            // Act
            var act = () => _stringConverterValidatorExt.CanBeConverted("0", (Format)format);

            // Assert
            act.Should().Throw<FormatNotAllowedException>().WithMessage("Format not allowed.");
        }

        [Theory]
        [InlineData("2")]
        [InlineData("-45")]
        [InlineData("0x7ffffff")]
        [InlineData("256.56")]
        [InlineData("-342342.432432")]
        public void Given_Valid_Number_As_stringToCheck_Returns_True(string str)
        {
            // Arrange and Act
            var result = _stringConverterValidatorExt.CanBeConverted(str, Format.Number);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("2wewe")]
        [InlineData("-452ew")]
        [InlineData("4sdfsdf")]
        [InlineData("-342342.4r2432")]
        [InlineData("-342342,4r2432")]
        public void Given_Invalid_Number_As_stringToCheck_Returns_False(string str)
        {
            // Arrange and Act
            var result = _stringConverterValidatorExt.CanBeConverted(str, Format.Number);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("12/05/2012")]
        [InlineData("30.10.2017")]
        [InlineData("9/1/1999")]
        [InlineData("10.2.2045")]
        [InlineData("Feb 16, 1978")]
        public void Given_Valid_Date_As_stringToCheck_Returns_True(string str)
        {
            // Arrange and Act
            var result = _stringConverterValidatorExt.CanBeConverted(str, Format.Date);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("5/28/2012")]
        [InlineData("05.18.2017")]
        [InlineData("09/17/1999")]
        [InlineData("1.27.2045")]
        [InlineData("30 Feb, 1978")]
        public void Given_Invalid_Date_As_stringToCheck_Returns_False(string str)
        {
            // Arrange and Act
            var result = _stringConverterValidatorExt.CanBeConverted(str, Format.Date);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("14:25:45.5554524")]
        [InlineData("2:10")]
        [InlineData("17")]
        [InlineData("2:45:22")]
        [InlineData("-02:15:59")]
        public void Given_Valid_TimeSpan_As_stringToCheck_Returns_True(string str)
        {
            // Arrange and Act
            var result = _stringConverterValidatorExt.CanBeConverted(str, Format.TimeSpan);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("25:45:23.878787878")]
        [InlineData("invalid time span")]
        [InlineData("17.")]
        public void Given_Invalid_TimeSpan_As_stringToCheck_Returns_False(string str)
        {
            // Arrange and Act
            var result = _stringConverterValidatorExt.CanBeConverted(str, Format.TimeSpan);

            // Assert
            result.Should().BeFalse();
        }
    }
}
