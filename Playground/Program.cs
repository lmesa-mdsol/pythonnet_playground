using Playground;
using Python.Runtime;

Runtime.PythonDLL = "/Library/Frameworks/Python.framework/Versions/3.10/bin/python3.10";
PythonEngine.Initialize();
using (Py.GIL())
{
    using (PyModule scope = Py.CreateScope())
    {
        var person = new Person("John", "Smith");
        PyObject pyPerson = person.ToPython();

        Console.WriteLine(pyPerson.GetAttr("FirstName"));

        scope.Set("person", pyPerson);
        scope.Set("age", new PyInt(24));

        var locals = new PyDict();
        locals.SetItem("person", pyPerson);
        locals.SetItem("age", new PyInt(24));

        string code = "person.FirstName + ' ' + person.LastName";

        object b = PythonEngine.Eval(code, null, locals)
            .AsManagedObject(typeof(string));

        var res = scope.Eval(code);
        scope.Exec("desc = person.FirstName + ' ' + person.LastName + ' (' + str(age) + ' years old)'");
        var desc = scope.GetAttr("desc");
        Console.WriteLine(desc);
    }
}
Console.WriteLine("Hello, World!");