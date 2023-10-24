
public interface IObstacle
{
  /// <summary>
  /// To deal damage to this obstacle
  /// </summary>
  /// <param name="weapon">Weapon which deal damage to this obstacle</param>
  void Damage(IWeapon weapon);
}
