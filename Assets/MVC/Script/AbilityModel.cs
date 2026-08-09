namespace Architecture.AbilitySystem
{
    public class AbilityModel
    {
        public readonly ObservList<Ability> Abilities = new ObservList<Ability>();

        public void AddAbility(Ability ability)
        {
            Abilities.Add(ability);
        }
    }


    public class Ability
    {
        public readonly AbilityData data;

        public Ability(AbilityData data)
        {
            this.data = data;
        }

        public AbilityCommand CreatAbilityCommand()
        {
            return new AbilityCommand(data);
        }
    }
    
        
    
}
