﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gere les capacités des monstres du type attaque et mouvement.
/// </summary>
public class MonsterCapacity : MonoBehaviour
{
    [SerializeField]
    private MonsterMain _monsterMain;

    [SerializeField]
    private Capacity _capacity;

    private Entity _target;

    public MapMain _mapMain;

    [HideInInspector]
    public bool HasAttacked = false;
    [HideInInspector]
    public bool HasMoved = false;
    [HideInInspector]
    public bool _hasSpecial = false;

    /// <summary>
    /// Permet de faire l' action du monstre en lui indiquant d'attaquer tout en lui enlevant des PA.
    /// </summary>
    /// <param name="target"></param>
    public void Attack(Entity target)
    {
        if (HasAttacked)
        {
            return;
        }

        if (target is CharacterMain tmp)
        {
            if (this._monsterMain.PaCurrent > 0)
            {
                tmp.CharacterHealth.TakeDamage(_monsterMain.Atk);
                this._monsterMain.PaCurrent--;
                HasAttacked = true;
                Special(target);
            }
        }
    }

    /// <summary>
    /// Permet de faire l'action du monstre en lui indiquant de se déplacer tout en lui enlevant des PA en utilisant l'A*.
    /// </summary>
    /// <param name="destination"></param>
    public void Move(WayPoint destination)
    {
        if (HasMoved)
        {
            return;
        }

        List<WayPoint> path = new List<WayPoint>();
        if (this._monsterMain.PaCurrent > 0 && destination != this._monsterMain.Position)
        {
            if (destination.obstacle)
            {
                return;
            }

            path = _mapMain.UseAStar(this._monsterMain.Position, destination);
            if (path.Count <= this._monsterMain.PaCurrent && path.Count > 0)
            {
                this._monsterMain.Position.obstacle = false;
                for (int i = 0; i < path.Count; i++)
                {
                    ChangeWaypoint(path[i]);

                    this._monsterMain.PaCurrent--;
                    if (i == path.Count - 1)
                    {
                        path[i].obstacle = true;
                        path[i].entity = _monsterMain;
                    }
                }
            }

            HasMoved = true;
            AttackAfterMove();
        }
    }

    public void ChangeWaypoint(WayPoint waypointToMoveTo)
    {
        this._monsterMain.Position = waypointToMoveTo;
        this.transform.position = waypointToMoveTo.transform.position;
    }

    public void Special(Entity target)
    {
        if (_hasSpecial)
        {
            return;
        }

        if (_capacity.isShielding)
        {
            if (this._monsterMain.PaCurrent > 0)
            {
                _monsterMain.Def += _capacity.damage;
                this._monsterMain.PaCurrent -= _capacity.cost;
                _hasSpecial = true;
            }
        }
        else
        {
            if (target is MonsterMain tmp)
            {
                if (this._monsterMain.PaCurrent > 0)
                {
                    tmp.MonsterHealth.TakeDamage(_capacity.damage);
                    this._monsterMain.PaCurrent -= _capacity.cost;
                    _hasSpecial = true;
                }
            }
        }
    }

    private void AttackAfterMove()
    {
        Attack(_target);
    }
}
