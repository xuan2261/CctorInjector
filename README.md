# CctorInjector
A library to inject classes in .NET modules.

**What is CctorInjector?**

CctorInjector is a library to inject classes in .NET modules, using this you can add calls for methods directly in the cctor.

```
Injector Injector = new Injector(module, typeof(Test));
Injector.Inject();

if (Injector.hasInjected)
    Injector.AddCall("ExecuteMePlease");
```
![demo](https://i.imgur.com/cIyvjCu.png)
