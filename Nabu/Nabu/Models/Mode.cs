namespace Nabu.Models
{
	public class Mode
	{
		public int? Id { get; set; }

		public string Short { get; set; }

		public string Long { get; set; }

		public override string ToString() => Long;
	}
}