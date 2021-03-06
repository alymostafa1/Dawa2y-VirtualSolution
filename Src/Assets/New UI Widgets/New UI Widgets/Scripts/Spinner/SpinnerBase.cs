// <auto-generated/>
// Auto-generated added to suppress names errors.

namespace UIWidgets
{
	using System.Collections;
	using UIWidgets.Attributes;
	using UIWidgets.Styles;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.Serialization;
	using UnityEngine.UI;

	/// <summary>
	/// Spinner validation.
	/// </summary>
	public enum SpinnerValidation
	{
		/// <summary>
		/// On key down.
		/// </summary>
		OnKeyDown = 0,

		/// <summary>
		/// On end input.
		/// </summary>
		OnEndInput = 1,
	}

	/// <summary>
	/// Spinner base class.
	/// </summary>
	/// <typeparam name="T">Type of spinner value.</typeparam>
	[DataBindSupport]
	public abstract class SpinnerBase<T> : MonoBehaviour, IStylable
		where T : struct
	{
#pragma warning disable 0649
		[SerializeField]
		[HideInInspector]
		Text m_TextComponent;

		[SerializeField]
		[HideInInspector]
		Graphic m_Placeholder;

		[SerializeField]
		[HideInInspector]
		Graphic m_TargetGraphic;
#pragma warning restore 0649

		[FormerlySerializedAs("onSubmit")]
		[FormerlySerializedAs("m_OnSubmit")]
		[FormerlySerializedAs("m_EndEdit")]
		[SerializeField]
		[HideInInspector]
		InputField.SubmitEvent m_OnEndEdit = new InputField.SubmitEvent();

		[FormerlySerializedAs("onValueChange")]
		[FormerlySerializedAs("m_OnValueChange")]
		[SerializeField]
		[HideInInspector]
		private InputField.OnChangeEvent m_OnValueChanged = new InputField.OnChangeEvent();

		IInputFieldExtended inputField;

		/// <summary>
		/// InputField proxy.
		/// </summary>
		protected IInputFieldExtended InputField
		{
			get
			{
				if (inputField == null)
				{
					InitInputField();
				}

				return inputField;
			}
		}

		/// <summary>
		/// The min.
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("_min")]
		protected T ValueMin;

		/// <summary>
		/// The max.
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("_max")]
		protected T ValueMax;

		/// <summary>
		/// The step.
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("_step")]
		protected T ValueStep;

		/// <summary>
		/// The value.
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("_value")]
		protected T SpinnerValue;

		/// <summary>
		/// The validation type.
		/// </summary>
		[SerializeField]
		public SpinnerValidation Validation = SpinnerValidation.OnKeyDown;

		/// <summary>
		/// Allow changing value during hold.
		/// </summary>
		[SerializeField]
		public bool AllowHold = true;

		/// <summary>
		/// Delay of hold in seconds for permanent increase/descrease value.
		/// </summary>
		[SerializeField]
		public float HoldStartDelay = 0.5f;

		/// <summary>
		/// Delay of hold in seconds between increase/descrease value.
		/// </summary>
		[SerializeField]
		public float HoldChangeDelay = 0.1f;

		/// <summary>
		/// Gets or sets the minimum.
		/// </summary>
		/// <value>The minimum.</value>
		[DataBindField]
		public T Min
		{
			get
			{
				return ValueMin;
			}

			set
			{
				ValueMin = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum.
		/// </summary>
		/// <value>The maximum.</value>
		[DataBindField]
		public T Max
		{
			get
			{
				return ValueMax;
			}

			set
			{
				ValueMax = value;
			}
		}

		/// <summary>
		/// Gets or sets the step.
		/// </summary>
		/// <value>The step.</value>
		[DataBindField]
		public T Step
		{
			get
			{
				return ValueStep;
			}

			set
			{
				ValueStep = value;
			}
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		[DataBindField]
		public T Value
		{
			get
			{
				return SpinnerValue;
			}

			set
			{
				SetValue(value);
			}
		}

		/// <summary>
		/// The plus button.
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("_plusButton")]
		protected ButtonAdvanced plusButton;

		/// <summary>
		/// The minus button.
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("_minusButton")]
		protected ButtonAdvanced minusButton;

		/// <summary>
		/// Gets or sets the plus button.
		/// </summary>
		/// <value>The plus button.</value>
		public ButtonAdvanced PlusButton
		{
			get
			{
				return plusButton;
			}

			set
			{
				if (plusButton != null)
				{
					plusButton.onClick.RemoveListener(OnPlusClick);
					plusButton.onPointerDown.RemoveListener(OnPlusButtonDown);
					plusButton.onPointerUp.RemoveListener(OnPlusButtonUp);
				}

				plusButton = value;

				if (plusButton != null)
				{
					plusButton.onClick.AddListener(OnPlusClick);
					plusButton.onPointerDown.AddListener(OnPlusButtonDown);
					plusButton.onPointerUp.AddListener(OnPlusButtonUp);
				}
			}
		}

		/// <summary>
		/// Gets or sets the minus button.
		/// </summary>
		/// <value>The minus button.</value>
		public ButtonAdvanced MinusButton
		{
			get
			{
				return minusButton;
			}

			set
			{
				if (minusButton != null)
				{
					minusButton.onClick.RemoveListener(OnMinusClick);
					minusButton.onPointerDown.RemoveListener(OnMinusButtonDown);
					minusButton.onPointerUp.RemoveListener(OnMinusButtonUp);
				}

				minusButton = value;

				if (minusButton != null)
				{
					minusButton.onClick.AddListener(OnMinusClick);
					minusButton.onPointerDown.AddListener(OnMinusButtonDown);
					minusButton.onPointerUp.AddListener(OnMinusButtonUp);
				}
			}
		}

		/// <summary>
		/// onPlusClick event.
		/// </summary>
		public UnityEvent onPlusClick = new UnityEvent();

		/// <summary>
		/// onMinusClick event.
		/// </summary>
		public UnityEvent onMinusClick = new UnityEvent();

		/// <summary>
		/// Increase value on step.
		/// </summary>
		public abstract void Plus();

		/// <summary>
		/// Decrease value on step.
		/// </summary>
		public abstract void Minus();

		/// <summary>
		/// Sets the value.
		/// </summary>
		/// <param name="newValue">New value.</param>
		protected abstract void SetValue(T newValue);

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected virtual void Start()
		{
			Init();
		}

		bool isInited;

		/// <summary>
		/// Init InputField.
		/// </summary>
		protected void InitInputField()
		{
			inputField = Compatibility.GetComponent<IInputFieldExtended>(this);

			if (inputField == null)
			{
				var input = gameObject.AddComponent<InputFieldExtended>();
				input.textComponent = m_TextComponent;
				input.placeholder = m_Placeholder;
				input.targetGraphic = m_TargetGraphic;

				input.onEndEdit = m_OnEndEdit;
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
				input.onValueChanged = m_OnValueChanged;
#else
				input.onValueChange = m_OnValueChanged;
#endif

				inputField = input;
			}
		}

		/// <summary>
		/// Init.
		/// </summary>
		public void Init()
		{
			if (isInited)
			{
				return;
			}

			isInited = true;

			InitInputField();

			InputField.Validation = SpinnerValidate;
			InputField.ValueChanged = ValueChange;
			InputField.ValueEndEdit = ValueEndEdit;

			PlusButton = plusButton;
			MinusButton = minusButton;
			Value = SpinnerValue;

			SetTextValue();
		}

		/// <summary>
		/// Set text value.
		/// </summary>
		protected virtual void SetTextValue()
		{
			InputField.Value = SpinnerValue.ToString();
		}

		/// <summary>
		/// Hold Plus coroutine.
		/// </summary>
		/// <returns>IEnumerator.</returns>
		protected virtual IEnumerator HoldPlus()
		{
			if (AllowHold)
			{
				yield return new WaitForSeconds(HoldStartDelay);
				while (AllowHold)
				{
					Plus();
					yield return new WaitForSeconds(HoldChangeDelay);
				}
			}
		}

		/// <summary>
		/// Hold Minus coroutine.
		/// </summary>
		/// <returns>IEnumerator.</returns>
		protected virtual IEnumerator HoldMinus()
		{
			if (AllowHold)
			{
				yield return new WaitForSeconds(HoldStartDelay);
				while (AllowHold)
				{
					Minus();
					yield return new WaitForSeconds(HoldChangeDelay);
				}
			}
		}

		/// <summary>
		/// Raises the minus click event.
		/// </summary>
		public void OnMinusClick()
		{
			Minus();
			onMinusClick.Invoke();
		}

		/// <summary>
		/// Raises the plus click event.
		/// </summary>
		public void OnPlusClick()
		{
			Plus();
			onPlusClick.Invoke();
		}

		IEnumerator coroutine;

		/// <summary>
		/// Raises the plus button down event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public void OnPlusButtonDown(PointerEventData eventData)
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
			}

			coroutine = HoldPlus();
			StartCoroutine(coroutine);
		}

		/// <summary>
		/// Raises the plus button up event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public void OnPlusButtonUp(PointerEventData eventData)
		{
			StopCoroutine(coroutine);
			coroutine = null;
		}

		/// <summary>
		/// Raises the minus button down event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public void OnMinusButtonDown(PointerEventData eventData)
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
			}

			coroutine = HoldMinus();
			StartCoroutine(coroutine);
		}

		/// <summary>
		/// Raises the minus button up event.
		/// </summary>
		/// <param name="eventData">Current event data.</param>
		public void OnMinusButtonUp(PointerEventData eventData)
		{
			StopCoroutine(coroutine);
			coroutine = null;
		}

		/// <summary>
		/// Ons the destroy.
		/// </summary>
		protected virtual void onDestroy()
		{
			PlusButton = null;
			MinusButton = null;
		}

		/// <summary>
		/// Called when value changed.
		/// </summary>
		/// <param name="value">Value.</param>
		protected abstract void ValueChange(string value);

		/// <summary>
		/// Called when end edit.
		/// </summary>
		/// <param name="value">Value.</param>
		protected abstract void ValueEndEdit(string value);

		char SpinnerValidate(string validateText, int charIndex, char addedChar)
		{
			if (Validation == SpinnerValidation.OnEndInput)
			{
				return ValidateShort(validateText, charIndex, addedChar);
			}
			else
			{
				return ValidateFull(validateText, charIndex, addedChar);
			}
		}

		/// <summary>
		/// Validate when key down for Validation=OnEndInput.
		/// </summary>
		/// <returns>The char.</returns>
		/// <param name="validateText">Validate text.</param>
		/// <param name="charIndex">Char index.</param>
		/// <param name="addedChar">Added char.</param>
		protected abstract char ValidateShort(string validateText, int charIndex, char addedChar);

		/// <summary>
		/// Validates when key down for Validation=OnKeyDown.
		/// </summary>
		/// <returns>The char.</returns>
		/// <param name="validateText">Validate text.</param>
		/// <param name="charIndex">Char index.</param>
		/// <param name="addedChar">Added char.</param>
		protected abstract char ValidateFull(string validateText, int charIndex, char addedChar);

		/// <summary>
		/// Clamps a value between a minimum and maximum value.
		/// </summary>
		/// <returns>The bounds.</returns>
		/// <param name="value">Value.</param>
		protected abstract T InBounds(T value);

		#region IStylable implementation

		/// <summary>
		/// Set the specified style.
		/// </summary>
		/// <returns><c>true</c>, if style was set for children gameobjects, <c>false</c> otherwise.</returns>
		/// <param name="style">Style data.</param>
		public virtual bool SetStyle(Style style)
		{
			SetStyle(style.Spinner, style);

			return true;
		}

		/// <summary>
		/// Set the specified style for specified spinner.
		/// </summary>
		/// <param name="styleSpinner">Spinner style data.</param>
		/// <param name="style">Style data.</param>
		public virtual void SetStyle(StyleSpinner styleSpinner, Style style)
		{
			InputField.SetStyle(styleSpinner, style);

			styleSpinner.Background.ApplyTo(transform.parent);
			styleSpinner.InputBackground.ApplyTo(GetComponent<Image>());

			if (minusButton != null)
			{
				styleSpinner.ButtonMinus.ApplyTo(minusButton.gameObject);
			}

			if (plusButton != null)
			{
				styleSpinner.ButtonPlus.ApplyTo(plusButton.gameObject);
			}
		}
		#endregion

#if UNITY_EDITOR
		/// <summary>
		/// Validate instance.
		/// </summary>
		protected virtual void OnValidate()
		{
			InitInputField();
		}
#endif
	}
}