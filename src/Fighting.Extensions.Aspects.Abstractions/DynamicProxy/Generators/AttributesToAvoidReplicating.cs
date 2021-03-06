// Copyright 2004-2016 Hermit Project - http://www.Hermitproject.org/
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fighting.Aspects.DynamicProxy.Generators
{
    public static class AttributesToAvoidReplicating
    {
        private static readonly IList<Type> attributes = new List<Type>();

        static AttributesToAvoidReplicating()
        {
            Add<System.Runtime.InteropServices.ComImportAttribute>();
            Add<System.Runtime.InteropServices.MarshalAsAttribute>();
            Add<System.Runtime.InteropServices.TypeIdentifierAttribute>();
        }

        public static void Add(Type attribute)
        {
            if (!attributes.Contains(attribute))
            {
                attributes.Add(attribute);
            }
        }

        public static void Add<T>()
        {
            Add(typeof(T));
        }

        public static bool Contains(Type attribute)
        {
            return attributes.Contains(attribute);
        }

        internal static bool ShouldAvoid(Type attribute)
        {
            return attributes.Any(attr => attr.GetTypeInfo().IsAssignableFrom(attribute.GetTypeInfo()));
        }
    }
}