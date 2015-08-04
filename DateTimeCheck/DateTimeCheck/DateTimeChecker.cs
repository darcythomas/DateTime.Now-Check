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
        
        public IEnumerable<string> MethodsContainingDateTime(IEnumerable<MethodDefinition> methods)
        {
            IList<string> dateTimeMethodNames = new List<String>()
            {
                "System.DateTime System.DateTime::get_Now()",
                "System.DateTime System.DateTime::get_UtcNow()",
                "System.DateTime System.DateTime::get_Today()"
            };

            foreach (var method in methods)
            {
                Debug.WriteLine(method.FullName);
                foreach (var instruction in method.Body.Instructions)
                {
                    if (instruction.OpCode == OpCodes.Call)
                    {
                        MethodReference methodCall = instruction.Operand as MethodReference;
                        if (methodCall != null && dateTimeMethodNames.Any(a => a == methodCall.FullName))
                            yield return method.FullName;
                    }
                }
            }
        }

        public IEnumerable<string> MethodsContainingDateTimeInAssemblyOfType(Type typeToSearch)
        {
            ModuleDefinition module = ModuleDefinition.ReadModule(typeToSearch.Module.FullyQualifiedName);
            IEnumerable<MethodDefinition> allMethods = module.Types.SelectMany(RecursivelyFindAllMethods);
            return MethodsContainingDateTime(allMethods);
        }

        public IEnumerable<MethodDefinition> RecursivelyFindAllMethods(TypeDefinition typeDefinition)
        {
            //In this level
            foreach (MethodDefinition method in typeDefinition.Methods)
            {
                yield return method;
            }
        
            //In deeper levels
            foreach (var type in typeDefinition.NestedTypes)
            {
                foreach (var method in RecursivelyFindAllMethods(type))
                {
                    yield return method;
                }
            }
        }
    }
}
