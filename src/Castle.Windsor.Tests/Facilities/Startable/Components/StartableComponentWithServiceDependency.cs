namespace Castle.Facilities.Startable.Tests.Components
{
	using Castle.Core;

	public class ServiceDependency
	{
	}

	[Transient]
	public class StartableComponentWithServiceDependency : IStartable
	{
		private bool _started;
		private bool _stopped;

		public bool Started
		{
			get { return _started; }
		}

		public bool Stopped
		{
			get { return _stopped; }
		}

		#region Implementation of IStartable

		public void Start()
		{
			_started = true;
		}

		public void Stop()
		{
			_stopped = true;
		}

		#endregion
	}
}