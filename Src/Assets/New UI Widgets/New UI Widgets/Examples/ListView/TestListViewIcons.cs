namespace UIWidgets.Examples
{
	using UIWidgets;
	using UnityEngine;
	using UnityEngine.Serialization;

	/// <summary>
	/// Test ListViewIcons.
	/// </summary>
	public class TestListViewIcons : MonoBehaviour
	{
		/// <summary>
		/// ListViewIcons.
		/// </summary>
		[SerializeField]
		protected ListViewIcons ListView;

		/// <summary>
		/// Notification template.
		/// </summary>
		[SerializeField]
		[FormerlySerializedAs("notifySimple")]
		protected Notify NotifyTemplate;

		/// <summary>
		/// Adds listeners.
		/// </summary>
		public void Start()
		{
			ListView.OnSelectObject.AddListener(Notification);
		}

		/// <summary>
		/// Remove listeners.
		/// </summary>
		public void OnDestroy()
		{
			ListView.OnSelectObject.RemoveListener(Notification);
		}

		/// <summary>
		/// Show notification for specified item index.
		/// </summary>
		/// <param name="index">Item index.</param>
		public void Notification(int index)
		{
			if (NotifyTemplate == null)
			{
				return;
			}

			var message = ListView.SelectedIndex == -1
				? "Nothing selected"
				: "Selected: " + ListView.SelectedItem.Name;

			NotifyTemplate.Clone().Show(
				message,
				customHideDelay: 5f);
		}

		/// <summary>
		/// Original DefaultItem.
		/// </summary>
		[SerializeField]
		protected ListViewIconsItemComponent DefaultItemOriginal;

		/// <summary>
		/// New DefaultItem.
		/// </summary>
		[SerializeField]
		protected ListViewIconsItemComponent DefaultItemNew;

		/// <summary>
		/// New DefaultItem prefab.
		/// </summary>
		[SerializeField]
		protected ListViewIconsItemComponent PrefabDefaultItemNew;

		/// <summary>
		/// Set original default item component.
		/// </summary>
		public void SetOriginal()
		{
			ListView.DefaultItem = DefaultItemOriginal;
		}

		/// <summary>
		/// Set new default item component.
		/// </summary>
		public void SetNew()
		{
			ListView.DefaultItem = DefaultItemNew;
		}

		/// <summary>
		/// Set prefab as new default item component.
		/// </summary>
		public void SetNewPrefab()
		{
			if (PrefabDefaultItemNew == null)
			{
				return;
			}

#if UNITY_2018_1_OR_NEWER
			var new_default_item = Instantiate(PrefabDefaultItemNew, ListView.Container, false);
#else
			var new_default_item = Instantiate(PrefabDefaultItemNew) as ListViewIconsItemComponent;
			new_default_item.transform.SetParent(ListView.Container, false);
#endif
			new_default_item.gameObject.SetActive(false);
			ListView.DefaultItem = new_default_item;
		}
	}
}