using BenchmarkDotNet.Attributes;
using FactFinder.Converters;
using FactFinder.Validators;

namespace FactFinder.Benchmarks
{
    [SimpleJob(launchCount: 5)]
    [MemoryDiagnoser]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class IsFormatOldVsNew
    {
        private readonly Random _rnd = new();
        private readonly IEnumerable<string> _numbers;
        private const string _numberFormat = "nUMber";
        private readonly StringFormatValidatorExt _stringFormatValidatorExt;
        private readonly StringConverterValidatorExt _stringConverterValidatorExt;

        public IsFormatOldVsNew()
        {
            _numbers = Enumerable.Range(1, 999999).Select(x => _rnd.Next().ToString());
            _stringFormatValidatorExt = new StringFormatValidatorExt(new Dictionary<string, Func<string, bool>>
            {
                [NumberValidator.Name] = NumberValidator.CanBeParsed,
                [DateValidator.Name] = DateValidator.CanBeParsed,
                [TimeSpanValidator.Name] = TimeSpanValidator.CanBeParsed
            });

            _stringConverterValidatorExt = new StringConverterValidatorExt(new Dictionary<Format, Func<string, bool>>
            {
                [Format.Number] = NumberValidator.CanBeParsed,
                [Format.Date] = DateValidator.CanBeParsed,
                [Format.TimeSpan] = TimeSpanValidator.CanBeParsed
            });
        }

        [Benchmark(Description = $"{nameof(Class1.IsFormat)} old")]
        public void IsFormatOld()
        {
            foreach (var i in _numbers)
            {
                Class1.IsFormat(i, _numberFormat);
            }
        }

        [Benchmark(Description = $"{nameof(StringFormatValidator.IsFormat)} new")]
        public void IsFormatNew()
        {
            foreach (var i in _numbers)
            {
                StringFormatValidator.IsFormat(i, _numberFormat);
            }
        }

        [Benchmark(Description = $"{nameof(StringFormatValidator.IsFormat)} new ext")]
        public void IsFormatNewExt()
        {
            foreach (var i in _numbers)
            {
                _stringFormatValidatorExt.IsFormat(i, _numberFormat);
            }
        }

        [Benchmark(Description = $"{nameof(StringConverterValidator.CanBeConverted)}")]
        public void CanBeConverted()
        {
            foreach (var i in _numbers)
            {
                StringConverterValidator.CanBeConverted(i, Format.Number);
            }
        }

        [Benchmark(Description = $"{nameof(StringConverterValidatorExt.CanBeConverted)}")]
        public void CanBeConvertedExt()
        {
            foreach (var i in _numbers)
            {
                _stringConverterValidatorExt.CanBeConverted(i, Format.Number);
            }
        }
    }
}
