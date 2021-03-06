// Copyright 2004-2009 Castle Project - http://www.castleproject.org/
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

#if (!SILVERLIGHT)
namespace Castle.Facilities.EventWiring.Tests
{
	using System;

	using Castle.Windsor.Tests;

	using NUnit.Framework;
	using Windsor;

	[TestFixture]
	public class SubscriberWithDependenciesTestCase
	{
		private WindsorContainer container;

		[SetUp]
		public void Setup()
		{
			container = new WindsorContainer(ConfigHelper.ResolveConfigPath("Facilities/EventWiring/" + GetConfigFile()));
		}

		protected string GetConfigFile()
		{
			return "Config/dependencies.config";
		}

		[Test]
		public void CanCreateComponent_WithSubscriber_WithDependency()
		{
			Object listener = container.Resolve("HasSubscriberWithDependency");
			Assert.IsNotNull(listener);
		}

		//See also FACILITIES-97
		[Test]
		public void CanCreateComponent_WithSubscriber_WithGenericDependency()
		{
			Object listener = container.Resolve("HasSubscriberWithGenericDependency");
			Assert.IsNotNull(listener);
		}
	}
}
#endif