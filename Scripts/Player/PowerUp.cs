using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update

    public float speedUpgrade;
    public float jumpUpgrade;
    public int dayDamageUpgrade;
    public int nightDamageUpgrade;
    public float gravityUpgrade;
    public int dayDoubleJumpUpgrade;
    public int nightDoubleJumpUpgrade;
    public float blockChanceUpgrade;
    public float cookChanceUpgrade;
    public GameObject killEffectUpgrade;
    public GameObject missEffectUpgrade;
    public int healthUpgrade;
    public float nightExtensionUpgrade;
    public float dayExtensionUpgrade;

    public int healAmount;

    public string itemTitle;
    public string itemDesc;

    public GameObject takeEffect;

    // Update is called once per frame
    public void TakePowerUp(PlayerMovement player)
    {
        player.speed += speedUpgrade;
        player.jumpForce += jumpUpgrade;
        player.dayDamageUpgrade += dayDamageUpgrade;
        player.nightDamageUpgrade += nightDamageUpgrade;
        player.rb2d.gravityScale -= gravityUpgrade;
        player.dayDoubleJumps += dayDoubleJumpUpgrade;
        player.nightDoubleJumps += nightDoubleJumpUpgrade;
        player.hp.blockChance += blockChanceUpgrade;
        player.cookChance += cookChanceUpgrade;
        player.hp.maxHp += healthUpgrade;
        player.dayNight.daySpeed -= dayExtensionUpgrade;
        player.dayNight.nightSpeed -= nightExtensionUpgrade;

        if (killEffectUpgrade)
        {
            player.killEffect.Add(killEffectUpgrade);
        }

        if (missEffectUpgrade)
        {
            player.missEffect.Add(missEffectUpgrade);
        }
		
		if (healAmount > 0)
		{
			player.hp.Heal(healAmount);
		}

        Instantiate(takeEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
