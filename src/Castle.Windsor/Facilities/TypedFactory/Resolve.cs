﻿// Copyright 2004-2009 Castle Project - http://www.castleproject.org/
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

namespace Castle.MicroKernel.Facilities.TypedFactory
{
	using Castle.Core.Interceptor;

	/// <summary>
	/// resolves componet selected by given <see cref="ITypedFactoryComponentSelector"/> from the container
	/// </summary>
	public class Resolve : ITypedFactoryMethod
	{
		private readonly IKernel kernel;
		private readonly ITypedFactoryComponentSelector selector;
		public Resolve(IKernel kernel, ITypedFactoryComponentSelector selector)
		{
			this.kernel = kernel;
			this.selector = selector;
		}

		public void Invoke(IInvocation invocation)
		{
			var component = selector.SelectComponent(invocation.Method, invocation.TargetType, invocation.Arguments);
			if(component == null)
			{
				throw new FacilityException(
					string.Format(
						"Selector {0} didn't select any component for method {1}. This usually signifies a bug in the selector.", selector,
						invocation.Method));
			}
			invocation.ReturnValue = component.Resolve(kernel);
		}
	}
}