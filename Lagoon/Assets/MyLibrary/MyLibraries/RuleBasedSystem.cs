using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
/// Simple RBS with no arbiter
/// </summary>
public class RuleBasedSystem
{
    protected List<Rule> rules = new List<Rule>();

    public virtual void AddRule(Rule rule)
    {
        rules.Add(rule);
    }

    public virtual void Update()
    {
        for (int i = 0; i < rules.Count; i++)
        {
            if (rules[i].IsRuleTrue())
            {
                rules[i].TriggerFunc();
                break;
            }
        }

    }

    public class Rule
    {
        Conditioners.BaseC[] conditioners;
        public System.Action TriggerFunc;

        public IReadOnlyList<Conditioners.BaseC> RuleConditioners
        {
            get { return System.Array.AsReadOnly<Conditioners.BaseC>(conditioners); }
        }

        public Rule(System.Action trigger_func, params Conditioners.BaseC[] conditioners_)
        {
            TriggerFunc = trigger_func;
            conditioners = conditioners_;
        }
        public bool IsRuleTrue()
        {
            for (int i = 0; i < conditioners.Length; i++)
            {
                if (!conditioners[i].IsConditionMet())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
