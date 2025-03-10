using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public event Action onTakeDamage;

    private Coroutine coroutine;


    private void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (health.curValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void UseStamina(float Used)
    {
        stamina.Subtract(Used);
    }

    public bool CurStamina(int Used)
    {
        return stamina.curValue < Used ?  false : true;
    }


    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }

    public void SpeedUP(float amount)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(UseSpeedItem(amount));
    }

    private IEnumerator UseSpeedItem(float amount)
    {
        float startSpeed = CharacterManager.Instance.Player.controller.moveSpeed;
        CharacterManager.Instance.Player.controller.moveSpeed += amount;

        yield return new WaitForSeconds(20);

        CharacterManager.Instance.Player.controller.moveSpeed = startSpeed;
    }

}