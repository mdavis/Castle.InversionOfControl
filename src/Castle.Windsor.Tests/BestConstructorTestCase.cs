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

namespace Castle.MicroKernel.Tests
{
	using Castle.Core.Configuration;
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.SubSystems.Configuration;
	using Castle.MicroKernel.Tests.ClassComponents;

	using NUnit.Framework;

	/// <summary>
	/// Summary description for BestConstructorTestCase.
	/// </summary>
	[TestFixture]
	public class BestConstructorTestCase
	{
		private IKernel kernel;

		[SetUp]
		public void Init()
		{
			kernel = new DefaultKernel();
		}

		[TearDown]
		public void Dispose()
		{
			kernel.Dispose();
		}

		[Test]
		public void ConstructorWithMoreArguments()
		{
			kernel.AddComponent("a", typeof(A));
			kernel.AddComponent("b", typeof(B));
			kernel.AddComponent("c", typeof(C));
			kernel.AddComponent("service", typeof(ServiceUser));

			ServiceUser service = (ServiceUser) kernel["service"];

			Assert.IsNotNull(service);
			Assert.IsNotNull(service.AComponent);
			Assert.IsNotNull(service.BComponent);
			Assert.IsNotNull(service.CComponent);
		}

		[Test]
		public void ConstructorWithTwoArguments()
		{
			kernel.AddComponent("a", typeof(A));
			kernel.AddComponent("b", typeof(B));
			kernel.AddComponent("service", typeof(ServiceUser));

			ServiceUser service = (ServiceUser) kernel["service"];

			Assert.IsNotNull(service);
			Assert.IsNotNull(service.AComponent);
			Assert.IsNotNull(service.BComponent);
			Assert.IsNull(service.CComponent);
		}

		[Test]
		public void ConstructorWithOneArgument()
		{
			kernel.AddComponent("a", typeof(A));
			kernel.AddComponent("service", typeof(ServiceUser));

			ServiceUser service = (ServiceUser) kernel["service"];

			Assert.IsNotNull(service);
			Assert.IsNotNull(service.AComponent);
			Assert.IsNull(service.BComponent);
			Assert.IsNull(service.CComponent);
		}

		[Test]
		public void ParametersAndServicesBestCase()
		{
			DefaultConfigurationStore store = new DefaultConfigurationStore();

			MutableConfiguration config = new MutableConfiguration("component");
			MutableConfiguration parameters = new MutableConfiguration("parameters");
			config.Children.Add(parameters);
			parameters.Children.Add(new MutableConfiguration("name", "hammett"));
			parameters.Children.Add(new MutableConfiguration("port", "120"));

			store.AddComponentConfiguration("service", config);

			kernel.ConfigurationStore = store;

			kernel.AddComponent("a", typeof(A));
			kernel.AddComponent("service", typeof(ServiceUser2));

			ServiceUser2 service = (ServiceUser2) kernel["service"];

			Assert.IsNotNull(service);
			Assert.IsNotNull(service.AComponent);
			Assert.IsNull(service.BComponent);
			Assert.IsNull(service.CComponent);
			Assert.AreEqual("hammett", service.Name);
			Assert.AreEqual(120, service.Port);
		}

		[Test]
		public void ParametersAndServicesBestCase2()
		{
			DefaultConfigurationStore store = new DefaultConfigurationStore();

			MutableConfiguration config = new MutableConfiguration("component");
			MutableConfiguration parameters = new MutableConfiguration("parameters");
			config.Children.Add(parameters);
			parameters.Children.Add(new MutableConfiguration("name", "hammett"));
			parameters.Children.Add(new MutableConfiguration("port", "120"));
			parameters.Children.Add(new MutableConfiguration("Scheduleinterval", "22"));

			store.AddComponentConfiguration("service", config);

			kernel.ConfigurationStore = store;

			kernel.AddComponent("a", typeof(A));
			kernel.AddComponent("service", typeof(ServiceUser2));

			ServiceUser2 service = (ServiceUser2) kernel["service"];

			Assert.IsNotNull(service);
			Assert.IsNotNull(service.AComponent);
			Assert.IsNull(service.BComponent);
			Assert.IsNull(service.CComponent);
			Assert.AreEqual("hammett", service.Name);
			Assert.AreEqual(120, service.Port);
			Assert.AreEqual(22, service.ScheduleInterval);
		}

		[Test]
		public void Two_constructors_equal_number_of_parameters_pick_one_that_can_be_satisfied()
		{
			kernel.Register(Component.For<ICommon>().ImplementedBy<CommonImpl1>(),
			                Component.For<HasTwoConstructors>());

			kernel.Resolve<HasTwoConstructors>();
		}

		[Test]
		public void Two_satisfiable_constructors_pick_one_with_more_inline_parameters()
		{
			kernel.Register(Component.For<ICommon>().ImplementedBy<CommonImpl1>(),
			                Component.For<HasTwoConstructors2>()
			                	.Parameters(Parameter.ForKey("param").Eq("foo")));

			var component = kernel.Resolve<HasTwoConstructors2>();

			Assert.AreEqual("foo", component.Param);
		}

		[Test]
		public void Two_satisfiable_constructors_equal_number_of_inline_parameters_pick_one_with_more_service_overrides()
		{
			kernel.Register(Component.For<ICommon>().ImplementedBy<CommonImpl1>().Named("Mucha"),
			                Component.For<ICustomer>().ImplementedBy<CustomerImpl>().Named("Stefan"),
			                Component.For<HasTwoConstructors>().Named("first")
			                	.ServiceOverrides(new { customer = "Stefan" }),
			                Component.For<HasTwoConstructors>().Named("second")
			                	.ServiceOverrides(new { common = "Mucha" }));

			var first = kernel.Resolve<HasTwoConstructors>("first");
			var second = kernel.Resolve<HasTwoConstructors>("second");

			Assert.IsNotNull(first.Customer);
			Assert.IsNotNull(second.Common);
		}

		[Test]
		public void Two_satisfiable_constructors_identical_dependency_kinds_pick_based_on_parameter_names()
		{
			kernel.Register(Component.For<ICommon>().ImplementedBy<CommonImpl1>(),
			                Component.For<ICustomer>().ImplementedBy<CustomerImpl>(),
			                Component.For<HasTwoConstructors>());

			var component = kernel.Resolve<HasTwoConstructors>();

			// common is 'smaller' so we pick ctor with dependency named 'common'
			Assert.Less("common", "customer");
			Assert.IsNotNull(component.Common);
		}
	}
}