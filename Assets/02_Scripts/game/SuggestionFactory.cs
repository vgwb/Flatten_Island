using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SuggestionFactory
{
    List<Suggestion> catalog;

	public SuggestionFactory()
	{
        catalog = GetSuggestionsCatalog();
	}

	public Suggestion[] CreateNextSuggestions(Suggestion last, LocalPlayer state)
	{
        // Creates 3 suggestions from 3 advisors, different from the last one picked
        Suggestion[] next = new Suggestion[3];
        next[0] = GetValidSuggestion(last, next, state);
        next[1] = GetValidSuggestion(last, next, state);
        next[2] = GetValidSuggestion(last, next, state);
        return next;
    }

    private Suggestion GetValidSuggestion(Suggestion last, Suggestion[] next, LocalPlayer state)
    {
        // Keeps randomising until the options are compatible with the last and with the state
        // TODO review, but this shouldn't be too slow or an infinite loop 
        Suggestion suggestion;
        do {
            suggestion = catalog[RandomGenerator.GetRandom(0, catalog.Count)];
        } while (
            !IsApplicable(suggestion, state) || 
            !AreCompatible(suggestion, last, next)
        );
        return suggestion;
    }

    private bool IsApplicable(Suggestion next, LocalPlayer state)
    {
        // TODO. Validate the suggestion is within boundaries (e.g. not reducing the patients under 0, money under 0, etc)
        return true; 
    }

    private bool AreCompatible(Suggestion next, Suggestion last, Suggestion[] nextSuggestions)
    {
        bool isCompatible = last == null ? true : IsCompatible(next, last);
 
        // we want 3 compatible alternatives from 3 advisors
        for (int i=0; i< 3; i++)
        {
            if (nextSuggestions[i] != null)
            {
                isCompatible = isCompatible && IsCompatible(next, nextSuggestions[i]);
            }
        }
        return isCompatible;
    }

    private bool IsCompatible(Suggestion next, Suggestion other)
    {
        return next.advisor != other.advisor;
    }

    private List<Suggestion> GetSuggestionsCatalog()
    {
        // TODO load from internet or from local XML or just a factory
        var catalog = new List<Suggestion>()
        {
            new Suggestion()
            {
                advisor=Advisor.PR,
                title="Hula hoops!",
                description="People are freaking out and they are queuing for hours in front of supermarkets without respecting the safe distance. Let’s purchase millions of hula hoops so people can exercise while maintaining the correct distance in line.",
                moneyModifier=-1000,
                growthRateModifier=0,
                patientsModifier=0,
                capacityModifier=0,
                publicOpinionModifier=0.1f
            },
            new Suggestion()
            {
                advisor=Advisor.Treasurer,
                title="Lottery masks",
                description="Everybody is looking for face masks but it’s impossible to find them anywhere. We should organize a lottery with thousands of face masks as the main prize. Obviously, we will have to take them from the hospitals stock.",
                moneyModifier=1000,
                growthRateModifier=0,
                patientsModifier=0,
                capacityModifier=-1,
                publicOpinionModifier=0.1f
            },
            new Suggestion()
            {
                advisor=Advisor.HospitalManager,
                title="More beds",
                description="We need more beds on out hospitals. People is starting to smell each other when sleeping together",
                moneyModifier=-1000,
                growthRateModifier=0,
                patientsModifier=0,
                capacityModifier=1,
                publicOpinionModifier=0.1f
            },
            new Suggestion()
            {
                advisor=Advisor.ExpertDoctor,
                title="Everybody home",
                description="We can instantaneously send 2000 people home by buying them home robots",
                moneyModifier=-2000,
                growthRateModifier=0,
                patientsModifier=-2000,
                capacityModifier=0,
                publicOpinionModifier=-0.1f
            },
            new Suggestion()
            {
                advisor=Advisor.Scientist,
                title="New inmunity discovery",
                description="We need to apply a new technique to make people inmune in the streets",
                moneyModifier=2000,
                growthRateModifier=-2,
                patientsModifier=0,
                capacityModifier=1,
                publicOpinionModifier=-0.1f
            }
        };
        return catalog;
    }
}
