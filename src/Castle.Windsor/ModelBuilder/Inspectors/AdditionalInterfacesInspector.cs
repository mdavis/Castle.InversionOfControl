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
	using System.Collections.Generic;
	using System.Linq;

	using Castle.Core;
	using Castle.MicroKernel.Proxy;

#if !(SILVERLIGHT)
	[Serializable]
#endif
	public class AdditionalInterfacesInspector : IContributeComponentModelConstruction
	{
		public void ProcessModel(IKernel kernel, ComponentModel model)
		{
			if (model.Configuration == null) return;

			var interfaces = model.Configuration.Children["additionalInterfaces"];
			if (interfaces == null) return;

			var list = new List<Type>();
			foreach (var @interface in interfaces.Children
				.Where(c => c.Name.Equals("add", StringComparison.InvariantCultureIgnoreCase)))
			{
				var interfaceTypeName = @interface.Attributes["interface"];
				list.Add(ObtainType(interfaceTypeName));
			}

			var options = ProxyUtil.ObtainProxyOptions(model, true);
			options.AddAdditionalInterfaces(list.ToArray());
		}

		private static Type ObtainType(String typeName)
		{
			try
			{
				return Type.GetType(typeName, true, false);
			}
			catch (Exception e)
			{
				var message = String.Format("The type name {0} could not be located.", typeName);
				throw new Exception(message, e);
			}
		}
	}
}
