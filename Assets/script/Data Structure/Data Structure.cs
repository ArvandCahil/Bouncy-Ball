using UnityEngine.UI;

public class playerInformation
{
    public profile profile;
    public currency currency;
    public inventory inventory;
}

public class profile 
{
    public string name;
}

public class currency
{
    public int star;
}

public class inventory
{
    public int equippedSkinID;
    public int[] ownedSkinID;
}

public class skinInformation
{
    public skin[] skins;
}

public class skin 
{ 
    public string name;
    public string description;
    public int ID;
    public Image ballImage;
}

public enum graphic
{
    low,
    medium,
    high
}

public class setting
{
    public float volume;
    public graphic graphic;
}