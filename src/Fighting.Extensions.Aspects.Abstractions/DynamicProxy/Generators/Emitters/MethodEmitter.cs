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
using Fighting.Aspects.DynamicProxy.Generators.Emitters.CodeBuilders;
using Fighting.Aspects.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Fighting.Aspects.DynamicProxy.Generators.Emitters
{
    [DebuggerDisplay("{builder.Name}")]
    public class MethodEmitter : IMemberEmitter
    {
        private readonly MethodBuilder builder;
        private readonly GenericTypeParameterBuilder[] genericTypeParams;

        private ArgumentReference[] arguments;

        private MethodCodeBuilder codebuilder;

        protected internal MethodEmitter(MethodBuilder builder)
        {
            this.builder = builder;
        }

        internal MethodEmitter(AbstractTypeEmitter owner, String name, MethodAttributes attributes)
            : this(owner.TypeBuilder.DefineMethod(name, attributes))
        {
        }

        internal MethodEmitter(AbstractTypeEmitter owner, String name,
                               MethodAttributes attributes, Type returnType,
                               params Type[] argumentTypes)
            : this(owner, name, attributes)
        {
            SetParameters(argumentTypes);
            SetReturnType(returnType);
        }

        internal MethodEmitter(AbstractTypeEmitter owner, String name,
                               MethodAttributes attributes, MethodInfo methodToUseAsATemplate)
            : this(owner, name, attributes)
        {
            var name2GenericType = GenericUtil.GetGenericArgumentsMap(owner);

            var returnType = GenericUtil.ExtractCorrectType(methodToUseAsATemplate.ReturnType, name2GenericType);
            var baseMethodParameters = methodToUseAsATemplate.GetParameters();
            var parameters = GenericUtil.ExtractParametersTypes(baseMethodParameters, name2GenericType);

            genericTypeParams = GenericUtil.CopyGenericArguments(methodToUseAsATemplate, builder, name2GenericType);
            SetParameters(parameters);
            SetReturnType(returnType);
            SetSignature(returnType, methodToUseAsATemplate.ReturnParameter, parameters, baseMethodParameters);
            DefineParameters(baseMethodParameters);
        }

        public ArgumentReference[] Arguments
        {
            get { return arguments; }
        }

        public virtual MethodCodeBuilder CodeBuilder
        {
            get
            {
                if (codebuilder == null)
                {
                    codebuilder = new MethodCodeBuilder(builder.GetILGenerator());
                }
                return codebuilder;
            }
        }

        public GenericTypeParameterBuilder[] GenericTypeParams
        {
            get { return genericTypeParams; }
        }

        public MethodBuilder MethodBuilder
        {
            get { return builder; }
        }

        public MemberInfo Member
        {
            get { return builder; }
        }

        public Type ReturnType
        {
            get { return builder.ReturnType; }
        }

        private bool ImplementedByRuntime
        {
            get
            {
#if FEATURE_LEGACY_REFLECTION_API
				var attributes = builder.GetMethodImplementationFlags();
#else
                var attributes = builder.MethodImplementationFlags;
#endif
                return (attributes & MethodImplAttributes.Runtime) != 0;
            }
        }

        public void DefineCustomAttribute(CustomAttributeBuilder attribute)
        {
            builder.SetCustomAttribute(attribute);
        }

        public void SetParameters(Type[] paramTypes)
        {
            builder.SetParameters(paramTypes);
            arguments = ArgumentsUtil.ConvertToArgumentReference(paramTypes);
            ArgumentsUtil.InitializeArgumentsByPosition(arguments, MethodBuilder.IsStatic);
        }

        public virtual void EnsureValidCodeBlock()
        {
            if (ImplementedByRuntime == false && CodeBuilder.IsEmpty)
            {
                CodeBuilder.AddStatement(new NopStatement());
                CodeBuilder.AddStatement(new ReturnStatement());
            }
        }

        public virtual void Generate()
        {
            if (ImplementedByRuntime)
            {
                return;
            }

            codebuilder.Generate(this, builder.GetILGenerator());
        }

        private void DefineParameters(ParameterInfo[] parameters)
        {
            foreach (var parameter in parameters)
            {
                var parameterBuilder = builder.DefineParameter(parameter.Position + 1, parameter.Attributes, parameter.Name);
                foreach (var attribute in parameter.GetNonInheritableAttributes())
                {
                    parameterBuilder.SetCustomAttribute(attribute.Builder);
                }
            }
        }

        private void SetReturnType(Type returnType)
        {
            builder.SetReturnType(returnType);
        }

        private void SetSignature(Type returnType, ParameterInfo returnParameter, Type[] parameters,
                                  ParameterInfo[] baseMethodParameters)
        {
            builder.SetSignature(
                returnType,
#if FEATURE_EMIT_CUSTOMMODIFIERS
				returnParameter.GetRequiredCustomModifiers().Reverse().ToArray(),
				returnParameter.GetOptionalCustomModifiers().Reverse().ToArray(),
#else
                null,
                null,
#endif
                parameters,
#if FEATURE_EMIT_CUSTOMMODIFIERS
				baseMethodParameters.Select(x => x.GetRequiredCustomModifiers().Reverse().ToArray()).ToArray(),
				baseMethodParameters.Select(x => x.GetOptionalCustomModifiers().Reverse().ToArray()).ToArray()
#else
                null,
                null
#endif
            );
        }
    }
}