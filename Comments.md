Three consecutive if statements could be replaced with a switch statement like this:
switch (MyObj)
    case Type1 t1: 
    case Type2 t2:
    case Type3 t3:
    default: Exception

This option is awailable in C# 7.0
In this case, an exception will be thrown in case if a default option.

Exception type could be replaced with ArgumentException, as it describes it better.

There is a couple of places where a NullReferenceException is possible,
it's better to check it for null, and throw an AgrumentNullException if it is so

There is no exception handling if any exception will come from external microservices

Hardcoded default values (for ukVat and percentage) probably should be configurable instead

In case of any kind of failure on the microservice side we will never know what caused the problem.
We need to return the actual list of errors instead of a numeric value.

We should probably add some logging mechanism, for trace a debug purposes.

VatRates could be a Enum instead of a static properties.

All the applications probably should have private setters for security purposes

