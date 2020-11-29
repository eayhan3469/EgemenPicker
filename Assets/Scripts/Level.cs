using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public List<Part> Parts;
    public Dictionary<Part, ObjectHolder> ObjectHolders;

    void Start()
    {
        ObjectHolders = new Dictionary<Part, ObjectHolder>();
        var partNumber = 1;

        foreach (var part in Parts)
        {
            var objectHolder = part.GetComponentInChildren<ObjectHolder>();
            part.PartNumber = partNumber;
            partNumber++;

            if (objectHolder != null)
                ObjectHolders.Add(part, objectHolder);
        }

        foreach (var objectHolder in ObjectHolders)
            objectHolder.Value.NumberOfObjectForPass = (10 * GameManager.Instance.LevelMultiplier) + (objectHolder.Key.PartNumber - 1) * 10;
    }
}
