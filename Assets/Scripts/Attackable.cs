public interface Attackable {
    // void TakeDamage(HeroController _hero, AttackType _type);
    void TakeDamage(HeroController _hero, float _amount, AttackType _type);
    HeroController GetHeroController();
}
