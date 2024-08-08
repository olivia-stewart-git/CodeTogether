namespace CodeTogether.Data.Seeding;

public interface ISeedStep
{
	public int Order { get; }
	public void Seed();
}