namespace Nabu.Models
{
	public class Unit
	{
		public Vocabulary Vocabulary { get; set; }

		public Mode Mode { get; set; }

		public string Lection { get; set; }

		public int VocabularyIndex { get; set; }
		public int ModeIndex { get; set; }
		public int LectionIndex { get; set; }
	}
}