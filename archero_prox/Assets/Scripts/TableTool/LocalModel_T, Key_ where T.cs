




using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace TableTool
{
	public abstract class LocalModel<T, Key> where T : LocalBean, new()
	{
		private IList<T> _BeanList = new List<T>();

		private Dictionary<Key, T> _BeanMap = new Dictionary<Key, T>();

		private List<Key> _BeanKeyList = new List<Key>();

		protected abstract string Filename
		{
			get;
		}

		protected string FullPath => "ExcelData/" + Filename;

		public LocalModel()
		{
			Initialise();
		}

		protected void Initialise()
		{
			ReadFromFile();
			ArrangeBeans();
		}

		protected abstract Key GetBeanKey(T bean);

		protected virtual void ArrangeBeans()
		{
			IEnumerator<T> enumerator = _BeanList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				T current = enumerator.Current;
				Key beanKey = GetBeanKey(current);
				_BeanMap[beanKey] = current;
				_BeanKeyList.Add(beanKey);
			}
		}

		public IList<T> GetAllBeans()
		{
			return _BeanList;
		}

		public T GetBeanById(Key key)
		{
			T value = (T)null;
			if (_BeanMap.TryGetValue(key, out value))
			{
				return value;
			}
			return (T)null;
		}

		public Dictionary<Key, T> GetBeanDic()
		{
			return _BeanMap;
		}

		public List<Key> GetBeanKeyList()
		{
			return _BeanKeyList;
		}

		protected T CreateBean()
		{
			return new T();
		}

		protected bool ReadFromFile()
		{
			bool flag = true;
			string text = Utils.FormatString("{0}.bytes", Filename);
			try
			{
				_BeanList.Clear();
				byte[] array = null;
				array = FileUtils.GetFileBytes("data/excel", text);
				if (array != null)
				{
					if (array.Length < 4)
					{
						flag = false;
					}
					else
					{
						int beanCount = GetBeanCount(array);
						int startPos = 4;
						for (int i = 0; i < beanCount; i++)
						{
							T item = CreateBean();
							startPos = item.readFromBytes(array, startPos);
							_BeanList.Add(item);
						}
					}
				}
				else
				{
					flag = false;
				}
			}
			catch
			{
				flag = false;
			}
			if (!flag)
			{
				PlayerPrefs.SetString(text, string.Empty);
				_BeanList.Clear();
				try
				{
					TextAsset textAsset = ResourceManager.Load<TextAsset>(FullPath);
					byte[] bytes = textAsset.bytes;
					if (bytes == null || bytes.Length < 4)
					{
						return false;
					}
					int beanCount2 = GetBeanCount(bytes);
					int startPos2 = 4;
					for (int j = 0; j < beanCount2; j++)
					{
						T item2 = CreateBean();
						startPos2 = item2.readFromBytes(bytes, startPos2);
						_BeanList.Add(item2);
					}
				}
				catch (Exception)
				{
					UnityEngine.Debug.LogError(Filename + " load error");
					return false;
				}
				finally
				{
				}
			}
			return true;
		}

		protected int GetBeanCount(byte[] raws)
		{
			int network = BitConverter.ToInt32(new byte[4]
			{
				raws[0],
				raws[1],
				raws[2],
				raws[3]
			}, 0);
			return IPAddress.NetworkToHostOrder(network);
		}

		public void DoNothing()
		{
		}
	}
}
