




using PureMVC.Interfaces;
using System;
using System.Collections.Generic;

namespace PureMVC.Patterns
{
	public class MacroCommand : Notifier, ICommand, INotifier
	{
		private readonly IList<object> m_subCommands;

		public MacroCommand()
		{
			m_subCommands = new List<object>();
			InitializeMacroCommand();
		}


        //@TODO MacroCommand(IEnumerable_003CType_003E types)
        //public MacroCommand(IEnumerable_003CType_003E types)
        //{
        //	m_subCommands = new List<object>(types);
        //	InitializeMacroCommand();
        //}

        public MacroCommand(IEnumerable<ICommand> commands)
		{
			m_subCommands = new List<object>(commands);
			InitializeMacroCommand();
		}

		public MacroCommand(IEnumerable<object> commandCollection)
		{
			m_subCommands = new List<object>(commandCollection);
			InitializeMacroCommand();
		}

		public void Execute(INotification notification)
		{
			while (m_subCommands.Count > 0)
			{
				Type type = m_subCommands[0] as Type;
				if (type != null)
				{
					object obj = Activator.CreateInstance(type);
					if (obj is ICommand)
					{
						ICommand command = (ICommand)obj;
						command.InitializeNotifier(base.MultitonKey);
						command.Execute(notification);
					}
				}
				else
				{
					ICommand command2 = m_subCommands[0] as ICommand;
					if (command2 != null)
					{
						command2.InitializeNotifier(base.MultitonKey);
						command2.Execute(notification);
					}
				}
				m_subCommands.RemoveAt(0);
			}
		}

		protected virtual void InitializeMacroCommand()
		{
		}

		protected void AddSubCommand(Type commandType)
		{
			m_subCommands.Add(commandType);
		}

		protected void AddSubCommand(ICommand command)
		{
			m_subCommands.Add(command);
		}
	}
}
