using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterAttribute
{
    Agility,
    Intellect,
    Stamina,
    Strength
}

[Serializable]//직렬화 : 클래스의 인스턴스를 파일에 저장하거나 네트워크를 통해 전송할 수 있도록 하는 것
public class ItemBuffer
{
    public CharacterAttribute stat; // 캐릭터에 적용할 상태
    public int value;
    
    [SerializeField] private int min;
    [SerializeField] private int max;
    public int Min => min;
    public int Max => max;

    public ItemBuffer(int min, int max)
    {
        this.min = min;
        this.max = max;

        GenerateValue();
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }

    public void AddValue(ref int v)
    {
        v += value;
    }
}
