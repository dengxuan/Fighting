// Copyright 2004-2017 Hermit Project - http://www.Hermitproject.org/
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
using Fighting.Reflection.Extensions;
using System;
using System.Reflection;

namespace Fighting.Aspects.DynamicProxy
{
    internal static class ExceptionMessageBuilder
    {
        /// <summary>
        /// Creates a message to inform clients that a proxy couldn't be created due to reliance on an
        /// inaccessible type (perhaps itself).
        /// </summary>
        /// <param name="inaccessibleType">the inaccessible type that prevents proxy creation</param>
        /// <param name="typeToProxy">the type that couldn't be proxied</param>
        public static string CreateMessageForInaccessibleType(Type inaccessibleType, Type typeToProxy)
        {
            var targetAssembly = typeToProxy.GetTypeInfo().Assembly;

            string inaccessibleTypeDescription = inaccessibleType == typeToProxy
                ? "it"
                : "type " + inaccessibleType.GetBestName();

            var messageFormat = "Can not create proxy for type {0} because {1} is not accessible. ";

            var message = string.Format(messageFormat,
                typeToProxy.GetBestName(),
                inaccessibleTypeDescription);

            var instructions = InternalsUtil.CreateInstructionsToMakeVisible(targetAssembly);

            return message + instructions;
        }
    }
}