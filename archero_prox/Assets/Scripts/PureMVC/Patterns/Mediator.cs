




using PureMVC.Interfaces;
using System.Collections.Generic;

namespace PureMVC.Patterns
{
	public class Mediator : Notifier, IMediator, INotifier
	{
		public const string NAME = "Mediator";

		protected string m_mediatorName;

		protected object m_viewComponent;

		public virtual IEnumerable<string> ListNotificationInterests => new List<string>();

		public virtual string MediatorName => m_mediatorName;

		public object ViewComponent
		{
			get
			{
				return m_viewComponent;
			}
			set
			{
				m_viewComponent = value;
			}
		}

		public Mediator()
			: this("Mediator", null)
		{
		}

		public Mediator(string mediatorName)
			: this(mediatorName, null)
		{
		}

		public Mediator(string mediatorName, object viewComponent)
		{
			m_mediatorName = (mediatorName ?? "Mediator");
			m_viewComponent = viewComponent;
		}

		public virtual void HandleNotification(INotification notification)
		{
		}

		public virtual void OnRegister()
		{
		}

		public virtual void OnRemove()
		{
		}

		public virtual void PublicNotification(INotification notification)
		{
		}

		public virtual void Blur(bool blur)
		{
		}

		public virtual object GetEvent(string eventName)
		{
			return null;
		}
	}
}
