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

using Fighting.Aspects.DynamicProxy.Tokens;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Fighting.Aspects.DynamicProxy.Generators.Emitters.SimpleAST
{
    public class MethodTokenExpression : Expression
    {
        private readonly MethodInfo method;
        private readonly Type declaringType;

        public MethodTokenExpression(MethodInfo method)
        {
            this.method = method;
            declaringType = method.DeclaringType;
        }

        public override void Emit(IMemberEmitter member, ILGenerator gen)
        {
            gen.Emit(OpCodes.Ldtoken, method);
            if (declaringType == null)
            {
                throw new GeneratorException("declaringType can't be null for this situation");
            }
            gen.Emit(OpCodes.Ldtoken, declaringType);

            var minfo = MethodBaseMethods.GetMethodFromHandle;
            gen.Emit(OpCodes.Call, minfo);
            gen.Emit(OpCodes.Castclass, typeof(MethodInfo));
        }
    }
}