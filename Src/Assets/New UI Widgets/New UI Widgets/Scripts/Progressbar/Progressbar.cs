namespace UIWidgets
{
	using System;
	using System.Collections;
	using UIWidgets.Attributes;
	using UnityEngine;
	using UnityEngine.Serialization;
	using UnityEngine.UI;

	/// <summary>
	/// Progressbar.
	/// http://ilih.ru/images/unity-assets/UIWidgets/ProgressbarDeterminate.png
	/// http://ilih.ru/images/unity-assets/UIWidgets/ProgressbarIndeterminate.png
	/// <example>
	/// // Example of using Progressbar for show instantiating progress.
	/// <code>
	/// var bar = GetComponent&lt;Progressbar&gt;();
	/// bar.Max = 10000;
	/// for (int i = 0; i &lt; 10000; i++)
	/// {
	/// 	Instantiate(prefab);
	/// 	if (i % 100)
	/// 	{
	/// 		bar.Value = i;
	/// 		yield return null;//wait 1 frame
	/// 	}
	/// }
	/// </code>
	/// </example>
	/// </summary>
	[DataBindSupport]
	public class Progressbar : MonoBehaviour
	{
		/// <summary>
		/// Max value of progress.
		/// </summary>
		[SerializeField]
		[DataBindField]
		public int Max = 100;

		[SerializeField]
		[FormerlySerializedAs("_value")]
		int progressValue;

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		[DataBindField]
		public int Value
		{
			get
			{
				return progressValue;
			}

			set
			{
				if (value > Max)
				{
					value = Max;
				}

				progressValue = value;
				UpdateProgressbar();
			}
		}

		[SerializeField]
		ProgressbarDirection Direction = ProgressbarDirection.Horizontal;

		[SerializeField]
		ProgressbarTypes type = ProgressbarTypes.Determinate;

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		public ProgressbarTypes Type
		{
			get
			{
				return type;
			}

			set
			{
				type = value;
				ToggleType();
			}
		}

		/// <summary>
		/// The indeterminate bar.
		/// Use texture type "texture" and set wrap mode = repeat;
		/// </summary>
		[SerializeField]
		public RawImage IndeterminateBar;

		/// <summary>
		/// The determinate bar.
		/// </summary>
		[SerializeField]
		public GameObject DeterminateBar;

		/// <summary>
		/// The empty bar.
		/// </summary>
		[SerializeField]
		public Image EmptyBar;

		/// <summary>
		/// The empty bar text.
		/// </summary>
		[SerializeField]
		public Text EmptyBarText;

		[SerializeField]
		Image fullBar;

		/// <summary>
		/// Gets or sets the full bar.
		/// </summary>
		/// <value>The full bar.</value>
		public Image FullBar
		{
			get
			{
				return fullBar;
			}

			set
			{
				fullBar = value;
			}
		}

		RectTransform FullBarRectTransform
		{
			get
			{
				return fullBar.transform as RectTransform;
			}
		}

		/// <summary>
		/// The full bar text.
		/// </summary>
		[SerializeField]
		public Text FullBarText;

		/// <summary>
		/// The bar mask.
		/// </summary>
		[SerializeField]
		public RectTransform BarMask;

		[SerializeField]
		ProgressbarTextTypes textType = ProgressbarTextTypes.None;

		/// <summary>
		/// Gets or sets the type of the text.
		/// </summary>
		/// <value>The type of the text.</value>
		[SerializeField]
		public ProgressbarTextTypes TextType
		{
			get
			{
				return textType;
			}

			set
			{
				textType = value;
				ToggleTextType();
			}
		}

		/// <summary>
		/// The speed.
		/// For Determinate depends of SpeedType.
		/// For Indeterminate speed of changing uvRect coordinates.
		/// </summary>
		[SerializeField]
		public float Speed = 0.1f;

		/// <summary>
		/// The type of the speed.
		/// </summary>
		[SerializeField]
		public ProgressbarSpeedType SpeedType = ProgressbarSpeedType.ConstantSpeed;

		/// <summary>
		/// The unscaled time.
		/// </summary>
		[SerializeField]
		public bool UnscaledTime = false;

		Func<Progressbar, string> textFunc = TextPercent;

		/// <summary>
		/// Gets or sets the text function.
		/// </summary>
		/// <value>The text function.</value>
		public Func<Progressbar, string> TextFunc
		{
			get
			{
				return textFunc;
			}

			set
			{
				textFunc = value;
				UpdateText();
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is animation run.
		/// </summary>
		/// <value><c>true</c> if this instance is animation run; otherwise, <c>false</c>.</value>
		public bool IsAnimationRun
		{
			get;
			protected set;
		}

		/// <summary>
		/// Don't show progress text.
		/// </summary>
		/// <returns>string.Empty</returns>
		/// <param name="bar">Progressbar.</param>
		public static string TextNone(Progressbar bar)
		{
			return string.Empty;
		}

		/// <summary>
		/// Show progress with percent.
		/// </summary>
		/// <returns>string.Empty</returns>
		/// <param name="bar">Progressbar.</param>
		public static string TextPercent(Progressbar bar)
		{
			return string.Format("{0:P0}", (float)bar.Value / bar.Max);
		}

		/// <summary>
		/// Show progress with range.
		/// </summary>
		/// <returns>The range.</returns>
		/// <param name="bar">Progressbar.</param>
		public static string TextRange(Progressbar bar)
		{
			return string.Format("{0} / {1}", bar.Value, bar.Max);
		}

		IEnumerator currentAnimation;

		/// <summary>
		/// Animate the progressbar to specified targetValue.
		/// </summary>
		/// <param name="targetValue">Target value.</param>
		public void Animate(int? targetValue = null)
		{
			if (currentAnimation != null)
			{
				StopCoroutine(currentAnimation);
			}

			if (Type == ProgressbarTypes.Indeterminate)
			{
				currentAnimation = IndeterminateAnimation();
			}
			else
			{
				if (SpeedType == ProgressbarSpeedType.TimeToValueChangedOnOne)
				{
					currentAnimation = AnimationPerOne(targetValue ?? Max);
				}
				else if (SpeedType == ProgressbarSpeedType.ConstantSpeed)
				{
					currentAnimation = AnimationConstantSpeed(targetValue ?? Max);
				}
				else if (SpeedType == ProgressbarSpeedType.ConstantTime)
				{
					currentAnimation = AnimationConstantTime(targetValue ?? Max);
				}
			}

			IsAnimationRun = true;
			StartCoroutine(currentAnimation);
		}

		/// <summary>
		/// Stop animation.
		/// </summary>
		public void Stop()
		{
			if (IsAnimationRun)
			{
				StopCoroutine(currentAnimation);
				IsAnimationRun = false;
			}
		}

		IEnumerator AnimationPerOne(int targetValue)
		{
			if (targetValue > Max)
			{
				targetValue = Max;
			}

			var delta = targetValue - Value;

			if (delta != 0)
			{
				while (true)
				{
					if (delta > 0)
					{
						progressValue += 1;
					}
					else
					{
						progressValue -= 1;
					}

					UpdateProgressbar();
					if (progressValue == targetValue)
					{
						break;
					}

					yield return new WaitForSeconds(Speed);
				}
			}

			IsAnimationRun = false;
		}

		/// <summary>
		/// Gets the time.
		/// </summary>
		/// <returns>The time.</returns>
		[Obsolete("Use Utilites.GetTime(UnscaledTime).")]
		protected virtual float GetTime()
		{
			return Utilites.GetTime(UnscaledTime);
		}

		IEnumerator AnimationConstantSpeed(int targetValue)
		{
			if (targetValue > Max)
			{
				targetValue = Max;
			}

			var start = Value;
			var delta = targetValue - start;

			if (delta != 0)
			{
				var step = Speed / Max;
				var total_time = Mathf.Abs(step * delta);

				var start_time = Utilites.GetTime(UnscaledTime);
				var end_time = start_time + total_time;

				do
				{
					var progress = Mathf.Min(1, (Utilites.GetTime(UnscaledTime) - start_time) / total_time);

					progressValue = start + Mathf.RoundToInt(progress * delta);

					UpdateProgressbar();

					yield return null;
				}
				while (end_time > Utilites.GetTime(UnscaledTime));

				progressValue = targetValue;
				UpdateProgressbar();
			}

			IsAnimationRun = false;
		}

		IEnumerator AnimationConstantTime(int targetValue)
		{
			if (targetValue > Max)
			{
				targetValue = Max;
			}

			var start = Value;
			var delta = targetValue - start;

			if (delta != 0)
			{
				var total_time = Speed;

				var start_time = Utilites.GetTime(UnscaledTime);
				var end_time = start_time + total_time;

				do
				{
					var progress = Mathf.Min(1, (Utilites.GetTime(UnscaledTime) - start_time) / total_time);

					progressValue = start + Mathf.RoundToInt(progress * delta);

					UpdateProgressbar();

					yield return null;
				}
				while (end_time > Utilites.GetTime(UnscaledTime));

				progressValue = targetValue;
				UpdateProgressbar();
			}

			IsAnimationRun = false;
		}

		IEnumerator IndeterminateAnimation()
		{
			while (true)
			{
				var r = IndeterminateBar.uvRect;
				if (Direction == ProgressbarDirection.Horizontal)
				{
					r.x = (Utilites.GetTime(UnscaledTime) * Speed) % 1;
				}
				else
				{
					r.y = (Utilites.GetTime(UnscaledTime) * Speed) % 1;
				}

				IndeterminateBar.uvRect = r;
				yield return null;
			}
		}

		/// <summary>
		/// Update progressbar.
		/// </summary>
		public void Refresh()
		{
			FullBar = fullBar;
			ToggleType();
			ToggleTextType();
			UpdateProgressbar();
		}

		void UpdateProgressbar()
		{
			if ((BarMask != null) && (FullBar != null))
			{
				var range = Value / (float)Max;

				BarMask.sizeDelta = (Direction == ProgressbarDirection.Horizontal)
					? new Vector2(FullBarRectTransform.rect.width * range, BarMask.sizeDelta.y)
					: new Vector2(BarMask.sizeDelta.x, FullBarRectTransform.rect.height * range);
			}

			UpdateText();
		}

		/// <summary>
		/// Updates the text.
		/// </summary>
		protected virtual void UpdateText()
		{
			var text = textFunc(this);
			if (FullBarText != null)
			{
				FullBarText.text = text;
			}

			if (EmptyBarText != null)
			{
				EmptyBarText.text = text;
			}
		}

		void ToggleType()
		{
			bool is_deterimate = type == ProgressbarTypes.Determinate;

			if (DeterminateBar != null)
			{
				DeterminateBar.gameObject.SetActive(is_deterimate);
			}

			if (IndeterminateBar != null)
			{
				IndeterminateBar.gameObject.SetActive(!is_deterimate);
			}
		}

		void ToggleTextType()
		{
			if (TextType == ProgressbarTextTypes.None)
			{
				textFunc = TextNone;
				return;
			}

			if (TextType == ProgressbarTextTypes.Percent)
			{
				textFunc = TextPercent;
				return;
			}

			if (TextType == ProgressbarTextTypes.Range)
			{
				textFunc = TextRange;
				return;
			}
		}
	}
}