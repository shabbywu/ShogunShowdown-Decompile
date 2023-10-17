public class Hit
{
	public int Damage { get; set; }

	public bool IsDirectional { get; set; }

	public bool IsCollision { get; set; }

	public string Description { get; set; }

	public Hit(int damage, bool isDirectional, bool isCollision = false, string description = "")
	{
		Damage = damage;
		IsDirectional = isDirectional;
		IsCollision = isCollision;
		Description = description;
	}

	public Hit Clone()
	{
		return new Hit(Damage, IsDirectional, IsCollision, Description);
	}
}
