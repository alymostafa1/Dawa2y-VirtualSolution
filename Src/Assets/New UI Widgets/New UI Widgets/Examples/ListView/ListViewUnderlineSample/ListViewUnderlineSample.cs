namespace UIWidgets.Examples
{
	using System;
	using UIWidgets;

	/// <summary>
	/// ListViewUnderline sample.
	/// </summary>
	public class ListViewUnderlineSample : ListViewCustom<ListViewUnderlineSampleComponent, ListViewUnderlineSampleItemDescription>
	{
		Comparison<ListViewUnderlineSampleItemDescription> itemsComparison = (x, y) => x.Name.CompareTo(y.Name);

		/// <summary>
		/// Set items comparison.
		/// </summary>
		public override void Init()
		{
			base.Init();
			DataSource.Comparison = itemsComparison;
		}
	}
}