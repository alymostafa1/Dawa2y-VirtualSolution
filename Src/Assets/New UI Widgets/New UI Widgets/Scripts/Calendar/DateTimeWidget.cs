namespace UIWidgets
{
	using System;
	using System.Globalization;
	using UIWidgets.Attributes;
	using UIWidgets.Styles;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// DateTime widget.
	/// </summary>
	public class DateTimeWidget : DateTimeWidgetBase, IStylable
	{
		/// <summary>
		/// The calendar widget.
		/// </summary>
		[SerializeField]
		protected DateBase calendar;

		/// <summary>
		/// The calendar widget.
		/// </summary>
		/// <value>The calendar.</value>
		public DateBase Calendar
		{
			get
			{
				return calendar;
			}

			set
			{
				RemoveListeners();
				calendar = value;
				AddListeners();
			}
		}

		/// <summary>
		/// The time widget.
		/// </summary>
		[SerializeField]
		protected TimeBase time;

		/// <summary>
		/// The time widget.
		/// </summary>
		/// <value>The time.</value>
		public TimeBase Time
		{
			get
			{
				return time;
			}

			set
			{
				RemoveListeners();
				time = value;
				AddListeners();
			}
		}

		/// <summary>
		/// Culture to parse date.
		/// </summary>
		public override CultureInfo Culture
		{
			get
			{
				return calendar.Culture;
			}

			set
			{
				calendar.Culture = value;
			}
		}

		/// <summary>
		/// Is used widgets scroller?
		/// </summary>
		public bool IsScroller = false;

		/// <summary>
		/// Updates the widgets.
		/// </summary>
		protected override void UpdateWidgets()
		{
			calendar.Date = dateTime;
			time.Time = dateTime.TimeOfDay;
		}

		/// <summary>
		/// Adds the listeners.
		/// </summary>
		protected override void AddListeners()
		{
			calendar.OnDateChanged.AddListener(DateChanged);
			time.OnTimeChanged.AddListener(TimeChanged);
		}

		/// <summary>
		/// Removes the listeners.
		/// </summary>
		protected override void RemoveListeners()
		{
			calendar.OnDateChanged.RemoveListener(DateChanged);
			time.OnTimeChanged.RemoveListener(TimeChanged);
		}

		/// <summary>
		/// Process changed date.
		/// </summary>
		/// <param name="d">Date.</param>
		protected virtual void DateChanged(DateTime d)
		{
			var dt = d;
			dt += time.Time - dt.TimeOfDay;
			DateTime = dt;
		}

		/// <summary>
		/// Process changed time.
		/// </summary>
		/// <param name="t">Time.</param>
		protected virtual void TimeChanged(TimeSpan t)
		{
			var dt = calendar.Date;
			dt += t - dt.TimeOfDay;
			DateTime = dt;
		}

		#region IStylable implementation

		/// <summary>
		/// Set the specified style.
		/// </summary>
		/// <returns><c>true</c>, if style was set for children gameobjects, <c>false</c> otherwise.</returns>
		/// <param name="style">Style data.</param>
		public bool SetStyle(Style style)
		{
			if (IsScroller)
			{
				Calendar.SetStyle(style);
				Time.SetStyle(style);

				style.Scroller.Background.ApplyTo(GetComponent<Image>());

				return true;
			}

			return false;
		}
		#endregion
	}
}