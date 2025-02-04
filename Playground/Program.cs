﻿using Playground;
using Python.Runtime;

// Dynamic Link Library
// .dylib on mac
Runtime.PythonDLL = "/Library/Frameworks/Python.framework/Versions/3.10/bin/python3.10";
PythonEngine.Initialize();

using (Py.GIL())
{
    var person = new Person("John", "Smith");
    PyObject pyPerson = person.ToPython();

    Console.WriteLine(pyPerson.GetAttr("FirstName"));
    
    var locals = new PyDict();
    locals.SetItem("person", pyPerson);
    locals.SetItem("age", new PyInt(24));

    string code = "person.FirstName + ' ' + person.LastName + ' ' + str(age)";

    object? res = PythonEngine.Eval(code, null, locals).AsManagedObject(typeof(string));
    Console.WriteLine(res);
}

PythonEngine.Shutdown();
Console.Clear();




// --------------------------------------------------------------




Runtime.PythonDLL = "/Library/Frameworks/Python.framework/Versions/3.10/bin/python3.10";
PythonEngine.Initialize();

using (Py.GIL())
{
    using (PyModule scope = Py.CreateScope())
    {
        var person = new Person("John", "Smith");
        PyObject pyPerson = person.ToPython();
        
        scope.Set("person", pyPerson);
        scope.Set("age", new PyInt(24));
        
        var res = scope.Eval("person.FirstName + ' ' + person.LastName");
        Console.WriteLine($"Response from calling Eval: {res}");
        
        scope.Exec("desc = person.FirstName + ' ' + person.LastName + ' (' + str(age) + ' years old)'");
        var desc = scope.GetAttr("desc");
        Console.WriteLine(desc);

        scope.Exec("desc = desc + ' Hello World!'");
        Console.WriteLine(scope.GetAttr("desc"));

        var str = scope.Get<string>("desc");
        Console.WriteLine(str);
    }
}

PythonEngine.Shutdown();
Console.Clear();




// --------------------------------------------------------------




Runtime.PythonDLL = "/Library/Frameworks/Python.framework/Versions/3.10/bin/python3.10";
PythonEngine.Initialize();

using (Py.GIL())
{
    dynamic sys = Py.Import("sys");
    sys.path.append("/Users/lmesa/Repos/pythonnet_playground/Playground/");
    
    var myScript = Py.Import("test_script");
    
    var hello = myScript.InvokeMethod("get_hello");
    Console.WriteLine(hello);

    var param = new PyString("workbench");
    var res = myScript.InvokeMethod("test_with_param", new PyObject[] { param });
    Console.WriteLine(res);
    
    // Dynamic
    dynamic dynamicScript = Py.Import("test_script");
    var dynRes = dynamicScript.test_with_param("224");
    Console.WriteLine(dynRes);
}

PythonEngine.Shutdown();