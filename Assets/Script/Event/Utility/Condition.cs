using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Condition {
    public int AuthorityUpperThreshold=0;
    public int AuthorityLowerThreshold = 0;
    public int MentalHealthUpperThreshold = 0;
    public int MentalHealthLowerThreshold = 0;
    public int PhysicalHealthUpperThreshold = 0;
    public int PhysicalHealthLowerThreshold = 0;
    
    public bool FullCheck()
    {
        if (AuthorityCheck())
        {
            SurvivorManager s = SurvivorManager.GetInstance();
            foreach( Survivor survivor in s.Survivors )
            {
                if (MentalHealthCheck(survivor) && PhysicalHealthCheck(survivor))
                    return true;
            }
        }
        return false;
    }

    public bool AuthorityCheck()
    {
        Village v = Village.GetInstance();
        if (AuthorityUpperThreshold == 0 && AuthorityLowerThreshold == 0)
            return true;

        if (AuthorityUpperThreshold > 0 && v.Authority > AuthorityUpperThreshold)
            return true;

        if (AuthorityLowerThreshold > 0 && v.Authority < AuthorityLowerThreshold)
            return true;

        return false;
    }

    public bool MentalHealthCheck(Survivor s)
    {
        if (MentalHealthUpperThreshold == 0 && MentalHealthLowerThreshold == 0)
            return true;

        if (MentalHealthUpperThreshold > 0 && s.MentalHealth > MentalHealthUpperThreshold)
            return true;

        if (MentalHealthLowerThreshold > 0 && s.MentalHealth < MentalHealthLowerThreshold)
            return true;

        return false;
    }

    public bool PhysicalHealthCheck(Survivor s)
    {
        if (PhysicalHealthUpperThreshold == 0 && PhysicalHealthLowerThreshold == 0)
            return true;

        if (PhysicalHealthUpperThreshold > 0 && s.PhysicalHealth > PhysicalHealthUpperThreshold)
            return true;

        if (PhysicalHealthLowerThreshold > 0 && s.PhysicalHealth < PhysicalHealthLowerThreshold)
            return true;

        return false;
    }
}
