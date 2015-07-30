using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace DateTimeCheck
{
    public class DateTimeChecker
    {
        public void IsItInHere(Type typeToSearch)
        {
            // var module = ModuleDefinition.ReadModule("CecilTest.exe");
            var module = ModuleDefinition.ReadModule(typeToSearch.Module.FullyQualifiedName);

            //    var type = module.Types.First(x => x.Name == "ClassWithDateTime");
            //    var method = type.Methods.First(x => x.Name == "MethodWithDateTime");
            //var q =    FilterMethods(method);

            Collection<TypeDefinition> type = module.Types; //.First(x => x.Name == "ClassWithDateTime");
            IEnumerable<MethodDefinition> method = type.SelectMany(s => s.Methods);


            IEnumerable<string> q = method.SelectMany(s => FilterMethods(s));


            //var type = module.Types.SelectMany(s=> s.Methods);
            //var q = type.Select( FilterMethods);
            foreach (var methodName in q)
            {
                Console.WriteLine(methodName);
            }


        }

        public static IEnumerable<string> FilterMethods(MethodDefinition method)
        {
            Console.WriteLine(method.Name);
            foreach (var instruction in method.Body.Instructions)
            {
                if (instruction.OpCode == OpCodes.Call)
                {
                    MethodReference methodCall = instruction.Operand as MethodReference;
                    if (methodCall != null)
                        yield return methodCall.FullName;
                }
            }
        }

        public IEnumerable<string> MethodsNamesContaining(IEnumerable<MethodDefinition> methods,
            string fullMethodName)
        {

            foreach (var method in methods)
            {


                Debug.WriteLine(method.Name);
                foreach (var instruction in method.Body.Instructions)
                {
                    if (instruction.OpCode == OpCodes.Call)
                    {
                        MethodReference methodCall = instruction.Operand as MethodReference;
                        if (methodCall != null && methodCall.FullName == fullMethodName)
                            yield return method.Name;
                    }
                }
            }
        }

        public IEnumerable<string> MethodsContainingDateTimeInAssembly(Type typeToSearch)
        {
            string dateTimeNowName = "System.DateTime System.DateTime::get_Now()";

            ModuleDefinition module = ModuleDefinition.ReadModule(typeToSearch.Module.FullyQualifiedName);

            IEnumerable<MethodDefinition> allMethods = module.Types.SelectMany(s => RecurseAllMethods(s));
            return MethodsNamesContaining(allMethods, dateTimeNowName);

        }

        public IEnumerable<MethodDefinition> RecurseAllMethods(TypeDefinition module)
        {
            foreach (MethodDefinition method in module.Methods)
            {
                yield return method;
            }
        
            foreach (var type in module.NestedTypes)
            {
                foreach (var method in RecurseAllMethods(type))
                {
                    yield return method;
                }

            }

        }
    }
}
