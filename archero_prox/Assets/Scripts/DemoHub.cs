




#if ENABLE_BEST_HTTP
using BestHTTP.Examples;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
#endif
using UnityEngine;

internal class DemoHub
#if ENABLE_BEST_HTTP
    : Hub
#endif
{
	private float longRunningJobProgress;

	private string longRunningJobStatus = "Not Started!";

	private string fromArbitraryCodeResult = string.Empty;

	private string groupAddedResult = string.Empty;

	private string dynamicTaskResult = string.Empty;

	private string genericTaskResult = string.Empty;

	private string taskWithExceptionResult = string.Empty;

	private string genericTaskWithExceptionResult = string.Empty;

	private string synchronousExceptionResult = string.Empty;

	private string invokingHubMethodWithDynamicResult = string.Empty;

	private string simpleArrayResult = string.Empty;

	private string complexTypeResult = string.Empty;

	private string complexArrayResult = string.Empty;

	private string voidOverloadResult = string.Empty;

	private string intOverloadResult = string.Empty;

	private string readStateResult = string.Empty;

	private string plainTaskResult = string.Empty;

	private string genericTaskWithContinueWithResult = string.Empty;

#if ENABLE_BEST_HTTP
	private GUIMessageList invokeResults = new GUIMessageList();
#endif

	public DemoHub()
#if ENABLE_BEST_HTTP
		: base("demo")
#endif
	{
#if ENABLE_BEST_HTTP
		On("invoke", Invoke);
		On("signal", Signal);
		On("groupAdded", GroupAdded);
		On("fromArbitraryCode", FromArbitraryCode);
#endif
	}

	public void ReportProgress(string arg)
	{
#if ENABLE_BEST_HTTP
		Call("reportProgress", OnLongRunningJob_Done, null, OnLongRunningJob_Progress, arg);
#endif
	}

#if ENABLE_BEST_HTTP
	public void OnLongRunningJob_Progress(Hub hub, ClientMessage originialMessage, ProgressMessage progress)
	{
		longRunningJobProgress = (float)progress.Progress;
		longRunningJobStatus = progress.Progress.ToString() + "%";
	}

	public void OnLongRunningJob_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		longRunningJobStatus = result.ReturnValue.ToString();
		MultipleCalls();
	}
#endif

	public void MultipleCalls()
	{
#if ENABLE_BEST_HTTP
		Call("multipleCalls");
#endif
	}

	public void DynamicTask()
	{
#if ENABLE_BEST_HTTP
		Call("dynamicTask", OnDynamicTask_Done, OnDynamicTask_Failed);
#endif
	}

#if ENABLE_BEST_HTTP
	private void OnDynamicTask_Failed(Hub hub, ClientMessage originalMessage, FailureMessage result)
	{
		dynamicTaskResult = $"The dynamic task failed :( {result.ErrorMessage}";
	}

	private void OnDynamicTask_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		dynamicTaskResult = $"The dynamic task! {result.ReturnValue}";
	}
#endif

	public void AddToGroups()
	{
#if ENABLE_BEST_HTTP
		Call("addToGroups");
#endif
	}

	public void GetValue()
	{
#if ENABLE_BEST_HTTP
		Call("getValue", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			genericTaskResult = $"The value is {result.ReturnValue} after 5 seconds";
		});
#endif
	}

	public void TaskWithException()
	{
#if ENABLE_BEST_HTTP
		Call("taskWithException", null, delegate(Hub hub, ClientMessage msg, FailureMessage error)
		{
			taskWithExceptionResult = $"Error: {error.ErrorMessage}";
		});
#endif
	}

	public void GenericTaskWithException()
	{
#if ENABLE_BEST_HTTP
		Call("genericTaskWithException", null, delegate(Hub hub, ClientMessage msg, FailureMessage error)
		{
			genericTaskWithExceptionResult = $"Error: {error.ErrorMessage}";
		});
#endif
	}

	public void SynchronousException()
	{
#if ENABLE_BEST_HTTP
		Call("synchronousException", null, delegate(Hub hub, ClientMessage msg, FailureMessage error)
		{
			synchronousExceptionResult = $"Error: {error.ErrorMessage}";
		});
#endif
	}

	public void PassingDynamicComplex(object person)
	{
#if ENABLE_BEST_HTTP
		Call("passingDynamicComplex", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			invokingHubMethodWithDynamicResult = $"The person's age is {result.ReturnValue}";
		}, person);
#endif
	}

	public void SimpleArray(int[] array)
	{
#if ENABLE_BEST_HTTP
		Call("simpleArray", delegate
		{
			simpleArrayResult = "Simple array works!";
		}, array);
#endif
	}

	public void ComplexType(object person)
	{
#if ENABLE_BEST_HTTP
		Call("complexType", delegate
		{
			complexTypeResult = string.Format("Complex Type -> {0}", ((IHub)this).Connection.JsonEncoder.Encode(base.State["person"]));
		}, person);
#endif
	}

	public void ComplexArray(object[] complexArray)
	{
#if ENABLE_BEST_HTTP
		Call("ComplexArray", delegate
		{
			complexArrayResult = "Complex Array Works!";
		}, new object[1]
		{
			complexArray
		});
#endif
	}

	public void Overload()
	{
#if ENABLE_BEST_HTTP
		Call("Overload", OnVoidOverload_Done);
#endif
	}

#if ENABLE_BEST_HTTP
	private void OnVoidOverload_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		voidOverloadResult = "Void Overload called";
		Overload(101);
	}
#endif

	public void Overload(int number)
	{
#if ENABLE_BEST_HTTP
		Call("Overload", OnIntOverload_Done, number);
#endif
	}

#if ENABLE_BEST_HTTP
	private void OnIntOverload_Done(Hub hub, ClientMessage originalMessage, ResultMessage result)
	{
		intOverloadResult = $"Overload with return value called => {result.ReturnValue.ToString()}";
	}
#endif

	public void ReadStateValue()
	{
#if ENABLE_BEST_HTTP
		Call("readStateValue", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			readStateResult = $"Read some state! => {result.ReturnValue}";
		});
#endif
	}

	public void PlainTask()
	{
#if ENABLE_BEST_HTTP
		Call("plainTask", delegate
		{
			plainTaskResult = "Plain Task Result";
		});
#endif
	}

	public void GenericTaskWithContinueWith()
	{
#if ENABLE_BEST_HTTP
		Call("genericTaskWithContinueWith", delegate(Hub hub, ClientMessage msg, ResultMessage result)
		{
			genericTaskWithContinueWithResult = result.ReturnValue.ToString();
		});
#endif
	}

#if ENABLE_BEST_HTTP
	private void FromArbitraryCode(Hub hub, MethodCallMessage methodCall)
	{
		fromArbitraryCodeResult = (methodCall.Arguments[0] as string);
	}

	private void GroupAdded(Hub hub, MethodCallMessage methodCall)
	{
		if (!string.IsNullOrEmpty(groupAddedResult))
		{
			groupAddedResult = "Group Already Added!";
		}
		else
		{
			groupAddedResult = "Group Added!";
		}
	}

	private void Signal(Hub hub, MethodCallMessage methodCall)
	{
		dynamicTaskResult = $"The dynamic task! {methodCall.Arguments[0]}";
	}

	private void Invoke(Hub hub, MethodCallMessage methodCall)
	{
		invokeResults.Add(string.Format("{0} client state index -> {1}", methodCall.Arguments[0], base.State["index"]));
	}
#endif

	public void Draw()
	{
		GUILayout.Label("Arbitrary Code");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label($"Sending {fromArbitraryCodeResult} from arbitrary code without the hub itself!");
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Group Added");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(groupAddedResult);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Dynamic Task");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(dynamicTaskResult);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Report Progress");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.BeginVertical();
		GUILayout.Label(longRunningJobStatus);
		GUILayout.HorizontalSlider(longRunningJobProgress, 0f, 100f);
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Generic Task");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(genericTaskResult);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Task With Exception");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(taskWithExceptionResult);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Generic Task With Exception");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(genericTaskWithExceptionResult);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Synchronous Exception");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(synchronousExceptionResult);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Invoking hub method with dynamic");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(invokingHubMethodWithDynamicResult);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Simple Array");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(simpleArrayResult);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Complex Type");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(complexTypeResult);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Complex Array");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(complexArrayResult);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Overloads");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.BeginVertical();
		GUILayout.Label(voidOverloadResult);
		GUILayout.Label(intOverloadResult);
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Read State Value");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(readStateResult);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Plain Task");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(plainTaskResult);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Generic Task With ContinueWith");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label(genericTaskWithContinueWithResult);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Message Pump");
		GUILayout.BeginHorizontal();
		GUILayout.Space(20f);
#if ENABLE_BEST_HTTP
		invokeResults.Draw(Screen.width - 40, 270f);
#endif
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
	}
}
