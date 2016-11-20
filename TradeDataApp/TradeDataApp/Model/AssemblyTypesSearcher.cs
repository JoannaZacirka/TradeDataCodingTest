using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TradeDataApp.Model
{
    public static class AssemblyTypesSearcher
    {
        public static IEnumerable<Type> GetTypesImplementing<T>(string assemblyFileName)
        {
            try
            {
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyFileName);
                Assembly assembly = Assembly.Load(assemblyName);
                Type readerType = typeof(T);
                if (assembly != null)
                {
                    Type[] assemblyTypes = assembly.GetTypes();
                    return from type in assemblyTypes
                            where !type.IsAbstract && !type.IsInterface &&
                                type.IsPublic &&
                                type.GetInterface(readerType.FullName) != null
                            select type;
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Exception occured when processing the file: " + assemblyFileName + Environment.NewLine + e.Message);
                Trace.Flush();
            }
            return null;
        }

    }
}
