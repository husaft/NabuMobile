namespace Nabu.Models
{
	public class Vocabulary
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public Mapping Mapping { get; set; }

		public Source Src { get; set; }

		public Mode[] Modes { get; set; }
	}
}