// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
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

namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;

	using Castle.Core;
	using Castle.MicroKernel.Proxy;
	using Castle.MicroKernel.Util;

#if !(SILVERLIGHT)
	[Serializable]
#endif
	public class MixinInspector : IContributeComponentModelConstruction
	{
		public void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (model.Configuration == null) return;

			var mixins = model.Configuration.Children["mixins"];
			if (mixins == null) return;

			var options = ProxyUtil.ObtainProxyOptions(model, true);
			foreach (var mixin in mixins.Children)
			{
				var value = mixin.Value;

				if (!ReferenceExpressionUtil.IsReference(value))
				{
					throw new Exception(
						String.Format("The value for the mixin must be a reference to a component (Currently {0})", value));
				}

				var componentKey = ReferenceExpressionUtil.ExtractComponentKey(value);
				var mixIn = new ComponentReference<object>(componentKey);
				options.AddMixinReference(mixIn);
				model.Dependencies.Add(CreateDependencyModel(componentKey));
			}

		}

		protected DependencyModel CreateDependencyModel(string componentKey)
		{
			return new DependencyModel(DependencyType.ServiceOverride, componentKey, typeof(object), false);
		}
	}
}