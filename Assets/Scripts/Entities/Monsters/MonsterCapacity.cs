﻿using System.Collections.Generic;
using UnityEngine;

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
            }
            else
            {
                Debug.Log("No more PA");
            }
        }
    }

    public void Move(WayPoint destination)
    {
        if (HasMoved)
        {
            return;
        }

        List<WayPoint> path = new List<WayPoint>();
        if (this._monsterMain.PaCurrent > 0 && destination != this._monsterMain.Position)
        {
            if(destination.obstacle)
            {
                Debug.Log("Destination is an obstacle");
                return;
            }

            path = _mapMain.UseAStar(this._monsterMain.Position, destination);
            if (path.Count <= this._monsterMain.PaCurrent && path.Count > 0)
            {
                this._monsterMain.Position.obstacle = false;
                for (int i = 0; i < path.Count; i++)
                {
                    ChangeWaypoint(path[i]);

                    //this._monsterMain.Position = path[i];
                    //this.transform.position = path[i].transform.position;
                    this._monsterMain.PaCurrent--;
                    if (i == path.Count - 1)
                    {
                        path[i].obstacle = true;
                        path[i].entity = _monsterMain;
                    }
                }
            }
            else
            {
                Debug.Log("No more PA");
            }

            HasMoved = true;
            AttackAfterMove();
        }
        else
        {
            Debug.Log("No more PA");
            Debug.Log("Destination is the same as the current position");
        }
    }

    private void AttackAfterMove()
    {
        Attack(_target);
    }

    public void ChangeWaypoint(WayPoint waypointToMoveTo)
    {
        this._monsterMain.Position = waypointToMoveTo;
        this.transform.position = waypointToMoveTo.transform.position;
    }
}
