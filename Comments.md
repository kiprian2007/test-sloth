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

