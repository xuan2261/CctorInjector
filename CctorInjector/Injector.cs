using System;
using System.Linq;
using System.Collections.Generic;

using CctorInjector.InjectorHelper;

using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace CctorInjector
{
    public class Injector
    {
        private readonly ModuleDef ModuleDef;
        private readonly Type Type;

        private MethodDef Constructor;

        private IEnumerable<IDnlibDef> Members;

        public bool hasInjected;

        public Injector(ModuleDef module, Type type)
        {
            ModuleDef = module;
            Type = type;
        }

        public void Inject()
        {
            Constructor = CreateStaticConstructor();
            try
            {
                var typeModule = ModuleDefMD.Load(Type.Module);
                var typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(Type.MetadataToken));
                Members = InjectHelper.Inject(typeDef, ModuleDef.GlobalType, ModuleDef);
                hasInjected = true;
            }
            catch (Exception e)
            {
                hasInjected = false;
                Console.WriteLine(e.Message);
            }
        }

        public void AddCall(string methodName)
        {
            try
            {
                MethodDef methodDef = (MethodDef)Members.Single(method => method.Name == methodName);
                Constructor.Body.Instructions.Insert(Constructor.Body.Instructions.Count - 1, new Instruction(OpCodes.Call, methodDef));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private MethodDef CreateStaticConstructor() => ModuleDef.GlobalType.FindOrCreateStaticConstructor();
    }
}