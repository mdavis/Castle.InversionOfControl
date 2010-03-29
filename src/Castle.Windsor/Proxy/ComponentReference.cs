﻿// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
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

namespace Castle.MicroKernel.Proxy
{
	using System;

	/// <summary>
	/// Reference to component obtained from a container.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ComponentReference<T> : IReference<T>
	{
		private readonly string componentKey;

		public ComponentReference(string componentKey)
		{
			if (componentKey == null)
			{
				throw new ArgumentNullException("componentKey");
			}

			this.componentKey = componentKey;
		}

		public T Resolve(IKernel kernel, CreationContext context)
		{
			var handler = kernel.GetHandler(componentKey);
			if (handler == null)
			{
				throw new Exception(string.Format("Component {0} could not be resolved. Make sure you didn't misspell the name, and that component is registered.", componentKey));
			}

			try
			{
				return (T)handler.Resolve(context);
			}
			catch (InvalidCastException e)
			{
				throw new Exception(string.Format("Component {0} is not compatible with type {1}.", componentKey, typeof(T)), e);
			}
		}
	}
}