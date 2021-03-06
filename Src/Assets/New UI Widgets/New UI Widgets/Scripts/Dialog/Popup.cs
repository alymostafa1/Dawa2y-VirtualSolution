namespace UIWidgets
{
	using System;
	using UIWidgets.Styles;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Popup.
	/// </summary>
	public class Popup : MonoBehaviour, ITemplatable, IStylable
	{
		[SerializeField]
		Text titleText;

		/// <summary>
		/// Gets or sets the text component.
		/// </summary>
		/// <value>The text.</value>
		public Text TitleText
		{
			get
			{
				return titleText;
			}

			set
			{
				titleText = value;
			}
		}

		[SerializeField]
		Text contentText;

		/// <summary>
		/// Gets or sets the text component.
		/// </summary>
		/// <value>The text.</value>
		public Text ContentText
		{
			get
			{
				return contentText;
			}

			set
			{
				contentText = value;
			}
		}

		[SerializeField]
		Image popupIcon;

		/// <summary>
		/// Gets or sets the icon component.
		/// </summary>
		/// <value>The icon.</value>
		public Image Icon
		{
			get
			{
				return popupIcon;
			}

			set
			{
				popupIcon = value;
			}
		}

		DialogInfoBase info;

		/// <summary>
		/// Gets the info component.
		/// </summary>
		/// <value>The info component.</value>
		public DialogInfoBase Info
		{
			get
			{
				if (info == null)
				{
					info = GetComponent<DialogInfoBase>();
				}

				return info;
			}
		}

		bool isTemplate = true;

		/// <summary>
		/// Gets a value indicating whether this instance is template.
		/// </summary>
		/// <value><c>true</c> if this instance is template; otherwise, <c>false</c>.</value>
		public bool IsTemplate
		{
			get
			{
				return isTemplate;
			}

			set
			{
				isTemplate = value;
			}
		}

		/// <summary>
		/// Gets the name of the template.
		/// </summary>
		/// <value>The name of the template.</value>
		public string TemplateName
		{
			get;
			set;
		}

		static Templates<Popup> templates;

		/// <summary>
		/// Popup templates.
		/// </summary>
		public static Templates<Popup> Templates
		{
			get
			{
				if (templates == null)
				{
					templates = new Templates<Popup>();
				}

				return templates;
			}

			set
			{
				templates = value;
			}
		}

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		protected virtual void Awake()
		{
			if (IsTemplate)
			{
				gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// This function is called when the MonoBehaviour will be destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			if (!IsTemplate)
			{
				templates = null;
				return;
			}

			// if FindTemplates never called than TemplateName == null
			if (TemplateName != null)
			{
				Templates.Delete(TemplateName);
			}
		}

		/// <summary>
		/// Return popup instance by the specified template name.
		/// </summary>
		/// <param name="templateName">Template name.</param>
		/// <returns>New popup instance.</returns>
		[Obsolete("Use Clone(templateName) instead.")]
		public static Popup Template(string templateName)
		{
			return Clone(templateName);
		}

		/// <summary>
		/// Return popup instance using current instance as template.
		/// </summary>
		/// <returns>New popup instance.</returns>
		[Obsolete("Use Clone() instead.")]
		public Popup Template()
		{
			return Clone();
		}

		/// <summary>
		/// Return popup instance by the specified template name.
		/// </summary>
		/// <param name="templateName">Template name.</param>
		/// <returns>New popup instance.</returns>
		public static Popup Clone(string templateName)
		{
			return Templates.Instance(templateName);
		}

		/// <summary>
		/// Return popup instance using current instance as template.
		/// </summary>
		/// <returns>New popup instance.</returns>
		public Popup Clone()
		{
			if ((TemplateName != null) && Templates.Exists(TemplateName))
			{
				// do nothing
			}
			else if (!Templates.Exists(gameObject.name))
			{
				Templates.Add(gameObject.name, this);
			}
			else if (Templates.Get(gameObject.name) != this)
			{
				Templates.Add(gameObject.name, this);
			}

			var id = gameObject.GetInstanceID().ToString();
			if (!Templates.Exists(id))
			{
				Templates.Add(id, this);
			}
			else if (Templates.Get(id) != this)
			{
				Templates.Add(id, this);
			}

			return Templates.Instance(id);
		}

		/// <summary>
		/// The modal key.
		/// </summary>
		protected int? ModalKey;

		/// <summary>
		/// Show popup.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="position">Position.</param>
		/// <param name="icon">Icon.</param>
		/// <param name="modal">If set to <c>true</c> modal.</param>
		/// <param name="modalSprite">Modal sprite.</param>
		/// <param name="modalColor">Modal color.</param>
		/// <param name="canvas">Canvas.</param>
		public virtual void Show(
			string title = null,
			string message = null,
			Vector3? position = null,
			Sprite icon = null,
			bool modal = false,
			Sprite modalSprite = null,
			Color? modalColor = null,
			Canvas canvas = null)
		{
			if (IsTemplate)
			{
				Debug.LogWarning("Use the template clone, not the template itself: PopupTemplate.Clone().Show(...), not PopupTemplate.Show(...)");
			}

			if (position == null)
			{
				position = new Vector3(0, 0, 0);
			}

			SetInfo(title, message, icon);

			var parent = (canvas != null) ? canvas.transform : Utilites.FindTopmostCanvas(gameObject.transform);
			if (parent != null)
			{
				transform.SetParent(parent, false);
			}

			if (modal)
			{
				ModalKey = ModalHelper.Open(this, modalSprite, modalColor, Close);
			}
			else
			{
				ModalKey = null;
			}

			transform.SetAsLastSibling();

			transform.localPosition = (Vector3)position;
			gameObject.SetActive(true);
		}

		/// <summary>
		/// Sets the info.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="icon">Icon.</param>
		public virtual void SetInfo(string title = null, string message = null, Sprite icon = null)
		{
			if (Info != null)
			{
				Info.SetInfo(title, message, icon);
			}
			else
			{
				if ((title != null) && (TitleText != null))
				{
					TitleText.text = title;
				}

				if ((message != null) && (ContentText != null))
				{
					ContentText.text = message;
				}

				if ((icon != null) && (Icon != null))
				{
					Icon.sprite = icon;
				}
			}
		}

		/// <summary>
		/// Close popup.
		/// </summary>
		public virtual void Close()
		{
			if (ModalKey != null)
			{
				ModalHelper.Close((int)ModalKey);
			}

			Return();
		}

		/// <summary>
		/// Return this instance to cache.
		/// </summary>
		protected virtual void Return()
		{
			Templates.ToCache(this);

			ResetParametres();
		}

		/// <summary>
		/// Resets the parametres.
		/// </summary>
		protected virtual void ResetParametres()
		{
			var template = Templates.Get(TemplateName);

			var title = template.TitleText != null ? template.TitleText.text : string.Empty;
			var content = template.ContentText != null ? template.ContentText.text : string.Empty;
			var icon = template.Icon != null ? template.Icon.sprite : null;

			SetInfo(title, content, icon);
		}

		#region IStylable implementation

		/// <summary>
		/// Set the specified style.
		/// </summary>
		/// <returns><c>true</c>, if style was set for children gameobjects, <c>false</c> otherwise.</returns>
		/// <param name="style">Style data.</param>
		public bool SetStyle(Style style)
		{
			style.Dialog.Background.ApplyTo(GetComponent<Image>());
			style.Dialog.Title.ApplyTo(TitleText);

			style.Dialog.ContentBackground.ApplyTo(transform.Find("Content"));
			style.Dialog.ContentText.ApplyTo(ContentText);

			style.Dialog.Delimiter.ApplyTo(transform.Find("Delimiter/Delimiter"));
			style.Dialog.Button.ApplyTo(transform.Find("Buttons/Close"));

			style.ButtonClose.Background.ApplyTo(transform.Find("Header/CloseButton"));

			return true;
		}
		#endregion
	}
}