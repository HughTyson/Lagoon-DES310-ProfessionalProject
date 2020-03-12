using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleBasedDialog : RuleBasedSystem
{

    // Sorts added rules by applying greatest condition counts to the front
    public override void AddRule(Rule rule)
    {
        int index = rules.FindIndex(y => rule.RuleConditioners.Count >= y.RuleConditioners.Count);
        if (index < 0)
            index = 0;
        rules.Insert(index, rule);
    }

    List<Rule> arbiterChoiceRules = new List<Rule>();
    public override void Update()
    {
        int arbiterConditions = 0;
        arbiterChoiceRules.Clear();

        for (int i = 0; i < rules.Count; i++)
        {
            if (rules[i].RuleConditioners.Count < arbiterConditions)
                break;

            if (rules[i].IsRuleTrue())
            {
                arbiterConditions = rules[i].RuleConditioners.Count;
                arbiterChoiceRules.Add(rules[i]);
            }
        }

        if (arbiterConditions != 0)
        {
            arbiterChoiceRules[Random.Range(0, arbiterChoiceRules.Count - 1)].TriggerFunc();
        }
    }
}
