using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradable
{
    public int upgradeClass { get; }

    public void Upgrade();
}
