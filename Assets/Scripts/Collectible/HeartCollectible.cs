using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCollectible : ACollectible
{
    protected override void OnCollect()
    {
        if (Player.Instance.life < 5)
            Player.Instance.life++;
    }
}
