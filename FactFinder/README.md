Hello and thank you for taking this task.
I did a quick round of code review and found couple of issues.
By reading the code I assume that the following contract should be applied:
a method should accept 2 parameters
* string represantation of the data
* string represantation of the format of the data

Also, system allows to accept only the folling formats
* number
* date 
* timespan

Unfortunately the contract does not restrict to a specific type of number. 
So I assume that we can expect at least integers, numbers with floating point and hexadecimal numbers (despite of the fact taht in the code we are using Int32.TryParse method) 

With this in mind lets have a look at the code.

First we initialize dictionary with 3 key/value pairs where key is an allowed format and boolean as a value.
Here are the issues with this approach:
* initalization happens every time we are calling this method which leads to unnecessary allocations
* values of the dictionary are never used

Second block where the format check happens doesn't follows the business logic
* for loop starts at from 1 and ends on 2 which with combination with the following code `allowedDict.Keys.ToArray()[i - 1]` never reaches key/value pair at index 2 which is timespan
* `allowedDict.Keys.ToArray()[i - 1]` could be changed to `allowedDict.ElementAt(i - 1).Key`
* `isNotAllowed |= 1 << i;` I don't understand the purpose of this code.

  Let's take it step by step:
  We will check 'number' as a format. In this case we will reach the code at first iteration when i is and isNotAllowed is 0.
  By applying bitwise OR operator between 0 and 1 we will get 1 and by shifting it to 1 bit we will get 2 which be assigned to isNotAllowed.
  In that case ` if (isNotAllowed > 0)
                throw new Exception("Format not allowed.");` 
  will throw an exception. Exact the opposite  of that we want.
  Same for format 'date' but instead of 2 4 will be assigned to isNotAllowed and same exception will be thrown.

Regarding if else statements I would prefer avoiding them in this case and use switch statement instead. Also, it's not a good idea to lowercase f each time you are working with it.

As a conclusion I've prepared several possible suggestions:
* if a contract cannot be changed and it should be static method with 2 string parametes I would go with StringFormatValidator.IsFormat solution
* if we can change a contract a bit and use another method naming and get use of enum instead of the string (but keep all the logic incapsulated) I would go with StringConverterValidator.CanBeConverted
* if we want more extendable solution I've prepared a quick one (StringConverterValidatorExt.CanBeConverted) without going into deep with interfaces and patterns. You can have a look at it just to get some of the ideas. I also have one following the exact contract with static StringFormatValidatorDI.IsFormat and non-static StringFormatValidatorExt.IsFormat


At the end here is some benchmarks for each solution. I hope this helps you choose the right approach.

Method       |     Mean |   Error |  StdDev |      Min |      Max |   Median |        Gen0 | Allocated |
-------------|--------- |---------|---------|----------|----------|----------|-------------|-----------|
'IsFormat old'| 307.6 ms | 2.09 ms | 6.44 ms | 299.1 ms | 341.3 ms | 306.2 ms | 133500.0000 | 400.81 MB |
'IsFormat new' | 250.5 ms | 0.55 ms | 1.39 ms | 246.3 ms | 253.4 ms | 250.6 ms |  26666.6667 |  80.38 MB |
'IsFormat new ext' | 250.5 ms | 0.44 ms | 1.08 ms | 248.4 ms | 253.8 ms | 250.5 ms |  26500.0000 |  80.37 MB |
CanBeConverted | 171.4 ms | 0.58 ms | 1.43 ms | 168.1 ms | 174.7 ms | 171.5 ms |  14000.0000 |  42.23 MB |
CanBeConverted | 184.5 ms | 1.05 ms | 2.60 ms | 180.4 ms | 190.9 ms | 183.8 ms |  14000.0000 |  42.22 MB |

As for me if conract is a must I would go with StringFormatValidator.IsFormat, otherwise (to keep it simple) - StringConverterValidator.CanBeConverted.
