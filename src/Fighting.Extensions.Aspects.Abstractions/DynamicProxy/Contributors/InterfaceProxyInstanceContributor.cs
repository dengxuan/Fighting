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

using Fighting.Aspects.DynamicProxy.Generators.Emitters;
using Fighting.Aspects.DynamicProxy.Generators.Emitters.SimpleAST;
using Fighting.Aspects.DynamicProxy.Tokens;
using Fighting.Aspects.DynamicProxy.Generators.Emitters.CodeBuilders;
using System;

namespace Fighting.Aspects.DynamicProxy.Contributors
{
    public class InterfaceProxyInstanceContributor : ProxyInstanceContributor
    {
        protected override Expression GetTargetReferenceExpression(ClassEmitter emitter)
        {
            return emitter.GetField("__target").ToExpression();
        }

        public InterfaceProxyInstanceContributor(Type targetType, string proxyGeneratorId, Type[] interfaces)
            : base(targetType, interfaces, proxyGeneratorId)
        {
        }

        protected override void CustomizeGetObjectData(AbstractCodeBuilder codebuilder, ArgumentReference serializationInfo,
                                                       ArgumentReference streamingContext, ClassEmitter emitter)
        {
            var targetField = emitter.GetField("__target");

            codebuilder.AddStatement(new ExpressionStatement(
                                         new MethodInvocationExpression(serializationInfo, SerializationInfoMethods.AddValue_Object,
                                                                        new ConstReference("__targetFieldType").ToExpression(),
                                                                        new ConstReference(
                                                                            targetField.Reference.FieldType.AssemblyQualifiedName).
                                                                            ToExpression())));

            codebuilder.AddStatement(new ExpressionStatement(
                                         new MethodInvocationExpression(serializationInfo, SerializationInfoMethods.AddValue_Object,
                                                                        new ConstReference("__theInterface").ToExpression(),
                                                                        new ConstReference(targetType.AssemblyQualifiedName).
                                                                            ToExpression())));
        }
    }
}