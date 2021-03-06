namespace UIWidgets.Styles
{
	using System;
	using UnityEngine;

	/// <summary>
	/// Style for the scroller.
	/// </summary>
	[Serializable]
	public class StyleScroller : IStyleDefaultValues
	{
		/// <summary>
		/// Style for the background.
		/// </summary>
		[SerializeField]
		public StyleImage Background;

		/// <summary>
		/// Style for the highlight.
		/// </summary>
		[SerializeField]
		public StyleImage Highlight;

		/// <summary>
		/// Style for the current date.
		/// </summary>
		[SerializeField]
		public StyleText Text;

#if UNITY_EDITOR
		/// <summary>
		/// Sets the default values.
		/// </summary>
		public void SetDefaultValues()
		{
			Background.SetDefaultValues();
			Highlight.SetDefaultValues();
			Text.SetDefaultValues();
		}
#endif
	}
}