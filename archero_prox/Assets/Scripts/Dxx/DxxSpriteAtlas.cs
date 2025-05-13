




using System.Collections.Generic;
using UnityEngine;

namespace Dxx
{
	[CreateAssetMenu]
	public class DxxSpriteAtlas : ScriptableObject
	{
		public string tag;

		public List<Sprite> sprites = new List<Sprite>();

		private Dictionary<string, int> nameToIndex = new Dictionary<string, int>();

		private void Initialize()
		{
			if (nameToIndex.Count == 0)
			{
				nameToIndex.Clear();
				for (int i = 0; i < sprites.Count; i++)
				{
					nameToIndex.Add(sprites[i].name.ToLower(), i);
				}
			}
		}

		public Sprite GetSprite(string spriteName)
		{
			Initialize();
			spriteName = spriteName.ToLower();
			if (nameToIndex.ContainsKey(spriteName))
			{
				return sprites[nameToIndex[spriteName]];
			}
			return null;
		}
	}
}
