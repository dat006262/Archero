




namespace UnityEngine.UI
{
	public class HighLightMask : MaskableGraphic, ICanvasRaycastFilter
	{
		[SerializeField]
		private RectTransform _target;

		private Vector3 _targetMin = Vector3.zero;

		private Vector3 _targetMax = Vector3.zero;

		private bool _canRefresh = true;

		private Transform _cacheTrans;

		public void SetTarget(RectTransform target)
		{
			_canRefresh = true;
			_target = target;
			_RefreshView();
		}

		private void _SetTarget(Vector3 tarMin, Vector3 tarMax)
		{
			if (!(tarMin == _targetMin) || !(tarMax == _targetMax))
			{
				_targetMin = tarMin;
				_targetMax = tarMax;
				SetAllDirty();
			}
		}

		private void _RefreshView()
		{
			if (_canRefresh)
			{
				_canRefresh = false;
				if (null == _target)
				{
					_SetTarget(Vector3.zero, Vector3.zero);
					SetAllDirty();
				}
				else
				{
					Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(_cacheTrans, _target);
					_SetTarget(bounds.min, bounds.max);
				}
			}
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (_targetMin == Vector3.zero && _targetMax == Vector3.zero)
			{
				base.OnPopulateMesh(vh);
				return;
			}
			vh.Clear();
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = color;
			Vector2 pivot = base.rectTransform.pivot;
			Rect rect = base.rectTransform.rect;
			float x = (0f - pivot.x) * rect.width;
			float y = (0f - pivot.y) * rect.height;
			float x2 = (1f - pivot.x) * rect.width;
			float y2 = (1f - pivot.y) * rect.height;
			simpleVert.position = new Vector3(x, y2);
			vh.AddVert(simpleVert);
			simpleVert.position = new Vector3(x2, y2);
			vh.AddVert(simpleVert);
			simpleVert.position = new Vector3(x2, y);
			vh.AddVert(simpleVert);
			simpleVert.position = new Vector3(x, y);
			vh.AddVert(simpleVert);
			simpleVert.position = new Vector3(_targetMin.x, _targetMax.y);
			vh.AddVert(simpleVert);
			simpleVert.position = new Vector3(_targetMax.x, _targetMax.y);
			vh.AddVert(simpleVert);
			simpleVert.position = new Vector3(_targetMax.x, _targetMin.y);
			vh.AddVert(simpleVert);
			simpleVert.position = new Vector3(_targetMin.x, _targetMin.y);
			vh.AddVert(simpleVert);
			vh.AddTriangle(4, 0, 1);
			vh.AddTriangle(4, 1, 5);
			vh.AddTriangle(5, 1, 2);
			vh.AddTriangle(5, 2, 6);
			vh.AddTriangle(6, 2, 3);
			vh.AddTriangle(6, 3, 7);
			vh.AddTriangle(7, 3, 0);
			vh.AddTriangle(7, 0, 4);
		}

		bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 screenPos, Camera eventCamera)
		{
			if (null == _target)
			{
				return true;
			}
			return !RectTransformUtility.RectangleContainsScreenPoint(_target, screenPos, eventCamera);
		}

		protected override void Awake()
		{
			base.Awake();
			_cacheTrans = GetComponent<RectTransform>();
		}
	}
}
