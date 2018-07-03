using UnityEngine;

public interface IParseable
{
    string Name { get; }
    void Load(string line);
}
