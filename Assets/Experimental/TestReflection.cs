﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestReflection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var v = Shard.ShardDictionary["Hello"];
    }
}
