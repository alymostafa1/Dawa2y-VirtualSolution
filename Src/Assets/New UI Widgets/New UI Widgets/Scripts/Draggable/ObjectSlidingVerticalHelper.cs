﻿namespace UIWidgets
{
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Helper for ObjectSliding.
	/// Set allowed positions to current, current + height(ObjectsOnLeft), current - height(ObjectsOnRight)
	/// </summary>
	[RequireComponent(typeof(ObjectSliding))]
	public class ObjectSlidingVerticalHelper : MonoBehaviour
	{
		/// <summary>
		/// Objects on top.
		/// </summary>
		[SerializeField]
		protected List<RectTransform> ObjectsOnTop = new List<RectTransform>();

		/// <summary>
		/// Objects on bottom.
		/// </summary>
		[SerializeField]
		protected List<RectTransform> ObjectsOnBottom = new List<RectTransform>();

		/// <summary>
		/// Current ObjectSliding.
		/// </summary>
		[HideInInspector]
		protected ObjectSliding Sliding;

		/// <summary>
		/// Start this instance.
		/// </summary>
		public virtual void Start()
		{
			Init();
		}

		bool isInited;

		/// <summary>
		/// Adds listeners and calculate positions.
		/// </summary>
		public virtual void Init()
		{
			if (isInited)
			{
				return;
			}

			isInited = true;

			Sliding = GetComponent<ObjectSliding>();

			Sliding.Direction = ObjectSlidingDirection.Vertical;

			AddListeners();

			CalculatePositions();
		}

		/// <summary>
		/// Adds listener.
		/// </summary>
		/// <param name="rect">RectTransform</param>
		protected virtual void AddListener(RectTransform rect)
		{
			var rl = Utilites.GetOrAddComponent<ResizeListener>(rect);
			rl.OnResize.AddListener(CalculatePositions);
		}

		/// <summary>
		/// Adds listeners.
		/// </summary>
		protected virtual void AddListeners()
		{
			ObjectsOnTop.ForEach(AddListener);
			ObjectsOnBottom.ForEach(AddListener);
		}

		/// <summary>
		/// Remove listener.
		/// </summary>
		/// <param name="rect">RectTransform.</param>
		protected virtual void RemoveListener(RectTransform rect)
		{
			var rl = rect.GetComponent<ResizeListener>();
			if (rl != null)
			{
				rl.OnResize.RemoveListener(CalculatePositions);
			}
		}

		/// <summary>
		/// Remove listener.
		/// </summary>
		protected virtual void RemoveListeners()
		{
			ObjectsOnTop.ForEach(RemoveListener);
			ObjectsOnBottom.ForEach(RemoveListener);
		}

		/// <summary>
		/// Get summary height.
		/// </summary>
		/// <param name="list">Items list.</param>
		/// <returns>Summary height.</returns>
		protected static float SumHeight(List<RectTransform> list)
		{
			var result = 0f;

			for (int i = 0; i < list.Count; i++)
			{
				result += list[i].rect.height;
			}

			return result;
		}

		/// <summary>
		/// Calculate positions.
		/// </summary>
		protected virtual void CalculatePositions()
		{
			var pos = (Sliding.transform as RectTransform).anchoredPosition.y;
			var top = pos - SumHeight(ObjectsOnTop);
			var bottom = pos + SumHeight(ObjectsOnBottom);

			Sliding.Positions.Clear();
			Sliding.Positions.Add(pos);
			Sliding.Positions.Add(top);
			Sliding.Positions.Add(bottom);
		}

		/// <summary>
		/// Remove listeners on destroy.
		/// </summary>
		protected virtual void OnDestroy()
		{
			RemoveListeners();
		}
	}
}