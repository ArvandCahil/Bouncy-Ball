using UnityEngine.UI;

public class playerInformation
{
    public playerData playerData;
    public currency currency;
    public playerInventory inventory;

}

public class playerData 
{
    public string playerName;
}

public class currency
{
    public int star;
}

public class playerInventory
{
    public int equippedBallId;
    public int[] ownedBallId;
}

public class ball_Information
{
    public ball[] balls;
}

public class ball 
{ 
    public string name;
    public string description;
    public int id;
    public Image ballImage;
}