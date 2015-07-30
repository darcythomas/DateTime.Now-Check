using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace DateTimeCheck
{
    public class DateTimeChecker
    {
        public  IEnumerable<string> FilterMethods(MethodDefinition method)
        {
            Debug.WriteLine(method.Name);
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

        public IEnumerable<string> MethodsNamesContaining(IEnumerable<MethodDefinition> methods, IList<string> fullMethodNames)
        {

            foreach (var method in methods)
            {

                Debug.WriteLine(method.Name);
                foreach (var instruction in method.Body.Instructions)
                {
                    if (instruction.OpCode == OpCodes.Call)
                    {
                        MethodReference methodCall = instruction.Operand as MethodReference;
                        if (methodCall != null && fullMethodNames.Any(a=> a == methodCall.FullName))
                            yield return method.Name;
                    }
                }
            }
        }

        public IEnumerable<string> MethodsContainingDateTimeInAssembly(Type typeToSearch)
        {
            IList<string> dateTimeMethodNames = new List<String>()
            {
                "System.DateTime System.DateTime::get_Now()",
                "System.DateTime System.DateTime::get_UtcNow()",
                "System.DateTime System.DateTime::get_Today()"
            };

            ModuleDefinition module = ModuleDefinition.ReadModule(typeToSearch.Module.FullyQualifiedName);
            IEnumerable<MethodDefinition> allMethods = module.Types.SelectMany(RecurseAllMethods);
            return MethodsNamesContaining(allMethods, dateTimeMethodNames);

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
