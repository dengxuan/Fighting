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

using Fighting.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fighting.Aspects.DynamicProxy
{
    [Serializable]
    public class ProxyGenerationOptions : ISerializable
    {
        public static readonly ProxyGenerationOptions Default = new ProxyGenerationOptions();

        private List<object> mixins;
        internal readonly IList<Attribute> attributesToAddToGeneratedTypes = new List<Attribute>();
        private readonly IList<CustomAttributeInfo> additionalAttributes = new List<CustomAttributeInfo>();

        [NonSerialized]
        private MixinData mixinData; // this is calculated dynamically on proxy type creation

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ProxyGenerationOptions" /> class.
        /// </summary>
        /// <param name = "hook">The hook.</param>
        public ProxyGenerationOptions(IProxyGenerationHook hook)
        {
            BaseTypeForInterfaceProxy = typeof(object);
            Hook = hook;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ProxyGenerationOptions" /> class.
        /// </summary>
        public ProxyGenerationOptions()
            : this(new AllMethodsHook())
        {
        }

        private ProxyGenerationOptions(SerializationInfo info, StreamingContext context)
        {
            Hook = (IProxyGenerationHook)info.GetValue("hook", typeof(IProxyGenerationHook));
            Selector = (IInterceptorSelector)info.GetValue("selector", typeof(IInterceptorSelector));
            mixins = (List<object>)info.GetValue("mixins", typeof(List<object>));
            BaseTypeForInterfaceProxy = Type.GetType(info.GetString("baseTypeForInterfaceProxy.AssemblyQualifiedName"));
        }

        public void Initialize()
        {
            if (mixinData == null)
            {
                try
                {
                    mixinData = new MixinData(mixins);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidMixinConfigurationException(
                        "There is a problem with the mixins added to this ProxyGenerationOptions: " + ex.Message, ex);
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("hook", Hook);
            info.AddValue("selector", Selector);
            info.AddValue("mixins", mixins);
            info.AddValue("baseTypeForInterfaceProxy.AssemblyQualifiedName", BaseTypeForInterfaceProxy.AssemblyQualifiedName);
        }

        public IProxyGenerationHook Hook { get; set; }

        public IInterceptorSelector Selector { get; set; }

        public Type BaseTypeForInterfaceProxy { get; set; }

        public IList<CustomAttributeInfo> AdditionalAttributes
        {
            get { return additionalAttributes; }
        }

        public MixinData MixinData
        {
            get
            {
                if (mixinData == null)
                {
                    throw new InvalidOperationException("Call Initialize before accessing the MixinData property.");
                }
                return mixinData;
            }
        }

        public void AddMixinInstance(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (mixins == null)
            {
                mixins = new List<object>();
            }

            mixins.Add(instance);
            mixinData = null;
        }

        public object[] MixinsAsArray()
        {
            if (mixins == null)
            {
                return new object[0];
            }

            return mixins.ToArray();
        }

        public bool HasMixins
        {
            get { return mixins != null && mixins.Count != 0; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var proxyGenerationOptions = obj as ProxyGenerationOptions;
            if (proxyGenerationOptions is null)
            {
                return false;
            }

            // ensure initialization before accessing MixinData
            Initialize();
            proxyGenerationOptions.Initialize();

            if (!Equals(Hook, proxyGenerationOptions.Hook))
            {
                return false;
            }
            if (!Equals(Selector == null, proxyGenerationOptions.Selector == null))
            {
                return false;
            }
            if (!Equals(MixinData, proxyGenerationOptions.MixinData))
            {
                return false;
            }
            if (!Equals(BaseTypeForInterfaceProxy, proxyGenerationOptions.BaseTypeForInterfaceProxy))
            {
                return false;
            }
            if (!Collections.Extensions.CollectionExtensions.AreEquivalent(AdditionalAttributes, proxyGenerationOptions.AdditionalAttributes))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            // ensure initialization before accessing MixinData
            Initialize();

            var result = Hook != null ? Hook.GetType().GetHashCode() : 0;
            result = 29 * result + (Selector != null ? 1 : 0);
            result = 29 * result + MixinData.GetHashCode();
            result = 29 * result + (BaseTypeForInterfaceProxy != null ? BaseTypeForInterfaceProxy.GetHashCode() : 0);
            result = 29 * result + Collections.Extensions.CollectionExtensions.GetContentsHashCode(AdditionalAttributes);
            return result;
        }
    }
}