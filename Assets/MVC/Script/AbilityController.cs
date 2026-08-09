using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
namespace Architecture.AbilitySystem
{
    public class AbilityController 
    {
       readonly AbilityModel abilityModel;
        readonly AbilityView abilityView;
        readonly Queue<AbilityCommand> abilityQueue = new Queue<AbilityCommand>();
         float timer = 1f; // Interval between command executions
         bool timerIsrunning = false;

        public AbilityController(AbilityModel abilityModel, AbilityView abilityView)
        {
            this.abilityModel = abilityModel;
            this.abilityView = abilityView;

            ConnectModel();
            ConnectView();
        }

        void ConnectModel()
        {
            abilityModel.Abilities.OnListChanged += UpdateButtons;
        }

        public void Update()
        {
            timer -= Time.deltaTime;
            abilityView.UpdateRadial(timer);
            if (timer <= 0)
            {
                timer = 1;
                timerIsrunning = false;
            }
            if (timer > 0 && timer < 1f) timerIsrunning = true;


            if (!timerIsrunning && abilityQueue.TryDequeue(out AbilityCommand cmd))
            {
                cmd.Execute();
                timer = cmd.duration;
                
                
            }
        }

        void ConnectView()
        {
            for(int i=0;i<abilityView.abilityButtons.Length;i++)
            {
                abilityView.abilityButtons[i].RegisterListenser(OnAbilityButtonPressed);
            }
            abilityView.UpdateButtonSprites(abilityModel.Abilities);
        }

        void OnAbilityButtonPressed(int index)
        {
            if(timer<0.25f || !timerIsrunning)
            {
                if (abilityModel.Abilities[index] != null)
                {
                  abilityQueue.Enqueue(abilityModel.Abilities[index].CreatAbilityCommand());
                }
            }
            Debug.Log("EventBus Send Event");
        }

        void UpdateButtons(IList<Ability> updatedAbilities) => abilityView.UpdateButtonSprites(updatedAbilities);


        public class Builder
        {
          readonly AbilityModel abilityModel = new AbilityModel();

            public Builder WithAbilities(AbilityData[] datas)
            {
                foreach (var data in datas)
                {
                    abilityModel.AddAbility(new Ability(data));
                }

                return this;
            }

            public AbilityController Build(AbilityView view)
            {
              if(view == null)
              {
                Debug.LogError("AbilityView is null. Please provide a valid AbilityView instance.");
                }
                return new AbilityController(abilityModel, view);
            }

        }

    }
}
