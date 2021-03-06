// Copyright 2004-2011 Hermit Project - http://www.Hermitproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Fighting.Aspects.DynamicProxy.Internal;
using Fighting.Aspects.DynamicProxy.Serialization;
using Fighting.Aspects.DynamicProxy.Contributors;
using Fighting.Aspects.DynamicProxy.Generators.Emitters;
using Fighting.Aspects.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;

namespace Fighting.Aspects.DynamicProxy.Generators
{
    public class InterfaceProxyWithoutTargetGenerator : InterfaceProxyWithTargetGenerator
    {
        public InterfaceProxyWithoutTargetGenerator(ModuleScope scope, Type @interface) : base(scope, @interface)
        {
        }

        protected override string GeneratorType
        {
            get { return ProxyTypeConstants.InterfaceWithoutTarget; }
        }

        protected override ITypeContributor AddMappingForTargetType(
            IDictionary<Type, ITypeContributor> interfaceTypeImplementerMapping, Type proxyTargetType,
            ICollection<Type> targetInterfaces, ICollection<Type> additionalInterfaces, INamingScope namingScope)
        {
            var contributor = new InterfaceProxyWithoutTargetContributor(namingScope, (c, m) => NullExpression.Instance)
            { Logger = Logger };
            foreach (var @interface in targetType.GetAllInterfaces())
            {
                contributor.AddInterfaceToProxy(@interface);
                AddMappingNoCheck(@interface, contributor, interfaceTypeImplementerMapping);
            }
            return contributor;
        }

        protected override Type GenerateType(string typeName, Type proxyTargetType, Type[] interfaces,
                                             INamingScope namingScope)
        {
            var allInterfaces = GetTypeImplementerMapping(interfaces, targetType, out IEnumerable<ITypeContributor> contributors, namingScope);
            var model = new MetaType();
            // collect elements
            foreach (var contributor in contributors)
            {
                contributor.CollectElementsToProxy(ProxyGenerationOptions.Hook, model);
            }

            ProxyGenerationOptions.Hook.MethodsInspected();

            var baseType = Init(typeName, out ClassEmitter emitter, proxyTargetType, out FieldReference interceptorsField, allInterfaces);

            // Constructor

            var cctor = GenerateStaticConstructor(emitter);
            var mixinFieldsList = new List<FieldReference>();

            foreach (var contributor in contributors)
            {
                contributor.Generate(emitter, ProxyGenerationOptions);

                // TODO: redo it
                if (contributor is MixinContributor)
                {
                    mixinFieldsList.AddRange((contributor as MixinContributor).Fields);
                }
            }

            var ctorArguments = new List<FieldReference>(mixinFieldsList) { interceptorsField, targetField };
            var selector = emitter.GetField("__selector");
            if (selector != null)
            {
                ctorArguments.Add(selector);
            }

            GenerateConstructors(emitter, baseType, ctorArguments.ToArray());

            // Complete type initializer code body
            CompleteInitCacheMethod(cctor.CodeBuilder);

            // Crosses fingers and build type
            var generatedType = emitter.BuildType();

            InitializeStaticFields(generatedType);
            return generatedType;
        }
    }
}