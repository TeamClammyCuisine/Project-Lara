using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    float Speed{get;set;}
    float Health{get;set;}
    float AttackDamage{get;set;}
    float AttackSpeed{get;set;}
}
